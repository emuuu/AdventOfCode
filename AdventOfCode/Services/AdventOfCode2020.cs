using AdventOfCode.Extensions;
using System;
using System.Collections.Generic;
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
            if (inputMethod != null)
            {
                if (puzzle1Method != null)
                {
                    var input = inputMethod.Invoke(this, new object[] { inputPath });
                    result.Add("Puzzle 1: " + puzzle1Method.Invoke(this, new object[] { input }));
                }
                else
                {
                    result.Add("Puzzle 1: not implemented");
                }
                if (puzzle2Method != null)
                {
                    var input = inputMethod.Invoke(this, new object[] { inputPath });
                    result.Add("Puzzle 2: " + puzzle2Method.Invoke(this, new object[] { input }));
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
    }
}
