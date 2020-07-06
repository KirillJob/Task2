using System;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Task2.Model;
using System.Windows.Input;

namespace Task2.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {

        readonly Dispatcher _dispatcher;
        public MainWindowViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            MainModel.GetInstance();
        }

        #region Cущности связанные с Проверками
        ObservableCollection<Test> _tests;
        public ObservableCollection<Test> Tests
        {
            get
            {
                if (_tests == null)
                _dispatcher.Invoke(() => { _tests = MainModel.Tests; });

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
                    _selTest = new Test
                    {
                        TestDate = DateTime.Now
                    };
                }

                return _selTest;
            }
            set
            {
                _selTest = value;
                OnPropertyChanged();
                Parameters = (ObservableCollection<Parameter>)_selTest.Parameters;
            }
        }
        #endregion 

        #region Логика формы (Добавить новую проверку)
        Test _newTest;
        public Test NewTest
        {
            get
            {
                if (_newTest == null)
                {
                    _newTest = new Test
                    {
                        TestDate = DateTime.Now
                    };
                }    
                return _newTest;
            }
            set
            {
                _newTest = value;
                OnPropertyChanged();
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
            MainModel.AddTest(NewTest);
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
        RelayCommand _saveTestChanges;
        public ICommand SaveTestChanges
        {
            get
            {
                if (_saveTestChanges == null)
                    _saveTestChanges = new RelayCommand(ExecuteChangeTest, CanExecuteChangeTest);
                return _saveTestChanges;
            }
        }

        public void ExecuteChangeTest(object parameter)
        {
            MainModel.ChangeTest();
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
            MainModel.DelTest(SelTest);
        }

        public bool CanExecuteDelTest(object parameter)
        {
            if (string.IsNullOrEmpty(SelTest.BlockName))
                return false;
            return true;
        }
        #endregion

        #region Сущности связанные с Параметрами

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
                OnPropertyChanged();
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

                OnPropertyChanged();
            }
        }
        #endregion

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
                _newParameter.Test = SelTest;
                return _newParameter;
            }
            set
            {
                _newParameter = value;
                OnPropertyChanged();
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
            MainModel.AddParameter(NewParameter);
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
            MainModel.ChangeParameter();
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
            MainModel.RemoveParameter(SelParameter);
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
