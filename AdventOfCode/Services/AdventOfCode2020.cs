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

        #region Day1 
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

        #region Day2
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

        #region Day3
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

        #region Day4
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

        #region Day5
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
    }
}
