using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClickDefend
{
    class Clicker
    {
        public int amount { get; private set; }
        public int bonus { get; set; }
        //private Timer timer;

        public Clicker()
        {
            amount = 0;
            bonus = 1;
            //timer = new Timer(1000);
            //timer.Elapsed += Tick;
            //timer.Start();
        }

        //private void Tick(Object source, ElapsedEventArgs e)
        //{
        //    amount += 1*bonus;
        //}
        public void ChangeAmount(int am)
        {
            if (am > 0)
                amount += am * bonus;
            else
                amount += am;
        }
    }
}
