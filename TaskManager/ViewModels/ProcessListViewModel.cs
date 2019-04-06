using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Models;
using TaskManager.Tools;
using TaskManager.Windows;

namespace TaskManager.ViewModels
{
    internal class ProcessListViewModel: BaseViewModel
    {
        #region Fields

        private ObservableCollection<SingleProcess> _processes;
        private bool _isControlEnabled = true;
        private SingleProcess _selectedProcess;

        private Thread _workingThread;
        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _tokenSource;

        #region Commands
        #region Sort
        private RelayCommand<object> _sortById;
        private RelayCommand<object> _sortByName;
        private RelayCommand<object> _sortByIsActive;
        private RelayCommand<object> _sortByCPUPercents;
        private RelayCommand<object> _sortByRAMAmount;
        private RelayCommand<object> _sortByThreadsNumber;
        private RelayCommand<object> _sortByUser;
        private RelayCommand<object> _sortByFilepath;
        private RelayCommand<object> _sortByStartingTime;
        #endregion
        private RelayCommand<object> _endTask;
        private RelayCommand<object> _openFolder;
        private RelayCommand<object> _showThreads;
        private RelayCommand<object> _showModules;
        #endregion
        #endregion

        #region Properties

        public SingleProcess SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SingleProcess> Processes
        {
            get { return _processes; }
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> EndTask
        {
            get
            {
                return _endTask ?? (_endTask = new RelayCommand<object>(
                           EndTaskImplementation, o=>CanExecuteCommand()));
            }
        }

        public RelayCommand<object> OpenFolder
        {
            get
            {
                return _openFolder ?? (_openFolder = new RelayCommand<object>(
                           OpenFolderImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> ShowThreads
        {
            get
            {
                return _showThreads ?? (_showThreads = new RelayCommand<object>(
                           ShowThreadsImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> ShowModules
        {
            get
            {
                return _showModules ?? (_showModules = new RelayCommand<object>(
                           ShowModulesImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> SortById
        {
            get
            {
                return _sortById ?? (_sortById = new RelayCommand<object>(o =>
                               SortImplementation(o, 0)));
            }
        }
        public RelayCommand<object> SortByName
        {
            get
            {
                return _sortByName ?? (_sortByName = new RelayCommand<object>(o =>
                           SortImplementation(o, 1)));
            }
        }
        public RelayCommand<object> SortByIsActive
        {
            get
            {
                return _sortByIsActive ?? (_sortByIsActive = new RelayCommand<object>(o =>
                           SortImplementation(o, 2)));
            }
        }
        public RelayCommand<object> SortByCPUPercents
        {
            get
            {
                return _sortByCPUPercents ?? (_sortByCPUPercents = new RelayCommand<object>(o =>
                           SortImplementation(o, 3)));
            }
        }
        public RelayCommand<object> SortByRAMAmount
        {
            get
            {
                return _sortByRAMAmount ?? (_sortByRAMAmount = new RelayCommand<object>(o =>
                           SortImplementation(o, 4)));
            }
        }
        public RelayCommand<object> SortByThreadsNumber
        {
            get
            {
                return _sortByThreadsNumber ?? (_sortByThreadsNumber = new RelayCommand<object>(o =>
                           SortImplementation(o, 5)));
            }
        }
        public RelayCommand<object> SortByUser
        {
            get
            {
                return _sortByUser ?? (_sortByUser = new RelayCommand<object>(o =>
                           SortImplementation(o, 6)));
            }
        }
        public RelayCommand<object> SortByFilepath
        {
            get
            {
                return _sortByFilepath ?? (_sortByFilepath = new RelayCommand<object>(o =>
                           SortImplementation(o, 7)));
            }
        }
        public RelayCommand<object> SortByStartingTime
        {
            get
            {
                return _sortByStartingTime ?? (_sortByStartingTime = new RelayCommand<object>(o =>
                           SortImplementation(o, 8)));
            }
        }

        #endregion

        private async void EndTaskImplementation(object obj)
        {
            await Task.Run(() => { 
            if (_selectedProcess.checkAvailability())
            {
                _selectedProcess.ProcessItself?.Kill(); //_selectedProcess.ID
                StationManager.UpdateProcessList();
                _selectedProcess = null;
                Processes = new ObservableCollection<SingleProcess>(StationManager.ProcessList);
            }
            else
            {
                MessageBox.Show("Have no access");
            }
        });
    }
        
        private void OpenFolderImplementation(object obj)
        {
            try
            {
                Process.Start("explorer.exe",
                    _selectedProcess.Filepath.Substring(0,
                        _selectedProcess.Filepath.LastIndexOf("\\", StringComparison.Ordinal)));
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while accessing processing data");
            }
        }

        private void ShowModulesImplementation(object obj)
        {
            IsControlEnabled = false;
            ShowModulesWindow smw = new ShowModulesWindow(ref _selectedProcess);
            smw.ShowDialog();

            IsControlEnabled = true;
        }

        private void ShowThreadsImplementation(object obj)
        {
            IsControlEnabled = false;
            ShowThreadsWindow smw = new ShowThreadsWindow(ref _selectedProcess);
            smw.ShowDialog();

            IsControlEnabled = true;

        }

        private async void SortImplementation(object obj, int param)
        {
            await Task.Run(() =>
            {
                try
                {
                    StationManager.SortingParameter = param;
                    StationManager.UpdateProcessList();
                    Processes = new ObservableCollection<SingleProcess>(StationManager.ProcessList);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error occurred while accessing process data");
                }
            });
         }

        
        internal ProcessListViewModel()
        {
            _processes = new ObservableCollection<SingleProcess>(StationManager.ProcessList);
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartWorkingThread();
            StationManager.StopThreads += StopWorkingThread;
        }

        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
        }

        private void WorkingThreadProcess()
        {
            int temp = 0;
            while (!_token.IsCancellationRequested)
            {
                if (_selectedProcess != null)
                {
                    temp = _selectedProcess.ID;
                }
                StationManager.UpdateProcessList();
                
                Processes = new ObservableCollection<SingleProcess>(StationManager.ProcessList);

                if (_selectedProcess != null)
                { 
                    _selectedProcess = Processes.Single(i => i.ID == temp);
                }
                for (int i = 0; i < 4; i++)
                {
                    Thread.Sleep(1000);
                    if (_token.IsCancellationRequested)
                        break;
                }
                if (_token.IsCancellationRequested)
                    break;
            }
        }

        internal void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(1000);
            _workingThread.Abort();
            _workingThread = null;
        }

        private bool CanExecuteCommand()
        {
            return SelectedProcess != null;
        }
    }
}
