using Fitness.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Views
{
    public class MainMenu_ViewModel : ViewModelBase
    {
        private string _title = "Конвертер валют";
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

    }
}
