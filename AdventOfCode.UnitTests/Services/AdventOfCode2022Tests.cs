using NUnit.Framework;
using AdventOfCode.Services;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services.UnitTests
{
    [TestFixture]
    public class AdventOfCode2022Tests
    {
        private AdventOfCode2022 _service;

        [SetUp]
        public void Setup()
        {
            _service = new AdventOfCode2022();
        }

        #region Day 02
        [Test]
        [TestCase(new string[] { "A Y", "B X", "C Z" }, 15)]
        public void Day02_CalculateScore1(string[] input, int result)
        {

            Assert.AreEqual(result, _service.Day2_PuzzleOne(input.ToList()));
        }

        [Test]
        [TestCase(new string[] { "A Y", "B X", "C Z" }, 12)]
        public void Day02_CalculateScore2(string[] input, int result)
        {

            Assert.AreEqual(result, _service.Day2_PuzzleTwo(input.ToList()));
        }
        #endregion

        #region Day 06
        [Test]
        [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
        [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
        [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6)]
        [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
        [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
        public void Day06_FindPacketStart(string input, int result)
        {

            Assert.AreEqual(result, _service.Day6_PuzzleOne(input));
        }

        [Test]
        [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
        [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
        [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 23)]
        [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
        [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
        public void Day06_FindMessageStart(string input, int result)
        {

            Assert.AreEqual(result, _service.Day6_PuzzleTwo(input));
        }
        #endregion
    }
}