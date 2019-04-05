using System;
using System.Diagnostics;

namespace TaskManager.Models
{
    class SingleProcess
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
                PerformanceCounter performance = new PerformanceCounter("Process", "% Processor Time", _process.ProcessName);
                return performance.NextValue() / 100;
            }
        }
        public float RAMPercents
        {
            get
            {
                return _process.PrivateMemorySize64 / 1024;
            }
        }
        public float RAMAmount
        {
            get { return _RAMAmount; }
        }
        public int ThreadsNumber
        {
            get { return _threadsNumber; }
        }
        public string User
        {
            get { return _user; }
        }
        public string Filepath
        {
            get { return _filepath; }
        }
        public DateTime StartingTime
        {
            get { return _startingTime; }
        }
        #endregion

        internal SingleProcess(Process process)
        {
            
        }
    }
}
