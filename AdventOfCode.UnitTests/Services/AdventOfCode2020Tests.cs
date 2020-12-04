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

        #region Day 1
        [Test]
        [TestCase(new string[] { "1721", "979", "366", "299", "675", "1456" }, 514579)]
        public void Day1_Calcualte2020Expenses_With2Numbers(string[] input, int result)
        {
            Assert.AreEqual(result, _service.CalculateExpenses(input.Select(x => int.Parse(x)).ToArray(), false));
        }

        [Test]
        [TestCase(new string[] { "1721", "979", "366", "299", "675", "1456" }, 241861950)]
        public void Day1_Calcualte2020Expenses_With3Numbers(string[] input, int result)
        {
            Assert.AreEqual(result, _service.CalculateExpenses(input.Select(x => int.Parse(x)).ToArray(), true));
        }
        #endregion

        #region Day 2
        [Test]
        [TestCase("1-3 a: abcde", true)]
        [TestCase("1-3 b: cdefg", false)]
        [TestCase("2-9 c: ccccccccc", true)]
        public void Day2_ValidSledRentalPasswordPolicy(string input, bool result)
        {
            Assert.AreEqual(result, _service.ValidPasswordPolicy(input.Split(':')[0], input.Split(':')[1].Trim(), false));
        }
        [Test]
        [TestCase("1-3 a: abcde", true)]
        [TestCase("1-3 b: cdefg", false)]
        [TestCase("2-9 c: ccccccccc", false)]
        public void Day2_ValidTobogganPasswordPolicy(string input, bool result)
        {
            Assert.AreEqual(result, _service.ValidPasswordPolicy(input.Split(':')[0], input.Split(':')[1].Trim(), true));
        }
        #endregion

        #region Day 3
        [Test]
        [TestCase(7)]
        public void Day3_RunMap(int result)
        {
            var testMapInput = new string[] { "..##.......", "#...#...#..", ".#....#..#.", "..#.#...#.#", ".#...##..#.", "..#.##.....", ".#.#.#....#", ".#........#", "#.##...#...", "#...##....#", ".#..#...#.#" };
            var testMap = testMapInput.Select(x => x.ToCharArray()).ToArray();

            Assert.AreEqual(result, _service.RunMap(testMap, 3, 1));
        }
        #endregion

        #region Day 4
        [Test]
        [TestCase("ecl:gry pid:860033327 eyr:2020 hcl:#fffffd byr:1937 iyr:2017 cid:147 hgt:183cm", true)]
        [TestCase("iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884 hcl:#cfa07d byr:1929", false)]
        [TestCase("hcl:#ae17e1 iyr:2013 eyr:2024 ecl:brn pid:760753108 byr:1931 hgt:179cm", true)]
        [TestCase("hcl:#cfa07d eyr:2025 pid:166559648 iyr:2011 ecl:brn hgt:59in", false)]
        [TestCase("byr:1983 iyr:2017 pid:796082981 cid:129 eyr:2030 ecl:oth hgt:182cm", false)]
        public void Day4_ValidatePassportsWithoutValueValidation(string input, bool result)
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
        public void Day4_ValidatePassportsWithValueValidation(string input, bool result)
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
        public void Day4_ValidatePassportValue(string key, string value, bool valid)
        {
            Assert.AreEqual(valid, _service.ValidatePassportValue(new KeyValuePair<string, string>(key, value)));
        }
        #endregion

    }
}