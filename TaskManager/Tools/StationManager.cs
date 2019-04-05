using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        internal static void UpdateProcessList()
        {                                                    //@TODO DELETING
            AddMissingProcesses();
            SortProcessList();
        }

        private static void AddMissingProcesses()
        {
            foreach (var item in Process.GetProcesses())
            {
                if (item != null)
                {
                    if(!FoundTheSame(item.Id))
                   _processList.Add(new SingleProcess(item));
                }
            }
        }

        private static bool FoundTheSame(int processId)
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
