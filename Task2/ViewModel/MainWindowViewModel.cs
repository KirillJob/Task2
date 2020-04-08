using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Task2.Model;

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



    }
}
