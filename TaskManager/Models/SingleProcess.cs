using System;
using System.Diagnostics;

namespace TaskManager.Models
{
    public class SingleProcess
    {
        #region Fields

        private readonly Process _process;
        #endregion

        #region Properties
        public Process ProcessItself
        {
            get { return _process; }
        }

        public int ID
        {
            get { return _process.Id; }
        }
        public string Name
        {
            get { return _process.ProcessName; }
        }
        public bool IsActive
        {
            get { return _process.Responding; }
        }
        public float CPUPercents
        {
            get
            {
                PerformanceCounter p = new PerformanceCounter("Process", "% Processor Time", _process.ProcessName);
                return p.NextValue() / 100;
            }
        }
        public float RAMAmount
        {
            get { return _process.PrivateMemorySize64 / 1024; }
        }
        public int Threads
        {
            get { return _process.Threads.Count; }
        }
        public string User
        {
            get { return _process.MachineName; }
        }

        public ProcessModuleCollection Modules
        {
            get
            {
                return _process.Modules;
            }
        }

        public ProcessThreadCollection ThreadsCollection
        {
            get
            {
                return _process.Threads;
            }
        }

        public string Filepath
        {
            get
            {
                try
                {
                    return _process.MainModule.FileName;
                }
                catch (Exception e) //because of security
                {
                    return "Access denied";
                }
            }
        }

        public bool checkAvailability()
        {
            if (StartingTime == "Access denied")
            {
                return false;
            }
            return true;
        }

        public string StartingTime
        {
            get
            {
                try
                {
                    return _process.StartTime.ToString("HH:mm:ss dd/MM/yyyy");
                }
                catch (Exception e)
                {
                    return "Access denied";
                }
            }
        }
        #endregion

        internal SingleProcess(Process process)
        {
            _process = process;
        }
    }
}
