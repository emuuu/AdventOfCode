using NUnit.Framework;
using AdventOfCode.Services;
using System.Linq;
using System.Collections.Generic;
using System;

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

    }
}