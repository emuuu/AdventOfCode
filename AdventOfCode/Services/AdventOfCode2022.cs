﻿using AdventOfCode.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services
{
    public class AdventOfCode2022
    {
        public AdventOfCode2022()
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
        public List<List<int>> ConvertDay1Input(string inputPath)
        {
            return File.ReadAllText(inputPath).Split("\r\n\r\n").Select(x => x.Split("\r\n").Select(y => int.Parse(y)).ToList()).ToList();
        }

        public int Day1_PuzzleOne(List<List<int>> input)
        {
            var list = input.Select(y => (Sum: y.Sum(), Items: y)).OrderByDescending(y => y.Sum).ToList();
            return list.First().Sum;
        }

        public int Day1_PuzzleTwo(List<List<int>> input)
        {
            var list = input.Select(y => (Sum: y.Sum(), Items: y)).OrderByDescending(y => y.Sum).ToList();
            return list.Select(x => x.Sum).Take(3).Sum();
        }

        #endregion

    }
}

