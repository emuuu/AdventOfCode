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
    public class AdventOfCode2020
    {
        public AdventOfCode2020()
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
        public int[] ConvertDay1Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => int.Parse(x)).ToArray();
        }

        public int Day1_PuzzleOne(int[] input)
        {
            return CalculateExpenses(input, false);
        }

        public int Day1_PuzzleTwo(int[] input)
        {
            return CalculateExpenses(input, true);
        }

        public int CalculateExpenses(int[] report, bool threeNumbers)
        {
            for (var i = 0; i < report.Length; i++)
            {
                for (var j = 0; j < report.Length; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    if (!threeNumbers)
                    {
                        if (report[i] + report[j] == 2020)
                        {
                            return report[i] * report[j];
                        }
                    }
                    else
                    {
                        for (var k = 0; k < report.Length; k++)
                        {
                            if (j == k || i == k)
                            {
                                continue;
                            }
                            if (report[i] + report[j] + report[k] == 2020)
                            {
                                return report[i] * report[j] * report[k];
                            }
                        }
                    }
                }
            }
            return 0;
        }
        #endregion

        #region Day 02
        public string[] ConvertDay2Input(string inputPath)
        {
            return File.ReadAllLines(inputPath);
        }

        public int Day2_PuzzleOne(IEnumerable<string> input)
        {
            return input.Count(x => ValidPasswordPolicy(x.Split(':')[0], x.Split(':')[1].Trim(), false));
        }

        public int Day2_PuzzleTwo(IEnumerable<string> input)
        {
            return input.Count(x => ValidPasswordPolicy(x.Split(':')[0], x.Split(':')[1].Trim(), true));
        }

        public bool ValidPasswordPolicy(string policy, string password, bool tobogganPolicy)
        {
            var policySplit = policy.Split(' ');

            if (!int.TryParse(policySplit[0].Split('-')[0], out int min) || !int.TryParse(policySplit[0].Split('-')[1], out int max))
                throw new Exception("Could not parse password policy.");

            var validationChar = policySplit[1][0];
            if (!tobogganPolicy)
            {
                var charCount = password.Count(x => x == validationChar);
                return charCount >= min && charCount <= max;
            }
            else
            {
                min--;
                max--;
                return (password[min] == validationChar || password[max] == validationChar) && !(password[min] == validationChar && password[max] == validationChar);
            }
        }

        #endregion

        #region Day 03
        public char[][] ConvertDay3Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => x.ToCharArray()).ToArray();
        }

        public int Day3_PuzzleOne(char[][] input)
        {
            return RunMap(input, 3, 1);
        }

        public long Day3_PuzzleTwo(char[][] input)
        {
            var slopeVectors = new List<int[]> { new int[] { 1, 1 }, new int[] { 3, 1 }, new int[] { 5, 1 }, new int[] { 7, 1 }, new int[] { 1, 2 } };
            var results = slopeVectors.Select(x => (long)RunMap(input, x[0], x[1]));
            return results.Aggregate((long)1, (x, y) => x * y);
        }


        public int RunMap(char[][] map, int vectorX, int vectorY)
        {
            var i = 0;
            var j = 0;
            var treeCount = 0;
            while (i < map.Length)
            {
                if (map[i][j] == '#')
                {
                    treeCount++;
                }
                if (j + vectorX >= map[i].Length)
                {
                    j = vectorX - (map[i].Length - j);
                }
                else
                {
                    j += vectorX;
                }
                i += vectorY;
            }

            return treeCount;
        }
        #endregion

        #region Day 04
        public List<List<KeyValuePair<string, string>>> ConvertDay4Input(string inputPath)
        {
            var input = new List<List<KeyValuePair<string, string>>>();
            var currentPassport = new List<KeyValuePair<string, string>>();
            foreach (var line in File.ReadAllLines(inputPath))
            {
                if (string.IsNullOrEmpty(line))
                {
                    input.Add(currentPassport);
                    currentPassport = new List<KeyValuePair<string, string>>();
                }
                else
                {
                    currentPassport.AddRange(line.Split(' ').Select(x => new KeyValuePair<string, string>(x.Split(':')[0], x.Split(':')[1])));
                }
            }
            input.Add(currentPassport);
            return input;
        }

        public int Day4_PuzzleOne(List<List<KeyValuePair<string, string>>> input)
        {
            return input.Count(x => ValidPassport(x, false));
        }

        public long Day4_PuzzleTwo(List<List<KeyValuePair<string, string>>> input)
        {
            return input.Count(x => ValidPassport(x, true));
        }

        public static List<KeyValuePair<string, bool>> PassportFields = new List<KeyValuePair<string, bool>>() {
            new KeyValuePair<string, bool>("byr", true),
            new KeyValuePair<string, bool>("iyr", true),
            new KeyValuePair<string, bool>("eyr", true),
            new KeyValuePair<string, bool>("hgt", true),
            new KeyValuePair<string, bool>("hcl", true),
            new KeyValuePair<string, bool>("ecl", true),
            new KeyValuePair<string, bool>("pid", true),
            new KeyValuePair<string, bool>("cid", false),
        };

        public bool ValidPassport(IEnumerable<KeyValuePair<string, string>> passport, bool validateValue)
        {
            if (validateValue && passport.Any(x => !ValidatePassportValue(x)))
                return false;

            return !PassportFields.Any(x => !(!x.Value || passport.Any(y => y.Key == x.Key)));
        }

        public bool ValidatePassportValue(KeyValuePair<string, string> passportField)
        {
            switch (passportField.Key)
            {
                case "byr":
                    {
                        if (!int.TryParse(passportField.Value, out int birthYear))
                            return false;
                        return birthYear >= 1920 && birthYear <= 2002;
                    }
                case "iyr":
                    {
                        if (!int.TryParse(passportField.Value, out int issueYear))
                            return false;
                        return issueYear >= 2010 && issueYear <= 2020;
                    }
                case "eyr":
                    {
                        if (!int.TryParse(passportField.Value, out int expirationYear))
                            return false;
                        return expirationYear >= 2020 && expirationYear <= 2030;
                    }
                case "hgt":
                    {
                        if (!int.TryParse(new string(passportField.Value.Where(c => char.IsDigit(c)).ToArray()), out int height))
                            return false;
                        if (passportField.Value.EndsWith("cm"))
                            return height >= 150 && height <= 193;
                        else if (passportField.Value.EndsWith("in"))
                            return height >= 59 && height <= 76;
                        else
                            return false;
                    }
                case "hcl":
                    {
                        if (!passportField.Value.StartsWith('#'))
                            return false;
                        var hairColor = passportField.Value.Replace("#", string.Empty);
                        if (hairColor.Length != 6)
                            return false;
                        return !hairColor.Any(x => !((x >= 'a' && x <= 'f') || (x >= '0' && x <= '9')));
                    }
                case "ecl":
                    {
                        return passportField.Value == "amb" || passportField.Value == "blu" || passportField.Value == "brn" || passportField.Value == "gry" || passportField.Value == "grn" || passportField.Value == "hzl" || passportField.Value == "oth";
                    }
                case "pid":
                    {
                        return passportField.Value.Length == 9 && int.TryParse(passportField.Value, out int pid);
                    }
                case "cid":
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        #endregion

        #region Day 05
        public IEnumerable<string> ConvertDay5Input(string inputPath)
        {
            return File.ReadAllLines(inputPath);
        }

        public int Day5_PuzzleOne(IEnumerable<string> input)
        {
            var seatIDs = input.Select(x => CalculateSeatID(x));
            return seatIDs.Max();
        }

        public int Day5_PuzzleTwo(IEnumerable<string> input)
        {
            var seatIDs = input.Select(x => CalculateSeatID(x)).OrderBy(x => x).ToList();
            for (var i = 0; i < seatIDs.Count - 1; i++)
            {
                if (seatIDs[i] + 2 == seatIDs[i + 1])
                    return seatIDs[i] + 1;
            }

            return default;
        }

        public int CalculateSeatID(string input)
        {
            var row = new int[] { 0, 127 };
            var column = new int[] { 0, 7 };
            foreach (var rowCode in input.Substring(0, 7))
            {
                SolveSeatStep(row, rowCode);
            }
            foreach (var colCode in input.Substring(7, 3))
            {
                SolveSeatStep(column, colCode);
            }

            return row[0] * 8 + column[0];
        }

        public int[] SolveSeatStep(int[] range, char input)
        {
            var rangeStep = (range[1] - range[0] + 1) / 2;

            if (input == 'F' || input == 'L')
                range[1] -= rangeStep;
            else
                range[0] += rangeStep;
            return range;
        }

        #endregion

        #region Day 06
        public string ConvertDay6Input(string inputPath)
        {
            return File.ReadAllText(inputPath);
        }

        public int Day6_PuzzleOne(string input)
        {
            return input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None).Select(x => CountAnswers(x, true)).Sum();
        }

        public int Day6_PuzzleTwo(string input)
        {
            return input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None).Select(x => CountAnswers(x, false)).Sum();
        }

        public int CountAnswers(string answerSheet, bool testOne)
        {
            var charSet = answerSheet.Replace("\r\n", string.Empty).Select(x => x).Distinct();

            if (testOne)
                return charSet.Count();
            else
                return charSet.Where(x => !answerSheet.Split(new string[] { "\r\n" }, StringSplitOptions.None).Any(y => !y.Contains(x))).Count();
        }
        #endregion

        #region Day 07
        public Dictionary<string, Dictionary<string, int>> ConvertDay7Input(string inputPath)
        {
            return CreateRules(File.ReadAllLines(inputPath));
        }

        public int Day7_PuzzleOne(Dictionary<string, Dictionary<string, int>> input)
        {
            return RunBagRules(new List<string> { "shiny gold" }, input, true).Count - 1;
        }

        public int Day7_PuzzleTwo(Dictionary<string, Dictionary<string, int>> input)
        {
            return RunBagRules(new List<string> { "shiny gold" }, input, false).Count - 1;
        }

        public Dictionary<string, Dictionary<string, int>> CreateRules(IEnumerable<string> ruleSet)
        {
            return ruleSet.ToDictionary(
                x => x.Split(new string[] { "bags contain" }, StringSplitOptions.None)[0].Trim(),
                x => x.Split(new string[] { "bags contain" }, StringSplitOptions.None)[1].Split(',').Where(y => !y.Contains("no other")).ToDictionary(
                    y => y.Trim().Split(' ')[1] + " " + y.Trim().Split(' ')[2].Trim(),
                    y => int.Parse(y.Trim().Split(' ')[0])));
        }

        public List<string> RunBagRules(List<string> searchedBags, Dictionary<string, Dictionary<string, int>> rules, bool runUp)
        {
            var addedBags = new List<string>();
            foreach (var searchedBag in searchedBags)
            {
                if (runUp)
                    addedBags.AddRange(RunBagRules(rules.Where(x => x.Value.ContainsKey(searchedBag)).Select(x => x.Key).ToList(), rules, runUp));
                else
                    addedBags.AddRange(RunBagRules(rules.Where(x => x.Key == searchedBag).SelectMany(x => x.Value.SelectMany(y => Enumerable.Repeat(y.Key, y.Value))).ToList(), rules, runUp));
            }
            if (runUp)
                return searchedBags.Union(addedBags).ToList();
            else
            {
                searchedBags.AddRange(addedBags);
                return searchedBags;
            }
        }
        #endregion

        #region Day 08
        public List<Tuple<string, int>> ConvertDay8Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => new Tuple<string, int>(x.Split(' ')[0], int.Parse(x.Split(' ')[1]))).ToList();
        }

        public int Day8_PuzzleOne(List<Tuple<string, int>> input)
        {
            RunBootProgram(input, out int accumulatorValue);
            return accumulatorValue;
        }

        public int Day8_PuzzleTwo(List<Tuple<string, int>> input)
        {
            return TryFixBootProgram(input);
        }


        public bool RunBootProgram(List<Tuple<string, int>> instructions, out int accumulatorValue)
        {
            accumulatorValue = 0;
            var pastCommands = new List<int>();

            for (var i = 0; i < instructions.Count; i++)
            {
                if (pastCommands.Contains(i))
                    return false;

                pastCommands.Add(i);
                switch (instructions[i].Item1)
                {
                    case "acc":
                        {
                            accumulatorValue += instructions[i].Item2;
                            break;
                        }
                    case "jmp":
                        {
                            i += instructions[i].Item2 - 1;
                            break;
                        }
                    case "nop":
                        {
                            break;
                        }
                }
            }
            return true;
        }

        public int TryFixBootProgram(List<Tuple<string, int>> instructions)
        {
            var changableIndices = instructions.Select((instruction, index) => new { Instruction = instruction, Index = index }).Where(o => o.Instruction.Item1 == "jmp" || o.Instruction.Item1 == "nop").Select(o => o.Index).ToList();
            foreach (var changeIndex in changableIndices)
            {
                var updatedInstructions = instructions.Select(x => x).ToList();
                updatedInstructions[changeIndex] = new Tuple<string, int>(instructions[changeIndex].Item1 == "jmp" ? "nop" : "jmp", instructions[changeIndex].Item2);
                if (RunBootProgram(updatedInstructions, out int accumulatorValue))
                    return accumulatorValue;
            }

            return default;
        }
        #endregion

        #region Day 09
        public IEnumerable<long> ConvertDay9Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => long.Parse(x));
        }

        public long Day9_PuzzleOne(IEnumerable<long> input)
        {
            return FindInvalidNumber(input, 25);
        }

        public long Day9_PuzzleTwo(IEnumerable<long> input)
        {
            var contiguouSet = FindContiguousSet(Day9_PuzzleOne(input), input.ToList());

            return contiguouSet.Min() + contiguouSet.Max();
        }

        public long FindInvalidNumber(IEnumerable<long> inputStream, int preambleLength)
        {
            return inputStream.Select((input, index) => new { Input = input, Index = index }).Skip(preambleLength).FirstOrDefault(x => !ValidStream(x.Input, inputStream.Skip(x.Index - preambleLength).Take(preambleLength))).Input;
        }

        public bool ValidStream(long validationNumber, IEnumerable<long> numberStream)
        {
            return numberStream.Any(x => numberStream.Any(y => y != x && x + y == validationNumber));
        }

        public IEnumerable<long> FindContiguousSet(long validationNumber, List<long> numberStream)
        {
            for (var i = 0; i < numberStream.Count; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (j == i)
                        continue;
                    if (numberStream.Skip(j).Take(i - j).Sum() == validationNumber)
                    {
                        return numberStream.Skip(j).Take(i - j);
                    }
                }
            }
            return default;
        }
        #endregion

        #region Day 10
        public List<int> ConvertDay10Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => int.Parse(x)).ToList();
        }

        public long Day10_PuzzleOne(List<int> input)
        {
            return SetupAdapters(input, true);
        }

        public long Day10_PuzzleTwo(List<int> input)
        {
            return SetupAdapters(input, false);
        }

        public long SetupAdapters(List<int> adapterRatings, bool useAll)
        {
            adapterRatings.Add(0);
            adapterRatings.Add(adapterRatings.Max() + 3);
            var sortedAdapters = adapterRatings.OrderBy(x => x).Select((rating, index) => new { Rating = rating, Index = index }).ToList();

            if (useAll)
            {
                return sortedAdapters.Skip(1).Where(x => x.Rating - sortedAdapters[x.Index - 1].Rating == 1).Count() * sortedAdapters.Skip(1).Where(x => x.Rating - sortedAdapters[x.Index - 1].Rating == 3).Count();
            }
            else
            {
                var combinations = new Dictionary<int, long>();
                foreach (var adapter in sortedAdapters)
                {
                    long foundCombinations = 0;
                    if (adapter.Rating == 0) foundCombinations++;
                    for (var n = 1; n <= 3; n++)
                    {
                        if (combinations.ContainsKey(adapter.Rating - n)) foundCombinations += combinations[adapter.Rating - n];
                    }
                    combinations.Add(adapter.Rating, foundCombinations);
                }
                return combinations[sortedAdapters.Last().Rating];
            }
        }
        #endregion

        #region Day 11
        public char[][] ConvertDay11Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => x.Select(y => y).ToArray()).ToArray();
        }

        public long Day11_PuzzleOne(char[][] input)
        {
            return GetFinalSeatPlan(input, 4, true).Select(x => x.Count(y => y == '#')).Sum();
        }

        public long Day11_PuzzleTwo(char[][] input)
        {
            return GetFinalSeatPlan(input, 5, false).Select(x => x.Count(y => y == '#')).Sum();
        }

        public IEnumerable<IEnumerable<char>> GetFinalSeatPlan(IList<IList<char>> seatPlan, int occupiedSeatTolerance, bool adjacentSeats)
        {
            IList<IList<char>> oldSeatPlan = new List<IList<char>>();

            while (!ComparePlans(seatPlan, oldSeatPlan))
            {
                oldSeatPlan = seatPlan;
                seatPlan = AssignSeats(seatPlan, occupiedSeatTolerance, adjacentSeats);
            }
            return seatPlan;
        }

        public bool ComparePlans(IList<IList<char>> seatPlan1, IList<IList<char>> seatPlan2)
        {
            if (seatPlan1.Count != seatPlan2.Count)
                return false;
            for (var i = 0; i < seatPlan1.Count; i++)
            {
                for (var j = 0; j < seatPlan1[i].Count; j++)
                {
                    if (seatPlan1[i][j] != seatPlan2[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public IList<IList<char>> AssignSeats(IList<IList<char>> seatPlan, int occupiedSeatTolerance, bool adjacentSeats)
        {
            var newSeatPlan = seatPlan.Select(x => x.Select(y => y).ToList()).ToList();
            for (var i = 0; i < seatPlan.Count; i++)
            {
                for (var j = 0; j < seatPlan[i].Count; j++)
                {
                    if (seatPlan[i][j] == '.')
                    {
                        continue;
                    }
                    else
                    {
                        var surroundingOccupiedSeats = SurroundingOccupiedSeats(i, j, seatPlan, adjacentSeats);
                        if (seatPlan[i][j] == 'L' && surroundingOccupiedSeats == 0)
                        {
                            newSeatPlan[i][j] = '#';
                        }
                        else if (seatPlan[i][j] == '#' && surroundingOccupiedSeats >= occupiedSeatTolerance)
                        {
                            newSeatPlan[i][j] = 'L';
                        }
                    }
                }
            }

            IList<IList<char>> result = new List<IList<char>>();
            newSeatPlan.ForEach(x => result.Add(x));
            return result;
        }

        private static List<(int, int)> Directions = new List<(int, int)>() { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) };


        public int SurroundingOccupiedSeats(int seatY, int seatX, IList<IList<char>> seatPlan, bool adjacentSeats)
        {
            var result = 0;
            if (adjacentSeats)
            {
                for (var i = seatY - 1; i <= seatY + 1; i++)
                {
                    if (i < 0 || i >= seatPlan.Count)
                        continue;
                    for (var j = seatX - 1; j <= seatX + 1; j++)
                    {
                        if (j < 0 || j >= seatPlan[i].Count || (i == seatY && j == seatX))
                            continue;
                        if (seatPlan[i][j] == '#') result++;
                    }
                }
            }
            else
            {
                var foundDirections = new List<(int, int)>();
                for (var i = 1; i < seatPlan.Count; i++)
                {
                    foreach (var direction in Directions)
                    {
                        if (!foundDirections.Contains(direction))
                        {
                            var viewX = seatX + i * direction.Item2;
                            var viewY = seatY + i * direction.Item1;
                            if (viewY < 0 || viewY >= seatPlan.Count || viewX < 0 || viewX >= seatPlan[viewY].Count)
                            {
                                foundDirections.Add(direction);
                            }
                            else
                            {
                                var position = seatPlan[seatY + i * direction.Item1][seatX + i * direction.Item2];
                                if (position != '.')
                                    foundDirections.Add(direction);
                                if (position == '#')
                                    result++;
                            }
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region Day 12
        public IEnumerable<string> ConvertDay12Input(string inputPath)
        {
            return File.ReadAllLines(inputPath);
        }

        public long Day12_PuzzleOne(IEnumerable<string> input)
        {
            var ship = new Ship
            {
                UseWaypoint = false,
                VectorX = 1
            };
            foreach (var instruction in input)
            {
                ship.ProcessInstruction(instruction);
            }
            return ship.ManhattanDistance;
        }

        public long Day12_PuzzleTwo(IEnumerable<string> input)
        {
            var ship = new Ship
            {
                UseWaypoint = true,
                WaypointX = 10,
                WaypointY = 1
            };
            foreach (var instruction in input)
            {
                ship.ProcessInstruction(instruction);
            }
            return ship.ManhattanDistance;
        }

        public class Ship
        {
            private int vectorX;
            private int vectorY;

            public bool UseWaypoint { get; set; }

            public int PositionX { get; set; }

            public int PositionY { get; set; }

            public int WaypointX { get; set; }

            public int WaypointY { get; set; }

            public int VectorX
            {
                get
                {
                    if (UseWaypoint)
                        return WaypointX - PositionX;
                    return vectorX;
                }
                set
                {
                    vectorX = value;
                }
            }

            public int VectorY
            {
                get
                {
                    if (UseWaypoint)
                        return WaypointY - PositionY;
                    return vectorY;
                }
                set
                {
                    vectorY = value;
                }
            }

            public int ManhattanDistance
            {
                get
                {
                    return Math.Abs(PositionX) + Math.Abs(PositionY);
                }
            }

            public void ProcessInstruction(string instruction)
            {
                var command = Regex.Replace(instruction, @"[\d-]", string.Empty);
                int.TryParse(instruction.Replace(command, ""), out int value);
                if (command == "L" || command == "R") Rotate(command, value);
                else Move(command, value);
            }

            public void Move(string direction, int distance)
            {
                if (direction == "S" || direction == "W") distance = -distance;
                if (direction == "N" || direction == "S")
                {
                    if (UseWaypoint)
                        WaypointY += distance;
                    else
                        PositionY += distance;
                }
                if (direction == "E" || direction == "W")
                {
                    if (UseWaypoint)
                        WaypointX += distance;
                    else
                        PositionX += distance;
                }
                if (direction == "F")
                {
                    var vectorSumX = distance * VectorX;
                    var vectorSumY = distance * VectorY;
                    PositionX += vectorSumX;
                    PositionY += vectorSumY;
                    if (UseWaypoint)
                    {
                        WaypointX += vectorSumX;
                        WaypointY += vectorSumY;
                    }
                }
            }

            public void Rotate(string direction, int degree)
            {
                var radians = Math.PI * degree / 180.0;
                var lastVectorX = VectorX;
                var lastWaypointX = WaypointX;
                if (direction == "R")
                {
                    if (UseWaypoint)
                    {
                        WaypointX = Convert.ToInt32(Math.Cos(radians) * (WaypointX - PositionX) + Math.Sin(radians) * (WaypointY - PositionY)) + PositionX;
                        WaypointY = Convert.ToInt32(-Math.Sin(radians) * (lastWaypointX - PositionX) + Math.Cos(radians) * (WaypointY - PositionY)) + PositionY;
                    }
                    else
                    {
                        vectorX = Convert.ToInt32(Math.Cos(radians) * VectorX + Math.Sin(radians) * VectorY);
                        vectorY = Convert.ToInt32(-Math.Sin(radians) * lastVectorX + Math.Cos(radians) * VectorY);
                    }
                }
                else
                {
                    if (UseWaypoint)
                    {
                        WaypointX = Convert.ToInt32(Math.Cos(radians) * (WaypointX - PositionX) - Math.Sin(radians) * (WaypointY - PositionY)) + PositionX;
                        WaypointY = Convert.ToInt32(Math.Sin(radians) * (lastWaypointX - PositionX) + Math.Cos(radians) * (WaypointY - PositionY)) + PositionY;
                    }
                    else
                    {
                        vectorX = Convert.ToInt32(Math.Cos(radians) * VectorX - Math.Sin(radians) * VectorY);
                        vectorY = Convert.ToInt32(Math.Sin(radians) * lastVectorX + Math.Cos(radians) * VectorY);
                    }
                }
            }
        }
        #endregion

        #region Day 13
        public List<string> ConvertDay13Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).ToList();
        }

        public long Day13_PuzzleOne(List<string> input)
        {
            var timestamp = int.Parse(input[0]);
            var schedules = input[1].Split(',').Where(x => !x.Equals("x")).Select(x => int.Parse(x));
            var closestBus = GetClosestBus(timestamp, schedules);
            return (long)(closestBus * Math.Ceiling(timestamp / (double)closestBus) - timestamp) * closestBus;

        }

        public long Day13_PuzzleTwo(List<string> input)
        {
            return GetEarliestTimestamp(input[1].Split(','));
        }

        public int GetClosestBus(int timestamp, IEnumerable<int> schedules)
        {
            return schedules.Aggregate((x1, x2) => x1 * Math.Ceiling(timestamp / (double)x1) - timestamp < x2 * Math.Ceiling(timestamp / (double)x2) - timestamp ? x1 : x2);
        }

        public long GetEarliestTimestamp(IEnumerable<string> schedules)
        {
            var activeBuses = schedules.Select((value, index) => new { Value = value, Index = index }).Where(x => !x.Value.Equals("x")).Select(x => new { Value = long.Parse(x.Value), Index = x.Index }).OrderByDescending(x => x.Value).ToList();
            var timestamp = activeBuses.Select(x => x.Value - x.Index).First();
            var stepSize = timestamp;
            for (var i = 1; i <= activeBuses.Count; i++)
            {
                while (activeBuses.Take(i).Any(x => (timestamp + x.Index) % x.Value != 0))
                {
                    timestamp += stepSize;
                }
                stepSize = activeBuses.Take(i).Select(t => t.Value).Aggregate(LCM);

            }
            return timestamp;
        }

        private long LCM(long x, long y)
        {
            return x * y / GCD(x, y);
        }
        private long GCD(long x, long y)
        {
            return y == 0 ? x : GCD(y, x % y);
        }
        #endregion

        #region Day 14
        public Dictionary<string, List<long[]>> ConvertDay14Input(string inputPath)
        {
            return ParseDay14Input(File.ReadAllLines(inputPath));
        }

        public Dictionary<string, List<long[]>> ParseDay14Input(IEnumerable<string> input)
        {
            var result = new Dictionary<string, List<long[]>>();
            var currentMask = string.Empty;
            var currentMaskAddressUpdates = new List<long[]>();
            foreach (var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    if (currentMask != string.Empty)
                        result.Add(currentMask, currentMaskAddressUpdates);
                    currentMask = line.Split(' ').Last();
                    currentMaskAddressUpdates = new List<long[]>();
                }
                else
                {
                    var index = long.Parse(String.Join(";", Regex.Matches(line, @"\[(.+?)\]")
                                    .Cast<Match>()
                                    .Select(m => m.Groups[1].Value)));
                    var value = long.Parse(line.Split(' ').Last());

                    currentMaskAddressUpdates.Add(new long[] { index, value });
                }
            }
            result.Add(currentMask, currentMaskAddressUpdates);

            return result;
        }


        public long Day14_PuzzleOne(Dictionary<string, List<long[]>> input)
        {
            var memory = new Dictionary<long, long>();
            foreach (var mask in input)
            {
                memory = ProcessBitMask(memory, mask.Key, mask.Value, true);
            }
            return memory.Values.Sum();
        }

        public long Day14_PuzzleTwo(Dictionary<string, List<long[]>> input)
        {
            var memory = new Dictionary<long, long>();
            foreach (var mask in input)
            {
                memory = ProcessBitMask(memory, mask.Key, mask.Value, false);
            }
            return memory.Values.Sum();
        }

        public Dictionary<long, long> ProcessBitMask(Dictionary<long, long> memory, string maskInput, List<long[]> values, bool versionOne)
        {
            var mask = maskInput.ToCharArray();
            mask.Reverse();

            foreach (var value in values)
            {
                if (versionOne)
                {
                    var binary = ConvertToBit(value[1], 36);
                    for (var i = 0; i < mask.Length; i++)
                    {
                        binary[i] = mask[i] == 'X' ? binary[i] : mask[i];
                    }
                    memory[value[0]] = Convert.ToInt64(new string(binary), 2);
                }
                else
                {
                    var binary = ConvertToBit(value[0], 36);
                    for (var i = 0; i < mask.Length; i++)
                    {
                        binary[i] = mask[i] == '0' ? binary[i] : mask[i];
                    }
                    var floatingIndices = binary.Select((value, index) => new { Value = value, Index = index }).Where(x => x.Value == 'X').Select(x => x.Index).ToList();

                    if (!floatingIndices.Any())
                    {
                        memory[Convert.ToInt64(new string(binary), 2)] = value[1];
                    }
                    else
                    {
                        for (var i = 0; i < (int)Math.Pow(2, floatingIndices.Count); i++)
                        {
                            var updateBits = ConvertToBit(i, floatingIndices.Count);
                            for (var j = 0; j < floatingIndices.Count; j++)
                            {
                                binary[floatingIndices[j]] = updateBits[j];
                            }
                            memory[Convert.ToInt64(new string(binary), 2)] = value[1];
                        }
                    }
                }

            }
            return memory;
        }

        private char[] ConvertToBit(long input, int length)
        {
            var bitValue = Convert.ToString(input, 2);
            while (bitValue.Length < length)
            {
                bitValue = "0" + bitValue;
            }
            var bit = bitValue.ToCharArray();
            bit.Reverse();
            return bit;
        }
        #endregion

        #region Day 15
        public List<int> ConvertDay15Input(string inputPath)
        {
            return File.ReadAllText(inputPath).Split(',').Select(x => int.Parse(x)).ToList();
        }


        public long Day15_PuzzleOne(List<int> input)
        {
            return MemorySolver(input, 2020);
        }

        public long Day15_PuzzleTwo(List<int> input)
        {
            return MemorySolver(input, 30000000);
        }

        public long MemorySolver(List<int> starterNumbers, int resultTurn)
        {
            var recentNumbers = new Dictionary<int, int>();
            for (int i = 0; i < starterNumbers.Count - 1; i++)
                recentNumbers[starterNumbers[i]] = i + 1;

            var lastValue = starterNumbers.Last();

            for (int turn = starterNumbers.Count; turn < resultTurn; turn++)
            {
                if (!recentNumbers.TryGetValue(lastValue, out int previous))
                    previous = -1;
                recentNumbers[lastValue] = turn;
                lastValue = previous != -1 ? turn - previous : 0;
            }
            return lastValue;
        }
        #endregion

        #region Day 16
        public TicketOverview ConvertDay16Input(string inputPath)
        {
            return ParseTicketOverview(File.ReadAllLines(inputPath).ToList());
        }

        public TicketOverview ParseTicketOverview(List<string> input)
        {
            var lines = input.Select((line, index) => new { Line = line, Index = index });
            var lastEmptyline = -1;
            var blocks = new List<List<string>>();
            foreach (var index in lines.Where(x => string.IsNullOrEmpty(x.Line)).Select(x => x.Index))
            {
                blocks.Add(lines.Skip(lastEmptyline + 1).Take(index - lastEmptyline - 1).Select(x => x.Line).ToList());
                lastEmptyline = index;
            }
            blocks.Add(lines.Skip(lastEmptyline + 1).Select(x => x.Line).ToList());

            return new TicketOverview
            {
                Rules = blocks[0].ToDictionary(
                                    x => x.Split(':')[0],
                                    x => x.Split(':')[1].Split(new string[] { "or" }, StringSplitOptions.None).Select(y => new Tuple<int, int>(int.Parse(y.Trim().Split('-')[0]), int.Parse(y.Trim().Split('-')[1]))).ToList()),
                OwnTicket = blocks[1][1].Split(',').Select(x => int.Parse(x)).ToList(),
                NearbyTickets = blocks[2].Skip(1).Select(x => x.Split(',').Select(y => int.Parse(y)).ToList()).ToList()
            };
        }


        public int Day16_PuzzleOne(TicketOverview input)
        {
            return input.ScanningErrorRate;
        }

        public long Day16_PuzzleTwo(TicketOverview input)
        {
            input.DiscardInvalidTickets();
            var ticket = input.AllignTicketValuesWithRules();
            return ticket.Where(x => x.Key.StartsWith("departure")).Select(x => (long)x.Value).Aggregate((a, x) => a * x);
        }

        public class TicketOverview
        {
            public Dictionary<string, List<Tuple<int, int>>> Rules { get; set; }

            public List<int> OwnTicket { get; set; }

            public List<List<int>> NearbyTickets { get; set; }

            public void DiscardInvalidTickets()
            {
                NearbyTickets = NearbyTickets.Where(x => !x.Any(y => !CompliesToAnyRule(y))).ToList();
            }

            public Dictionary<string, int> AllignTicketValuesWithRules()
            {
                NearbyTickets.Add(OwnTicket);
                var ticketFields = NearbyTickets.SelectMany(inner => inner.Select((item, index) => new { item, index }))
                                    .GroupBy(i => i.index, i => i.item)
                                    .Select(g => g.ToList())
                                    .Select((value, Index) => new { Value = value, Index = Index })
                                    .ToList();
                var ruleMatches = Rules.SelectMany(x => ticketFields.Where(y => !y.Value.Any(z => !CompliesToRule(x.Value, z))).Select(y => new Tuple<string, int>(x.Key, y.Index)).ToList()).ToList();
                ruleMatches.Sort((a, b) => ruleMatches.Count(x => x.Item1 == a.Item1) - ruleMatches.Count(x => x.Item1 == b.Item1));

                while (ruleMatches.Any(x => ruleMatches.Count(y => y.Item1 == x.Item1) > 1))
                {
                    foreach (var singleMatch in ruleMatches.Where(y => ruleMatches.Count(z => z.Item1 == y.Item1) == 1 && ruleMatches.Count(z => z.Item2 == y.Item2) > 1))
                    {
                        ruleMatches = ruleMatches.Where(x => x.Item2 != singleMatch.Item2 || (x.Item1 == singleMatch.Item1)).ToList();
                    }
                }
                return ruleMatches.OrderBy(x => x.Item2).ToDictionary(x => x.Item1, x => OwnTicket[x.Item2]);
            }

            private bool CompliesToRule(List<Tuple<int, int>> rule, int ticketValue)
            {
                return rule.Any(x => x.Item1 <= ticketValue && x.Item2 >= ticketValue);
            }

            private bool CompliesToAnyRule(int ticketValue)
            {
                return Rules.Any(x => CompliesToRule(x.Value, ticketValue));
            }

            public int ScanningErrorRate
            {
                get
                {
                    return NearbyTickets.SelectMany(x => x.Select(y => y)).Where(x => !CompliesToAnyRule(x)).Sum();
                }
            }
        }
        #endregion

        #region Day 17
        public IEnumerable<(int, int, int, int)> ConvertDay17Input(string inputPath)
        {
            return ParseDay17Input(File.ReadAllLines(inputPath));
        }

        public IEnumerable<(int, int, int, int)> ParseDay17Input(IEnumerable<string> input)
        {
            return input.SelectMany((l, x) =>
                l.Select((c, y) => (c, y)).Where(ay => ay.c == '#')
                .Select(ay => (x: x, y: ay.y, z: 0, w: 0))
            );
        }


        public int Day17_PuzzleOne(IEnumerable<(int, int, int, int)> input)
        {
            return RunGridCycles(input, 6, false);
        }

        public long Day17_PuzzleTwo(IEnumerable<(int, int, int, int)> input)
        {
            return RunGridCycles(input, 6, true);
        }

        public int RunGridCycles(IEnumerable<(int, int, int, int)> input, int iterations, bool fourthDimension)
        {
            var neighbors = Enumerable.Range(-1, 3)
            .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .SelectMany(z => Enumerable.Range(-1, 3)
                            .Select(w => (x: x, y: y, z: z, w: w))
                        )
                    )
            ).ToImmutableList();
            neighbors = neighbors.Remove((0, 0, 0, 0));

            var activeHyperCube = input.ToImmutableHashSet();
            int passes = 0;
            while (passes < iterations)
            {
                passes++;

                var rangeX = ExpanseRange(0, activeHyperCube);
                var rangeY = ExpanseRange(1, activeHyperCube);
                var rangeZ = ExpanseRange(2, activeHyperCube);
                var rangeW = ExpanseRange(3, activeHyperCube);

                var theExpanse = rangeX
                    .SelectMany(x => rangeY
                        .SelectMany(y => rangeZ
                            .SelectMany(z =>
                                fourthDimension switch
                                {
                                    true => rangeW.Select(w => (x: x, y: y, z: z, w: w)),
                                    false => new List<(int x, int y, int z, int w)> { (x: x, y: y, z: z, w: 0) }
                                })
                        )
                    ).ToImmutableHashSet();

                activeHyperCube = theExpanse.AsParallel().Where(expanse =>
                {
                    var nCount = neighbors.Count(nbr => activeHyperCube.Contains(CoordAdd(expanse, nbr)));
                    return activeHyperCube.Contains(expanse) switch
                    {
                        true => nCount >= 2 && nCount <= 3,
                        false => nCount == 3
                    };
                }).ToImmutableHashSet();
            }

            return activeHyperCube.Count();
        }
        IEnumerable<int> ExpanseRange(int coordIx, IEnumerable<(int x, int y, int z, int w)> input)
        {
            var coordValues = input.Select(i => coordIx switch
            {
                0 => i.x,
                1 => i.y,
                2 => i.z,
                3 => i.w,
                _ => 0
            }).Distinct();

            return Enumerable.Range(coordValues.Min() - 1, coordValues.Max() - coordValues.Min() + 3);
        }

        (int x, int y, int z, int w) CoordAdd((int x, int y, int z, int w) a, (int x, int y, int z, int w) b)
        {
            return (x: a.x + b.x, y: a.y + b.y, z: a.z + b.z, w: a.w + b.w);
        }
        #endregion

        #region Day 18
        public List<List<char>> ConvertDay18Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => $"({x})".Replace(" ", string.Empty).ToCharArray().ToList()).ToList();
        }

        public long Day18_PuzzleOne(List<List<char>> input)
        {
            return input.Select(x => SolveTerm(x, false)).Sum();
        }

        public long Day18_PuzzleTwo(List<List<char>> input)
        {
            return input.Select(x => SolveTerm(x, true)).Sum();
        }

        public long SolveTerm(List<char> term, bool additionPrecedence)
        {
            var openingBrackets = term.Select((x, index) => new { X = x, Index = index }).Where(x => x.X == '(').Select(x => x.Index).ToList();
            var closingBrackets = term.Select((x, index) => new { X = x, Index = index }).Where(x => x.X == ')').Select(x => x.Index).ToList();
            var bracketRanges = openingBrackets.Select(ob => (a: ob, b: closingBrackets.Where(cb => cb > ob && openingBrackets.Count(x => x > ob && x < cb) - closingBrackets.Count(x => x > ob && x < cb) == 0).First())).ToList();

            if (additionPrecedence)
            {
                var additions = term.Count(x => x == '+');
                for (var i = 0; i < term.Count(x => x == '+'); i++)
                {
                    var adjustTerm = term.Select(x => x.ToString()).ToList();
                    var additionIndex = term.Select((x, index) => new { X = x, Index = index }).Where(x => x.X == '+').Skip(i).Select(x => x.Index).First();
                    var bottomBracket = bracketRanges.FirstOrDefault(x => x.Item2 == additionIndex - 1);
                    var bottomAdjustIndex = bottomBracket == default ? additionIndex - 1 : bottomBracket.a;

                    var topBracket = bracketRanges.FirstOrDefault(x => x.Item1 == additionIndex + 1);
                    var topAdjustIndex = topBracket == default ? additionIndex + 1 : topBracket.b;

                    if (!bracketRanges.Any(x => x.a == bottomAdjustIndex - 1 && x.b == topAdjustIndex + 1))
                    {
                        adjustTerm[bottomAdjustIndex] = "(" + adjustTerm[bottomAdjustIndex];
                        adjustTerm[topAdjustIndex] = adjustTerm[topAdjustIndex] + ")";
                    }

                    term = adjustTerm.SelectMany(x => x.ToCharArray()).ToList();
                    openingBrackets = term.Select((x, index) => new { X = x, Index = index }).Where(x => x.X == '(').Select(x => x.Index).ToList();
                    closingBrackets = term.Select((x, index) => new { X = x, Index = index }).Where(x => x.X == ')').Select(x => x.Index).ToList();
                    bracketRanges = openingBrackets.Select(ob => (a: ob, b: closingBrackets.Where(cb => cb > ob && openingBrackets.Count(x => x > ob && x < cb) - closingBrackets.Count(x => x > ob && x < cb) == 0).First())).ToList();
                }
            }
            return SolveBracket(term, bracketRanges, (0, term.Count - 1));
        }


        private long SolveBracket(List<char> term, List<(int, int)> allBrackets, (int, int) bracket)
        {
            long val1 = 0;
            for (var i = bracket.Item1 + 1; i < bracket.Item2; i++)
            {
                var subBracket = allBrackets.FirstOrDefault(x => x.Item1 == i);
                if (subBracket == default && !char.IsDigit(term[i]))
                    continue;

                var val2 = subBracket == default ? (int)char.GetNumericValue(term[i]) : SolveBracket(term, allBrackets, subBracket);

                val1 = i == bracket.Item1 + 1 || term[i - 1] == '+' ? val1 + val2 : val1 * val2;

                if (subBracket != default)
                    i = subBracket.Item2;
                else
                    i++;
            }
            return val1;
        }
        #endregion

        #region Day 19
        public (Dictionary<string, string>, List<string>) ConvertDay19Input(string inputPath)
        {
            var segments = File.ReadAllText(inputPath).Split("\r\n\r\n");
            return (segments[0].Split("\r\n")
                            .Select(x => x.Split(':', StringSplitOptions.TrimEntries))
                            .ToDictionary(x => x[0], x => x[1]),
                            segments[1].Split("\r\n").ToList());
        }

        public long Day19_PuzzleOne((Dictionary<string, string>, List<string>) input)
        {
            var processed = new Dictionary<string, string>();
            var regex = new Regex("^" + BuildRegex(input.Item1, processed, "0") + "$");
            return input.Item2.Count(regex.IsMatch);
        }

        public long Day19_PuzzleTwo((Dictionary<string, string>, List<string>) input)
        {
            var processed = new Dictionary<string, string>();
            var regex = new Regex($@"^({BuildRegex(input.Item1, processed, "42")})+(?<open>{BuildRegex(input.Item1, processed, "42")})+(?<close-open>{BuildRegex(input.Item1, processed, "31")})+(?(open)(?!))$");
            return input.Item2.Count(regex.IsMatch);
        }

        public string BuildRegex(Dictionary<string, string> rules, Dictionary<string, string> processed, string input)
        {
            if (processed.TryGetValue(input, out var s))
                return s;

            var orig = rules[input];
            if (orig.StartsWith('\"'))
                return processed[input] = orig.Replace("\"", "");

            if (!orig.Contains("|"))
                return processed[input] = string.Join("", orig.Split().Select(x => BuildRegex(rules, processed, x)));

            return processed[input] = $"({string.Join("", orig.Split().Select(x => x == "|" ? x : BuildRegex(rules, processed, x)))})";
        }
        #endregion

        #region Day 19
        public List<Tile> ConvertDay20Input(string inputPath)
        {
            var segments = File.ReadAllText(inputPath).Replace("#", "1").Replace(".", "0").Split("\r\n\r\n");
            return segments.Select(x => new Tile(x.Trim().Split("\r\n"))).ToList();
        }

        public long Day20_PuzzleOne(List<Tile> input)
        {
            return SolvePuzzle(input);
        }

        public long Day20_PuzzleTwo(List<Tile> input)
        {
            return default;
        }

        public long SolvePuzzle(List<Tile> tiles)
        {
            var puzzleDimension = (int)Math.Sqrt(tiles.Count);

            var possibleCorners = tiles.SelectMany(x => CornerAlternatives(x, tiles)).ToList();
            var borderAlternatives = new List<(List<Tile> UsedTiles, Tile[,] Puzzle)>();
            for (var i = 0; i < possibleCorners.Count; i++)
            {
                var possibleCorner = possibleCorners[i];
                var puzzle = new Tile[puzzleDimension, puzzleDimension];
                puzzle[0, 0] = possibleCorner.Puzzle[0, 0];
                puzzle[0, 1] = possibleCorner.Puzzle[0, 1];
                puzzle[1, 0] = possibleCorner.Puzzle[1, 0];
                puzzle[1, 1] = possibleCorner.Puzzle[1, 1];
                possibleCorner.Puzzle = puzzle;

                var alternatives = BorderAlternatives(puzzleDimension, possibleCorner);
                if (alternatives != null)
                    borderAlternatives.AddRange(alternatives);
            }
            var possibleResults = WalkLines(puzzleDimension, borderAlternatives);
            var selectedResult = possibleResults.FirstOrDefault();
            if (selectedResult == default) return default;
            return selectedResult.Puzzle[0, 0].ID * selectedResult.Puzzle[0, puzzleDimension - 1].ID * selectedResult.Puzzle[puzzleDimension - 1, 0].ID * selectedResult.Puzzle[puzzleDimension - 1, puzzleDimension - 1].ID;
        }

        public List<(List<Tile> LeftTiles, Tile[,] Puzzle)> CornerAlternatives(Tile cornerTile, List<Tile> availableTiles)
        {
            var result = new List<(List<Tile> LeftTiles, Tile[,] Puzzle)>();
            foreach (var combination in cornerTile.Combinations)
            {
                var botMatches = new List<Tile>();
                foreach (var checkTile in availableTiles.Where(x => x.ID != cornerTile.ID))
                {
                    foreach (var checkTileCombination in checkTile.Combinations.Where(x => x.Top == combination.Bot))
                    {
                        botMatches.Add(new Tile
                        {
                            ID = checkTile.ID,
                            SelectedCombination = checkTileCombination
                        });
                    }
                }
                if (botMatches.Count == 0)
                    continue;

                var rightMatches = new List<Tile>();
                foreach (var checkTile in availableTiles.Where(x => x.ID != cornerTile.ID && !botMatches.Any(y => y.ID == x.ID)))
                {
                    foreach (var checkTileCombination in checkTile.Combinations.Where(x => x.Left == combination.Right))
                    {
                        rightMatches.Add(new Tile
                        {
                            ID = checkTile.ID,
                            SelectedCombination = checkTileCombination
                        });
                    }
                }
                if (rightMatches.Count == 0)
                    continue;

                foreach (var botMatch in botMatches)
                {
                    foreach (var rightMatch in rightMatches)
                    {
                        if (availableTiles.Any(x => x.ID != cornerTile.ID && x.ID != botMatch.ID && x.ID != rightMatch.ID && x.Combinations.Any(y => y.Top == rightMatch.SelectedCombination.Bot && y.Left == botMatch.SelectedCombination.Right)))
                        {
                            result.Add((availableTiles.Where(x => x.ID != cornerTile.ID && x.ID != botMatch.ID && x.ID != rightMatch.ID).ToList(), new Tile[,] { { new Tile { ID = cornerTile.ID, SelectedCombination = combination }, rightMatch }, { botMatch, null } }));
                        }
                        /*
                        foreach (var blockMatch in availableTiles.Where(x => x.ID != cornerTile.ID && x.ID != botMatch.ID && x.ID != rightMatch.ID ))
                        {
                            foreach(var blockCombination in blockMatch.Combinations)
                            {
                                if(blockCombination.Top == rightMatch.SelectedCombination.Bot && blockCombination.Left == botMatch.SelectedCombination.Right)
                                {
                                    blockMatch.SelectedCombination = blockCombination;
                                    result.Add(new Tile[,] { { new Tile { ID = cornerTile.ID, SelectedCombination = combination }, rightMatch }, { botMatch, new Tile { ID = blockMatch.ID, SelectedCombination = blockCombination } } });
                                }
                                
                            }
                        }
                        */
                    }
                }
            }

            return result;
        }

        public List<(List<Tile> LeftTiles, Tile[,] Puzzle)> BorderAlternatives(int puzzleDimension, (List<Tile> LeftTiles, Tile[,] Puzzle) corner)
        {

            if (corner.Puzzle[0, 0].ID == 1951 && corner.Puzzle[0, 1].ID == 2311 && corner.Puzzle[1, 0].ID == 2729)
            {
                var asdf = "";
            }
            var possibleAlternatives = new List<(List<Tile> LeftTiles, Tile[,] Puzzle)> { (new List<Tile>(corner.LeftTiles), (Tile[,])corner.Puzzle.Clone()) };
            for (var i = 2; i < puzzleDimension; i++)
            {
                var newAlternatives = new List<(List<Tile> UsedTiles, Tile[,] Puzzle)>();
                foreach (var possibleAlternative in possibleAlternatives)
                {
                    foreach (var possibleTile in possibleAlternative.LeftTiles)
                    {
                        foreach (var combination in possibleTile.Combinations)
                        {
                            if (combination.Left == possibleAlternative.Puzzle[0, i - 1].SelectedCombination.Right)
                            {
                                var matchTile = new Tile
                                {
                                    ID = possibleTile.ID,
                                    SelectedCombination = combination
                                };
                                var alternative = (Tile[,])possibleAlternative.Puzzle.Clone();
                                alternative[0, i] = matchTile;
                                var leftTilesAlternative = (new List<Tile>(possibleAlternative.LeftTiles)).Where(x=>x.ID != matchTile.ID).ToList();
                                newAlternatives.Add((leftTilesAlternative, alternative));
                            }
                        }
                    }
                }
                if (newAlternatives.Count == 0)
                    return default;
                possibleAlternatives = newAlternatives;
            }
            for (var i = 2; i < puzzleDimension; i++)
            {
                var newAlternatives = new List<(List<Tile> UsedTiles, Tile[,] Puzzle)>();
                foreach (var possibleAlternative in possibleAlternatives)
                {
                    foreach (var possibleTile in possibleAlternative.LeftTiles)
                    {
                        foreach (var combination in possibleTile.Combinations)
                        {
                            if (combination.Top == possibleAlternative.Puzzle[i - 1, 0].SelectedCombination.Bot)
                            {
                                possibleTile.SelectedCombination = combination;
                                var alternative = (Tile[,])possibleAlternative.Puzzle.Clone();
                                alternative[i, 0] = possibleTile;
                                var leftTilesAlternative = new List<Tile>(possibleAlternative.LeftTiles);
                                leftTilesAlternative.Remove(possibleTile);
                                if (!newAlternatives.Any(x => x.Puzzle.Equals(alternative)))
                                    newAlternatives.Add((leftTilesAlternative, alternative));
                            }
                        }
                    }
                }
                if (newAlternatives.Count == 0)
                    return default;
                possibleAlternatives = newAlternatives;
            }
            return possibleAlternatives;
        }

        public List<(List<Tile> LeftTiles, Tile[,] Puzzle)> WalkLines(int puzzleDimension, List<(List<Tile> LeftTiles, Tile[,] Puzzle)> alternatives)
        {
            var possibleAlternatives = new List<(List<Tile> LeftTiles, Tile[,] Puzzle)>(alternatives);
            for (var i = 1; i < puzzleDimension; i++)
            {
                for (var j = 1; j < puzzleDimension; j++)
                {
                    var newAlternatives = new List<(List<Tile> UsedTiles, Tile[,] Puzzle)>();
                    foreach (var possibleAlternative in possibleAlternatives)
                    {
                        if (possibleAlternative.Puzzle[0, 0].ID == 1951 && possibleAlternative.Puzzle[0, 1].ID == 2311 && possibleAlternative.Puzzle[1, 0].ID == 2729)
                        {
                            var asdf = "";
                        }
                        foreach (var possibleTile in possibleAlternative.LeftTiles)
                        {
                            foreach (var combination in possibleTile.Combinations)
                            {
                                if (combination.Left == possibleAlternative.Puzzle[i, j - 1].SelectedCombination.Right && combination.Top == possibleAlternative.Puzzle[i - 1, j].SelectedCombination.Bot)
                                {
                                    possibleTile.SelectedCombination = combination;
                                    var alternative = (Tile[,])possibleAlternative.Puzzle.Clone();
                                    alternative[i, j] = possibleTile;
                                    var usedTilesAlternative = new List<Tile>(possibleAlternative.LeftTiles);
                                    usedTilesAlternative.Remove(possibleTile);
                                    newAlternatives.Add((usedTilesAlternative, alternative));
                                }
                            }
                        }
                    }
                    possibleAlternatives = newAlternatives;
                }
            }

            return possibleAlternatives;
        }
        public class Tile
        {
            public Tile()
            {

            }

            public Tile(IEnumerable<string> input)
            {
                ID = long.Parse(input.First().Replace("Tile ", "").Replace(":", ""));
                var data = input.Skip(1).ToArray();
                if (ID == 2473)
                {
                    var asd = "";
                }

                var dataBase = data.Select(x => x.ToArray()).ToArray();

                Data = new char[data.Length, data.Length];
                for (var i = 0; i < data.Length; ++i)
                {
                    for (var j = 0; j < data[i].Length; ++j)
                    {
                        Data[i, j] = data[i][j];
                    }
                }
                var dimension = data.Length - 1;
                Combinations = new HashSet<(int Top, int Right, int Bot, int Left)>();
                for (var i = 0; i < 4; i++)
                {
                    var shiftArray = RotateData(i);
                    var top = Convert.ToInt32(String.Join("", shiftArray.GetRow(0)), 2);
                    var bot = Convert.ToInt32(String.Join("", shiftArray.GetRow(dimension)), 2);

                    var topInvert = Convert.ToInt32(data[0].Reverse(), 2);
                    var botInvert = Convert.ToInt32(String.Join("", shiftArray.GetRow(dimension)).Reverse(), 2);

                    var leftSideInvert = String.Join("", shiftArray.GetColumn(0));
                    var rightSideInvert = String.Join("", shiftArray.GetColumn(dimension));

                    var left = Convert.ToInt32(leftSideInvert.Reverse(), 2);
                    var leftInvert = Convert.ToInt32(leftSideInvert, 2);
                    var right = Convert.ToInt32(rightSideInvert.Reverse(), 2);
                    var rightInvert = Convert.ToInt32(rightSideInvert, 2);

                    Combinations.Add((top, right, bot, left));
                    Combinations.Add((bot, rightInvert, top, leftInvert));
                    Combinations.Add((topInvert, left, botInvert, right));
                    Combinations.Add((botInvert, leftInvert, topInvert, rightInvert));
                }
            }

            public long ID { get; set; }

            public (int Top, int Right, int Bot, int Left) SelectedCombination { get; set; }

            public HashSet<(int Top, int Right, int Bot, int Left)> Combinations { get; set; }

            public char[,] Data { get; set; }

            public char[,] RotateData(int turns)
            {
                var result = (char[,])Data.Clone();
                var length = (int)Math.Sqrt(Data.Length);
                for (var n = 1; n <= turns; n++)
                {
                    char[,] ret = new char[length, length];

                    for (int i = 0; i < length; ++i)
                    {
                        for (int j = 0; j < length; ++j)
                        {
                            ret[i, j] = result[length - j - 1, i];
                        }
                    }
                    result = ret;
                }
                return result;
            }
            #endregion
        }
    }

    public static class Extensions
    {
        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static char[] GetColumn(this char[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        public static char[] GetRow(this char[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }
}
