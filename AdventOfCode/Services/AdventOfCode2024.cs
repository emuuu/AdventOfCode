using AdventOfCode.Extensions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode.Services
{
    public class AdventOfCode2024
    {
        public AdventOfCode2024()
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
        public List<int[]> ConvertDay1Input(string inputPath)
        {
            List<int[]> result = new List<int[]>();
            result.Add(File.ReadAllLines(inputPath).Select(x => int.Parse(x.Split("   ")[0])).ToArray());
            result.Add(File.ReadAllLines(inputPath).Select(x => int.Parse(x.Split("   ")[1])).ToArray());
            return result;
        }

        public int Day1_PuzzleOne(List<int[]> input)
        {
            input[0] = input[0].OrderBy(x => x).ToArray();
            input[1] = input[1].OrderBy(x => x).ToArray();

            return (int)Enumerable.Range(0, input[0].Length).Sum(x => Math.Abs(input[0][x] - input[1][x]));
        }

        public int Day1_PuzzleTwo(List<int[]> input)
        {
            input[0] = input[0].OrderBy(x => x).ToArray();
            input[1] = input[1].OrderBy(x => x).ToArray();

            return (int)Enumerable.Range(0, input[0].Length).Sum(x => input[0][x] * input[1].Count(y => y == input[0][x]));
        }


        #endregion
    }
}

