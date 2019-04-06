using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    internal class SingleModule
    {
        #region Fields

        private readonly ProcessModule _module;

        #endregion

        public string Name
        {
            get { return _module.ModuleName; }
        }

        public string Filepath
        {

            get
            {
                try
                {
                    return _module.FileName;
                }
                catch (Exception e) //because of security
                {
                    return "Access denied";
                }
            }

        }

        internal SingleModule(ProcessModule module)
        {
            _module = module;
        }
    }
}
