using CurrencyConverter.Models;
using System.Xml;
using System.Xml.Linq;

namespace CurrencyConverter.Services
{
    public class XMLDataParser
    {
        private string _pathToFile;

        public XMLDataParser(string path)
        {
            _pathToFile = path;
        }

        public List<CurrencyData> LoadData()
        {
            List<CurrencyData> listCurrencyDatas = new List<CurrencyData>();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(_pathToFile);
            XmlElement? xRoot = xDoc.DocumentElement;

            if (xRoot != null )
            {
                foreach (XmlNode node in xRoot)
                {
                    //if (node != null) continue;

                    XmlNode? IdAttr = node.Attributes.GetNamedItem("ID");
                    if (IdAttr is null) continue;

                    //< Valute ID = "R01010" >
                    //    < NumCode > 036 </ NumCode >
                    //    < CharCode > AUD </ CharCode >
                    //    < Nominal > 1 </ Nominal >
                    //    < Name > Австралийский доллар </ Name >
                    //    < Value > 53,3107 </ Value >
                    //    < VunitRate > 53,3107 </ VunitRate >
                    //</ Valute >

                    CurrencyData currencyData = new CurrencyData();
                    currencyData.Id = IdAttr.Value;

                    foreach (XmlNode innerNode in node.ChildNodes)
                    {
                        if (innerNode.Name == "NumCode")
                        {
                            currencyData.Id = innerNode.InnerText;
                        }

                        if (innerNode.Name == "CharCode")
                        {
                            currencyData.CharCode = innerNode.InnerText;
                        }

                        if (innerNode.Name == "Nominal")
                        {
                            currencyData.Nominal = int.Parse(innerNode.InnerText);
                        }

                        if (innerNode.Name == "Name")
                        {
                            currencyData.Name = innerNode.InnerText;
                        }

                        if (innerNode.Name == "Value")
                        {
                            currencyData.Value = decimal.Parse(innerNode.InnerText);
                        }

                        if (innerNode.Name == "VunitRate")
                        {
                            currencyData.VunitRate = decimal.Parse(innerNode.InnerText);
                        }

                        listCurrencyDatas.Add(currencyData);

                    }

                }

            }

            return listCurrencyDatas;   

        }
    }
}
