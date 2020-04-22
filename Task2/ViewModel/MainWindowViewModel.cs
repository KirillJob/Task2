using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using Task2.Model;
using System.Windows.Input;
using System.Threading;

namespace Task2.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {

        readonly Dispatcher _dispatcher;
        public MainWindowViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        #region Cущности связанные с Проверками
        ObservableCollection<Test> _tests;
        public ObservableCollection<Test> Tests
        {
            get
            {
                if (_tests == null)
                    //_dispatcher.BeginInvoke(new Action(() => { _tests = WorkModel.GetAllTests(); }));
                _dispatcher.Invoke(() => { _tests = WorkModel.GetAllTests(); });
                //_tests = WorkModel.GetAllTests();
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
        #endregion 

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
                    _addTest = new RelayCommand(ExecuteAddTestAsync, CanExecuteAddTest);
                return _addTest;
            }
        }

        public async void ExecuteAddTestAsync(object parameter)
        {
            Tests.Add(NewTest);
            await Task.Run(() => WorkModel.SaveChanges());
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
                    _changeTest = new RelayCommand(ExecuteChangeTestAsync, CanExecuteChangeTest);
                return _changeTest;
            }
        }

        public async void ExecuteChangeTestAsync(object parameter)
        {
            await Task.Run(() => WorkModel.SaveChanges());
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
                    _delTest = new RelayCommand(ExecuteDelTestAsync, CanExecuteDelTest);
                return _delTest;
            }
        }

        public async void ExecuteDelTestAsync(object parameter)
        {
            Tests.Remove(SelTest);
            await Task.Run(() => WorkModel.SaveChanges());
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

        public async void ExecuteSelectedItemChanged(object parameter)
        {
            Parameters.Clear();

            await Task.Run(() => 
            {
                foreach (Parameter param in WorkModel.GetParametrsForSelTest(SelTest))
                {
                    _dispatcher.BeginInvoke((ThreadStart)delegate ()
                    {
                        Parameters.Add(param);
                    });
                }
            });
        }

        #region Сущности связанные с Параметрами
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
                    _addParameter = new RelayCommand(ExecuteAddParameterAsync, CanExecuteAddParameter);
                return _addParameter;
            }
        }

        public async void ExecuteAddParameterAsync(object parameter)
        {
            NewParameter.TestId = SelTest.TestId;
            Parameters.Add(NewParameter);
            AllParameters.Add(NewParameter);
            await Task.Run (() => WorkModel.SaveChanges());
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
                    _changeParameter = new RelayCommand(ExecuteChangeParameterAsync, CanExecuteChangeParameter);
                return _changeParameter;
            }
        }

        public async void ExecuteChangeParameterAsync(object parameter)
        {
           await Task.Run (() => WorkModel.SaveChanges());
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
                    _delParameter = new RelayCommand(ExecuteDelParameterAsync, CanExecuteDelParameter);
                return _delParameter;
            }
        }

        public async void ExecuteDelParameterAsync(object parameter)
        {
            AllParameters.Remove(SelParameter);
            Parameters.Remove(SelParameter);
            await Task.Run (() => WorkModel.SaveChanges());
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
