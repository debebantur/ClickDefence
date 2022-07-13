using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ClickDefend
{
    [TestFixture]
    class MonsterTests
    {
        [Test]
        public void CreatedCorrect()
        {
            var monster = new Monster(100, 10);
            Assert.AreEqual(100, monster.HealthPoint);
            Assert.AreEqual(10, monster.Speed);
        }

        [Test]
        public void GetsDamage()
        {
            var monster = new Monster(100, 10);
            monster.GotHit(30);
            Assert.AreEqual(70, monster.HealthPoint);
        }

        [Test]
        public void DiesAfterFatalDamage()
        {
            var monster = new Monster(100, 10);
            monster.GotHit(100);
            Assert.IsTrue(monster.DeathFlag);
        }
    }
}
