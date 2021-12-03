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
    public class AdventOfCode2021Tests
    {
        private AdventOfCode2021 _service;

        [SetUp]
        public void Setup()
        {
            _service = new AdventOfCode2021();
        }

        #region Day 01
        [Test]
        [TestCase(new int[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 }, 7)]
        public void Day01_FilterMeasurements(int[] input, int result)
        {
            Assert.AreEqual(result, _service.FilterMeasurements(input.ToList()).Count);
        }

        [Test]
        [TestCase(new int[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 }, 5)]
        public void Day01_FilterSlidingWindow(int[] input, int result)
        {
            var slidingWindows = _service.CreateSlidingWindow(input.ToList());
            Assert.AreEqual(result, _service.FilterMeasurements(slidingWindows).Count);
        }
        #endregion

        #region Day 02
        [Test]
        [TestCase(new string[] { "forward 5", "down 5", "forward 8", "up 3", "down 8", "forward 2" }, 150)]
        public void Day02_ProcessSubmarineCommands(string[] input, int result)
        {
            var endPosition = _service.ProcessSubmarineCommands(input, false);
            Assert.AreEqual(result, endPosition.X * endPosition.Y);
        }
        [Test]
        [TestCase(new string[] { "forward 5", "down 5", "forward 8", "up 3", "down 8", "forward 2" }, 900)]
        public void Day02_ProcessSubmarineCommandsWithAim(string[] input, int result)
        {
            var endPosition = _service.ProcessSubmarineCommands(input, true);
            Assert.AreEqual(result, endPosition.X * endPosition.Y);
        }
        #endregion
    }
}