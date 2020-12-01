using NUnit.Framework;
using AdventOfCode.Services;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Services.UnitTests
{
    [TestFixture]
    public class AdventOfCode2019Tests
    {
        private AdventOfCode2019 _service;

        [SetUp]
        public void Setup()
        {
            _service = new AdventOfCode2019();
        }

        #region Day 1
        [Test]
        [TestCase(12, 2)]
        [TestCase(14, 2)]
        [TestCase(1969, 654)]
        [TestCase(100756, 33583)]
        public void Day1_CalcualteFuel(int mass, int result)
        {
            Assert.AreEqual(result, _service.CalculateFuel(mass, false));
        }

        [Test]
        [TestCase(12, 2)]
        [TestCase(14, 2)]
        [TestCase(1969, 966)]
        [TestCase(100756, 50346)]
        public void Day1_CalcualteFuel_WithFuelMass(int mass, int result)
        {
            Assert.AreEqual(result, _service.CalculateFuel(mass, true));
        }

        [Test]
        [TestCase(new string[] { "12", "14", "1969", "100756" }, 34241)]
        public void Day1_PuzzleOne(string[] input, int result)
        {
            Assert.AreEqual(result, _service.Day1_PuzzleOne(input));
        }

        [Test]
        [TestCase(new string[] { "12", "14", "1969", "100756" }, 51316)]
        public void Day1_PuzzleTwo(string[] input, int result)
        {
            Assert.AreEqual(result, _service.Day1_PuzzleTwo(input));
        }
        #endregion

        #region Day 2
        [Test]
        [TestCase(new int[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }, new int[] { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 })]
        [TestCase(new int[] { 1, 0, 0, 0, 99 }, new int[] { 2, 0, 0, 0, 99 })]
        [TestCase(new int[] { 2, 3, 0, 3, 99 }, new int[] { 2, 3, 0, 6, 99 })]
        [TestCase(new int[] { 1101, 100, -1, 4, 0 }, new int[] { 1101, 100, -1, 4, 99 })]
        [TestCase(new int[] { 1002, 4, 3, 4, 33 }, new int[] { 1002, 4, 3, 4, 99 })]
        [TestCase(new int[] { 2, 4, 4, 5, 99, 0 }, new int[] { 2, 4, 4, 5, 99, 9801 })]
        [TestCase(new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, new int[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 })]
        public void IntcommandSolver(int[] input, int[] result)
        {
            Assert.AreEqual(result, _service.IntcodeSolver(input));
        }
        #endregion

        #region Day 3
        [Test]
        [TestCase(new string[] { "R8,U5,L5,D3", "U7,R6,D4,L4" }, 6)]
        [TestCase(new string[] { "R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83" }, 159)]
        [TestCase(new string[] { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" }, 135)]

        public void Day3_PuzzleOne(string[] input, int result)
        {

            Assert.AreEqual(result, _service.Day3_PuzzleOne(input.ToList().Select(x => x.Split(',')).ToArray()));
        }
        [Test]
        [TestCase(new string[] { "R8,U5,L5,D3", "U7,R6,D4,L4" }, 30)]
        [TestCase(new string[] { "R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83" }, 610)]
        [TestCase(new string[] { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" }, 410)]

        public void Day3_PuzzleTwo(string[] input, int result)
        {

            Assert.AreEqual(result, _service.Day3_PuzzleTwo(input.ToList().Select(x => x.Split(',')).ToArray()));
        }
        #endregion

        #region Day 4
        [Test]
        [TestCase(111111, false, true)]
        [TestCase(223450, false, false)]
        [TestCase(123789, false, false)]
        [TestCase(112233, true, true)]
        [TestCase(123444, true, false)]
        [TestCase(111122, true, true)]
        [TestCase(355666, true, true)]
        [TestCase(355777, true, true)]
        [TestCase(446888, true, true)]
        [TestCase(448999, true, true)]
        [TestCase(347778, true, false)]
        public void MatchesCritera(int input, bool rangeRule, bool result)
        {
            Assert.AreEqual(result, _service.MatchesCritera(input, rangeRule));
        }
        #endregion

        #region Day 5

        [Test]
        [TestCase(new int[] { 3, 0, 4, 0, 99 }, 1, 1)]
        [TestCase(new int[] { 3, 0, 4, 0, 99 }, 2, 2)]
        [TestCase(new int[] { 3, 0, 4, 0, 99 }, 10, 10)]
        [TestCase(new int[] { 3, 0, 4, 0, 99 }, 999, 999)]
        [TestCase(new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, 8, 1)]
        [TestCase(new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, 9, 0)]
        [TestCase(new int[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }, 7, 1)]
        [TestCase(new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, 8, 1)]
        [TestCase(new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, 9, 0)]
        [TestCase(new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, 7, 1)]
        [TestCase(new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, 8, 0)]
        [TestCase(new int[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }, 0, 0)]
        [TestCase(new int[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }, 1, 1)]
        [TestCase(new int[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }, 0, 0)]
        [TestCase(new int[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }, 1, 1)]
        [TestCase(new int[] { 3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99 }, 7, 999)]
        [TestCase(new int[] { 3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99 }, 8, 1000)]
        [TestCase(new int[] { 3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99 }, 9, 1001)]
        public void RunDiagnosis_Test(int[] program, int input, int result)
        {
            Assert.AreEqual(result, _service.RunDiagnosis(program, input));
        }
        #endregion

        #region Day 6
        [Test]
        [TestCase("COM)B-B)C-C)D-D)E-E)F-B)G-G)H-D)I-E)J-J)K-K)L", 42)]
        public void Day6_PuzzleOne(string input, int result)
        {
            var relations = input.Split('-').Select(x => new OrbitRelation(x.Split(')')[0], x.Split(')')[1])).ToList();
            Assert.AreEqual(result, _service.Day6_PuzzleOne(relations));
        }
        [Test]
        [TestCase("COM)B-B)C-C)D-D)E-E)F-B)G-G)H-D)I-E)J-J)K-K)L-K)YOU-I)SAN", 4)]
        public void Day6_PuzzleTwo(string input, int result)
        {
            var relations = input.Split('-').Select(x => new OrbitRelation(x.Split(')')[0], x.Split(')')[1])).ToList();
            Assert.AreEqual(result, _service.Day6_PuzzleTwo(relations));
        }
        #endregion

    }
}