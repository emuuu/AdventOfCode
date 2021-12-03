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

        #region Day 02
        public IEnumerable<string> ConvertDay2Input(string inputPath)
        {
            return File.ReadAllLines(inputPath);
        }

        public int Day2_PuzzleOne(IEnumerable<string> input)
        {
            var finalPosition = ProcessSubmarineCommands(input, false);
            return finalPosition.X * finalPosition.Y;
        }

        public int Day2_PuzzleTwo(IEnumerable<string> input)
        {
            var finalPosition = ProcessSubmarineCommands(input, true);
            return finalPosition.X * finalPosition.Y;
        }

        public (int X, int Y, int Aim) ProcessSubmarineCommands(IEnumerable<string> commands, bool aim)
        {
            var position = (X: 0, Y: 0, Aim: 0);
            foreach (var command in commands)
            {
                position = MoveSubmarine(position, command, aim);
            }
            return position;
        }

        public (int, int, int) MoveSubmarine((int X, int Y, int Aim) position, string command, bool aim)
        {
            if (int.TryParse(command.Split(' ').Last(), out int movementRange))
            {
                switch (command[0])
                {
                    case 'f':
                        {
                            position.X += movementRange;
                            if (aim)
                                position.Y += movementRange * position.Aim;
                            break;
                        }
                    case 'u':
                        {
                            if (aim)
                                position.Aim -= movementRange;
                            else
                                position.Y -= movementRange;

                            break;
                        }
                    case 'd':
                        {
                            if (aim)
                                position.Aim += movementRange;
                            else
                                position.Y += movementRange;

                            break;
                        }
                }
            }
            return position;
        }
        #endregion

        #region Day 03
        public IEnumerable<string> ConvertDay3Input(string inputPath)
        {
            return File.ReadAllLines(inputPath);
        }

        public int Day3_PuzzleOne(IEnumerable<string> input)
        {
            var report = CalculateGammaEpsilon(input);
            return report.Gamma * report.Epsilon;
        }

        public int Day3_PuzzleTwo(IEnumerable<string> input)
        {
            var report = CalculateOxygenCO2(input);
            return report.OxygenGeneratorRating * report.CO2ScrubberRating;
        }

        public (int Gamma, int Epsilon) CalculateGammaEpsilon(IEnumerable<string> input)
        {
            if (!input.Any())
                return default;

            var length = input.First().Length;
            var gamma = 0;
            var epsilon = 0;
            for (var i = 0; i < length; i++)
            {
                var avg = input.Select(x => int.Parse(x[i].ToString())).Average();
                gamma += (int)Math.Pow(2, length - 1 - i) * (avg > 0.5 ? 1 : 0);
                epsilon += (int)Math.Pow(2, length - 1 - i) * (avg < 0.5 ? 1 : 0);
            }
            return (Gamma: gamma, Epsilon: epsilon);
        }

        public (int OxygenGeneratorRating, int CO2ScrubberRating) CalculateOxygenCO2(IEnumerable<string> input)
        {
            if (!input.Any())
                return default;

            var length = input.First().Length;

            IEnumerable<string> oxygenReport = input.ToList();
            IEnumerable<string> co2Report = input.ToList();
            while (oxygenReport.Count() > 1 || co2Report.Count() > 1)
            {
                for (var i = 0; i < length; i++)
                {
                    if (oxygenReport.Count() > 1)
                    {
                        var oxygenAverage = oxygenReport.Select(x => int.Parse(x[i].ToString())).Average();
                        oxygenReport = oxygenReport.Where(x => x[i] == (oxygenAverage >= 0.5 ? '1' : '0')).ToList();
                    }

                    if (co2Report.Count() > 1)
                    {
                        var co2Average = co2Report.Select(x => int.Parse(x[i].ToString())).Average();
                        co2Report = co2Report.Where(x => x[i] == (co2Average >= 0.5 ? '0' : '1')).ToList();
                    }
                }
            }
            return (OxygenGeneratorRating: oxygenReport.First().BinaryToInt(), CO2ScrubberRating: co2Report.First().BinaryToInt());
        }
        #endregion
    }
}

