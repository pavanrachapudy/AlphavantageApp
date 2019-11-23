using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntappCodingApp
{
    class Equity
    {
        public DateTime Date { get; set; }
        public double OpenValue { get; set; }
        public double HighValue { get; set; }
        public double LowValue { get; set; }
        public double CloseValue { get; set; }
        public double Volume { get; set; }

        public double DifferenceValue { get; set; }

        public static List<Equity> GetEquityData(string symbol, string sizeType = "compact")
        {
            var client = new RestClient("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + symbol + "&apikey=V8Z0A2GML8HOV6U9");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            Dictionary<string, string> response = client.Execute<Dictionary<string, string>>(request).Data;
            var equityInfo = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(response["Time Series (Daily)"]);
            List<Equity> lstEquity = new List<Equity>();
            foreach (var record in equityInfo)
            {
                Equity objEquity = new Equity();
                objEquity.Date = Convert.ToDateTime(record.Key);
                foreach (var info in record.Value)
                {
                    if (info.Key.Contains("open"))
                    {
                        objEquity.OpenValue = Convert.ToDouble(info.Value);
                    }
                    else if (info.Key.Contains("close"))
                    {
                        objEquity.CloseValue = Convert.ToDouble(info.Value);
                    }
                    else if (info.Key.Contains("low"))
                    {
                        objEquity.LowValue = Convert.ToDouble(info.Value);
                    }
                    else if (info.Key.Contains("high"))
                    {
                        objEquity.HighValue = Convert.ToDouble(info.Value);
                    }
                    else if (info.Key.Contains("volume"))
                    {
                        objEquity.Volume = Convert.ToDouble(info.Value);
                    }
                    objEquity.DifferenceValue = objEquity.OpenValue - objEquity.CloseValue;
                }
                lstEquity.Add(objEquity);
            }
            return lstEquity;
        }

        /// <summary>
        /// Assuming return is difference between close price of stock on last day and open price of stock on first day
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public static string GetLargestReturn(string symbols)
        {
            string[] arrSymbols = symbols.Split(',');
            string returnSymbol = "";
            double returnValue = 0;
            foreach (var symbol in arrSymbols)
            {
                List<Equity> lstEquity = GetEquityData(symbol).Where(i => i.Date >= (DateTime.Now.AddMonths(-1))).ToList();
                double tempValue = lstEquity.OrderByDescending(i => i.Date).First().CloseValue - lstEquity.OrderBy(i => i.Date).First().OpenValue;
                if (tempValue > returnValue)
                {
                    returnValue = tempValue;
                    returnSymbol = symbol;
                }

            }
            return returnSymbol;
        }
    }
}
