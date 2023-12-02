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
    }
}