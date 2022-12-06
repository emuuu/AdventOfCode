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

        #region Day 02 
        public List<string> ConvertDay2Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).ToList();
        }

        public int Day2_PuzzleOne(List<string> input)
        {
            var results = input.Select(x => x switch
            {
                "A X" => 3 + 1,
                "A Y" => 6 + 2,
                "A Z" => 0 + 3,
                "B X" => 0 + 1,
                "B Y" => 3 + 2,
                "B Z" => 6 + 3,
                "C X" => 6 + 1,
                "C Y" => 0 + 2,
                "C Z" => 3 + 3,
            });
            return results.Sum();
        }

        public int Day2_PuzzleTwo(List<string> input)
        {
            var results = input.Select(x => x switch
            {
                "A X" => 3,
                "A Y" => 4,
                "A Z" => 8,
                "B X" => 1,
                "B Y" => 5,
                "B Z" => 9,
                "C X" => 2,
                "C Y" => 6,
                "C Z" => 7,
            });
            return results.Sum();
        }
        #endregion

        #region Day 03
        public List<(List<int>, List<int>, List<int>)> ConvertDay3Input(string inputPath)
        {
            var i = 1;
            var alphabet = Enumerable.Range('a', 'z' - 'a' + 1).ToDictionary(x => (char)x, x => i++).Union(Enumerable.Range('A', 'Z' - 'A' + 1).ToDictionary(x => (char)x, x => i++)).ToDictionary(x => x.Key, x => x.Value);

            return File.ReadAllLines(inputPath).Select(x => (x.Select(y => alphabet[y]).ToList(), x.Take(x.Length / 2).Select(y => alphabet[y]).ToList(), x.Skip(x.Length / 2).Select(y => alphabet[y]).ToList())).ToList();
        }

        public int Day3_PuzzleOne(List<(List<int>, List<int>, List<int>)> input)
        {
            return input.SelectMany(x => x.Item2.Intersect(x.Item3)).Sum();
        }

        public int Day3_PuzzleTwo(List<(List<int>, List<int>, List<int>)> input)
        {
            var sum = 0;
            for (var i = 0; i < input.Count; i = i + 3)
            {
                var set = input.Skip(i).Take(3).ToList();
                sum += set[0].Item1.Intersect(set[1].Item1.Intersect(set[2].Item1)).Sum();
            }
            return sum;

        }
        #endregion

        #region Day 04
        public IEnumerable<(IEnumerable<int>, IEnumerable<int>)> ConvertDay4Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(line => (Enumerable.Range(int.Parse(line.Split(',')[0].Split('-')[0]), int.Parse(line.Split(',')[0].Split('-')[1]) - int.Parse(line.Split(',')[0].Split('-')[0]) + 1), Enumerable.Range(int.Parse(line.Split(',')[1].Split('-')[0]), int.Parse(line.Split(',')[1].Split('-')[1]) - int.Parse(line.Split(',')[1].Split('-')[0]) + 1)));
        }

        public int Day4_PuzzleOne(IEnumerable<(IEnumerable<int>, IEnumerable<int>)> input)
        {
            return input.Select(x => x.Item1.Intersect(x.Item2).SequenceEqual(x.Item1) || x.Item1.Intersect(x.Item2).SequenceEqual(x.Item2) ? 1 : 0).Sum();
        }

        public int Day4_PuzzleTwo(IEnumerable<(IEnumerable<int>, IEnumerable<int>)> input)
        {
            return input.Select(x => x.Item1.Intersect(x.Item2).Any() ? 1 : 0).Sum();
        }
        #endregion

        #region Day 05
        public (List<List<char>>, IEnumerable<int>) ConvertDay5Input(string inputPath)
        {
            var lines = File.ReadAllLines(inputPath);
            var stackEnd = lines.Select((line, i) => (line, i)).First(x => x.line.StartsWith(" 1")).i;
            var stacks = Enumerable.Range(1, int.Parse(lines[stackEnd].Trim().Last().ToString())).Select(x => lines.Take(stackEnd).Where(y => y[3 * (x - 1) + x] != ' ').Select(y => y[3 * (x - 1) + x]).ToList()).ToList();
            var commands = lines.Skip(stackEnd + 2).Select(x => int.Parse(new string(x.Where(c => char.IsDigit(c)).ToArray())));
            return (stacks, commands);
        }

        public string Day5_PuzzleOne((List<List<char>>, IEnumerable<int>) input)
        {
            foreach (var command in input.Item2)
            {
                var to = command % 10 - 1;
                var from = (command % 100 - to) / 10 - 1;
                var amount = (command % 1000 - from - to) / 100;
                if (command >= 1000)
                {
                    amount = (command % 10000 - from - to - amount) / 100;
                }

                for (var i = 0; i < amount; i++)
                {
                    input.Item1[to].InsertRange(0, input.Item1[from].Take(1));
                    input.Item1[from].RemoveRange(0, 1);
                }

            }
            return new string(input.Item1.Select(x => x[0]).ToArray());
        }

        public string Day5_PuzzleTwo((List<List<char>>, IEnumerable<int>) input)
        {
            foreach (var command in input.Item2)
            {
                var to = command % 10 - 1;
                var from = (command % 100 - to) / 10 - 1;
                var amount = (command % 1000 - from - to) / 100;
                if (command >= 1000)
                {
                    amount = (command % 10000 - from - to - amount) / 100;
                }

                input.Item1[to].InsertRange(0, input.Item1[from].Take(amount));
                input.Item1[from].RemoveRange(0, amount);

            }
            return new string(input.Item1.Select(x => x[0]).ToArray());
        }
        #endregion

        #region Day 06
        public string ConvertDay6Input(string inputPath)
        {
            return File.ReadAllText(inputPath);
        }

        public int Day6_PuzzleOne(string input)
        {
            var queue = new Queue<char>();
            for(var i = 0; i < input.Length; i++)
            {
                queue.Enqueue(input[i]);

                if (queue.Count > 4)
                {
                    queue.Dequeue();
                }
                
                if (queue.Distinct().Count() == 4)
                {
                    return i + 1;
                }
            }

            return default;
        }

        public int Day6_PuzzleTwo(string input)
        {
            var queue = new Queue<char>();
            for (var i = 0; i < input.Length; i++)
            {
                queue.Enqueue(input[i]);

                if (queue.Count > 14)
                {
                    queue.Dequeue();
                }

                if (queue.Distinct().Count() == 14)
                {
                    return i + 1;
                }
            }

            return default;
        }
        #endregion
    }
}

