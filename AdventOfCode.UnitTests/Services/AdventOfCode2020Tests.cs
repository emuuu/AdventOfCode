using NUnit.Framework;
using AdventOfCode.Services;
using System.Linq;
using System.Collections.Generic;
using System;
using static AdventOfCode.Services.AdventOfCode2020;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services.UnitTests
{
    [TestFixture]
    public class AdventOfCode2020Tests
    {
        private AdventOfCode2020 _service;

        [SetUp]
        public void Setup()
        {
            _service = new AdventOfCode2020();
        }

        #region Day 01
        [Test]
        [TestCase(new string[] { "1721", "979", "366", "299", "675", "1456" }, 514579)]
        public void Day01_Calcualte2020Expenses_With2Numbers(string[] input, int result)
        {
            Assert.AreEqual(result, _service.CalculateExpenses(input.Select(x => int.Parse(x)).ToArray(), false));
        }

        [Test]
        [TestCase(new string[] { "1721", "979", "366", "299", "675", "1456" }, 241861950)]
        public void Day01_Calcualte2020Expenses_With3Numbers(string[] input, int result)
        {
            Assert.AreEqual(result, _service.CalculateExpenses(input.Select(x => int.Parse(x)).ToArray(), true));
        }
        #endregion

        #region Day 02
        [Test]
        [TestCase("1-3 a: abcde", true)]
        [TestCase("1-3 b: cdefg", false)]
        [TestCase("2-9 c: ccccccccc", true)]
        public void Day02_ValidSledRentalPasswordPolicy(string input, bool result)
        {
            Assert.AreEqual(result, _service.ValidPasswordPolicy(input.Split(':')[0], input.Split(':')[1].Trim(), false));
        }
        [Test]
        [TestCase("1-3 a: abcde", true)]
        [TestCase("1-3 b: cdefg", false)]
        [TestCase("2-9 c: ccccccccc", false)]
        public void Day02_ValidTobogganPasswordPolicy(string input, bool result)
        {
            Assert.AreEqual(result, _service.ValidPasswordPolicy(input.Split(':')[0], input.Split(':')[1].Trim(), true));
        }
        #endregion

        #region Day 03
        [Test]
        [TestCase(7)]
        public void Day03_RunMap(int result)
        {
            var testMapInput = new string[] { "..##.......", "#...#...#..", ".#....#..#.", "..#.#...#.#", ".#...##..#.", "..#.##.....", ".#.#.#....#", ".#........#", "#.##...#...", "#...##....#", ".#..#...#.#" };
            var testMap = testMapInput.Select(x => x.ToCharArray()).ToArray();

            Assert.AreEqual(result, _service.RunMap(testMap, 3, 1));
        }
        #endregion

        #region Day 04
        [Test]
        [TestCase("ecl:gry pid:860033327 eyr:2020 hcl:#fffffd byr:1937 iyr:2017 cid:147 hgt:183cm", true)]
        [TestCase("iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884 hcl:#cfa07d byr:1929", false)]
        [TestCase("hcl:#ae17e1 iyr:2013 eyr:2024 ecl:brn pid:760753108 byr:1931 hgt:179cm", true)]
        [TestCase("hcl:#cfa07d eyr:2025 pid:166559648 iyr:2011 ecl:brn hgt:59in", false)]
        [TestCase("byr:1983 iyr:2017 pid:796082981 cid:129 eyr:2030 ecl:oth hgt:182cm", false)]
        public void Day04_ValidatePassportsWithoutValueValidation(string input, bool result)
        {
            var passport = input.Split(' ').Select(x => new KeyValuePair<string, string>(x.Split(':')[0], x.Split(':')[1]));

            Assert.AreEqual(result, _service.ValidPassport(passport, false));
        }

        [Test]
        [TestCase("pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980 hcl:#623a2f", true)]
        [TestCase("eyr:2029 ecl:blu cid:129 byr:1989 iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm", true)]
        [TestCase("hcl:#888785 hgt:164cm byr:2001 iyr:2015 cid:88 pid:545766238 ecl:hzl eyr:2022", true)]
        [TestCase("iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719", true)]
        [TestCase("eyr:1972 cid:100 hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926", false)]
        [TestCase("iyr:2019 hcl:#602927 eyr:1967 hgt:170cm ecl:grn pid:012533040 byr:1946", false)]
        [TestCase("hcl:dab227 iyr:2012 ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277", false)]
        [TestCase("hgt:59cm ecl:zzz eyr:2038 hcl:74454a iyr:2023 pid:3556412378 byr:2007", false)]
        public void Day04_ValidatePassportsWithValueValidation(string input, bool result)
        {
            var passport = input.Split(' ').Select(x => new KeyValuePair<string, string>(x.Split(':')[0], x.Split(':')[1]));

            Assert.AreEqual(result, _service.ValidPassport(passport, true));
        }

        [Test]
        [TestCase("byr", "2002", true)]
        [TestCase("byr", "2003", false)]
        [TestCase("hgt", "60in", true)]
        [TestCase("hgt", "190cm", true)]
        [TestCase("hgt", "190in", false)]
        [TestCase("hgt", "190", false)]
        [TestCase("hcl", "#123abc", true)]
        [TestCase("hcl", "#123abz", false)]
        [TestCase("hcl", "123abc", false)]
        [TestCase("ecl", "brn", true)]
        [TestCase("ecl", "wat", false)]
        [TestCase("pid", "000000001", true)]
        [TestCase("pid", "0123456789", false)]
        public void Day04_ValidatePassportValue(string key, string value, bool valid)
        {
            Assert.AreEqual(valid, _service.ValidatePassportValue(new KeyValuePair<string, string>(key, value)));
        }
        #endregion

        #region Day 05
        [Test]
        [TestCase('F', new int[] { 0, 127 }, new int[] { 0, 63 })]
        [TestCase('B', new int[] { 0, 63 }, new int[] { 32, 63 })]
        [TestCase('F', new int[] { 32, 63 }, new int[] { 32, 47 })]
        [TestCase('B', new int[] { 32, 47 }, new int[] { 40, 47 })]
        [TestCase('B', new int[] { 40, 47 }, new int[] { 44, 47 })]
        [TestCase('F', new int[] { 44, 47 }, new int[] { 44, 45 })]
        [TestCase('R', new int[] { 0, 7 }, new int[] { 4, 7 })]
        [TestCase('L', new int[] { 4, 7 }, new int[] { 4, 5 })]
        public void Day05_RangeStep(char input, int[] range, int[] result)
        {
            Assert.AreEqual(result, _service.SolveSeatStep(range, input));
        }

        [Test]
        [TestCase("FBFBBFF", new int[] { 0, 127 }, 44)]
        [TestCase("RLR", new int[] { 0, 7 }, 5)]
        public void Day05_SeatCalculation(string seatCode, int[] range, int result)
        {
            foreach (var c in seatCode)
            {
                _service.SolveSeatStep(range, c);
            }
            Assert.AreEqual(result, range[0]);

        }

        [Test]
        [TestCase("FBFBBFFRLR", 357)]
        [TestCase("BFFFBBFRRR", 567)]
        [TestCase("FFFBBBFRRR", 119)]
        [TestCase("BBFFBBFRLL", 820)]
        public void Day05_SeatIDCalculation(string seatCode, int result)
        {
            Assert.AreEqual(result, _service.CalculateSeatID(seatCode));
        }
        #endregion

        #region Day 06
        [Test]
        [TestCase("abcx\r\nabcy\r\nabcz", 6)]
        [TestCase("abc", 3)]
        [TestCase("a\r\nb\r\nc", 3)]
        [TestCase("ab\r\nac", 3)]
        [TestCase("a\r\na\r\na\r\na", 1)]
        [TestCase("b", 1)]

        public void Day06_CountAnswersTestOne(string input, int result)
        {
            Assert.AreEqual(result, _service.CountAnswers(input, true));
        }

        [Test]
        [TestCase("abc", 3)]
        [TestCase("a\r\nb\r\nc", 0)]
        [TestCase("ab\r\nac", 1)]
        [TestCase("a\r\na\r\na\r\na", 1)]
        [TestCase("b", 1)]

        public void Day06_CountAnswersTestTwo(string input, int result)
        {
            Assert.AreEqual(result, _service.CountAnswers(input, false));
        }
        #endregion

        #region Day 07
        public static IEnumerable<TestCaseData> RunBagUp_TestData()
        {
            yield return new TestCaseData(
                "shiny gold",
                new List<string>
                {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                    "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.",
                    "dotted black bags contain no other bags."
                },
                4);
        }
        public static IEnumerable<TestCaseData> RunBagDown_TestData()
        {
            yield return new TestCaseData(
                "shiny gold",
                new List<string>
                {
                    "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                    "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                    "bright white bags contain 1 shiny gold bag.",
                    "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                    "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                    "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                    "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                    "faded blue bags contain no other bags.",
                    "dotted black bags contain no other bags."
                },
                32);
        }

        [Test]
        [TestCaseSource("RunBagUp_TestData")]
        public void Day07_RunBagRulesUp(string searchedBag, List<string> ruleInput, int result)
        {
            var rules = _service.CreateRules(ruleInput);
            Assert.AreEqual(result, _service.RunBagRules(new List<string> { searchedBag }, rules, true).Count - 1);
        }

        [Test]
        [TestCaseSource("RunBagDown_TestData")]
        public void Day07_RunBagRulesDown(string searchedBag, List<string> ruleInput, int result)
        {
            var rules = _service.CreateRules(ruleInput);
            Assert.AreEqual(result, _service.RunBagRules(new List<string> { searchedBag }, rules, false).Count - 1);
        }
        #endregion

        #region Day 08
        [Test]
        [TestCase("nop +0,acc +1,jmp +4,acc +3,jmp -3,acc -99,acc +1,jmp -4,acc +6", false, 5)]

        public void Day08_RunBootProgram(string input, bool terminationResult, int result)
        {
            var convertedInput = input.Split(',').Select(x => new Tuple<string, int>(x.Split(' ')[0], int.Parse(x.Split(' ')[1]))).ToList();
            var terminated = _service.RunBootProgram(convertedInput, out int accumulatorValue);
            Assert.AreEqual(result, accumulatorValue);
            Assert.AreEqual(terminationResult, terminated);
        }
        [Test]
        [TestCase("nop +0,acc +1,jmp +4,acc +3,jmp -3,acc -99,acc +1,jmp -4,acc +6", 8)]

        public void Day08_TryFixBootProgram(string input, int result)
        {
            var convertedInput = input.Split(',').Select(x => new Tuple<string, int>(x.Split(' ')[0], int.Parse(x.Split(' ')[1]))).ToList();
            Assert.AreEqual(result, _service.TryFixBootProgram(convertedInput));
        }
        #endregion

        #region Day 09
        [Test]
        [TestCase(new long[] { 35, 20, 15, 25, 47 }, 40, true)]
        [TestCase(new long[] { 20, 15, 25, 47, 40 }, 62, true)]
        [TestCase(new long[] { 15, 25, 47, 40, 62 }, 55, true)]
        [TestCase(new long[] { 25, 47, 40, 62, 55 }, 65, true)]
        [TestCase(new long[] { 47, 40, 62, 55, 65 }, 95, true)]
        [TestCase(new long[] { 40, 62, 55, 65, 95 }, 102, true)]
        [TestCase(new long[] { 62, 55, 65, 95, 102 }, 117, true)]
        [TestCase(new long[] { 55, 65, 95, 102, 117 }, 150, true)]
        [TestCase(new long[] { 65, 95, 102, 117, 150 }, 182, true)]
        [TestCase(new long[] { 95, 102, 117, 150, 182 }, 127, false)]
        [TestCase(new long[] { 102, 117, 150, 182, 127 }, 219, true)]
        [TestCase(new long[] { 117, 150, 182, 127, 219 }, 299, true)]
        [TestCase(new long[] { 150, 182, 127, 219, 299 }, 277, true)]
        [TestCase(new long[] { 182, 127, 219, 299, 277 }, 309, true)]
        [TestCase(new long[] { 127, 219, 299, 277, 309 }, 576, true)]
        public void Day09_ValidStream(long[] numberStream, long validationNumber, bool result)
        {
            Assert.AreEqual(result, _service.ValidStream(validationNumber, numberStream));
        }

        [Test]
        [TestCase(new long[] { 35, 20, 15, 25, 47, 40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576 }, 127)]
        public void Day09_FindInvalidNumber(long[] numberStream, long result)
        {
            Assert.AreEqual(result, _service.FindInvalidNumber(numberStream, 5));
        }

        [Test]
        [TestCase(new long[] { 35, 20, 15, 25, 47, 40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576 }, 62)]
        public void Day09_FindContiguousSet(long[] numberStream, long result)
        {
            var contiguouSet = _service.FindContiguousSet(_service.FindInvalidNumber(numberStream, 5), numberStream.ToList());
            Assert.AreEqual(result, contiguouSet.Min() + contiguouSet.Max());
        }
        #endregion

        #region Day 10
        [Test]
        [TestCase(new int[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 }, 7, 5)]
        [TestCase(new int[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3 }, 22, 10)]
        public void Day10_SetupAdaptersWithAllAdapters(int[] adpaterRatings, int oneJoltDifferences, int threeJoltDifferences)
        {
            Assert.AreEqual(oneJoltDifferences * threeJoltDifferences, _service.SetupAdapters(adpaterRatings.ToList(), true));
        }

        [Test]
        [TestCase(new int[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4 }, 8)]
        [TestCase(new int[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3 }, 19208)]
        public void Day10_SetupAdaptersWithDistincConfiguration(int[] adpaterRatings, int result)
        {
            Assert.AreEqual(result, _service.SetupAdapters(adpaterRatings.ToList(), false));
        }
        #endregion

        #region Day 11
        [Test]
        [TestCase("L.LL.LL.LL,LLLLLLL.LL,L.L.L..L..,LLLL.LL.LL,L.LL.LL.LL,L.LLLLL.LL,..L.L.....,LLLLLLLLLL,L.LLLLLL.L,L.LLLLL.LL", "#.##.##.##,#######.##,#.#.#..#..,####.##.##,#.##.##.##,#.#####.##,..#.#.....,##########,#.######.#,#.#####.##")]
        [TestCase("#.##.##.##,#######.##,#.#.#..#..,####.##.##,#.##.##.##,#.#####.##,..#.#.....,##########,#.######.#,#.#####.##", "#.LL.L#.##,#LLLLLL.L#,L.L.L..L..,#LLL.LL.L#,#.LL.LL.LL,#.LLLL#.##,..L.L.....,#LLLLLLLL#,#.LLLLLL.L,#.#LLLL.##")]
        [TestCase("#.LL.L#.##,#LLLLLL.L#,L.L.L..L..,#LLL.LL.L#,#.LL.LL.LL,#.LLLL#.##,..L.L.....,#LLLLLLLL#,#.LLLLLL.L,#.#LLLL.##", "#.##.L#.##,#L###LL.L#,L.#.#..#..,#L##.##.L#,#.##.LL.LL,#.###L#.##,..#.#.....,#L######L#,#.LL###L.L,#.#L###.##")]
        [TestCase("#.##.L#.##,#L###LL.L#,L.#.#..#..,#L##.##.L#,#.##.LL.LL,#.###L#.##,..#.#.....,#L######L#,#.LL###L.L,#.#L###.##", "#.#L.L#.##,#LLL#LL.L#,L.L.L..#..,#LLL.##.L#,#.LL.LL.LL,#.LL#L#.##,..L.L.....,#L#LLLL#L#,#.LLLLLL.L,#.#L#L#.##")]
        [TestCase("#.#L.L#.##,#LLL#LL.L#,L.L.L..#..,#LLL.##.L#,#.LL.LL.LL,#.LL#L#.##,..L.L.....,#L#LLLL#L#,#.LLLLLL.L,#.#L#L#.##", "#.#L.L#.##,#LLL#LL.L#,L.#.L..#..,#L##.##.L#,#.#L.LL.LL,#.#L#L#.##,..L.L.....,#L#L##L#L#,#.LLLLLL.L,#.#L#L#.##")]
        public void Day11_AssignSeats(string inputString, string resultString)
        {
            var input = inputString.Split(',').Select(x => x.Select(y => y).ToArray()).ToArray();
            var result = resultString.Split(',').Select(x => x.Select(y => y).ToArray()).ToArray();
            Assert.AreEqual(result, _service.AssignSeats(input, 4, true));
        }

        [Test]
        [TestCase("L.LL.LL.LL,LLLLLLL.LL,L.L.L..L..,LLLL.LL.LL,L.LL.LL.LL,L.LLLLL.LL,..L.L.....,LLLLLLLLLL,L.LLLLLL.L,L.LLLLL.LL", "#.##.##.##,#######.##,#.#.#..#..,####.##.##,#.##.##.##,#.#####.##,..#.#.....,##########,#.######.#,#.#####.##")]
        [TestCase("#.##.##.##,#######.##,#.#.#..#..,####.##.##,#.##.##.##,#.#####.##,..#.#.....,##########,#.######.#,#.#####.##", "#.LL.LL.L#,#LLLLLL.LL,L.L.L..L..,LLLL.LL.LL,L.LL.LL.LL,L.LLLLL.LL,..L.L.....,LLLLLLLLL#,#.LLLLLL.L,#.LLLLL.L#")]
        [TestCase("#.LL.LL.L#,#LLLLLL.LL,L.L.L..L..,LLLL.LL.LL,L.LL.LL.LL,L.LLLLL.LL,..L.L.....,LLLLLLLLL#,#.LLLLLL.L,#.LLLLL.L#", "#.L#.##.L#,#L#####.LL,L.#.#..#..,##L#.##.##,#.##.#L.##,#.#####.#L,..#.#.....,LLL####LL#,#.L#####.L,#.L####.L#")]
        [TestCase("#.L#.##.L#,#L#####.LL,L.#.#..#..,##L#.##.##,#.##.#L.##,#.#####.#L,..#.#.....,LLL####LL#,#.L#####.L,#.L####.L#", "#.L#.L#.L#,#LLLLLL.LL,L.L.L..#..,##LL.LL.L#,L.LL.LL.L#,#.LLLLL.LL,..L.L.....,LLLLLLLLL#,#.LLLLL#.L,#.L#LL#.L#")]
        [TestCase("#.L#.L#.L#,#LLLLLL.LL,L.L.L..#..,##LL.LL.L#,L.LL.LL.L#,#.LLLLL.LL,..L.L.....,LLLLLLLLL#,#.LLLLL#.L,#.L#LL#.L#", "#.L#.L#.L#,#LLLLLL.LL,L.L.L..#..,##L#.#L.L#,L.L#.#L.L#,#.L####.LL,..#.#.....,LLL###LLL#,#.LLLLL#.L,#.L#LL#.L#")]
        [TestCase("#.L#.L#.L#,#LLLLLL.LL,L.L.L..#..,##L#.#L.L#,L.L#.#L.L#,#.L####.LL,..#.#.....,LLL###LLL#,#.LLLLL#.L,#.L#LL#.L#", "#.L#.L#.L#,#LLLLLL.LL,L.L.L..#..,##L#.#L.L#,L.L#.LL.L#,#.LLLL#.LL,..#.L.....,LLL###LLL#,#.LLLLL#.L,#.L#LL#.L#")]
        public void Day11_AssignSeatsWithDirectionalOccupation(string inputString, string resultString)
        {
            var input = inputString.Split(',').Select(x => x.Select(y => y).ToArray()).ToArray();
            var result = resultString.Split(',').Select(x => x.Select(y => y).ToArray()).ToArray();
            Assert.AreEqual(result, _service.AssignSeats(input, 5, false));
        }

        [Test]
        [TestCase(".......#.,...#.....,.#.......,.........,..#L....#,....#....,.........,#........,...#.....", 3, 4, 8)]
        [TestCase(".............,.L.L.#.#.#.#.,.............", 1, 1, 0)]
        [TestCase(".##.##.,#.#.#.#,##...##,...L...,##...##,#.#.#.#,.##.##.", 3, 3, 0)]
        public void Day11_GetDirectionalOccupiedSeats(string inputString, int seatX, int seatY, int result)
        {
            var seatPlan = inputString.Split(',').Select(x => x.Select(y => y).ToArray()).ToArray();
            Assert.AreEqual(result, _service.SurroundingOccupiedSeats(seatY, seatX, seatPlan, false));
        }

        [Test]
        [TestCase("L.LL.LL.LL,LLLLLLL.LL,L.L.L..L..,LLLL.LL.LL,L.LL.LL.LL,L.LLLLL.LL,..L.L.....,LLLLLLLLLL,L.LLLLLL.L,L.LLLLL.LL", 4, true, 37)]
        [TestCase("L.LL.LL.LL,LLLLLLL.LL,L.L.L..L..,LLLL.LL.LL,L.LL.LL.LL,L.LLLLL.LL,..L.L.....,LLLLLLLLLL,L.LLLLLL.L,L.LLLLL.LL", 5, false, 26)]
        public void Day11_GetFinalSeatPlan(string inputString, int occupiedSeatTolerance, bool adjacentSeats, int result)
        {
            var input = inputString.Split(',').Select(x => x.Select(y => y).ToArray()).ToArray();
            var finalSeatPlan = _service.GetFinalSeatPlan(input, occupiedSeatTolerance, adjacentSeats);
            Assert.AreEqual(result, finalSeatPlan.Select(x => x.Count(y => y == '#')).Sum());
        }
        #endregion

        #region Day 12
        [Test]
        [TestCase("F10,N3,F7,R90,F11", 25)]
        public void Day12_MoveShip(string instructions, int result)
        {
            var ship = new Ship
            {
                UseWaypoint = false,
                VectorX = 1
            };

            foreach (var instruction in instructions.Split(','))
            {
                ship.ProcessInstruction(instruction);
            }
            Assert.AreEqual(result, ship.ManhattanDistance);
        }

        [Test]
        [TestCase("F10,N3,F7,R90,F11", new int[] { 100, 100, 170, 170, 214 }, new int[] { 10, 10, 38, 38, -72 }, new int[] { 110, 110, 180, 174, 218 }, new int[] { 11, 14, 42, 28, -82 }, 286)]
        public void Day12_MoveShipWithWaypoint(string instructions, int[] shipPosX, int[] shipPosY, int[] wayX, int[] wayY, int result)
        {
            var ship = new Ship
            {
                UseWaypoint = true,
                VectorX = 1,
                WaypointX = 10,
                WaypointY = 1
            };

            var i = 0;
            foreach (var instruction in instructions.Split(','))
            {
                ship.ProcessInstruction(instruction);
                Assert.AreEqual(shipPosX[i], ship.PositionX);
                Assert.AreEqual(shipPosY[i], ship.PositionY);
                Assert.AreEqual(wayX[i], ship.WaypointX);
                Assert.AreEqual(wayY[i], ship.WaypointY);
                i++;
            }
            Assert.AreEqual(result, ship.ManhattanDistance);
        }
        #endregion

        #region Day 13
        [Test]
        [TestCase(939, "7,13,x,x,59,x,31,19", 295)]
        public void Day13_GetClosestBus(int timestamp, string busIDs, int result)
        {
            var activeBuses = busIDs.Split(',').Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x));
            var closestBus = _service.GetClosestBus(timestamp, activeBuses);
            Assert.AreEqual(result, (closestBus * Math.Ceiling(timestamp / (double)closestBus) - timestamp) * closestBus);
        }

        [Test]
        [TestCase("7,13,x,x,59,x,31,19", 1068781)]
        [TestCase("17,x,13,19", 3417)]
        [TestCase("67,7,59,61", 754018)]
        [TestCase("67,x,7,59,61", 779210)]
        [TestCase("67,7,x,59,61", 1261476)]
        [TestCase("1789,37,47,1889", 1202161486)]
        public void Day13_GetEarliestTimestamp(string busIDs, int result)
        {
            Assert.AreEqual(result, _service.GetEarliestTimestamp(busIDs.Split(',')));
        }
        #endregion

        #region Day 14
        [Test]
        [TestCase("mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X,mem[8] = 11,mem[7] = 101,mem[8] = 0", true, 165)]
        [TestCase("mask = 000000000000000000000000000000X1001X,mem[42] = 100,mask = 00000000000000000000000000000000X0XX, mem[26] = 1", false, 208)]
        public void Day14_BitProcessor(string inputString, bool versionOne, int result)
        {
            var input = _service.ParseDay14Input(inputString.Split(','));
            var memory = new Dictionary<long, long>();
            foreach (var mask in input)
            {
                memory = _service.ProcessBitMask(memory, mask.Key, mask.Value, versionOne);
            }
            Assert.AreEqual(result, memory.Values.Sum());
        }
        #endregion

        #region Day 15
        [Test]
        [TestCase("0,3,6", 2020, 436)]
        [TestCase("1,3,2", 2020, 1)]
        [TestCase("2,1,3", 2020, 10)]
        [TestCase("1,2,3", 2020, 27)]
        [TestCase("2,3,1", 2020, 78)]
        [TestCase("3,2,1", 2020, 438)]
        [TestCase("3,1,2", 2020, 1836)]
        [TestCase("0,3,6", 30000000, 175594)]
        [TestCase("1,3,2", 30000000, 2578)]
        [TestCase("2,1,3", 30000000, 3544142)]
        [TestCase("1,2,3", 30000000, 261214)]
        [TestCase("2,3,1", 30000000, 6895259)]
        [TestCase("3,2,1", 30000000, 18)]
        [TestCase("3,1,2", 30000000, 362)]
        public void Day15_MemorySolver(string inputString, int resultTurn, int result)
        {
            var input = inputString.Split(',').Select(x => int.Parse(x)).ToList();
            Assert.AreEqual(result, _service.MemorySolver(input, resultTurn));
        }
        #endregion

        #region Day 16
        [Test]
        [TestCase("class: 1-3 or 5-7;row: 6-11 or 33-44;seat: 13-40 or 45-50;;your ticket:;7,1,14;;nearby tickets:;7,3,47;40,4,50;55,2,20;38,6,12", 71)]
        public void Day16_ScanningErrorRate(string inputString, int result)
        {
            var input = inputString.Split(';').Select(x => x).ToList();
            var ticketOverview = _service.ParseTicketOverview(input);
            Assert.AreEqual(result, ticketOverview.ScanningErrorRate);
        }

        [Test]
        [TestCase("class: 0-1 or 4-19;row: 0-5 or 8-19;seat: 0-13 or 16-19;;your ticket:;11,12,13;;nearby tickets:;3,9,18;15,1,5;5,14,9", "class:12,row:11,seat:13")]
        public void Day16_AllignTicketValuesWithRules(string inputString, string resultString)
        {
            var input = inputString.Split(';').Select(x => x).ToList();
            var result = resultString.Split(',').ToDictionary(x => x.Split(':')[0], x => int.Parse(x.Split(':')[1]));
            var ticketOverview = _service.ParseTicketOverview(input);
            ticketOverview.DiscardInvalidTickets();
            var ticket = ticketOverview.AllignTicketValuesWithRules();
            Assert.AreEqual(result, ticket);
        }
        #endregion

        #region Day 17
        /*
        brain got messed up by the example (different coordinate scale from actual cube position)
        lost joy after realising that I've been working on a wrong example so no unit test today
        */
        #endregion

        #region Day 18
        [Test]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 26)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Day18_SolveTerm(string input, int result)
        {
            input = $"({input})";
            var term = input.Replace(" ", string.Empty).ToCharArray().ToList();
            Assert.AreEqual(result, _service.SolveTerm(term, false));
        }
        [Test]
        [TestCase("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [TestCase("2 * 3 + (4 * 5)", 46)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Day18_SolveTermWithAdditionPrecedence(string input, int result)
        {
            input = $"({input})";
            var term = input.Replace(" ", string.Empty).ToCharArray().ToList();
            Assert.AreEqual(result, _service.SolveTerm(term, true));
        }
        #endregion

        #region Day 19
        [Test]
        [TestCase("0: 4 1 5;1: 2 3 | 3 2;2: 4 4 | 5 5;3: 4 5 | 5 4;4: \"a\";5: \"b", "ababbb;bababa;abbbab;aaabbb;aaaabbb", "0", 2)]
        public void Day19_BuildRules(string ruleString, string text, string ruleID, int result)
        {
            var rules = ruleString.Split(";").Select(x => x.Split(':', StringSplitOptions.TrimEntries)).ToDictionary(x => x[0], x => x[1]);
            var messages = text.Split(";").ToList();
            var regex = new Regex("^" + _service.BuildRegex(rules, new Dictionary<string, string>(), ruleID) + "$");
            Assert.AreEqual(result, messages.Count(regex.IsMatch));
        }
        #endregion

        #region Day 20
        [Test]
        [TestCase("Tile 2311:;..##.#..#.;##..#.....;#...##..#.;####.#...#;##.##.###.;##...#.###;.#.#.#..##;..#....#..;###...#.#.;..###..###!Tile 1951:;#.##...##.;#.####...#;.....#..##;#...######;.##.#....#;.###.#####;###.##.##.;.###....#.;..#.#..#.#;#...##.#..!Tile 1171:;####...##.;#..##.#..#;##.#..#.#.;.###.####.;..###.####;.##....##.;.#...####.;#.##.####.;####..#...;.....##...!Tile 1427:;###.##.#..;.#..#.##..;.#.##.#..#;#.#.#.##.#;....#...##;...##..##.;...#.#####;.#.####.#.;..#..###.#;..##.#..#.!Tile 1489:;##.#.#....;..##...#..;.##..##...;..#...#...;#####...#.;#..#.#.#.#;...#.#.#..;##.#...##.;..##.##.##;###.##.#..!Tile 2473:;#....####.;#..#.##...;#.##..#...;######.#.#;.#...#.#.#;.#########;.###.#..#.;########.#;##...##.#.;..###.#.#.!Tile 2971:;..#.#....#;#...###...;#.#.###...;##.##..#..;.#####..##;.#..####.#;#..#.#..#.;..####.###;..#.#.###.;...#.#.#.#!Tile 2729:;...#.#.#.#;####.#....;..#.#.....;....#..#.#;.##..##.#.;.#.####...;####.#.#..;##.####...;##..#.##..;#.##...##.!Tile 3079:;#.#.#####.;.#..######;..#.......;######....;####.#..#.;.#...#.##.;#.#####.##;..#.###...;..#.......;..#.###...", 20899048083289)]
        public void Day20_SolvePuzzle(string inputString, long result)
        {
            var tiles = inputString.Replace("#", "1").Replace(".", "0").Split('!').Select(x => new Tile(x.Split(';'))).ToList();
            var puzzle = _service.SolvePuzzle(tiles);
            Assert.AreEqual(result, _service.SolvePuzzle(tiles));
        }
        #endregion

    }
}