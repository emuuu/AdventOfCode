using NUnit.Framework;
using AdventOfCode.Services;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services.UnitTests
{
    [TestFixture]
    public class AdventOfCode2023Tests
    {
        private AdventOfCode2023 _service;

        [SetUp]
        public void Setup()
        {
            _service = new AdventOfCode2023();
        }

        #region Day 01
        [Test]
        [TestCase("1abc2", 12)]
        [TestCase("pqr3stu8vwx", 38)]
        [TestCase("a1b2c3d4e5f", 15)]
        [TestCase("treb7uchet", 77)]
        public void Day01_ExtractDigits(string input, int result)
        {

            Assert.AreEqual(result, _service.ExtractDigits(input, false));
        }

        [Test]
        [TestCase("two1nine", 29)]
        [TestCase("eightwothree", 83)]
        [TestCase("abcone2threexyz", 13)]
        [TestCase("xtwone3four", 24)]
        [TestCase("4nineeightseven2", 42)]
        [TestCase("zoneight234", 14)]
        [TestCase("7pqrstsixteen", 76)]
        public void Day01_ExtractDigitsWithWrittenDigits(string input, int result)
        {

            Assert.AreEqual(result, _service.ExtractDigits(input, true));
        }
        #endregion

        #region Day 02
        [Test]
        [TestCase("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", true)]
        [TestCase("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", true)]
        [TestCase("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", false)]
        [TestCase("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", false)]
        [TestCase("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", true)]
        public void Day02_ExtractDigits(string input, bool result)
        {
            var game = _service.ConvertDescriptionToGame(input);
            Assert.AreEqual(result, _service.GameIsPossible("12 red, 13 green, 14 blue", game.Sets));
        }

        [Test]
        [TestCase("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 48)]
        [TestCase("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 12)]
        [TestCase("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 1560)]
        [TestCase("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 630)]
        [TestCase("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 36)]
        public void Day02_SetPowers(string input, int result)
        {
            var game = _service.ConvertDescriptionToGame(input);
            Assert.AreEqual(result, _service.GetSetPower(game.Sets));
        }
        #endregion

        #region Day 03
        [Test]
        [TestCase("467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", true)]
        public void Day03_ExtractDigits(string input, bool result)
        {
            var lines = input.Split("\r\n").ToList();
            var test = _service.ParseEngineParts(lines);
            Assert.AreEqual(result, true);
        }
        #endregion
    }
}