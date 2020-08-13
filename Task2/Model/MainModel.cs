using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;
using Task2.Model.db;

namespace Task2.Model
{
    public class MainModel
    {
        public MainModel () 
        {
            db = new ModelDB();
            Tests = db.GetTests();
            Parameters = db.GetParameters();
        }

        private  IDB db;

        public ObservableCollection<Parameter> Parameters { get; set; } = new ObservableCollection<Parameter>();
        public ObservableCollection<Test> Tests { get; set; } = new ObservableCollection<Test>();


        private async void SaveChangesAsync ()
        {
            await Task.Run(() => db.SaveChanges());
        }

        public void AddTest(Test t)
        {
            Tests.Add(t);
            SaveChangesAsync();
        }
        public void ChangeTest()
        {
            SaveChangesAsync();
        }
        public void DelTest(Test t)
        {
            Tests.Remove(t);
            SaveChangesAsync();
        }
        public void AddParameter(Parameter p)
        {
            Parameters.Add(p);
            SaveChangesAsync();
        }
        public void ChangeParameter()
        {
            SaveChangesAsync();
        }
        public void RemoveParameter(Parameter p)
        {
            Parameters.Remove(p);
            SaveChangesAsync();
        }

    }
}
