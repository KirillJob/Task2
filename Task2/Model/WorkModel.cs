using System.Collections.ObjectModel;
using System.Data.Entity;

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
