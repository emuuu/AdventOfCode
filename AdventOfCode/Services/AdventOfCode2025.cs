using AdventOfCode.Extensions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Services
{
    public class AdventOfCode2025
    {
        public AdventOfCode2025()
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
                    var inputResult = inputMethod.Invoke(this, new object[] { inputPath });
                    var input = GetResultFromPossibleTask(inputResult);
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
                    var inputResult = inputMethod.Invoke(this, new object[] { inputPath });
                    var input = GetResultFromPossibleTask(inputResult);
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

        private object GetResultFromPossibleTask(object result)
        {
            if (result is Task task)
            {
                task.Wait();
                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty?.GetValue(task);
            }
            return result;
        }

        #region Day 01 
        public async Task<List<int>> ConvertDay1Input(string inputPath)
        {
            var commandList = await System.IO.File.ReadAllLinesAsync(inputPath);
            var commands = commandList.Select(x => int.Parse(x.Trim('R').Trim('L')) * (x.StartsWith("R") ? 1 : -1));

            return commands.ToList();
        }

        public int Day1_PuzzleOne(List<int> input)
        {
            var value = 50;
            var result = 0;
            foreach (var command in input)
            {
                value += command;
                while (value > 99) value -= 100;
                while (value < 0) value += 100;
                if (value == 0)
                    result++;
            }
            return result;
        }

        public int Day1_PuzzleTwo(List<int> input)
        {
            var value = 50;
            var result = 0;
            foreach (var command in input)
            {
                var step = command > 0 ? 1 : -1;
                for (var i = 0; i < Math.Abs(command); i++)
                {
                    value += step;
                    if (value > 99) value -= 100;
                    if (value < 0) value += 100;
                    if (value == 0)
                        result++;
                }

            }
            return result;
        }
        #endregion

        #region Day 02
        public async Task<List<string>> ConvertDay2Input(string inputPath)
        {
            return (await System.IO.File.ReadAllTextAsync(inputPath)).Split(',').ToList();
        }

        public long Day2_PuzzleOne(List<string> input)
        {
            long result = 0;
            foreach (var range in input)
            {
                var parts = range.Split('-');
                if (parts[0].StartsWith('0') || parts[1].StartsWith('0'))
                    continue;

                var first = long.Parse(parts[0]);
                var second = long.Parse(parts[1]);
                for (long i = first; i <= second; i++)
                {
                    var number = i.ToString().AsSpan();

                    if (number.Length % 2 != 0)
                        continue;

                    var haelfte = number.Length / 2;
                    if (number[..haelfte].SequenceEqual(number[haelfte..]))
                    {
                        result += i;
                    }
                }
            }
            return result;
        }

        public long Day2_PuzzleTwo(List<string> input)
        {
            long result = 0;
            foreach (var range in input)
            {
                var parts = range.Split('-');
                if (parts[0].StartsWith('0') || parts[1].StartsWith('0'))
                    continue;

                var first = long.Parse(parts[0]);
                var second = long.Parse(parts[1]);
                for (long i = first; i <= second; i++)
                {
                    var number = i.ToString();
                    if ((number + number).AsSpan(1, number.Length * 2 - 2).Contains(number, StringComparison.Ordinal))
                    {
                        result += i;
                    }
                }
            }
            return result;
        }
        #endregion

        #region Day 03
        public async Task<List<List<int>>> ConvertDay3Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(line => line.Select(digit => int.Parse(digit.ToString())).ToList()).ToList();
        }

        public long Day3_PuzzleOne(List<List<int>> input)
        {
            var requiredBatteries = 2;
            long result = 0;
            foreach (var line in input)
            {
                result += GetMaxJoltage(line, requiredBatteries);
            }
            return result;
        }

        public long Day3_PuzzleTwo(List<List<int>> input)
        {
            var requiredBatteries = 12;
            long result = 0;
            foreach (var line in input)
            {
                result += GetMaxJoltage(line, requiredBatteries);
            }
            return result;
        }

        private long GetMaxJoltage(List<int> stack, int requiredBatteries)
        {
            var lastBatteryIndex = -1;
            var selectedBatteryStack = new List<int>();
            while (requiredBatteries-- > 0)
            {
                var availableStack = stack.Skip(lastBatteryIndex + 1).Take((stack.Count - (lastBatteryIndex + 1)) - requiredBatteries).ToList();
                var maxValue = availableStack.Max();
                lastBatteryIndex = stack.IndexOf(maxValue, lastBatteryIndex + 1);
                selectedBatteryStack.Add(maxValue);
            }
            return long.Parse(string.Join("", selectedBatteryStack));
        }
        #endregion

        #region Day 04
        public async Task<bool[,]> ConvertDay4Input(string inputPath)
        {
            var lines = File.ReadAllLines(inputPath);
            int rows = lines.Length;
            int cols = lines[0].Length;

            bool[,] result = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = lines[i][j] == '@';
                }
            }
            return result;
        }

        private int[,] GetNeighboursMap(bool[,] map)
        {
            var neighboursMap = new int[map.GetLength(0), map.GetLength(1)];
            for (var x = 0; x < map.GetLength(0); x++)
            {
                for (var y = 0; y < map.GetLength(1); y++)
                {
                    if (!map[x, y])
                        continue;

                    for (var xRun = x - 1; xRun <= x + 1; xRun++)
                    {
                        for (var yRun = y - 1; yRun <= y + 1; yRun++)
                        {
                            if ((x == xRun && y == yRun) || xRun < 0 || yRun < 0 || xRun >= map.GetLength(0) || yRun >= map.GetLength(1))
                                continue;
                            if (map[xRun, yRun])
                            {
                                neighboursMap[x, y]++;
                            }
                        }
                    }
                }
            }
            return neighboursMap;
        }

        private bool[,] GetMovableRolls(int[,] neighboursMap, bool[,] map)
        {
            var movableRolls = new bool[neighboursMap.GetLength(0), neighboursMap.GetLength(1)];
            for (var x = 0; x < neighboursMap.GetLength(0); x++)
            {
                for (var y = 0; y < neighboursMap.GetLength(1); y++)
                {
                    movableRolls[x, y] = map[x, y] && neighboursMap[x, y] < 4;
                }
            }
            return movableRolls;
        }

        private int CountMovableRolls(bool[,] movableRolls)
        {
            var count = 0;
            for (var x = 0; x < movableRolls.GetLength(0); x++)
            {
                for (var y = 0; y < movableRolls.GetLength(1); y++)
                {
                    if (movableRolls[x, y])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void UpdateMap(bool[,] map, bool[,] movableRolls)
        {
            for (var x = 0; x < movableRolls.GetLength(0); x++)
            {
                for (var y = 0; y < movableRolls.GetLength(1); y++)
                {
                    if (movableRolls[x, y])
                        map[x, y] = false;
                }
            }
        }

        public long Day4_PuzzleOne(bool[,] map)
        {
            var neighboursMap = GetNeighboursMap(map);
            var movableRolls = GetMovableRolls(neighboursMap, map);
            return CountMovableRolls(movableRolls);
        }

        public long Day4_PuzzleTwo(bool[,] map)
        {
            var result = 0;
            while (true)
            {
                var neighboursMap = GetNeighboursMap(map);
                var movableRolls = GetMovableRolls(neighboursMap, map);
                var movableCount = CountMovableRolls(movableRolls);
                if (movableCount == 0)
                    break;

                result += movableCount;
                UpdateMap(map, movableRolls);
            }

            return result;
        }
        #endregion
    }
}

