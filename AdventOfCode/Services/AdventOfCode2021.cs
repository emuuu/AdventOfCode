using AdventOfCode.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services
{
    public class AdventOfCode2021
    {
        public AdventOfCode2021()
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
        public List<int> ConvertDay1Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => int.Parse(x)).ToList();
        }

        public int Day1_PuzzleOne(List<int> input)
        {
            var filteredMeasurements = FilterMeasurements(input);
            return filteredMeasurements.Count;
        }

        public int Day1_PuzzleTwo(List<int> input)
        {
            var slidingWindows = CreateSlidingWindow(input);
            var filteredMeasurements = FilterMeasurements(slidingWindows);
            return filteredMeasurements.Count;
        }

        public List<int> FilterMeasurements(List<int> input)
        {
            var result = new List<int>();
            for (var i = 1; i < input.Count; i++)
            {
                if (input[i] > input[i - 1])
                {
                    result.Add(input[i]);
                }
            }
            return result;
        }
        public List<int> CreateSlidingWindow(List<int> input)
        {
            var result = new List<int>();
            for (var i = 2; i < input.Count; i++)
            {
                result.Add(input.Skip(i - 2).Take(3).Sum());
            }
            return result;
        }
        #endregion



    }
}

