using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NUnit.Framework;

namespace ClickDefend
{
    [TestFixture]
    class ClickerTests
    {
        [Test]
        public void IsClickerCreated()
        {
            var click = new Clicker();
            Assert.AreEqual(0, click.amount);
        }

        [Test]
        public void IsClickerTicking()
        {
            var click = new Clicker();
            click.ChangeAmount(1);
            click.ChangeAmount(1);
            Assert.AreEqual(2, click.amount);
        }

        [Test]
        public void IsBonusWorking()
        {
            var click = new Clicker();
            click.bonus = 2;
            click.ChangeAmount(2);
            Assert.AreEqual(4, click.amount);
        }
    }
}
