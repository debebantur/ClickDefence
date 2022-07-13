using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClickDefend
{
    public class Tower
    {
        public int Damage { get; set; }
        public int Speed { get; set; }
        public string Image { get; set; }
        public Monster Target;
        private Timer cooldown;
        public delegate void Finished(Tower tower, int a);
        public event Finished Notify;
        public int WherePlaced;

        public Tower()
        {

        }

        public Tower(int damage, int speed)
        {
            Damage = damage;
            Speed = speed;
            cooldown = new Timer(speed*100);
            cooldown.Elapsed += Shoot;
        }

        public void SetTarget(Monster m)
        {
            Target = m;
            cooldown.Start();
        }

        private void Shoot(Object source, ElapsedEventArgs e)
        {
            if (Target == null)
            {
                Notify?.Invoke(this, WherePlaced);
                return;
            }
            Target.GotHit(Damage);
            if (Target.DeathFlag)
            {
                cooldown.Stop();
                Target = null;
                Notify?.Invoke(this, WherePlaced);
            }
        }
    }
}
