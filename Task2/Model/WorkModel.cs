using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Task2.Model
{
    public static class WorkModel
    {
        static KPKtestEntities kPKtestEntities;
        static ObservableCollection<Test> _allTest;
        static ObservableCollection<Parameter> _allParameters;

        static WorkModel ()
        {
            kPKtestEntities = new KPKtestEntities();
            kPKtestEntities.Tests.Load();
            kPKtestEntities.Parameters.Load();
            _allTest = kPKtestEntities.Tests.Local;
            _allParameters = kPKtestEntities.Parameters.Local;

        }

        public static ObservableCollection<Test> GetAllTests()
        {
            return _allTest;
        }

        public static ObservableCollection<Parameter> GetAllParameters()
        {
            return _allParameters;
        }

        //public static void AddTest(Test test)
        //{


        //}

        //public static ObservableCollection<Test> UpdateTests(ObservableCollection<Test> tests)
        //{

        //    return tests;
        //}

        public static ObservableCollection<Parameter> GetParametrsForSelTest(Test selTest)
        {
            ObservableCollection<Parameter> selParam = new ObservableCollection<Parameter>();
                foreach (Parameter par in _allParameters)
                {
                    if (par.TestId == selTest.TestId)
                    {
                        selParam.Add(par);
                    }
                }
            return selParam;
        }

        public static void SaveChanges()
        {
            kPKtestEntities.SaveChanges();
        }
    }

}
