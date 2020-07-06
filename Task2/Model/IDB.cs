using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Task2.Model
{
    interface IDB
    {
        ObservableCollection<Test> GetTests();
        ObservableCollection<Parameter> GetParameters();
        void SaveChanges();
    }
}
