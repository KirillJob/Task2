using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Task2.Model;
using System.Windows.Input;

//TODO: Почистить Xaml код: убрать лишние свойста, что можно то стилизовать, соурсы привязать к моделвью, нажатие на кнопки релизовать через команды
//TODO: Во вьюмодел: реализовать команды, реализовать свойства
//TODO: В модели: реализовать модключение к БД, локальное хранение представления БД, методы взаимодействия с вьюмодел
//TODO: Реализовать многопоточность (UI в своем потоке, вся логика в своем потоке)

namespace Task2.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        ObservableCollection<Test> _tests;
        public ObservableCollection<Test> Tests
        {
            get
            {
                if (_tests == null)
                    _tests = WorkModel.AllTests();
                return _tests;
            }
        }

        Test _newTest;
        public Test NewTest
        {
            get
            {
                if (_newTest == null)
                    _newTest = new Test();
                return _newTest;
            }
            set
            {
                _newTest = value;
                OnPropertyChanged("NewTest");
            }
        }


        RelayCommand _addTest;
        public ICommand AddTest
        {
            get
            {
                if (_addTest == null)
                    _addTest = new RelayCommand(ExecuteAddTest, CanExecuteAddTest);
                return _addTest;
            }
        }

        public void ExecuteAddTest(object parameter)
        {
            Tests.Add(NewTest);
            NewTest = null;
        }

        public bool CanExecuteAddTest(object parameter)
        {
            if (string.IsNullOrEmpty(NewTest.BlockName))
                return false;
            return true;
        }

        RelayCommand _changeTest;
        public ICommand ChangeTest
        {
            get
            {

                return _changeTest;
            }
        }

        RelayCommand _delTest;
        public ICommand DelTest
        {
            get
            {
                if (_delTest == null)
                    _delTest = new RelayCommand(ExecuteDelTest, CanExecuteDelTest);
                return _delTest;
            }
        }

        public void ExecuteDelTest(object parameter)
        {
            //TODO: реальзовать удаление test из списка
        }

        public bool CanExecuteDelTest(object parameter)
        {
            //TODO: организовать проверку, выбран ли элемент DGV
            return false;
        }
    }
}
