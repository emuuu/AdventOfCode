using NUnit.Framework;
using AdventOfCode.Services;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Services.UnitTests
{
    [TestFixture]
    public class AdventOfCode2025Tests
    {
        private AdventOfCode2023 _service;

        [SetUp]
        public void Setup()
        {
            _service = new AdventOfCode2025();
        }
    }
}