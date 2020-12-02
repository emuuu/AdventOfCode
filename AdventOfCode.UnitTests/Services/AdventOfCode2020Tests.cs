using NUnit.Framework;
using AdventOfCode.Services;
using System.Linq;
using System.Collections.Generic;

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

    }
}