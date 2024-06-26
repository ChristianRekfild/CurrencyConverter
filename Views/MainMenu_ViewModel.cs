﻿using CurrencyConverter.Commands;
using CurrencyConverter.Infrastructure.Commands.Base;
using CurrencyConverter.Models;
using CurrencyConverter.Services;
using Fitness.Views.Base;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Windows.Input;

namespace CurrencyConverter.Views
{
    public class MainMenu_ViewModel : ViewModelBase
    {
        #region Свойства
        private string _title = "Котировки";
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        private List<CurrencyData> _currencyData;
        /// <summary>
        /// Лист с котировками валют
        /// </summary>
        public List<CurrencyData> CurrencyData
        {
            get => _currencyData;
            set => SetValue(ref _currencyData, value);
        }

        private List<string> _currencyСodes = new List<string>();
        /// <summary>
        /// Коды валют
        /// </summary>
        public List<string> Сodes
        {
            get => _currencyСodes;
            set => SetValue(ref _currencyСodes, value);
        }

        private string _currencySearchName = string.Empty;
        /// <summary>
        /// Строка для поиска валюты
        /// </summary>
        public string CurrencySearchName
        {
            get => _currencySearchName;
            set => SetValue(ref _currencySearchName, value);
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set => SetValue(ref _date, value);
        }

        private List<string> _firstSelectedVal;
        /// <summary>
        /// Первая (верхняя) выбранная валюта
        /// </summary>
        public List<string> FirstSelectedVal
        {
            get => _firstSelectedVal;
            set => SetValue(ref _firstSelectedVal, value);
        }

        private List<string> _secondSelectedVal;
        /// <summary>
        /// Вторая (нижняя) выбранная валютс
        /// </summary>
        public List<string> SecondSelectedVal
        {
            get => _secondSelectedVal;
            set => SetValue(ref _secondSelectedVal, value);
        }

        #endregion

        #region Команды
        /// <summary>Команда загрузки данных</summary>
        private ICommand _loadCommand;

        /// <summary>Команда загрузки данных</summary>
        public ICommand LoadCommand => _loadCommand
            ??= new MyCommand(OnLoadCommandExecuted, CanLoadCommandExecute);

        /// <summary>Проверка возможности выполнения - Команда загрузки данных</summary>
        private bool CanLoadCommandExecute(object p) => true;

        /// <summary>Логика выполнения - Команда загрузки данных</summary>
        private async void OnLoadCommandExecuted(object p)
        {
            CurrencyData = await LoadData().ConfigureAwait(false);

            FirstSelectedVal  = CurrencyData.Select(x => x.CharCode).ToList();
            SecondSelectedVal = CurrencyData.Select(x => x.CharCode).ToList();
        }
        #endregion


        /// <summary>
        /// Загружает данные с сайта ЦБ на указанную дату
        /// </summary>
        /// <returns></returns>
        private async Task<List<CurrencyData>> LoadData()
        {
            #region Команда загрузки данных
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string xmlDowloadDir = $"{appDir}\\Downloads";
            if (!Directory.Exists(xmlDowloadDir))
                Directory.CreateDirectory(xmlDowloadDir);

            //string url = $"https://www.cbr.ru/scripts/XML_daily.asp?date_req=25/05/2023";
            string url = $"https://www.cbr.ru/scripts/XML_daily.asp?date_req={Date.ToString("dd")}/{Date.ToString("MM")}/{Date.ToString("yyyy")}";

            List<CurrencyData> currencyDatas = null;

            using (var client = new HttpClient())
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                client
                    .DefaultRequestHeaders
                    .Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Xml));
                client.Timeout = TimeSpan.FromSeconds(500);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var responseContentBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                    var enc1251 = CodePagesEncodingProvider.Instance.GetEncoding("windows-1251");
                    string responseContent = enc1251.GetString(responseContentBytes);

                    if (string.IsNullOrWhiteSpace(responseContent) == false)
                    {
                        using (StreamWriter writer = new StreamWriter($"{xmlDowloadDir}\\{Date.ToString("dd")}_{Date.ToString("MM")}_{Date.ToString("yyyy")}.xml", false, enc1251))
                        {
                            writer.Write(responseContent);
                        }
                    }

                    XMLDataParser XmlParser = new XMLDataParser($"{xmlDowloadDir}\\{Date.ToString("dd")}_{Date.ToString("MM")}_{Date.ToString("yyyy")}.xml");
                    currencyDatas = XmlParser.LoadData();


                    //int i = 0;
                    //i++;

                }
            }
            return currencyDatas;

            #endregion
        }

    }
}
