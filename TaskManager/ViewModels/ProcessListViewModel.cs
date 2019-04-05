using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Tools;

namespace TaskManager.ViewModels
{
    internal class ProcessListViewModel: BaseViewModel
    {
        #region Fields

        private ObservableCollection<SingleProcess> _processes;
        private bool _isControlEnabled = true;
        private SingleProcess _selectedProcess;

        private Thread _workingThread;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;

        #region Commands
        #region Sort
        private RelayCommand<object> _sortById;
        private RelayCommand<object> _sortByName;
        private RelayCommand<object> _sortByIsActive;
        private RelayCommand<object> _sortByCPUPercents;
        private RelayCommand<object> _sortByRAMPercents;
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
        public RelayCommand<object> SortByRAMPercents
        {
            get
            {
                return _sortByRAMPercents ?? (_sortByRAMPercents = new RelayCommand<object>(o =>
                           SortImplementation(o, 4)));
            }
        }
        public RelayCommand<object> SortByRAMAmount
        {
            get
            {
                return _sortByRAMAmount ?? (_sortByRAMAmount = new RelayCommand<object>(o =>
                           SortImplementation(o, 5)));
            }
        }
        public RelayCommand<object> SortByThreadsNumber
        {
            get
            {
                return _sortByThreadsNumber ?? (_sortByThreadsNumber = new RelayCommand<object>(o =>
                           SortImplementation(o, 6)));
            }
        }
        public RelayCommand<object> SortByUser
        {
            get
            {
                return _sortByUser ?? (_sortByUser = new RelayCommand<object>(o =>
                           SortImplementation(o, 7)));
            }
        }
        public RelayCommand<object> SortByFilepath
        {
            get
            {
                return _sortByFilepath ?? (_sortByFilepath = new RelayCommand<object>(o =>
                           SortImplementation(o, 8)));
            }
        }
        public RelayCommand<object> SortByStartingTime
        {
            get
            {
                return _sortByStartingTime ?? (_sortByStartingTime = new RelayCommand<object>(o =>
                           SortImplementation(o, 9)));
            }
        }

        #endregion

        private async void EndTaskImplementation(object obj)
        {
            await Task.Run(() =>
            {
                SelectedProcess.ProcessItself.Kill();
                StationManager.UpdateProcessList();
            });
        }

        private async void OpenFolderImplementation(object obj)
        {
            await Task.Run(() =>
            {
                //SelectedProcess.ProcessItself.Kill();
                //StationManager.UpdateProcessList();
            });
        }

        private async void ShowModulesImplementation(object obj)
        {
            await Task.Run(() =>
            {
                //SelectedProcess.ProcessItself.Kill();
                //StationManager.UpdateProcessList();
            });
        }

        private async void ShowThreadsImplementation(object obj)
        {
            await Task.Run(() =>
            {
                //SelectedProcess.ProcessItself.Kill();
                //StationManager.UpdateProcessList();
            });
        }

        private async void SortImplementation(object obj, int i)
        {
            await Task.Run(() =>
            {
                IOrderedEnumerable<SingleProcess> sortedProcesses;
                switch (i)
                {
                    case 0:
                        sortedProcesses = from u in _processes
                            orderby u.ID
                            select u;
                        break;

                    case 1:
                        sortedProcesses = from u in _processes
                                        orderby u.Name
                                        select u;
                        break;
                    case 2:
                        sortedProcesses = from u in _processes
                                          orderby u.IsActive
                                        select u;
                        break;
                    case 3:
                        sortedProcesses = from u in _processes
                                          orderby u.CPUPercents
                                        select u;
                        break;
                    case 4:
                        sortedProcesses = from u in _processes
                                          orderby u.RAMPercents
                                        select u;
                        break;
                    case 5:
                        sortedProcesses = from u in _processes
                                          orderby u.RAMAmount
                                        select u;
                        break;
                    case 6:
                        sortedProcesses = from u in _processes
                                          orderby u.ThreadsNumber
                                        select u;
                        break;
                    case 7:
                        sortedProcesses = from u in _processes
                                          orderby u.User
                                        select u;
                        break;
                    case 8:
                        sortedProcesses = from u in _processes
                                          orderby u.Filepath
                                        select u;
                        break;
                    default:
                        sortedProcesses = from u in _processes
                                          orderby u.StartingTime
                                        select u;
                        break;
                }
                Processes = new ObservableCollection<SingleProcess>(sortedProcesses);
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
            while (!_token.IsCancellationRequested)
            {
                StationManager.UpdateProcessList();

                //@TODO ADD METADATA UPDATING EVERY TWO SECONDS


                Processes = new ObservableCollection<SingleProcess>(StationManager.ProcessList);
                for (int i = 0; i < 5; i++)
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
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }

        private bool CanExecuteCommand()
        {
            return SelectedProcess != null;
        }
    }
}
