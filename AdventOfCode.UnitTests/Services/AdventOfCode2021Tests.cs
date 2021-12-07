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

        #region Day 03
        [Test]
        [TestCase(new string[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010" }, 198)]
        public void Day03_CalculatePowerConsumption(string[] input, int result)
        {
            var report = _service.CalculateGammaEpsilon(input);
            Assert.AreEqual(result, report.Gamma * report.Epsilon);
        }
        [Test]
        [TestCase(new string[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010" }, 230)]
        public void Day03_CalculateLifeSupportRating(string[] input, int result)
        {
            var report = _service.CalculateOxygenCO2(input);
            Assert.AreEqual(result, report.OxygenGeneratorRating * report.CO2ScrubberRating);
        }
        #endregion

        #region Day 04
        [Test]
        [TestCase(false, 4512)]
        [TestCase(true, 1924)]
        public void Day04_RunBingo( bool letSquidWin, int result)
        {
            var numbers = new int[] { 7, 4, 9, 5, 11, 17, 23, 2, 0, 14, 21, 24, 10, 16, 13, 6, 15, 25, 12, 22, 18, 20, 8, 19, 3, 26, 1 };
            var boards = new List<(int BoardID, List<List<int>> BoardNumbers)>
            {
                (BoardID: 1, BoardNumbers: new List<List<int>>
                {
                    new List<int>{ 22, 13, 17, 11, 0 },
                    new List<int>{ 8, 2, 23, 4, 24 },
                    new List<int>{ 21, 9, 14, 16, 7 },
                    new List<int>{ 6, 10, 3, 18, 5 },
                    new List<int>{ 1, 12, 20, 15, 19 },
                    new List<int>{ 22, 8, 21, 6, 1 },
                    new List<int>{ 13, 2, 9, 10, 12 },
                    new List<int>{ 17, 23, 14, 3, 20 },
                    new List<int>{ 11, 4, 16, 18, 15 },
                    new List<int>{ 0, 24, 7, 5, 19 }
                }),
                (BoardID: 2, BoardNumbers: new List<List<int>>
                {
                    new List<int>{ 3, 15, 0, 2, 22 },
                    new List<int>{ 9, 18, 13, 17, 5 },
                    new List<int>{ 19, 8, 7, 25, 23 },
                    new List<int>{ 20, 11, 10, 24, 4 },
                    new List<int>{ 14, 21, 16, 12, 6 },
                    new List<int>{ 3, 9, 19, 20, 14 },
                    new List<int>{ 15, 18, 8, 11, 21 },
                    new List<int>{ 0, 13, 7, 10, 16 },
                    new List<int>{ 2, 17, 25, 24, 12 },
                    new List<int>{ 22, 5, 23, 4, 6 }
                }),
                (BoardID: 3, BoardNumbers: new List<List<int>>
                {
                    new List<int>{ 14, 21, 17, 24, 4 },
                    new List<int>{ 10, 16, 15, 9, 19 },
                    new List<int>{ 18, 8, 23, 26, 20 },
                    new List<int>{ 22, 11, 13, 6, 5 },
                    new List<int>{ 2, 0, 12, 3, 7},
                    new List<int>{ 14, 10, 18, 22, 2 },
                    new List<int>{ 21, 16, 8, 11, 0 },
                    new List<int>{ 17, 15, 23, 13, 12 },
                    new List<int>{ 24, 9, 26, 6, 3 },
                    new List<int>{ 4, 19, 20, 5, 7 }
                })
            };

            ;
            Assert.AreEqual(result, _service.RunBingoGame(numbers, boards, letSquidWin));
        }
        #endregion

        #region Day 05
        [Test]
        [TestCase(false, 5)]
        [TestCase(true, 12)]
        public void Day05_IntersectLines(bool useDiagonals, int result)
        {
            var testInput = new List<string>
            {
                "0,9 -> 5,9",
                "8,0 -> 0,8",
                "9,4 -> 3,4",
                "2,2 -> 2,1",
                "7,0 -> 7,4",
                "6,4 -> 2,0",
                "0,9 -> 2,9",
                "3,4 -> 1,4",
                "0,0 -> 8,8",
                "5,5 -> 8,2"
            };
            var lines = _service.ReadPoints(testInput);
            ;
            Assert.AreEqual(result, _service.IntersectLines(lines, useDiagonals));
        }
        #endregion

        #region Day 06
        [Test]
        [TestCase(0, 5)]
        [TestCase(1, 5)]
        [TestCase(2, 6)]
        [TestCase(3, 7)]
        [TestCase(4, 9)]
        [TestCase(5, 10)]
        [TestCase(6, 10)]
        [TestCase(7, 10)]
        [TestCase(8, 10)]
        [TestCase(9, 11)]
        [TestCase(10, 12)]
        [TestCase(11, 15)]
        [TestCase(12, 17)]
        [TestCase(13, 19)]
        [TestCase(14, 20)]
        [TestCase(15, 20)]
        [TestCase(16, 21)]
        [TestCase(17, 22)]
        [TestCase(18, 26)]
        [TestCase(80, 5934)]
        [TestCase(256, 26984457539)]
        public void Day06_ModelLaternfishPouplation(int days, long fishNumber)
        {
            var fishMaturity = new List<long>() { 0, 1, 1, 2, 1, 0, 0, 0, 0 };
            Assert.AreEqual(fishNumber, _service.ModelLanternFishPopulation(days, fishMaturity));
        }
        #endregion

        #region Day 07
        [Test]
        [TestCase(new int[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 }, 2)]
        public void Day07_CalculateGeometricMean(int[] input, int median)
        {
            Assert.AreEqual(median, _service.CalculateGeometricMean(input));
        }

        [Test]
        [TestCase(new int[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 }, 5)]
        public void Day07_CalculateCenterOfMass(int[] input, int median)
        {
            Assert.AreEqual(median, _service.CalculateCenterOfMass(input));
        }

        [Test]
        [TestCase(new int[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 }, 2, false, 37)]
        [TestCase(new int[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 }, 5, true, 168)]
        public void Day07_CalculateFuelConsumption(int[] input, int targetPosition, bool useExtendedFuelConsumption, int fuelConsumption)
        {
            Assert.AreEqual(fuelConsumption, _service.CalculateFuelConsumption(targetPosition, input, useExtendedFuelConsumption));
        }
        #endregion
    }
}