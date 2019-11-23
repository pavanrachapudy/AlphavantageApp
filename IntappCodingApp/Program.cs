using System;
using System.Collections.Generic;
using System.Linq;

namespace IntappCodingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Average volume of MSFT in the past 7 days is : " + GetAverageVolume("MSFT"));
            Console.WriteLine("Highest closing price of AAPL in the past 6 months is : " + HighestClosingPrice("AAPL"));
            Console.WriteLine("Difference between open and close price for BA for every day in the last month is : ");
            foreach(Equity record in OpenCloseDifference("BA"))
            {
                Console.WriteLine("Date : " + record.Date.ToShortDateString() + " and Difference is " + record.DifferenceValue.ToString());
            }
            Console.Write("Enter stock symbols seperated by comma : ");
            string input = Console.ReadLine();
            Console.WriteLine("Symbol with the largest return over the past month is : " + LargestReturn(input));
            Console.ReadLine();
        }

        /// <summary>
        /// Average volume of specified symbol in the past 7 days
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string GetAverageVolume(string symbol)
        {
            List<Equity> lstEquity = Equity.GetEquityData(symbol);
            return lstEquity.Where(i => i.Date >= (DateTime.Now.AddDays(-7))).Average(i => i.Volume).ToString();
        }

        /// <summary>
        /// Highest closing price of specified symbol in the past 6 months
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string HighestClosingPrice(string symbol)
        {
            List<Equity> lstEquity = Equity.GetEquityData(symbol, "full");
            return lstEquity.Where(i => i.Date >= (DateTime.Now.AddMonths(-6))).Select(i => i.CloseValue).Max().ToString();
        }

        /// <summary>
        /// Difference between open and close price for specified symbol for every day in the last month
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static List<Equity> OpenCloseDifference(string symbol)
        {
            List<Equity> lstEquity = Equity.GetEquityData(symbol);
            return lstEquity.Where(i => i.Date.Month == DateTime.Now.Month - 1).Select(i => new Equity { DifferenceValue = i.DifferenceValue, Date = i.Date }).ToList();
        }

        /// <summary>
        /// Symbol with the largest return over the past month
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public static string LargestReturn(string symbols)
        {
            return Equity.GetLargestReturn(symbols);
        }
    }
}
