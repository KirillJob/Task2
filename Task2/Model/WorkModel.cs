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

        static WorkModel ()
        {
            kPKtestEntities = new KPKtestEntities();
            kPKtestEntities.Tests.Load();
        }

        public static ObservableCollection<Test> AllTests()
        {
            ObservableCollection<Test> allTest = kPKtestEntities.Tests.Local;
            return allTest;
        }

        public static void SaveChanges()
        {
            kPKtestEntities.SaveChanges();
        }
    }

}
