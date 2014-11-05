using System;
using NUnit.Framework;
using SmartQuant;

namespace UnitTest
{
    [TestFixture]
    public class StrategyManagerTest
    {
        [Test]
        public void TestModeChanging()
        {
            var f = Framework.Current;

            f.Mode = FrameworkMode.Realtime;
            Assert.IsTrue(f.Mode == FrameworkMode.Realtime);
            f.Mode = FrameworkMode.Simulation;
            Assert.IsTrue(f.Mode == FrameworkMode.Simulation);

            f.Mode = FrameworkMode.Realtime;
            f.StrategyManager.Mode = StrategyMode.Paper;
            f.StrategyManager.Mode = StrategyMode.Backtest;
            Assert.IsTrue(f.Mode == FrameworkMode.Simulation);

            f.Mode = FrameworkMode.Simulation;
            f.StrategyManager.Mode = StrategyMode.Backtest;
            f.StrategyManager.Mode = StrategyMode.Paper;
            Assert.IsTrue(f.Mode == FrameworkMode.Realtime);

            f.Mode = FrameworkMode.Simulation;
            f.StrategyManager.Mode = StrategyMode.Backtest;
            f.StrategyManager.Mode = StrategyMode.Live;
            Assert.IsTrue(f.Mode == FrameworkMode.Realtime);
        }
    }
}

