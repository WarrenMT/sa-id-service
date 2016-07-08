using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SouthAfricanIDWebApi.Models
{
    public class SAIdentification
    {

        private static Random random = new Random();

        public string IdNumber { get; set; }
        public int SumOddDigits { get; set; }
        public int SumOfEvenPositions { get; set; }
        public int TotalOfEvenPositionDigits { get; set; }
        public int LastDigit { get; set; }
        public string TotalOfEvenAndOddPositionDigits { get; set; }
        public bool Valid { get; set; }

        public static string generateDateSection()
        {
            string year = random.Next(0, 100).ToString().PadLeft(2, '0');
            string month = random.Next(1, 13).ToString().PadLeft(2, '0');
            string day = random.Next(1, 32).ToString().PadLeft(2, '0');

            string date = year + month + day;

            return date;
        }

        public static string generateGenderSection()
        {
            return random.Next(0, 10000).ToString();
        }

        public static string generateCountryIdSection()
        {
            return random.Next(0, 2).ToString();
        }

        public static string generatePreviouslyRaceSection()
        {
            return random.Next(8, 10).ToString();
        }

        public static string generateLastDigit()
        {
            return random.Next(0, 10).ToString();
        }

        public void verify(bool regenerateControlBit)
        {
            string numFromEvenPositionNumbers = "", digit = "", sumOfEvenPositionsStr = "";

            for (int i = 0; i < IdNumber.Length - 1; i++)
            {
                digit = IdNumber.Substring(i, 1);

                if ( (i % 2) == 0 )
                {
                    SumOddDigits += Int32.Parse(digit);
                }
                else
                {
                    numFromEvenPositionNumbers += digit;
                    
                }
            }

            SumOfEvenPositions = Int32.Parse(numFromEvenPositionNumbers) * 2;
            sumOfEvenPositionsStr = SumOfEvenPositions.ToString();

            foreach (var currentDigit in sumOfEvenPositionsStr)
            {
                TotalOfEvenPositionDigits += Int32.Parse(currentDigit.ToString());
            }
            
            TotalOfEvenAndOddPositionDigits = (SumOddDigits + TotalOfEvenPositionDigits).ToString();

            LastDigit = Int32.Parse(TotalOfEvenAndOddPositionDigits.Substring(TotalOfEvenAndOddPositionDigits.Length - 1, 1));

            string sumOfTenMinusLastDigit = (10 - LastDigit).ToString();
            string lastDigitOfSumOfTenMinusLastDigit = sumOfTenMinusLastDigit.Substring(sumOfTenMinusLastDigit.Length - 1, 1);

            if (regenerateControlBit)
            {
                IdNumber = IdNumber.Substring(0, 12) + lastDigitOfSumOfTenMinusLastDigit;
            }

            Valid = lastDigitOfSumOfTenMinusLastDigit == IdNumber.Substring(12, 1);
        }
    }
}