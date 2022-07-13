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
    class TowerTests
    {
        [Test]
        public void CreatesOk()
        {
            var tower = new Tower(20, 500);
            Assert.AreEqual(20, tower.Damage);
            Assert.AreEqual(500, tower.Speed);
        }

        [Test]
        public void DamagesMonster()
        {
            var tower = new Tower(20, 5);
            var monster = new Monster(100, 0);
            tower.SetTarget(monster);
            Thread.Sleep(1100);
            Assert.AreEqual(60, monster.HealthPoint);
        }

        [Test]
        public void KillsMonsterAndStops()
        {
            var tower = new Tower(50, 5);
            var monster = new Monster(100, 0);
            tower.SetTarget(monster);
            Thread.Sleep(1100);
            Assert.IsTrue(monster.DeathFlag);
            Thread.Sleep(1000);
            Assert.AreEqual(0, monster.HealthPoint);
        }
    }
}
