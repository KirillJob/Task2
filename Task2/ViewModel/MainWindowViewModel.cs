using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Task2.Model;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows;



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
                    _tests = WorkModel.GetAllTests();
                return _tests;
            }
        }

        Test _selTest;
        public Test SelTest
        {
            get
            {
                if (_selTest == null)
                {
                    _selTest = new Test();
                    _selTest.TestDate = DateTime.Now;
                }

                return _selTest;
            }
            set
            {
                _selTest = value;
                OnPropertyChanged("SelTest");
            }
        }


        #region Логика формы (Добавить новую проверку)
        Test _newTest;
        public Test NewTest
        {
            get
            {
                if (_newTest == null)
                {
                    _newTest = new Test();
                    _newTest.TestDate = DateTime.Now;
                }    
                    
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
            WorkModel.SaveChanges();
            NewTest = null;
        }

        public bool CanExecuteAddTest(object parameter)
        {
            if (string.IsNullOrEmpty(NewTest.BlockName))
                return false;
            return true;
        }
        #endregion

        #region Логика формы (Изменить выбранную проверку)
        RelayCommand _changeTest;
        public ICommand ChangeTest
        {
            get
            {
                if (_changeTest == null)
                    _changeTest = new RelayCommand(ExecuteChangeTest, CanExecuteChangeTest);
                return _changeTest;
            }
        }

        public void ExecuteChangeTest(object parameter)
        {
            WorkModel.SaveChanges();
        }

        public bool CanExecuteChangeTest(object parameter)
        {
            if (string.IsNullOrEmpty(SelTest.BlockName))
                return false;
            return true;
        }
        #endregion

        #region Логика формы (Удалить выбранную проверку)
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
            Tests.Remove(SelTest);
            WorkModel.SaveChanges();
        }

        public bool CanExecuteDelTest(object parameter)
        {
            if (string.IsNullOrEmpty(SelTest.BlockName))
                return false;
            return true;
        }
        #endregion

        RelayCommand _selectedItemChanged;
        public ICommand SelectedItemChanged
        {
            get
            {
                if (_selectedItemChanged == null)
                    _selectedItemChanged = new RelayCommand(ExecuteSelectedItemChanged);
                return _selectedItemChanged;
            }
        }

        public void ExecuteSelectedItemChanged(object parameter)
        {
            Parameters.Clear();
            foreach (Parameter param in WorkModel.GetParametrsForSelTest(SelTest))
            {
                Parameters.Add(param);
            }
        }



        ObservableCollection<Parameter> _allParameters;
        public ObservableCollection<Parameter> AllParameters
        {
            get
            {
                if (_allParameters == null)
                    _allParameters = WorkModel.GetAllParameters();
                return _allParameters;
            }
        }

        ObservableCollection<Parameter> _parameters;
        public ObservableCollection<Parameter> Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new ObservableCollection<Parameter>();
                return _parameters;
            }
            set 
            {
                _parameters = value;
                OnPropertyChanged("SelTest");
            }

        }

        Parameter _selParameter;
        public Parameter SelParameter
        {
            get
            {
                if (_selParameter == null)
                {
                    _selParameter = new Parameter();
                }

                return _selParameter;
            }
            set
            {
                _selParameter = value;
                OnPropertyChanged("SelParameter");
            }
        }

        #region Логика формы (Добавить новый параметр)
        Parameter _newParameter;
        public Parameter NewParameter
        {
            get
            {
                if (_newParameter == null)
                {
                    _newParameter = new Parameter();
                }

                return _newParameter;
            }
            set
            {
                _newParameter = value;
                OnPropertyChanged("NewParameter");
            }
        }

        RelayCommand _addParameter;
        public ICommand AddParameter
        {
            get
            {
                if (_addParameter == null)
                    _addParameter = new RelayCommand(ExecuteAddParameter, CanExecuteAddParameter);
                return _addParameter;
            }
        }

        public void ExecuteAddParameter(object parameter)
        {
            NewParameter.TestId = SelTest.TestId;
            Parameters.Add(NewParameter);
            AllParameters.Add(NewParameter);
            WorkModel.SaveChanges();
            NewParameter = null;
        }

        public bool CanExecuteAddParameter(object parameter)
        {
            if (string.IsNullOrEmpty(NewParameter.ParameterName))
                return false;
            return true;
        }
        #endregion

        #region Логика формы (Изменить выбранный параметр)
        RelayCommand _changeParameter;
        public ICommand ChangeParameter
        {
            get
            {
                if (_changeParameter == null)
                    _changeParameter = new RelayCommand(ExecuteChangeParameter, CanExecuteChangeParameter);
                return _changeParameter;
            }
        }

        public void ExecuteChangeParameter(object parameter)
        {
            WorkModel.SaveChanges();
        }

        public bool CanExecuteChangeParameter(object parameter)
        {
            if (string.IsNullOrEmpty(SelParameter.ParameterName))
                return false;
            return true;
        }
        #endregion

        #region Логика формы (Удалить выбранный параметр)
        RelayCommand _delParameter;
        public ICommand DelParameter
        {
            get
            {
                if (_delParameter == null)
                    _delParameter = new RelayCommand(ExecuteDelParameter, CanExecuteDelParameter);
                return _delParameter;
            }
        }

        public void ExecuteDelParameter(object parameter)
        {
            AllParameters.Remove(SelParameter);
            Parameters.Remove(SelParameter);
            WorkModel.SaveChanges();
        }

        public bool CanExecuteDelParameter(object parameter)
        {
            if (string.IsNullOrEmpty(SelParameter.ParameterName))
                return false;
            return true;
        }
        #endregion
    }
}
