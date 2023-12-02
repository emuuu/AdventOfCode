using AdventOfCode.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
            for (var i = 0; i < input.Length; i++)
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

        #region Day 07

        public class Dir
        {
            public string Name;
            public List<string> Contents = new List<string>();
            public long Size;
        }

        public List<Dir> ConvertDay7Input(string inputPath)
        {
            var dirs = new List<Dir> { new Dir { Name = "/" } };
            var lines = File.ReadAllLines(inputPath);
            var currentDir = dirs.First();
            for (var i = 1; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("$ ls"))
                {
                    while (i + 1 < lines.Length && !lines[i + 1].StartsWith("$"))
                    {
                        i++;
                        if (lines[i].StartsWith("dir"))
                        {
                            var dirName = $"{currentDir.Name}.{lines[i].Replace("dir ", "")}";
                            dirs.Add(new Dir { Name = dirName });
                            currentDir.Contents.Add(dirName);
                        }
                        else
                        {
                            currentDir.Size += long.Parse(lines[i].Split(' ')[0]);
                        }
                    }
                }

                if (lines[i].StartsWith("$ cd"))
                {
                    if (lines[i].EndsWith(".."))
                    {
                        currentDir = dirs.First(x => x.Contents.Contains(currentDir.Name));
                    }
                    else
                    {
                        currentDir = dirs.First(x => x.Name == $"{currentDir.Name}.{lines[i].Replace("$ cd ", "")}");
                    }
                }
            }

            dirs.Reverse();
            foreach (var dir in dirs)
            {
                foreach (var parentDir in dirs.Where(x => x.Contents.Contains(dir.Name)))
                {
                    parentDir.Size += dir.Size;
                }
            }

            return dirs;
        }

        public long Day7_PuzzleOne(List<Dir> input)
        {
            return input.Where(x => x.Size <= 100000).Sum(x => x.Size);
        }

        public long Day7_PuzzleTwo(List<Dir> input)
        {
            return input.Where(x => x.Size >= 30000000 - (70000000 - input.Last().Size)).Min(x => x.Size);
        }
        #endregion

        #region Day 08

        public List<List<int>> ConvertDay8Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();
        }

        public long Day8_PuzzleOne(List<List<int>> input)
        {
            var visibleTrees = input.Select((y, i) => y.Select((x, j) => i == 0 || j == 0 || i == input.Count - 1 || j == y.Count - 1 ? true : false).ToList()).ToList();

            for (var y = 1; y < input.Count - 1; y++)
            {
                var xMax = input[y].Max();
                var firstXMax = input[y].Select((a, i) => (a, i)).First(b => b.a == xMax).i;
                var lastXMax = input[y].Select((a, i) => (a, i)).Last(b => b.a == xMax).i;
                visibleTrees[y][firstXMax] = true;
                visibleTrees[y][lastXMax] = true;

                var xLeftMax = input[y][0];
                for (var xLeft = 1; xLeft < firstXMax; xLeft++)
                {
                    if (input[y][xLeft] > xLeftMax)
                    {
                        visibleTrees[y][xLeft] = true;
                        xLeftMax = input[y][xLeft];
                    }
                }

                var xRightMax = input[y][input[y].Count - 1];
                for (var xRight = input[y].Count - 2; xRight > lastXMax; xRight--)
                {
                    if (input[y][xRight] > xRightMax)
                    {
                        visibleTrees[y][xRight] = true;
                        xRightMax = input[y][xRight];
                    }
                }
            }

            for (var x = 1; x < input[0].Count - 1; x++)
            {
                var column = input.Select(a => a[x]).ToList();
                var yMax = column.Max();
                var firstYMax = column.Select((a, i) => (a, i)).First(b => b.a == yMax).i;
                var lastYMax = column.Select((a, i) => (a, i)).Last(b => b.a == yMax).i;
                visibleTrees[firstYMax][x] = true;
                visibleTrees[lastYMax][x] = true;

                var yTopMax = input[0][x];
                for (var yTop = 1; yTop < firstYMax; yTop++)
                {
                    if (input[yTop][x] > yTopMax)
                    {
                        visibleTrees[yTop][x] = true;
                        yTopMax = input[yTop][x];
                    }
                }

                var yBotMax = input[input.Count - 1][x];
                for (var yBot = input.Count - 2; yBot > lastYMax; yBot--)
                {
                    if (input[yBot][x] > yBotMax)
                    {
                        visibleTrees[yBot][x] = true;
                        yBotMax = input[yBot][x];
                    }
                }
            }


            return visibleTrees.SelectMany(x => x).Count(y => y);
        }

        public long Day8_PuzzleTwo(List<List<int>> input)
        {
            var maxScenicScore = 1;
            var maxHeight = input.SelectMany(a => a).Distinct().OrderDescending();
            foreach (var treeHeight in input.SelectMany(a => a).Distinct().OrderDescending())
            {
                foreach (var possibleTree in input.SelectMany((a, y) => a.Select((b, x) => (b, x, y))).Where(t => t.b == treeHeight && t.x > 0 && t.x < input[0].Count - 1 && t.y > 0 && t.y < input.Count - 1).Select(t => (t.x, t.y)))
                {
                    var scenicScore = 1;
                    var seenTrees = 0;
                    for (var x = possibleTree.x - 1; x >= 0; x--)
                    {
                        seenTrees++;
                        if (x == 0 || input[possibleTree.y][x] >= treeHeight)
                        {
                            scenicScore *= seenTrees;
                            seenTrees = 0;
                            break;
                        }
                    }
                    for (var x = possibleTree.x + 1; x < input[0].Count; x++)
                    {
                        seenTrees++;
                        if (x == input[possibleTree.y].Count - 1 || input[possibleTree.y][x] >= treeHeight)
                        {
                            scenicScore *= seenTrees;
                            seenTrees = 0;
                            break;
                        }
                    }

                    for (var y = possibleTree.y - 1; y >= 0; y--)
                    {
                        seenTrees++;
                        if (y == 0 || input[y][possibleTree.x] >= treeHeight)
                        {
                            scenicScore *= seenTrees;
                            seenTrees = 0;
                            break;
                        }
                    }
                    for (var y = possibleTree.y + 1; y < input.Count; y++)
                    {
                        seenTrees++;
                        if (y == input.Count - 1 || input[y][possibleTree.x] >= treeHeight)
                        {
                            scenicScore *= seenTrees;
                            seenTrees = 0;
                            break;
                        }
                    }

                    if (scenicScore > maxScenicScore)
                    {
                        maxScenicScore = scenicScore;
                    }
                }
            }
            return maxScenicScore;
        }
        #endregion

        #region Day 09

        public List<(int Steps, int X, int Y)> ConvertDay9Input(string inputPath)
        {
            var vectors = new List<(int Steps, int X, int Y)>();
            foreach (var line in File.ReadAllLines(inputPath).Select(x => (x.Split(' ')[0], x.Split(' ')[1])))
            {
                var steps = int.Parse(line.Item2);
                switch (line.Item1)
                {
                    case "R":
                        {
                            vectors.Add((steps, 1, 0));
                            break;
                        }
                    case "L":
                        {
                            vectors.Add((steps, -1, 0));
                            break;
                        }
                    case "U":
                        {
                            vectors.Add((steps, 0, 1));
                            break;
                        }
                    case "D":
                        {
                            vectors.Add((steps, 0, -1));
                            break;
                        }
                }
            }
            return vectors;
        }

        public long Day9_PuzzleOne(List<(int Steps, int X, int Y)> input)
        {
            return MoveKnots(2, input).Count;
        }

        public long Day9_PuzzleTwo(List<(int Steps, int X, int Y)> input)
        {
            return MoveKnots(10, input).Count;
        }

        private List<(int X, int y)> MoveKnots(int knotNumber, List<(int Steps, int X, int Y)> input)
        {
            var knots = Enumerable.Range(0, knotNumber).Select(x => new int[] { 0, 0 }).ToList();
            var tailVisits = new List<(int X, int Y)>();
            foreach (var vector in input)
            {
                for (var i = 1; i <= vector.Steps; i++)
                {
                    knots[0][0] += vector.X;
                    knots[0][1] += vector.Y;
                    for (var k = 1; k < knots.Count; k++)
                    {
                        var xDistance = knots[k - 1][0] - knots[k][0];
                        var yDistance = knots[k - 1][1] - knots[k][1];
                        var absXDistance = Math.Abs(xDistance);
                        var absYDistance = Math.Abs(yDistance);
                        if (absXDistance == 2 || absYDistance == 2)
                        {
                            if (absXDistance == 2 && absYDistance == 2)
                            {
                                knots[k][0] += xDistance / 2;
                                knots[k][1] += yDistance / 2;
                            }
                            else if (absYDistance == 2)
                            {
                                knots[k][1] += yDistance / 2;
                                if (absXDistance == 1)
                                {
                                    knots[k][0] += xDistance;
                                }
                            }
                            else if (absXDistance == 2)
                            {
                                knots[k][0] += xDistance / 2;
                                if (absYDistance == 1)
                                {
                                    knots[k][1] += yDistance;
                                }
                            }
                        }
                        if (k == knots.Count - 1)
                        {
                            if (!tailVisits.Any(a => a.X == knots[k][0] && a.Y == knots[k][1]))
                            {
                                tailVisits.Add((knots[k][0], knots[k][1]));
                            }
                        }
                    }
                }
            }
            return tailVisits;
        }
        #endregion

        #region Day 10

        public List<(int Cycles, int ValueChange)> ConvertDay10Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => x.StartsWith("noop") ? (1, 0) : (2, int.Parse(x.Split(' ')[1]))).ToList();
        }

        public int Day10_PuzzleOne(List<(int Cycles, int ValueChange)> input)
        {
            var values = ExecuteCommands(input);
            var relevantCylces = new List<int> { 20, 60, 100, 140, 180, 220 };

            return relevantCylces.Select(x => x * values[x - 1]).Sum();
        }

        public int Day10_PuzzleTwo(List<(int Cycles, int ValueChange)> input)
        {
            var values = ExecuteCommands(input);
            var crt = 0;
            foreach (var cycle in values)
            {
                if (crt >= cycle - 1 && crt <= cycle + 1)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(" ");
                }
                crt++;
                if (crt == 40)
                {
                    Console.Write("\r\n");
                    crt = 0;
                }
            }
            return default;
        }

        private List<int> ExecuteCommands(List<(int Cycles, int ValueChange)> commands)
        {
            var x = 1;
            var values = new List<int>();
            foreach (var command in commands)
            {
                for (var i = 0; i < command.Cycles; i++)
                {
                    values.Add(x);
                }
                x += command.ValueChange;
            }
            return values;
        }
        #endregion

        #region Day 11
        public class Monkey
        {
            public List<long> Items;
            public int OperatorValue;
            public bool OperationIsQuadrature;
            public bool OperationIsAddition;
            public int TestDivisor;
            public int TestTrueTarget;
            public int TestFalseTarget;
            public long InspectedItems;

            public long ChangeWorryLevel(long oldLevel, int reliefLevel)
            {
                var newLevel = OperationIsQuadrature ? oldLevel * oldLevel : OperationIsAddition ? oldLevel += OperatorValue : oldLevel *= OperatorValue;
                return newLevel / reliefLevel;
            }

            public int CheckItemTarget(long worryLevel)
            {
                return worryLevel % TestDivisor == 0 ? TestTrueTarget : TestFalseTarget;
            }
        }

        public List<Monkey> ConvertDay11Input(string inputPath)
        {
            var monkeys = new List<Monkey>();
            var lines = File.ReadAllLines(inputPath);
            for(var i = 0; i < lines.Length;i++)
            {
                var monkey = new Monkey
                {
                    Items = lines[i + 1].Replace(" ", "").Split(':')[1].Split(",").Select(x => long.Parse(x)).ToList(),
                    OperationIsQuadrature = lines[i + 2].EndsWith("old"),
                    TestDivisor = int.Parse(lines[i + 3].Split(' ').Last()),
                    TestTrueTarget = int.Parse(lines[i + 4].Split(' ').Last()),
                    TestFalseTarget = int.Parse(lines[i + 5].Split(' ').Last())
                };
                if (!monkey.OperationIsQuadrature)
                {
                    monkey.OperationIsAddition = lines[i + 2].Contains("+");
                    monkey.OperatorValue = int.Parse(lines[i + 2].Split(' ').Last());
                }
                monkeys.Add(monkey);
                i += 6;
            }
            return monkeys;
        }

        public long Day11_PuzzleOne(List<Monkey> input)
        {
            for(var round = 1; round <= 20; round++)
            {
                foreach (var monkey in input)
                {
                    foreach (var item in monkey.Items)
                    {
                        var newÍtem = monkey.ChangeWorryLevel(item, 3); ;
                        monkey.InspectedItems++;
                        input[monkey.CheckItemTarget(newÍtem)].Items.Add(newÍtem);
                    }
                    monkey.Items.Clear();
                }
            }

            return input.Select(x => x.InspectedItems).OrderDescending().Take(2).Aggregate((a, b) => a * b);
        }

        public long Day11_PuzzleTwo(List<Monkey> input)
        {
            var modulus = input.Select(x => (int)x.TestDivisor).Aggregate((a, b) => a * b);

            
            for (var round = 1; round <= 10000; round++)
            {
                foreach (var monkey in input)
                {
                    foreach (var item in monkey.Items)
                    {
                        var newItem = monkey.ChangeWorryLevel(item, 1);
                        monkey.InspectedItems++;
                        newItem %= modulus;
                        input[monkey.CheckItemTarget(newItem)].Items.Add(newItem);
                    }
                    monkey.Items.Clear();
                }
            }

            return input.Select(x => x.InspectedItems).OrderDescending().Take(2).Aggregate((a, b) => a * b);
        }
        #endregion

        #region Day 12

        public List<List<char>> ConvertDay12Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => x.Select(y =>y).ToList()).ToList();
        }

        public long Day12_PuzzleOne(List<List<char>> input)
        {
            var map = input.SelectMany((a, y) => a.Select((b ,x)  => (H: b, X: x, Y: y))).ToList();
            var startPosition = map.First(a => a.H == 'S');
            var targetPosition = map.First(a => a.H == 'E');
            var test = 'b' - 'a';
            return default;
        }

        public long Day12_PuzzleTwo(List<List<char>> input)
        {
            return default;
        }
        #endregion
    }
}

