using Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AsyncTask
    {
        static string names = "";
        private static readonly HttpClient client = new HttpClient();
        private const string access_key = "99a6f4202aa868d7538bc3f7eb0c7e36";


        public async Task UpdateCurrenciesDictionary(Dictionary<string, Coin> dictionary)
        {

            #region GET
            string currencies = await client.GetStringAsync("http://apilayer.net/api/live?access_key=" + access_key + "&source=USD&format=1");
            if (names == "")
                names = await client.GetStringAsync("http://apilayer.net/api/list?access_key=" + access_key + "&&format=1");
            #endregion

            Console.WriteLine("in async");
            JObject currenciesJson = JObject.Parse(currencies);
            JObject namesJson = JObject.Parse(names);
            dictionary.Clear();
            List<Coin> Currencies = new List<Coin>();

            foreach (var item in new List<string> { "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BRL", "BSD", "BTC", "BTN", "BWP", "BYN", "BYR", "BZD", "CAD", "CDF", "CHF", "CLF", "CLP", "CNY", "COP", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ERN", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GGP", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "IMP", "INR", "IQD", "IRR", "ISK", "JEP", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XCD", "XDR", "XOF", "XPF", "YER", "ZAR", "ZMK", "ZMW", "ZWL" })
            {
                string value = (((JObject)currenciesJson.GetValue("quotes")).GetValue("USD" + item)).ToString();
                string name = (((JObject)namesJson.GetValue("currencies")).GetValue(item)).ToString();
                Coin tmp = new Coin(decimal.Parse(value, NumberStyles.Float), name, item);
                dictionary.Add(item, tmp);
                Currencies.Add(tmp);

            }

            using (DBmanager context = new DBmanager())
            {
                context.Coins.AddRange(Currencies);
                await context.SaveChangesAsync();
            }

        }

        public async Task GetCurrenciesDictionaryByDate(Dictionary<string, HistoricalCoin> dictionary, DateTime dateTime)
        {
            try
            {
                string date = correctDate(dateTime);

                #region Check if record already exist

                using (var ctx = new DBmanager())
                {
                    var query = ctx.HistoricalCoins.Where(s => s.DateTime.Equals(dateTime)).FirstOrDefault<HistoricalCoin>();
                    if (query != null)
                    {
                        Console.WriteLine("this record is already exist");
                        return;
                    }
                }

                #endregion

                #region GET
                string responseString = await client.GetStringAsync("http://apilayer.net/api/historical?access_key=" + access_key + "&date=" + date + "&source=USD&format=1");
                if (names == "")
                    names = await client.GetStringAsync("http://apilayer.net/api/list?access_key=" + access_key + "&&format=1");
                #endregion

                Console.WriteLine("in async");
                JObject jObject = JObject.Parse(responseString);
                JObject namesJson = JObject.Parse(names);
                dictionary.Clear();
                List<HistoricalCoin> Currencies = new List<HistoricalCoin>();
                string timestamp = (((JObject)jObject).GetValue("timestamp")).ToString();

                foreach (var item in new List<string> { "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BRL", "BSD", "BTC", "BTN", "BWP", "BYN", "BYR", "BZD", "CAD", "CDF", "CHF", "CLF", "CLP", "CNY", "COP", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ERN", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GGP", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "IMP", "INR", "IQD", "IRR", "ISK", "JEP", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XCD", "XDR", "XOF", "XPF", "YER", "ZAR", "ZMK", "ZMW", "ZWL" })
                {
                    string name = (((JObject)namesJson.GetValue("currencies")).GetValue(item)).ToString();
                    try
                    {
                        string value = (((JObject)jObject.GetValue("quotes")).GetValue("USD" + item)).ToString();
                        HistoricalCoin tmp = new HistoricalCoin(decimal.Parse(value, NumberStyles.Float), name, item, dateTime);
                        dictionary.Add(item, tmp);
                        Currencies.Add(tmp);
                    }
                    catch (Exception)
                    {
                        dictionary.Add(item, new HistoricalCoin(-1, name, item, dateTime));
                    }
                }

                using (DBmanager context = new DBmanager())
                {

                    context.HistoricalCoins.AddRange(Currencies);
                    await context.SaveChangesAsync();
                }

                Console.WriteLine("this record added succesfully exist:    " + date);
                Console.WriteLine(DateTime.Now);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public async Task LoadLiveCurrencies(List<Coin> coins)
        {
            using (var ctx = new DBmanager())
            {
                List<Coin> list = await ctx.Coins.ToListAsync();
                foreach (var item in list)
                {
                    coins.Add(item);
                }
            }
        }

        private string correctDate(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;

            string nyear = year.ToString();
            string nmonth = month.ToString();
            string nday = day.ToString();

            if (year < 1999)
                throw new Exception("There is no record in this date, the first record is from 1999/01/01");
            else
            if (month < 10)
                nmonth = "0" + nmonth;
            if (day < 10)
                nday = "0" + nday;
            return nyear + "-" + nmonth + "-" + nday;
        }




        public List<HistoricalCoin> LoadSpecificCoinData(string CoinName)
        {
            DBmanager ctx = new DBmanager();
            List<HistoricalCoin> query = ctx.HistoricalCoins.Where(s => s.ShortName == CoinName).ToList<HistoricalCoin>();
            return query;

        }

        public List<HistoricalCoin>[] LoadTwoCoinsData(string firstCoin, string secondCoin, DateTime start, DateTime end)
        {
            List<HistoricalCoin>[] query = new List<HistoricalCoin>[2];

            using (DBmanager ctx = new DBmanager())
            {
                query[0] = ctx.HistoricalCoins.Where(s => s.ShortName == firstCoin && s.DateTime >= start && s.DateTime <= end).ToList<HistoricalCoin>();
                query[1] = ctx.HistoricalCoins.Where(s => s.ShortName == secondCoin && s.DateTime >= start && s.DateTime <= end).ToList<HistoricalCoin>();
            }

            DateTime current = start;
            Dictionary<string, HistoricalCoin> dictionary = new Dictionary<string, HistoricalCoin>();
            for (; current <= end; current = current.AddDays(1))
            {
                if (query[0].Find(x => x.DateTime == current) != null)
                    continue;
                else
                {
                    GetCurrenciesDictionaryByDate(dictionary, current).Wait();
                    query[0].Add(dictionary[firstCoin]);
                    query[1].Add(dictionary[secondCoin]);
                }
            }
            return query;
        }

        public List<HistoricalCoin>[] LoadTwoCoinsData(string firstCoin, string secondCoin)
        {
            List<HistoricalCoin>[] query = new List<HistoricalCoin>[2];

            using (DBmanager ctx = new DBmanager())
            {
                query[0] = ctx.HistoricalCoins.Where(s => s.ShortName == firstCoin).ToList<HistoricalCoin>();
                query[1] = ctx.HistoricalCoins.Where(s => s.ShortName == secondCoin).ToList<HistoricalCoin>();
            }
            return query;
        }



        public void LoadTwoCoinsRelativelyDataInPeriod(Dictionary<DateTime, decimal> dict, string firstCoin, string secondCoin, DateTime start, DateTime end)
        {
            List<HistoricalCoin>[] list = LoadTwoCoinsData(firstCoin, secondCoin, start, end);

            var zipped = list[0].Zip(list[1], (a, b) => new { a, b });
            foreach (var item in zipped)
            {
                decimal ratio = item.a.Value / item.b.Value;
                dict.Add(item.a.DateTime, ratio);
            }
        }
        public void LoadTwoCoinsRelativelyData(Dictionary<DateTime, decimal> dict, string firstCoin, string secondCoin)
        {
            List<HistoricalCoin>[] list = LoadTwoCoinsData(firstCoin, secondCoin);

            var zipped = list[0].Zip(list[1], (a, b) => new { a, b });
            foreach (var item in zipped)
            {
                decimal ratio = item.a.Value / item.b.Value;
                dict.Add(item.a.DateTime, ratio);
            }
        }

    }


}
