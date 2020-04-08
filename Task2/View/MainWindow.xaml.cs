using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Media;
using System.Globalization;


namespace Task2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapterTabTest, adapterTabParam;
        DataTable testsTable, parametersTable, parametersLookTable;
        int idSelTestsRow = 0;
        int idSelParamRow = 0;
        

        public MainWindow()
        {
            InitializeComponent();
            // получаем строку подключения из app.config
            connectionString = ConfigurationManager.ConnectionStrings["Task2.Properties.Settings.KPKtestConnectionString"].ConnectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            insertDataPicker.SelectedDate = DateTime.Now;
            changeDataPicker.SelectedDate = DateTime.Now;

            testsTable = new DataTable();
            parametersTable = new DataTable();
            parametersLookTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                //Подготовительные работы для таблицы "Проверок"
                SqlCommand commandTabTest = new SqlCommand("SELECT * FROM Tests", connection);
                adapterTabTest = new SqlDataAdapter(commandTabTest);

                // установка команды на добавление для вызова хранимой процедуры
                adapterTabTest.InsertCommand = new SqlCommand("sp_Tests", connection);
                adapterTabTest.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapterTabTest.InsertCommand.Parameters.Add(new SqlParameter("@testdate", SqlDbType.SmallDateTime, 0, "TestDate"));
                adapterTabTest.InsertCommand.Parameters.Add(new SqlParameter("@blockname", SqlDbType.NVarChar, 50, "BlockName"));
                adapterTabTest.InsertCommand.Parameters.Add(new SqlParameter("@note", SqlDbType.NVarChar, 200, "Note"));
                SqlParameter testId = adapterTabTest.InsertCommand.Parameters.Add("@testId", SqlDbType.Int, 0, "Id");
                testId.Direction = ParameterDirection.Output;

                //Заливаем таблицу "Проверок" из БД в DataTable и привязываем DataTable к DataGrid                 
                adapterTabTest.Fill(testsTable);
                testsGrid.ItemsSource = testsTable.DefaultView;

                //Подготовительные работы для таблицы "Параметров"
                SqlCommand command = new SqlCommand("SELECT * FROM Parameters", connection);
                adapterTabParam = new SqlDataAdapter(command);

                // установка команды на добавление для вызова хранимой процедуры
                adapterTabParam.InsertCommand = new SqlCommand("sp_Parameters", connection);
                adapterTabParam.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapterTabParam.InsertCommand.Parameters.Add(new SqlParameter("@testId", SqlDbType.Int, 0, "TestId"));
                adapterTabParam.InsertCommand.Parameters.Add(new SqlParameter("@parameterName", SqlDbType.NVarChar, 20, "ParameterName"));
                adapterTabParam.InsertCommand.Parameters.Add(new SqlParameter("requiredValue", SqlDbType.Decimal, 0, "RequiredValue"));
                adapterTabParam.InsertCommand.Parameters.Add(new SqlParameter("@measuredValue", SqlDbType.Decimal, 0, "MeasuredValue"));
                SqlParameter parameterId = adapterTabParam.InsertCommand.Parameters.Add("@parameterId", SqlDbType.Int, 0, "ParameterId");
                parameterId.Direction = ParameterDirection.Output;

                //Заливаем таблицу "Параметров" из БД в DataTable и привязываем DataTable к DataGrid                 
                adapterTabParam.Fill(parametersTable);
                adapterTabParam.Fill(parametersLookTable);
                parametersLookTable.Rows.Clear();
                parametrGrid.ItemsSource = parametersLookTable.DefaultView;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDB(SqlDataAdapter adapter, DataTable dt)
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dt);
        }
        private string ReplaceDot(string strsource)
        {
            CultureInfo culture = new CultureInfo("ru-RU");
            if (strsource.Contains("."))
            {
                strsource = strsource.Replace(".", ",");
            }
            return strsource;
        }

        private int SearchId(int parametersId)
        {
            for (int i = 0; i < parametersTable.Rows.Count; i++)
            {
                if ((int)parametersTable.Rows[i]["ParameterId"] == parametersId)
                {
                    return i;
                }
            }
            return 0;
        }

        private void UpdateLookTabParam(int idSelTestsRow)
        {
            foreach (DataRow row in parametersTable.Rows)
            {
                if ((int)row["TestId"] == idSelTestsRow)
                {
                    object[] copyrow = row.ItemArray; // отвязываю строку от таблицы, переводя в массив объектов
                    parametersLookTable.Rows.Add(copyrow);
                }
            }
        }

        private DateTime SetActualDataTime (DateTime dateTime)
        {
            if (dateTime.Hour == 0)
            {
                dateTime = dateTime.AddHours(DateTime.Now.Hour);
                dateTime = dateTime.AddMinutes(DateTime.Now.Minute);
                dateTime = dateTime.AddSeconds(DateTime.Now.Second);
                return dateTime;
            }
            return dateTime;
        }

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(blockNameInsertTextBox.Text) || blockNameInsertTextBox.Text == "Обязательное поле")
            {
                blockNameInsertTextBox.Background = Brushes.IndianRed;
                blockNameInsertTextBox.FontSize = 10;
                blockNameInsertTextBox.Text = "Обязательное поле"; 

                return;
            }

            blockNameInsertTextBox.Background = Brushes.White;
            blockNameInsertTextBox.FontSize = 12;

            DataRow newRow = testsTable.NewRow();
            newRow["TestDate"] = SetActualDataTime(insertDataPicker.SelectedDate.Value);
            
            newRow["BlockName"] = blockNameInsertTextBox.Text;
            newRow["Note"] = noteInsertTextBox.Text;
            testsTable.Rows.Add(newRow);
            
            UpdateDB(adapterTabTest, testsTable);
            testsTable.Rows.Clear();
            adapterTabTest.Fill(testsTable);
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(blockNameChangeTextBox.Text) || blockNameChangeTextBox.Text == "Обязательное поле")
            {
                blockNameChangeTextBox.Background = Brushes.IndianRed;
                blockNameChangeTextBox.FontSize = 10;
                blockNameChangeTextBox.Text = "Обязательное поле";
                return;
            }

            if (testsGrid.SelectedItems == null) return;
            DataRowView datarowView = testsGrid.SelectedItem as DataRowView;
            if (datarowView == null) return;

            DataRow dataRow = (DataRow)datarowView.Row;
            dataRow["TestDate"] = SetActualDataTime(changeDataPicker.SelectedDate.Value);
            dataRow["BlockName"] = blockNameChangeTextBox.Text;
            dataRow["Note"] = noteChangeTextBox.Text;

            UpdateDB(adapterTabTest, testsTable);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (testsGrid.SelectedItems != null)
            {
                DataRowView datarowView = testsGrid.SelectedItem as DataRowView;
                if (datarowView != null)
                {
                    DataRow dataRow = (DataRow)datarowView.Row;
                    dataRow.Delete();
                }
            }
            
            refreshButton.IsEnabled = false;
            insertTabParamButton.IsEnabled = false;
            deleteTabParamButton.IsEnabled = false;

            UpdateDB(adapterTabTest, testsTable);
            parametersLookTable.Rows.Clear();
        }

        private void testsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (testsGrid.SelectedItems != null)
            {
                refreshButton.IsEnabled = true;
                insertTabParamButton.IsEnabled = true;
                deleteTabParamButton.IsEnabled = true;
                parameterNameChangeTextBox.Text = string.Empty;
                requiredValueChangeTextBox.Text = string.Empty;
                measuredValueChangeTextBox.Text = string.Empty;

                DataRowView datarowView = testsGrid.SelectedItem as DataRowView;
                if (datarowView != null)
                {
                    DataRow dataRow = (DataRow)datarowView.Row;
                    idSelTestsRow = (int)dataRow["TestId"];
                    changeDataPicker.SelectedDate = dataRow.Field<System.DateTime>("TestDate");
                    blockNameChangeTextBox.Text = dataRow["BlockName"].ToString();
                    noteChangeTextBox.Text = dataRow["Note"].ToString();
                    labelSelTest.Content = "Выбрана проверка: " + dataRow["BlockName"].ToString() + " от " + dataRow.Field<System.DateTime>("TestDate").ToString();

                    //Находим все параметры связанные с проверкой
                    parametersLookTable.Rows.Clear();
                    UpdateLookTabParam(idSelTestsRow);
                }
            }
        }

        private void insertTabParamButton_Click(object sender, RoutedEventArgs e)
        {
            decimal requiredValue = decimal.Zero, measuredValue = decimal.Zero;
            
            #region Проверка правильности заполнения полей
            if (string.IsNullOrEmpty(parameterNameInsertTextBox.Text) || parameterNameInsertTextBox.Text == "Обязательное поле")
            {
                parameterNameInsertTextBox.Background = Brushes.IndianRed;
                parameterNameInsertTextBox.FontSize = 10;
                parameterNameInsertTextBox.Text = "Обязательное поле";
                return;
            }

            parameterNameInsertTextBox.Background = Brushes.White;
            parameterNameInsertTextBox.FontSize = 12;

            if (string.IsNullOrEmpty(requiredValueInsertTextBox.Text) || requiredValueInsertTextBox.Text == "Обязательное поле")
            {
                requiredValueInsertTextBox.Background = Brushes.IndianRed;
                requiredValueInsertTextBox.FontSize = 10;
                requiredValueInsertTextBox.Text = "Обязательное поле";
                return;
            }

            if (!decimal.TryParse(ReplaceDot(requiredValueInsertTextBox.Text), out requiredValue))
            {
                requiredValueInsertTextBox.Background = Brushes.IndianRed;
                requiredValueInsertTextBox.FontSize = 10;
                requiredValueInsertTextBox.Text = "Необходимо число";
                return;
            }

            requiredValueInsertTextBox.Background = Brushes.White;
            requiredValueInsertTextBox.FontSize = 12;

            if (string.IsNullOrEmpty(measuredValueInsertTextBox.Text) || measuredValueInsertTextBox.Text == "Обязательное поле")
            {
                measuredValueInsertTextBox.Background = Brushes.IndianRed;
                measuredValueInsertTextBox.FontSize = 10;
                measuredValueInsertTextBox.Text = "Обязательное поле";
                return;
            }

            if (!decimal.TryParse(ReplaceDot(measuredValueInsertTextBox.Text), out measuredValue))
            {
                measuredValueInsertTextBox.Background = Brushes.IndianRed;
                measuredValueInsertTextBox.FontSize = 10;
                measuredValueInsertTextBox.Text = "Необходимо число";
                return;
            }

            measuredValueInsertTextBox.Background = Brushes.White;
            measuredValueInsertTextBox.FontSize = 12;
            #endregion

            DataRow newRow = parametersTable.NewRow();
            newRow["TestId"] = idSelTestsRow;
            newRow["ParameterName"] = parameterNameInsertTextBox.Text;
            newRow["RequiredValue"] = requiredValue;
            newRow["MeasuredValue"] = measuredValue;
            parametersTable.Rows.Add(newRow);

            UpdateDB(adapterTabParam, parametersTable);
            parametersTable.Rows.Clear();
            adapterTabParam.Fill(parametersTable);

            //Обновляем теблицу параметров
            parametersLookTable.Rows.Clear();
            UpdateLookTabParam(idSelTestsRow);
        }


        private void changeTabParamButton_Click(object sender, RoutedEventArgs e)
        {
            decimal requiredValue = decimal.Zero, measuredValue = decimal.Zero;
            #region Проверка правильности заполнения полей
            if (string.IsNullOrEmpty(parameterNameChangeTextBox.Text) || parameterNameChangeTextBox.Text == "Обязательное поле")
            {
                parameterNameChangeTextBox.Background = Brushes.IndianRed;
                parameterNameChangeTextBox.FontSize = 10;
                parameterNameChangeTextBox.Text = "Обязательное поле";
                return;
            }

            parameterNameChangeTextBox.Background = Brushes.White;
            parameterNameChangeTextBox.FontSize = 12;

            if (string.IsNullOrEmpty(requiredValueChangeTextBox.Text) || requiredValueChangeTextBox.Text == "Обязательное поле")
            {
                requiredValueChangeTextBox.Background = Brushes.IndianRed;
                requiredValueChangeTextBox.FontSize = 10;
                requiredValueChangeTextBox.Text = "Обязательное поле";
                return;
            }

            if (!decimal.TryParse(ReplaceDot(requiredValueChangeTextBox.Text), out requiredValue))
            {
                requiredValueChangeTextBox.Background = Brushes.IndianRed;
                requiredValueChangeTextBox.FontSize = 10;
                requiredValueChangeTextBox.Text = "Необходимо число";
                return;
            }

            requiredValueChangeTextBox.Background = Brushes.White;
            requiredValueChangeTextBox.FontSize = 12;

            if (string.IsNullOrEmpty(measuredValueChangeTextBox.Text) || measuredValueChangeTextBox.Text == "Обязательное поле")
            {
                measuredValueChangeTextBox.Background = Brushes.IndianRed;
                measuredValueChangeTextBox.FontSize = 10;
                measuredValueChangeTextBox.Text = "Обязательное поле";
                return;
            }

            if (!decimal.TryParse(ReplaceDot(measuredValueChangeTextBox.Text), out measuredValue))
            {
                measuredValueChangeTextBox.Background = Brushes.IndianRed;
                measuredValueChangeTextBox.FontSize = 10;
                measuredValueChangeTextBox.Text = "Необходимо число";
                return;
            }

            measuredValueChangeTextBox.Background = Brushes.White;
            measuredValueChangeTextBox.FontSize = 12;
            #endregion

            if (parametrGrid.SelectedItems == null) return;
            DataRowView datarowView = parametrGrid.SelectedItem as DataRowView;
            if (datarowView == null) return;

            DataRow dataRow = (DataRow)datarowView.Row;
            idSelParamRow = SearchId((int)dataRow["ParameterId"]);

            parametersTable.Rows[idSelParamRow]["ParameterName"] = parameterNameChangeTextBox.Text;
            parametersTable.Rows[idSelParamRow]["RequiredValue"] = requiredValue;
            parametersTable.Rows[idSelParamRow]["MeasuredValue"] = measuredValue;

            UpdateDB(adapterTabParam, parametersTable);
            parametersTable.Rows.Clear();
            adapterTabParam.Fill(parametersTable);

            //Обновляем теблицу параметров
            parametersLookTable.Rows.Clear();
            UpdateLookTabParam(idSelTestsRow);
        }
        private void deleteTabParamButton_Click(object sender, RoutedEventArgs e)
        {
            changeTabParamButton.IsEnabled = false;

            if (parametrGrid.SelectedItems != null)
            {
                DataRowView datarowView = parametrGrid.SelectedItem as DataRowView;
                if (datarowView != null)
                {
                    DataRow dataRow = (DataRow)datarowView.Row;
                    idSelParamRow = SearchId((int)dataRow["ParameterId"]);

                    DataRow delRow = parametersTable.Rows[idSelParamRow];
                    delRow.Delete();

                    UpdateDB(adapterTabParam, parametersTable);
                    parametersTable.Rows.Clear();
                    adapterTabParam.Fill(parametersTable);

                    //Обновляем теблицу параметров
                    parametersLookTable.Rows.Clear();
                    UpdateLookTabParam(idSelTestsRow);
                }
            }
            UpdateDB(adapterTabTest, testsTable);
        }

        private void parametersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (parametrGrid.SelectedItems != null)
            {
                changeTabParamButton.IsEnabled = true;
                DataRowView datarowView = parametrGrid.SelectedItem as DataRowView;
                if (datarowView != null)
                {
                    DataRow dataRow = (DataRow)datarowView.Row;
                    
                    parameterNameChangeTextBox.Text = dataRow["ParameterName"].ToString();
                    requiredValueChangeTextBox.Text = dataRow["RequiredValue"].ToString();
                    measuredValueChangeTextBox.Text = dataRow["MeasuredValue"].ToString();
                }
            }
        }
    }
}
