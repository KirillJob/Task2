using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Task2.Model
{
    public static class WorkModel
    {
        public static ObservableCollection<Test> AllTests()
        {
            //TODO: Реализовать получение всех тестов из БД
            return new ObservableCollection<Test>();
        }
    }

}
