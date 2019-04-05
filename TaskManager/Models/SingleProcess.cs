using System;
using System.Diagnostics;

namespace TaskManager.Models
{
    class SingleProcess
    {
        #region Fields

        private Process _process;

        private readonly string _iD;
        private readonly string _name;
        private bool _isActive;
        private float _CPUPercents;
        private float _RAMPercents;
        private float _RAMAmount;
        private int _threadsNumber;
        private readonly string _user;
        private string _filepath;
        private readonly DateTime _startingTime;
        #endregion

        #region Properties
        public string ID
        {
            get { return _iD; }
        }
        public string Name
        {
            get { return _name; }
        }
        public bool IsActive
        {
            get { return _isActive; }
        }
        public float CPUPercents
        {
            get { return _CPUPercents; }
        }
        public float RAMPercents
        {
            get { return _RAMPercents; }
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

        internal SingleProcess(ref Process process)
        {
            
        }
    }
}
