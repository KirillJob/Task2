using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;
using Task2.Model.db;

namespace Task2.Model
{
    public class MainModel
    {
        private MainModel () { }

        private static MainModel _instance;
        private static IDB db;
        private static readonly object _lock = new object();

        public static ObservableCollection<Parameter> Parameters { get; set; } = new ObservableCollection<Parameter>();
        public static ObservableCollection<Test> Tests { get; set; } = new ObservableCollection<Test>();
        public static MainModel GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MainModel();
                        db = new ModelDB();
                        Tests = db.GetTests();
                        Parameters = db.GetParameters();
                    }
                }
            }
            return _instance;
        }

        private static async void SaveChangesAsync ()
        {
            await Task.Run(() => db.SaveChanges());
        }

        public static void AddTest(Test t)
        {
            Tests.Add(t);
            SaveChangesAsync();
        }
        public static void ChangeTest()
        {
            SaveChangesAsync();
        }
        public static void DelTest(Test t)
        {
            Tests.Remove(t);
            SaveChangesAsync();
        }
        public static void AddParameter(Parameter p)
        {
            Parameters.Add(p);
            SaveChangesAsync();
        }
        public static void ChangeParameter()
        {
            SaveChangesAsync();
        }
        public static void RemoveParameter(Parameter p)
        {
            Parameters.Remove(p);
            SaveChangesAsync();
        }

    }
}
