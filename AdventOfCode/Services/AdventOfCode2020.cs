using AdventOfCode.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode.Services
{
    public class AdventOfCode2020
    {
        public AdventOfCode2020()
        {

        }

        [MethodInList]
        public List<string> SolveDay(string inputPath, int day)
        {
            var type = this.GetType();
            var inputMethod = type.GetMethod("ConvertDay" + day + "Input");
            var puzzle1Method = type.GetMethod("Day" + day + "_PuzzleOne");
            var puzzle2Method = type.GetMethod("Day" + day + "_PuzzleTwo");

            var result = new List<string>() { "Day " + day };
            if (inputMethod != null)
            {
                if (puzzle1Method != null)
                {
                    var input = inputMethod.Invoke(this, new object[] { inputPath });
                    result.Add("Puzzle 1: " + puzzle1Method.Invoke(this, new object[] { input }));
                }
                else
                {
                    result.Add("Puzzle 1: not implemented");
                }
                if (puzzle2Method != null)
                {
                    var input = inputMethod.Invoke(this, new object[] { inputPath });
                    result.Add("Puzzle 2: " + puzzle2Method.Invoke(this, new object[] { input }));
                }
                else
                {
                    result.Add("Puzzle 2: not implemented");
                }
            }
            else
            {
                result.Add("Not implemented");
            }
            return result;
        }

        #region Day1 
        public int[] ConvertDay1Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => int.Parse(x)).ToArray();
        }

        public int Day1_PuzzleOne(int[] input)
        {
            return CalculateExpenses(input, false);
        }

        public int Day1_PuzzleTwo(int[] input)
        {
            return CalculateExpenses(input, true);
        }

        public int CalculateExpenses(int[] report, bool threeNumbers)
        {
            for (var i = 0; i < report.Length; i++)
            {
                for (var j = 0; j < report.Length; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    if (!threeNumbers)
                    {
                        if (report[i] + report[j] == 2020)
                        {
                            return report[i] * report[j];
                        }
                    }
                    else
                    {
                        for (var k = 0; k < report.Length; k++)
                        {
                            if (j == k || i == k)
                            {
                                continue;
                            }
                            if (report[i] + report[j] + report[k] == 2020)
                            {
                                return report[i] * report[j] * report[k];
                            }
                        }
                    }
                }
            }
            return 0;
        }
        #endregion

        #region Day2
        public string[] ConvertDay2Input(string inputPath)
        {
            return File.ReadAllLines(inputPath);
        }

        public int Day2_PuzzleOne(IEnumerable<string> input)
        {
            return input.Count(x => ValidPasswordPolicy(x.Split(':')[0], x.Split(':')[1].Trim(), false));
        }

        public int Day2_PuzzleTwo(IEnumerable<string> input)
        {
            return input.Count(x => ValidPasswordPolicy(x.Split(':')[0], x.Split(':')[1].Trim(), true));
        }

        public bool ValidPasswordPolicy(string policy, string password, bool tobogganPolicy)
        {
            var policySplit = policy.Split(' ');

            if (!int.TryParse(policySplit[0].Split('-')[0], out int min) || !int.TryParse(policySplit[0].Split('-')[1], out int max))
                throw new Exception("Could not parse password policy.");

            var validationChar = policySplit[1][0];
            if (!tobogganPolicy)
            {
                var charCount = password.Count(x => x == validationChar);
                return charCount >= min && charCount <= max;
            }
            else
            {
                min--;
                max--;
                return (password[min] == validationChar || password[max] == validationChar) && !(password[min] == validationChar && password[max] == validationChar);
            }
        }
       
        #endregion
    }
}
