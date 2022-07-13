using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDefend
{

    public class Monster
    {
        public int HealthPoint { get; set; }
        public int Speed { get; set; }
        public bool DeathFlag { get; private set; }

        public Monster(int health, int speed)
        {
            HealthPoint = health;
            Speed = speed;
            DeathFlag = false;
        }

        public void GotHit(int damage)
        {
            HealthPoint -= damage;
            if (HealthPoint <= 0)
                Die();
        }

        private void Die()
        {
            DeathFlag = true;
        }
    }
}
