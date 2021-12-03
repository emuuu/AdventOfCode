using System;

namespace AdventOfCode.Extensions
{
    public static class StringExtensions
    {
        public static int BinaryToInt(this string binaryString)
        {
            var result = 0;

            if (string.IsNullOrEmpty(binaryString))
                return result;

            for(var i = 0; i < binaryString.Length; i++)
            {
                if (binaryString[i] == '1')
                    result += (int)Math.Pow(2, binaryString.Length - 1 - i);
            }
            return result;
        }
    }
    
}
