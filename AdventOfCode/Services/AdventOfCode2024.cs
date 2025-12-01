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

        #region Day 01 
        public Dictionary<int, int[]> ConvertDay2Input(string inputPath)
        {
            var lines = File.ReadAllLines(inputPath);
            return Enumerable.Range(0, lines.Length).ToDictionary(
                x => x,
                x => lines[x].Split(" ").Select(y => int.Parse(y)).ToArray());
        }

        public int Day2_PuzzleOne(Dictionary<int, int[]> input)
        {
            return input.Count(x=>ReportIsSafe(x.Value, false));
        }

        public int Day2_PuzzleTwo(Dictionary<int, int[]> input)
        {
            return input.Count(x => ReportIsSafe(x.Value, true));
        }

        bool ReportIsSafe(int[] report, bool useProblemDampener)
        {
            bool problemDampenerUsed =false;
            var ascendingMode = report[1] - report[0] > 0;
            if (Math.Abs(report[1] - report[0]) < 1 || Math.Abs(report[1] - report[0]) > 3)
            {
                if (useProblemDampener)
                {
                    problemDampenerUsed = true;
                }
                else
                {
                    return false;
                }
            }
            for (var i = 1; i < report.Length; i++)
            {
                var difference = report[i] - report[i - 1];
                var absDifference = Math.Abs(difference);
                if ((ascendingMode != difference > 0) || (absDifference < 1 || absDifference > 3))
                {
                    if (useProblemDampener)
                    {
                        if (problemDampenerUsed)
                        {
                            return false;
                        }
                        problemDampenerUsed = true;

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
    }
}

