using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Models
{
    public class CurrencyData
    {
        public string Id { get; set; }
        public string NumCode {  get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal VunitRate { get; set; } // Что это вообще такое??!

        // По идее тут можно было бы ещё прописать надбавки за обмен, но такого в ТЗ не было =)

        public override string ToString()
        {
            return $"{NumCode}{CharCode}{Nominal}{Value}";
        }
    }
}
