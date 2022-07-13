using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDefend
{
    class Game
    {
        public Clicker clicker;
        public TowerDefence towDef;
        public int Lives = 3;
        public List<Tower> AvaliableTowers = new List<Tower>();
        public List<Monster> AvaliableMonsters = new List<Monster>();
        public Queue<Monster>[] Level1 = new Queue<Monster>[3];

        public Game()
        {
            clicker = new Clicker();
            towDef = new TowerDefence();
            AvaliableTowers.Add(new Tower {Image = "data\\tower1.png", Damage = 10, Speed =10 });
            AvaliableTowers.Add(new Tower { Image = "data\\tower2.png", Damage = 10, Speed = 8 });
            AvaliableTowers.Add(new Tower { Image = "data\\tower3.png", Damage = 15, Speed = 12 });

            Level1[0] = new Queue<Monster>();
            Level1[0].Enqueue(new Monster(20, 1));
            Level1[0].Enqueue(new Monster(20, 1));
            Level1[0].Enqueue(new Monster(30, 1));
            Level1[0].Enqueue(new Monster(30, 1));
            Level1[0].Enqueue(new Monster(40, 1));

            Level1[1] = new Queue<Monster>();
            Level1[1].Enqueue(new Monster(30, 1));
            Level1[1].Enqueue(new Monster(40, 1));
            Level1[1].Enqueue(new Monster(40, 1));
            Level1[1].Enqueue(new Monster(60, 1));
            Level1[1].Enqueue(new Monster(70, 1));

            Level1[2] = new Queue<Monster>();
            Level1[2].Enqueue(new Monster(70, 1));
            Level1[2].Enqueue(new Monster(70, 1));
            Level1[2].Enqueue(new Monster(80, 1));
            Level1[2].Enqueue(new Monster(90, 1));
            Level1[2].Enqueue(new Monster(100, 1));
            Level1[2].Enqueue(new Monster(100, 1));
            Level1[2].Enqueue(new Monster(100, 1));
        }
    }
}
