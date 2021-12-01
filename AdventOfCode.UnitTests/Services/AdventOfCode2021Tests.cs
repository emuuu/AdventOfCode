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
    }
}