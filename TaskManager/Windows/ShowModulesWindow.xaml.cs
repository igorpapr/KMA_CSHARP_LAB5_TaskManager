using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Windows
{
    /// <summary>
    /// Логика взаимодействия для ShowModulesWindow.xaml
    /// </summary>
    public partial class ShowModulesWindow : Window
    {
        public ShowModulesWindow(ref SingleProcess proc)
        {
            InitializeComponent();
            //ShowModulesViewModel vm 
                DataContext = new ShowModulesViewModel(ref proc);
            //DataContext = vm;
            //if (vm.CloseAction == null)
            //    vm.CloseAction = new Action(this.Close);
        }
    }
}
