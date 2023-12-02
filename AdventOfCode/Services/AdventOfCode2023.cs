using AdventOfCode.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode.Services
{
    public class AdventOfCode2023
    {
        public AdventOfCode2023()
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
            var watch = new Stopwatch();
            if (inputMethod != null)
            {
                if (puzzle1Method != null)
                {
                    var input = inputMethod.Invoke(this, new object[] { inputPath });
                    watch.Start();
                    var puzzleResult = puzzle1Method.Invoke(this, new object[] { input });
                    watch.Stop();

                    result.Add($"Puzzle 1: {puzzleResult} ({watch.ElapsedMilliseconds} ms)");
                }
                else
                {
                    result.Add("Puzzle 1: not implemented");
                }
                if (puzzle2Method != null)
                {
                    var input = inputMethod.Invoke(this, new object[] { inputPath });
                    watch.Restart();
                    var puzzleResult = puzzle2Method.Invoke(this, new object[] { input });
                    watch.Stop();
                    result.Add($"Puzzle 2: {puzzleResult} ({watch.ElapsedMilliseconds} ms)");
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

        #region Day 01 
        public List<string> ConvertDay1Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).ToList();
        }

        public int Day1_PuzzleOne(List<string> input)
        {
            return input.Sum(x => ExtractDigits(x, false));
        }

        public int Day1_PuzzleTwo(List<string> input)
        {
            return input.Sum(x => ExtractDigits(x, true));
        }

        public int ExtractDigits(string input, bool includeWrittenDigits)
        {
            var digits = new List<int>();

            var digitMap = new Dictionary<string, int>()
            {
                {"one", 1 },
                {"two", 2 },
                {"three", 3 },
                {"four", 4 },
                {"five", 5 },
                {"six", 6 },
                {"seven", 7 },
                {"eight", 8 },
                {"nine", 9 },
            };

            for(var i = 0; i < input.Length;  i++)
            {
                if(int.TryParse(input[i].ToString(), out int val))
                {
                    digits.Add(val);
                }
                else if (includeWrittenDigits)
                {
                    foreach(var digitMapping in digitMap.Where(x => x.Key.StartsWith(input[i])))
                    {
                        if(input.Length >= i + digitMapping.Key.Length && input.Substring(i, digitMapping.Key.Length) == digitMapping.Key)
                        {
                            digits.Add(digitMapping.Value);
                        }

                    }
                }
            }

            return 10 * digits.First() + digits.Last();
        }

        #endregion
    }
}

