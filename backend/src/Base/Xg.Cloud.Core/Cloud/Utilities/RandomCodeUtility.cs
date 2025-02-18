using System;
using System.Text;

namespace Cloud.Utilities
{
    public class RandomCodeUtility
    {
        private readonly static Random rnd = new();
        public static string GetRandomCode(int n)
        {
            var arrChar = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'A', 'C', 'D', 'E' };
            StringBuilder sb = new();
            for (int i = 0; i < n; i++)
            {
                sb.Append(arrChar[rnd.Next(0, arrChar.Length)]);
            }
            return sb.ToString();
        }

        public static int GetHundredNum() => rnd.Next(1, 100);

        public static int GetNum(int minValue, int maxValue) => rnd.Next(minValue, maxValue);
    }
}
