using AdventOfCode.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode.Services
{
    public class AdventOfCode2019
    {
        public AdventOfCode2019()
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
        public string[] ConvertDay1Input(string inputPath)
        {
            return File.ReadAllLines(inputPath);
        }

        public int Day1_PuzzleOne(string[] input)
        {
            return input.Select(x => CalculateFuel(int.Parse(x), false)).Sum();
        }

        public int Day1_PuzzleTwo(string[] input)
        {
            return input.Select(x => CalculateFuel(int.Parse(x), true)).Sum();
        }

        public int CalculateFuel(int mass, bool calcFuelMass)
        {
            var fuel = mass / 3 - 2;
            if (calcFuelMass) return (fuel <= 0) ? 0 : fuel + CalculateFuel(fuel, calcFuelMass);
            else return (fuel <= 0) ? 0 : fuel;
        }
        #endregion

        #region Day 2
        public int[] ConvertDay2Input(string inputPath)
        {
            return File.ReadAllText(inputPath).Split(',').Select(x => int.Parse(x)).ToArray();
        }

        public int Day2_PuzzleOne(int[] input)
        {
            input[1] = 12;
            input[2] = 2;
            input = IntcodeSolver(input);
            return input[0];
        }

        public int Day2_PuzzleTwo(int[] input)
        {
            for (var i = 1; i < 99; i++)
            {
                for (var j = 1; j < 99; j++)
                {
                    var resetInput = new int[input.Length];
                    input.CopyTo(resetInput, 0);
                    resetInput[1] = i;
                    resetInput[2] = j;
                    if (IntcodeSolver(resetInput)[0] == 19690720)
                    {
                        return 100 * i + j;
                    }
                }
            }
            return default;
        }

        public int[] IntcodeSolver_old(int[] input)
        {
            for (var i = 0; i < input.Length; i = i + 4)
            {
                switch (input[i])
                {
                    case 1:
                        {
                            input[input[i + 3]] = input[input[i + 1]] + input[input[i + 2]];
                            break;
                        }
                    case 2:
                        {
                            input[input[i + 3]] = input[input[i + 1]] * input[input[i + 2]];
                            break;
                        }
                    case 99:
                        {
                            return input;
                        }
                    default:
                        {
                            throw new Exception("Unexpected opcode");
                        }
                }
            }
            throw new Exception("Reached end without opcode");
        }
        #endregion

        #region Day 3
        public string[][] ConvertDay3Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => x.Split(',')).ToArray();
        }

        public int Day3_PuzzleOne(string[][] input)
        {
            var shortestDistance = int.MaxValue;

            var path1 = input[0];
            var path2 = input[1];

            var wire1 = new List<Point>() { new Point(0, 0) };
            var wire2 = new List<Point>() { new Point(0, 0) };

            for (var i = 0; i < ((path1.Length > path2.Length) ? path1.Length : path2.Length); i++)
            {
                if (i < path1.Length)
                {
                    wire1.AddRange(CommandSolver(wire1.Last(), path1[i]));
                }
                if (i < path2.Length)
                {
                    wire2.AddRange(CommandSolver(wire2.Last(), path2[i]));
                }

            }
            foreach (var point in wire1)
            {
                if (wire2.Any(x => x.X == point.X && x.Y == point.Y && (point.X != 0 || x.Y != 0)))
                {
                    var distance = ManhattanDistance(point.X, 0, point.Y, 0);
                    shortestDistance = (distance < shortestDistance) ? distance : shortestDistance;
                }
            }
            return shortestDistance;
        }

        public int Day3_PuzzleTwo(string[][] input)
        {
            var leastSteps = int.MaxValue;

            var path1 = input[0];
            var path2 = input[1];

            var wire1 = new List<Point>() { new Point(0, 0) };
            var wire2 = new List<Point>() { new Point(0, 0) };

            for (var i = 0; i < ((path1.Length > path2.Length) ? path1.Length : path2.Length); i++)
            {
                if (i < path1.Length)
                {
                    wire1.AddRange(CommandSolver(wire1.Last(), path1[i]));
                }
                if (i < path2.Length)
                {
                    wire2.AddRange(CommandSolver(wire2.Last(), path2[i]));
                }

            }
            foreach (var point in wire1)
            {
                foreach (var interesction in wire2.Where(x => x.X == point.X && x.Y == point.Y && (point.X != 0 || x.Y != 0)))
                {
                    var wire1Steps = wire1.IndexOf(point); //no +1 because of initial point (0,0);
                    var wire2Steps = wire2.IndexOf(point);
                    var totalSteps = wire1Steps + wire2Steps;
                    leastSteps = (totalSteps < leastSteps) ? totalSteps : leastSteps;
                }
            }
            return leastSteps;
        }

        public int ManhattanDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public List<Point> CommandSolver(Point point, string command)
        {
            var points = new List<Point>();
            var steps = int.Parse(command.Remove(0, 1));
            for (var i = 1; i <= steps; i++)
            {
                switch (command[0])
                {
                    case 'L':
                        {
                            point.X--;
                            break;
                        }
                    case 'R':
                        {
                            point.X++;
                            break;
                        }
                    case 'U':
                        {
                            point.Y++;
                            break;
                        }
                    case 'D':
                        {
                            point.Y--;
                            break;
                        }
                    default:
                        {
                            throw new Exception("Unexpected command");
                        }
                }
                points.Add(point);
            }
            return points;
        }
        #endregion

        #region Day 4
        public int[] ConvertDay4Input(string inputPath)
        {
            return File.ReadAllText(inputPath).Split('-').Select(x => int.Parse(x)).ToArray();
        }

        public int Day4_PuzzleOne(int[] input)
        {
            var validPasswords = 0;
            while (input[0] <= input[1])
            {
                if (MatchesCritera(input[0], false))
                {
                    validPasswords++;
                }
                input[0]++;
            }
            return validPasswords;
        }

        public int Day4_PuzzleTwo(int[] input)
        {
            var validPasswords = 0;
            while (input[0] <= input[1])
            {
                if (MatchesCritera(input[0], true))
                {
                    validPasswords++;
                }
                input[0]++;
            }
            return validPasswords;
        }

        public bool MatchesCritera(int input, bool partTwo)
        {
            var n = input;
            var digits = new List<int>();
            var blockedDigits = new List<Tuple<int, int>>();

            while (n != 0)
            {
                digits.Add(n % 10);
                n /= 10;
            }

            if (digits.Count != 6)
            {
                return false;
            }

            digits.Reverse();
            for (var i = 0; i < digits.Count; i++)
            {
                if (i > 0 && digits[i - 1] > digits[i])
                {
                    return false;
                }

                Tuple<int, int> lastBlock;
                if (blockedDigits.Any() && (lastBlock = blockedDigits.Last()).Item1 == digits[i])
                {
                    blockedDigits[blockedDigits.Count - 1] = new Tuple<int, int>(digits[i], lastBlock.Item2 + 1);
                }
                else
                {
                    blockedDigits.Add(new Tuple<int, int>(digits[i], 1));
                }
            }
            return (blockedDigits.Any(x => x.Item2 == 2) && partTwo) || (blockedDigits.Any(x => x.Item2 >= 2) && !partTwo);
        }
        #endregion

        #region Day 5
        public int[] ConvertDay5Input(string inputPath)
        {
            return File.ReadAllText(inputPath).Split(',').Select(x => int.Parse(x)).ToArray();
        }

        public int Day5_PuzzleOne(int[] input)
        {
            return RunDiagnosis(input, 1);
        }

        public int Day5_PuzzleTwo(int[] input)
        {
            return RunDiagnosis(input, 5);
        }

        private int _solverInput;
        private int _solverOutput;

        public int RunDiagnosis(int[] program, int input)
        {
            _solverInput = input;
            IntcodeSolver(program);
            return _solverOutput;
        }

        public int[] IntcodeSolver(int[] program)
        {
            var address = 0;
            while (program[address] != 99)
            {
                program = Solve_Step(program, program[address], address, new bool[2] { false, false }, out address);
            }
            return program;
        }

        public int[] Solve_Step(int[] program, int opcode, int address, bool[] modes, out int newAddress)
        {
            switch (opcode)
            {
                case 1:
                    {
                        program = StepSolver(program, opcode, program[address + 3], GetValue(program, address + 1, modes[0]), GetValue(program, address + 2, modes[1]));
                        newAddress = address + 4;
                        break;
                    }
                case 2:
                    {
                        program = StepSolver(program, opcode, program[address + 3], GetValue(program, address + 1, modes[0]), GetValue(program, address + 2, modes[1]));
                        newAddress = address + 4;
                        break;
                    }
                case 3:
                    {
                        program = StepSolver(program, opcode, program[address + 1], default, default);
                        newAddress = address + 2;
                        break;
                    }
                case 4:
                    {
                        _solverOutput = GetValue(program, address + 1, modes[0]);
                        newAddress = address + 2;
                        break;
                    }
                case 5:
                    {
                        var param1 = GetValue(program, address + 1, modes[0]);
                        var param2 = GetValue(program, address + 2, modes[1]);
                        newAddress = (param1 != 0) ? param2 : address + 3;
                        break;
                    }
                case 6:
                    {
                        var param1 = GetValue(program, address + 1, modes[0]);
                        var param2 = GetValue(program, address + 2, modes[1]);
                        newAddress = (param1 == 0) ? param2 : address + 3;
                        break;
                    }
                case 7:
                    {
                        var param1 = GetValue(program, address + 1, modes[0]);
                        var param2 = GetValue(program, address + 2, modes[1]);
                        var param3 = program[address + 3];
                        program[param3] = (param1 < param2) ? 1 : 0;
                        newAddress = address + 4;
                        break;
                    }
                case 8:
                    {
                        var param1 = GetValue(program, address + 1, modes[0]);
                        var param2 = GetValue(program, address + 2, modes[1]);
                        var param3 = program[address + 3];
                        program[param3] = (param1 == param2) ? 1 : 0;
                        newAddress = address + 4;
                        break;
                    }
                default:
                    {
                        var n = opcode;
                        var digits = new List<int>();
                        while (n != 0)
                        {
                            digits.Add(n % 10);
                            n /= 10;
                        }
                        digits[0] = digits[1] * 10 + digits[0];
                        digits.RemoveAt(1);
                        while (digits.Count < 3)
                        {
                            digits.Add(0);
                        }
                        program = Solve_Step(program, digits[0], address, digits.Skip(1).Select(x => x == 1).ToArray(), out newAddress);
                        break;
                    }
            }
            return program;
        }

        public int[] StepSolver(int[] program, int opcode, int address, int value1, int value2)
        {
            switch (opcode)
            {
                case 1:
                    {
                        program[address] = value1 + value2;
                        break;
                    }
                case 2:
                    {
                        program[address] = value1 * value2;
                        break;
                    }
                case 3:
                    {
                        program[address] = _solverInput;
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
            return program;
        }

        public int GetValue(int[] program, int address, bool immediateMode)
        {
            if (immediateMode)
            {
                return program[address];
            }
            else
            {
                return program[program[address]];
            }
        }
        #endregion

        #region Day 6
        public List<OrbitRelation> ConvertDay6Input(string inputPath)
        {
            return File.ReadAllLines(inputPath).Select(x => new OrbitRelation(x.Split(')')[0], x.Split(')')[1])).ToList();
        }

        public int Day6_PuzzleOne(List<OrbitRelation> input)
        {
            var rootOrbit= GenerateOrbitList(input);
            return rootOrbit.CountOrbits();
        }

        public int Day6_PuzzleTwo(List<OrbitRelation> input)
        {
            var rootOrbit = GenerateOrbitList(input);

            var santa = rootOrbit.FindChildren("SAN");
            var santasWay = new List<string>();
            while(santa.Parent != null)
            {
                santa = santa.Parent;
                santasWay.Add(santa.Name);
            }

            var you = rootOrbit.FindChildren("YOU");
            var yourWay = new List<string>();
            while (you.Parent != null)
            {
                you = you.Parent;
                yourWay.Add(you.Name);
            }

            for(var i = 0; i < yourWay.Count; i++)
            {
                if (santasWay.Any(x=>x == yourWay[i]))
                {
                    return i + santasWay.IndexOf(yourWay[i]);
                }
            }

            return default;
        }

        public Orbit GenerateOrbitList(List<OrbitRelation> relations)
        {
            var allOrbits = new List<Orbit>();
            foreach (var relation in relations)
            {
                if (allOrbits.Any(x => x.Name == relation.Orbiter))
                {
                    throw new Exception("Orbiter already has a parent");
                }
                allOrbits.Add(new Orbit(relation.Orbiter, relation.Target));
            }


            var rootOrbit = new Orbit("COM", null);
            rootOrbit.AssignChildren(allOrbits);
            return rootOrbit;
        }
        #endregion
    }

    #region Day 6 classes
    public class Orbit
    {
        public Orbit(string orbitName, string parentName)
        {
            Name = orbitName;
            ChildOrbits = new List<Orbit>();

            if (!string.IsNullOrEmpty(parentName))
            {
                Parent = new Orbit(parentName, null);
            }
        }

        public string Name { get; set; }

        public Orbit Parent { get; set; }

        public List<Orbit> ChildOrbits { get; set; }

        public void AssignChildren(List<Orbit> availableOrbits)
        {
            ChildOrbits = availableOrbits.Where(x => x.Parent.Name == this.Name).ToList();
            foreach (var child in ChildOrbits)
            {
                child.Parent = this;
                child.AssignChildren(availableOrbits);
            }
        }

        public Orbit FindChildren(string childName)
        {
            Orbit result = null;
            foreach(var child in this.ChildOrbits)
            {
                if (child.Name == childName)
                {
                    result = child;
                    break;
                }
                else if (child.ChildOrbits.Any())
                {
                    result = child.FindChildren(childName);
                }
                if (result != null)
                {
                    return result;
                }
            }
            return result;
        }

        public int CountOrbits(int parentOrbits = default)
        {
            var count = 0;
            foreach (var childOrbit in this.ChildOrbits)
            {
                count += parentOrbits + 1;
                count += childOrbit.CountOrbits(parentOrbits + 1);
            }
            return count;
        }
    }

    public class OrbitRelation
    {
        public OrbitRelation(string target, string orbiter)
        {
            Target = target;
            Orbiter = orbiter;
        }

        public string Target { get; set; }

        public string Orbiter { get; set; }
    }
    #endregion
}
