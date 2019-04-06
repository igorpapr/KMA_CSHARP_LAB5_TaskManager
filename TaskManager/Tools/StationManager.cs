using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Models;

namespace TaskManager.Tools
{
    internal static class StationManager
    {
        #region Fields
        public static event Action StopThreads;
        private static List<SingleProcess> _processList;
        #endregion

        #region Properties
        internal static List<SingleProcess> ProcessList
        {
            get { return _processList; }
        }
        internal static int SortingParameter { get; set; }
        #endregion

        internal static void Initialize()
        {
            SortingParameter = 1;
            _processList = new List<SingleProcess>();
            UpdateProcessList();
        }

        
        internal static void UpdateProcessList(int targetId = -1)
        {
            AddMissingProcesses();
            
            SortProcessList();
            //if (targetId != -1)
            //{
            //    _processList.Remove(_processList.Find(o =>
            //    {
            //        if (o.ID == targetId)
            //            return true;
            //        return false;
            //    }));
            //}
            ////@TODO DELETING AND SORTING
            ////RemoveKilledProcesses();
            //AddMissingProcesses();
        }

        internal static void SortProcessList()
        {            
            switch (SortingParameter)
            {
                case 0:
                    _processList = (from u in _processList
                                      orderby u.ID
                                      select u).ToList();
                    break;

                case 1:
                    _processList = (from u in _processList
                                      orderby u.Name
                                      select u).ToList();
                    break;
                case 2:
                    _processList = (from u in _processList
                                      orderby u.IsActive
                                      select u).ToList();
                    break;
                case 3:
                    _processList = (from u in _processList
                                      orderby u.CPUPercents
                                      select u).ToList();
                    break;
                case 4:
                    _processList = (from u in _processList
                                      orderby u.RAMAmount
                                      select u).ToList();
                    break;
                case 5:
                    _processList = (from u in _processList
                                      orderby u.Threads
                                      select u).ToList();
                    break;
                case 6:
                    _processList = (from u in _processList
                                      orderby u.User
                                      select u).ToList();
                    break;
                case 7:
                    _processList= (from u in _processList
                                      orderby u.Filepath
                                      select u).ToList();
                    break;
                default:
                    _processList = (from u in _processList
                                      orderby u.StartingTime
                                      select u).ToList();
                    break;
            }
        }

        private static void RemoveKilledProcesses()
        {
            foreach (var item in _processList)
            {
                if (item != null)
                {
                    if (!FoundTheSameInProcessArray(item.ID))
                        _processList.Remove(item);
                }
            }
        }

        private static bool FoundTheSameInProcessArray(int processId)
        {
            foreach (var item in Process.GetProcesses())
            {
                if (processId == item.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private static void AddMissingProcesses()
        {
            foreach (var item in Process.GetProcesses())
            {
                if (item != null)
                {
                    if(!FoundTheSameInProcessList(item.Id))
                   _processList.Add(new SingleProcess(item));
                }
            }
        }

        private static bool FoundTheSameInProcessList(int processId)
        {
            foreach (var item in _processList)
            {
                if (processId == item.ID)
                {
                    return true;
                }
            }
            return false;
        }

        internal static void CloseApp()
        {
            MessageBox.Show("Shutting Down");
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}
