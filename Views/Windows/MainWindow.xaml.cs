using CurrencyConverter.Models;
using CurrencyConverter.Services;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Text.Unicode;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            textbox_path.Text = AppDomain.CurrentDomain.BaseDirectory;

            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string xmlDowloadDir = $"{appDir}\\Downloads";
            if (!Directory.Exists(xmlDowloadDir))
                Directory.CreateDirectory(xmlDowloadDir);

            string url = "https://www.cbr.ru/scripts/XML_daily.asp?date_req=25/05/2023";

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
                        using (StreamWriter writer = new StreamWriter($"{xmlDowloadDir}\\25_05_2023.xml", false, enc1251))
                        {
                            writer.Write(responseContent);
                        }
                    }

                    XMLDataParser XmlParser = new XMLDataParser($"{xmlDowloadDir}\\25_05_2023.xml");
                    List<CurrencyData> currencyDatas = XmlParser.LoadData();


                    int i = 0;
                    i++;

                }
            }
        }
    }
}