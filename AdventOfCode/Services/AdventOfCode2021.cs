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

        #region Day 04
        public (List<int> Numbers, List<(int BoardID, List<List<int>> BoardNumbers)> Boards) ConvertDay4Input(string inputPath)
        {
            var lines = File.ReadAllLines(inputPath).ToList();
            var chosenNumbers = lines.First().Split(',').Select(x => int.Parse(x)).ToList();
            lines = lines.Skip(1).ToList();

            var boards = new List<(int BoardID, List<List<int>> BoardNumbers)>();
            for (var i = 0; i < lines.Count / 6; i++)
            {
                var horizontal = lines.Skip(i * 6 + 1).Take(5).Select(x => Enumerable.Range(0, 5).Select(y => int.Parse(new String(x.Skip(3 * y).Take(2).ToArray()))).ToList()).ToList();
                var vertical = Enumerable.Range(0, 5).Select(x => horizontal.Select(y => y[x]).ToList()).ToList();
                boards.Add((BoardID: i + 1, BoardNumbers: horizontal.Concat(vertical).ToList()));
            }
            return (chosenNumbers, boards);
        }

        public int Day4_PuzzleOne((List<int> Numbers, List<(int BoardID, List<List<int>> BoardNumbers)> Boards) input)
        {
            return RunBingoGame(input.Numbers, input.Boards, false);
        }

        public int Day4_PuzzleTwo((List<int> Numbers, List<(int BoardID, List<List<int>> BoardNumbers)> Boards) input)
        {
            return RunBingoGame(input.Numbers, input.Boards, true);
        }

        public int RunBingoGame(IEnumerable<int> chosenNumbers, List<(int BoardID, List<List<int>> BoardNumbers)> boards, bool letSquidWin)
        {
            foreach (var number in chosenNumbers)
            {
                boards.ForEach(x => x.BoardNumbers.ForEach(y => y.RemoveAll(z => z == number)));
                if (boards.Any(x => x.BoardNumbers.Any(y => y.Count == 0)))
                {
                    var winnerBoard = boards.First(x => x.BoardNumbers.Any(y => y.Count == 0));

                    if (letSquidWin)
                    {
                        boards.RemoveAll(x => boards.Where(y => y.BoardNumbers.Any(z => z.Count == 0)).Select(y => y.BoardID).Contains(x.BoardID));
                    }
                    if (!letSquidWin || boards.Count == 0)
                    {
                        return winnerBoard.BoardNumbers.SelectMany(x => x).Sum() / 2 * number;
                    }
                }
            }
            return default;
        }
        #endregion

        #region Day 05
        public List<(int x1, int y1, int x2, int y2)> ConvertDay5Input(string inputPath)
        {
            return ReadPoints(File.ReadAllLines(inputPath));
        }

        public int Day5_PuzzleOne(List<(int x1, int y1, int x2, int y2)> lines)
        {
            return IntersectLines(lines, false);
        }

        public int Day5_PuzzleTwo(List<(int x1, int y1, int x2, int y2)> lines)
        {
            return IntersectLines(lines, true);
        }


        public List<(int x1, int y1, int x2, int y2)> ReadPoints(IEnumerable<string> input)
        {
            return input.Select(x => x.Split(" -> ")).Select(y => (x1: int.Parse(y[0].Split(',')[0]), y1: int.Parse(y[0].Split(',')[1]), x2: int.Parse(y[1].Split(',')[0]), y2: int.Parse(y[1].Split(',')[1]))).ToList();

        }
        public int IntersectLines(List<(int x1, int y1, int x2, int y2)> lines, bool diagonals)
        {
            var map = new Dictionary<(int x, int y), int>();
            foreach (var line in lines)
            {
                if (diagonals || line.x2 == line.x1 || line.y1 == line.y2)
                {
                    var stepx = line.x1 == line.x2 ? 0 : (line.x1 < line.x2 ? 1 : -1);
                    var endx = line.x2 + (stepx == 0 ? 1 : stepx);

                    var stepy = line.y1 == line.y2 ? 0 : (line.y1 < line.y2 ? 1 : -1);
                    var endy = line.y2 + (stepy == 0 ? 1 : stepy);

                    for (int x = line.x1, y = line.y1; x != endx && y != endy; x += stepx, y += stepy)
                    {
                        var pos = (x, y);
                        map.TryGetValue((x, y), out var value);
                        map[pos] = value + 1;
                    }
                }
            }

            return map.Count(x => x.Value > 1);
        }
        #endregion

        #region Day 06
        public List<long> ConvertDay6Input(string inputPath)
        {
            var fishMaturity = new List<long>();
            var fishAges = File.ReadAllText(inputPath).Split(',').Select(x => long.Parse(x));
            for (var i = 0; i <= 8; i++)
            {
                fishMaturity.Add(fishAges.Count(x => x == i));
            }
            return fishMaturity;
        }

        public long Day6_PuzzleOne(List<long> fishMaturity)
        {
            return ModelLanternFishPopulation(80, fishMaturity);
        }

        public long Day6_PuzzleTwo(List<long> fishMaturity)
        {
            return ModelLanternFishPopulation(256, fishMaturity);
        }

        public long ModelLanternFishPopulation(int days, List<long> fishMaturity)
        {
            for (var i = 0; i < days; i++)
            {
                fishMaturity[7] += fishMaturity[0];
                fishMaturity.Add(fishMaturity[0]);
                fishMaturity.RemoveAt(0);
            }

            return fishMaturity.Sum();
        }
        #endregion

        #region Day 07
        public List<int> ConvertDay7Input(string inputPath)
        {
            return File.ReadAllText(inputPath).Split(',').Select(x=>int.Parse(x)).ToList();
        }

        public int Day7_PuzzleOne(List<int> crabPositions)
        {
            var geometricMean = CalculateGeometricMean(crabPositions);
            return CalculateFuelConsumption(geometricMean, crabPositions, false);
        }

        public int Day7_PuzzleTwo(List<int> crabPositions)
        {
            var centerOfMass = CalculateCenterOfMass(crabPositions);
            return CalculateFuelConsumption(centerOfMass, crabPositions, true);
        }


        public int CalculateGeometricMean(IEnumerable<int> input)
        {
            var sortedInput = input.ToArray();
            Array.Sort(sortedInput);

            int size = sortedInput.Length;
            int mid = size / 2;
            return (size % 2 != 0) ? sortedInput[mid] : (sortedInput[mid] + sortedInput[mid - 1]) / 2;
        }
        public int CalculateCenterOfMass(IEnumerable<int> input)
        {
            var massPoints = input.Distinct().ToDictionary(x => x, x => input.Count(y => y == x));
            return massPoints.Sum(x => x.Key * x.Value) / massPoints.Sum(x => x.Value);
        }



        public int CalculateFuelConsumption(int targetPosition, IEnumerable<int> positions, bool useGauss)
        {
            var positionRelatedFuelConsumption = positions.Distinct().ToDictionary(x => x, x => useGauss ? (int)(Math.Pow(Math.Abs(targetPosition - x),2)+ Math.Abs(targetPosition - x)) / 2 :  Math.Abs(targetPosition - x));
            return positions.Sum(x=>positionRelatedFuelConsumption[x]);
        }

        #endregion
    }
}

