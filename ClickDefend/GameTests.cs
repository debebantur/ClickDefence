using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ClickDefend
{
    [TestFixture]
    class GameTests
    {
        [Test]
        public void CreatesOk()
        {
            var game = new Game();
            Assert.AreEqual(0, game.clicker.amount);
        }
    }
}
