using AdventOfCode.Extensions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode.Services
{
    public class AdventOfCode2023
    {
        public AdventOfCode2023()
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
        public List<string> ConvertDay1Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).ToList();
        }

        public int Day1_PuzzleOne(List<string> input)
        {
            return input.Sum(x => ExtractDigits(x, false));
        }

        public int Day1_PuzzleTwo(List<string> input)
        {
            return input.Sum(x => ExtractDigits(x, true));
        }

        public int ExtractDigits(string input, bool includeWrittenDigits)
        {
            var digits = new List<int>();

            var digitMap = new Dictionary<string, int>()
            {
                {"one", 1 },
                {"two", 2 },
                {"three", 3 },
                {"four", 4 },
                {"five", 5 },
                {"six", 6 },
                {"seven", 7 },
                {"eight", 8 },
                {"nine", 9 },
            };

            for (var i = 0; i < input.Length; i++)
            {
                if (int.TryParse(input[i].ToString(), out int val))
                {
                    digits.Add(val);
                }
                else if (includeWrittenDigits)
                {
                    foreach (var digitMapping in digitMap.Where(x => x.Key.StartsWith(input[i])))
                    {
                        if (input.Length >= i + digitMapping.Key.Length && input.Substring(i, digitMapping.Key.Length) == digitMapping.Key)
                        {
                            digits.Add(digitMapping.Value);
                        }

                    }
                }
            }

            return 10 * digits.First() + digits.Last();
        }
        #endregion

        #region Day 02
        public int Day2_PuzzleOne(List<(int GameID, List<Dictionary<string, int>> Sets)> input)
        {
            var possibilityCondition = "12 red, 13 green, 14 blue";
            return input.Where(x => GameIsPossible(possibilityCondition, x.Sets)).Sum(x => x.GameID);
        }

        public int Day2_PuzzleTwo(List<(int GameID, List<Dictionary<string, int>> Sets)> input)
        {
            return input.Sum(x => GetSetPower(x.Sets));
        }

        public List<(int GameID, List<Dictionary<string, int>> Sets)> ConvertDay2Input(string inputPath)
        {
            var input = new List<(int GameID, List<Dictionary<string, int>> Sets)>();
            foreach (var line in File.ReadAllLines(inputPath))
            {
                input.Add(ConvertDescriptionToGame(line));
            }
            return input;
        }

        public (int GameID, List<Dictionary<string, int>> Sets) ConvertDescriptionToGame(string gameDescription)
        {
            var sets = new List<Dictionary<string, int>>();
            var gameID = int.Parse(gameDescription.Split(": ")[0].Replace("Game ", ""));
            foreach (var set in gameDescription.Split(": ")[1].Split("; "))
            {
                sets.Add(set.Split(", ").ToDictionary(
                    x => x.Split(' ')[1],
                    x => int.Parse(x.Split(' ')[0])));

            }
            return (gameID, sets);
        }

        public Dictionary<string, int> GetMaxNumberOfColor(List<Dictionary<string, int>> sets)
        {
            var result = new Dictionary<string, int>();
            foreach (var set in sets)
            {
                foreach (var color in set)
                {
                    if (result.TryGetValue(color.Key, out int currentMaxValue))
                    {
                        if (color.Value > currentMaxValue)
                        {
                            result[color.Key] = color.Value;
                        }
                    }
                    else
                    {
                        result[color.Key] = color.Value;
                    }
                }
            }
            return result;
        }

        public bool GameIsPossible(string possibilityConditions, List<Dictionary<string, int>> game)
        {
            var conditions = possibilityConditions.Split(", ").ToDictionary(
                x => x.Split(" ")[1],
            x => int.Parse(x.Split(" ")[0]));

            var gameMaxValues = GetMaxNumberOfColor(game);
            foreach (var condition in conditions)
            {
                if (gameMaxValues.TryGetValue(condition.Key, out int maxNumber))
                {
                    if (maxNumber > condition.Value)
                        return false;
                }
            }
            return true;
        }
        
        public int GetSetPower(List<Dictionary<string, int>> sets)
        {
            var maxValues = GetMaxNumberOfColor(sets);
            return maxValues.Select(x => x.Value).Aggregate((a, b) => a * b);
        }
        #endregion
    }
}

