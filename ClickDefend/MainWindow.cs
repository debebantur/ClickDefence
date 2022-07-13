using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickDefend
{
    public partial class MainWindow : Form
    {
        static Game MainGame;
        private Button start;
        private Button clicker;
        private Label clickerAmount = new Label();
        private Label livesAmount = new Label();
        private Panel towers;
        private Button[] towersControls;
        private Panel upgrades;
        private Button[] upgradeButtons = new Button[3];
        public MyLabel[] Path = new MyLabel[29];
        public TowerPlace[] towerPlaces = new TowerPlace[3];
        private Timer timer = new Timer();
        private int placingTowerId = -1;
        public Queue<Monster> OnLevel = new Queue<Monster>();
        public Queue<Monster> NotOnLevel = new Queue<Monster>();
        private int levelNum = 0;

        public MainWindow(Menue oldForm)
        {
            MainGame = new Game();
            towersControls = new Button[MainGame.AvaliableTowers.Count];
            this.WindowState = FormWindowState.Maximized;

            start = new Button();
            start.Click += NextWave;
            start.Font = new Font("Comic Sans MS", 14);
            start.Text = "Следующая волна";
            Controls.Add(start);

            clicker = new Button();
            clicker.Click += OnClickerClick;
            clicker.Font = new Font("Comic Sans MS", 14);
            clicker.Text = "кликни меня";
            Controls.Add(clicker);
            

            towers = new Panel();
            FillTowersBar();
            Controls.Add(towers);
            towers.BackgroundImage = Image.FromFile("data\\forest.jpg");
            towers.BackgroundImageLayout = ImageLayout.Tile;

            upgrades = new Panel();
            for (var i = 0; i < upgradeButtons.Length; i++)
            {
                upgradeButtons[i] = new Button();
                upgradeButtons[i].Name = (-i).ToString();
                upgradeButtons[i].Font = new Font("Comic Sans MS", 14);
                upgradeButtons[i].Text = "увеличение в " + (i + 2).ToString() + " раза" + "\n Цена: " + ((i + 1) * (i + 1) * 50).ToString();
                upgradeButtons[i].Click += (obj, args) =>
                {
                    var id = Math.Abs(int.Parse((obj as Button).Name));
                    if (MainGame.clicker.amount < (id + 1) * (id + 1) * 50)
                        return;
                    MainGame.clicker.ChangeAmount((id + 1) * (id + 1) * (-50));
                    MainGame.clicker.bonus *= (id + 2);
                    upgradeButtons[id].Enabled = false;
                    clickerAmount.Text = MainGame.clicker.amount.ToString();
                };
                upgrades.Controls.Add(upgradeButtons[i]);
            }
            Controls.Add(upgrades);
            upgrades.BackgroundImage = Image.FromFile("data\\forest.jpg");
            upgrades.BackgroundImageLayout = ImageLayout.Tile;

            clickerAmount.TextAlign = ContentAlignment.MiddleCenter;
            clickerAmount.Text = "Ресурсов: " + MainGame.clicker.amount.ToString();
            clickerAmount.Font = new Font("Comic Sans MS", 14);
            clickerAmount.BackColor = Color.LightBlue;
            Controls.Add(clickerAmount);

            livesAmount.Text = "Здоровье: "+MainGame.Lives.ToString();
            livesAmount.TextAlign = ContentAlignment.MiddleCenter;
            livesAmount.Font = new Font("Comic Sans MS", 14);
            livesAmount.BackColor = Color.Pink;
            Controls.Add(livesAmount);

            this.BackgroundImage = Image.FromFile("data\\grass.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.MinimizeBox = false;
            this.ShowIcon = false;

            InitializeComponent();

            
            SizeChanged += RepositionControls;
            Load += RepositionControls;
            FormClosed += (obj, args) => { oldForm.Show(); };

            DrawLevel(upgrades.Left / 20, upgrades.Left / 20);
            CreateTowerPlaces();
            RepositionTowers(upgrades.Left / 20, upgrades.Left / 20);

            timer.Tick += MonsterMove;
        }

        void RepositionControls(object sender, EventArgs args)
        {
            clicker.Location = new Point(0, 0);
            clicker.Size = new Size(ClientSize.Width / 6, ClientSize.Height / 6);

            start.Location = new Point(0, 0);
            start.Size = new Size(ClientSize.Width / 10, ClientSize.Height / 10);

            towers.Location = new Point(0, ClientSize.Height * 2 / 3);
            towers.Size = new Size(ClientSize.Width - ClientSize.Height / 3, ClientSize.Height - towers.Top);
            ChangeTowersLayout();

            clicker.Location = new Point(towers.Right, towers.Top);
            clicker.Size = new Size(ClientSize.Width - towers.Right, ClientSize.Height - towers.Top);

            upgrades.Location = new Point(clicker.Left, 0);
            upgrades.Size = new Size(clicker.Width, clicker.Top);
            RepositionUpgrades();
            RepositionLevel(upgrades.Left / 20, upgrades.Left / 20);
            RepositionTowers(upgrades.Left / 20, upgrades.Left / 20);
            clickerAmount.Location = new Point(upgrades.Left - ClientSize.Width / 10, 0);
            clickerAmount.Size = new Size(ClientSize.Width / 10, ClientSize.Height / 10);

            livesAmount.Location = new Point(clickerAmount.Left - ClientSize.Width / 10, 0);
            livesAmount.Size = new Size(ClientSize.Width / 10, ClientSize.Height / 10);
        }

        void DrawLevel(int width, int height)
        {
            Path[0] = new MyLabel();
            Path[0].Location = new Point(0, start.Bottom);
            Path[0].Size = new Size(width, height);
            Controls.Add(Path[0]);
            for (var i = 1; i < 29; i++)
            {
                Path[i] = new MyLabel();
                if(i==4|| i == 6 || i == 17 || i == 20 || i == 25)
                    Path[i].Location = new Point(Path[i - 1].Left, Path[i - 1].Top-height);
                else if((i>=9&&i<=11)||i==13)
                    Path[i].Location = new Point(Path[i - 1].Left, Path[i - 1].Bottom);
                else
                    Path[i].Location = new Point(Path[i - 1].Right, Path[i - 1].Top);
                Path[i].Size = new Size(width, height);
                Controls.Add(Path[i]);
            }
        }

        void RepositionLevel(int width, int height)
        {
            Path[0].Location = new Point(0, towers.Top / 2);
            Path[0].Size = new Size(width, height);
            DrawPathImage(Path[0]);
            for (var i = 1; i < 29; i++)
            {
                if (i == 4 || i == 6 || i == 17 || i == 20 || i == 25)
                    Path[i].Location = new Point(Path[i - 1].Left, Path[i - 1].Top - height);
                else if ((i >= 9 && i <= 11) || i == 13)
                    Path[i].Location = new Point(Path[i - 1].Left, Path[i - 1].Bottom);
                else
                    Path[i].Location = new Point(Path[i - 1].Right, Path[i - 1].Top);
                Path[i].Size = new Size(width, height);
                DrawPathImage(Path[i]);
            }
        }

        void NextWave(object sender, EventArgs args)
        {
            timer.Stop();          
            foreach (var e in MainGame.Level1[levelNum % 3])
                NotOnLevel.Enqueue(new Monster(e.HealthPoint, e.Speed));
            levelNum++;
            if (levelNum >= 3)
                start.Enabled = false;
            var newMonster = NotOnLevel.Dequeue();
            Path[0].monster = newMonster;
            if (towerPlaces.Where(x => x.tower != null && x.tower.Target == null).Any())
            {
                foreach (var e in towerPlaces.Where(x => x.tower != null && x.tower.Target == null))
                    e.tower.SetTarget(newMonster);
            }
            OnLevel.Enqueue(newMonster);
            DrawMonster(newMonster, Path[0]);
            timer.Interval = 1000;
            timer.Start();
        }

        private void DrawMonster(Monster newMonster, MyLabel label)
        {
            Image img = Image.FromFile("data\\monster" + ((int)newMonster.HealthPoint / 10).ToString() + ".PNG");
            Bitmap bit = new Bitmap(img, label.Width, label.Height);
            label.Image = bit;
        }
        private void DrawPathImage( MyLabel label)
        {
            Image img = Image.FromFile("data\\path.png");        
            Bitmap bit = new Bitmap(img, label.Width, label.Height);
            label.Image = bit;
        }

        void MonsterMove(object obj, EventArgs s)
        {
            if (Path[28].monster != null)
            {
                MainGame.Lives--;
                livesAmount.Text = "Здоровье: " + MainGame.Lives.ToString();
                if (MainGame.Lives <= 0) 
                {
                    timer.Stop();
                    var answer = MessageBox.Show("Вы проиграли(", "Жаль", MessageBoxButtons.OK);
                    if (answer == DialogResult.OK) this.Close();
                }
                Path[28].monster = null;
                DrawPathImage(Path[28]);
            }
            for (var i = 27; i >= 0; i--)
            {
                if (Path[i].monster != null)
                {
                    if (!Path[i].monster.DeathFlag)
                    {
                        Path[i + 1].monster = Path[i].monster;
                        DrawMonster(Path[i + 1].monster, Path[i + 1]);
                    }
                    Path[i].monster = null;
                    DrawPathImage(Path[i]);
                }
            }

            var clear = towerPlaces.Where(x => x.tower != null).All(x => x.tower.Target == null);
            if (clear && levelNum >= 3 && NotOnLevel.Count == 0 && OnLevel.Count == 0)
            {
                timer.Stop();
                var answer = MessageBox.Show("Вы выйграли!!!", "Так держать!", MessageBoxButtons.OK);
                if (answer == DialogResult.OK) this.Close();
            }

            if (NotOnLevel.Count > 0)
            {
                var newMonster = NotOnLevel.Dequeue();
                Path[0].monster = newMonster;
                OnLevel.Enqueue(newMonster);
                DrawMonster(newMonster, Path[0]);
                if (towerPlaces.Where(x => x.tower != null && x.tower.Target == null).Any())
                {
                    foreach (var e in towerPlaces.Where(x => x.tower != null && x.tower.Target == null))
                        e.tower.SetTarget(newMonster);
                }
            }
        }

        void OnClickerClick(object sender, EventArgs args)
        {
            MainGame.clicker.ChangeAmount(1);
            clickerAmount.Text = "Ресурсов: "+MainGame.clicker.amount.ToString();
        }

        void RepositionUpgrades()
        {
            upgradeButtons[0].Location = new Point(10, 10);
            upgradeButtons[0].Size = new Size(clicker.Width - 20, upgrades.Height / upgradeButtons.Length - 40);
            for (var i = 1; i < upgradeButtons.Length; i++)
            {
                upgradeButtons[i].Location = new Point(10, upgradeButtons[i - 1].Bottom + 10);
                upgradeButtons[i].Size = new Size(clicker.Width - 20, upgrades.Height / upgradeButtons.Length - 40);
            }
        }

        void FillTowersBar()
        {
            for (var i = 0; i < towersControls.Length; i++)
            {
                towersControls[i] = new Button();
                towersControls[i].Name = i.ToString();
                towersControls[i].Font = new Font("Comic Sans MS", 14);
                towersControls[i].Text = "Цена: " + ((i + 1) * 50).ToString();
                towersControls[i].Click += (obj, args) =>
                {
                    var id = int.Parse((obj as Button).Name);
                    placingTowerId = id;
                };
                towersControls[i].BackgroundImage = Image.FromFile(MainGame.AvaliableTowers[i].Image);
                towersControls[i].BackgroundImageLayout = ImageLayout.Stretch;
                towers.Controls.Add(towersControls[i]);
            }
        }

        void ChangeTowersLayout()
        {
            int buttonSize;
            if (towers.Width >= towersControls.Length * towers.Height)
                buttonSize = towers.Height;
            else
                buttonSize = (towers.Width / towersControls.Length) - 10;
            towersControls[0].Location = new Point(10, 10);
            towersControls[0].Size = new Size(buttonSize - 10, buttonSize - 10);
            for (var i = 1; i < towersControls.Length; i++)
            {
                towersControls[i].Location = new Point(towersControls[i - 1].Right + 10, 10);
                towersControls[i].Size = new Size(buttonSize - 10, buttonSize - 10);
            }
        }

        void CreateTowerPlaces()
        {
            for (var i = 0; i < 3; i++)
            {
                towerPlaces[i] = new TowerPlace();
                towerPlaces[i].Name = i.ToString();
               towerPlaces[i].BackgroundImage = Image.FromFile("data\\emptyPlace2.PNG");
                towerPlaces[i].BackgroundImageLayout = ImageLayout.Stretch;
                towerPlaces[i].Click += (obj, args) =>
                    {
                        if (placingTowerId >= 0)
                        {
                            var id = int.Parse((obj as Button).Name);
                            if ((placingTowerId + 1) * 50 > MainGame.clicker.amount)
                            {
                                placingTowerId = -1;
                                return;
                            }
                            MainGame.clicker.ChangeAmount(-(placingTowerId + 1) * 50);
                            clickerAmount.Text = MainGame.clicker.amount.ToString();                           // var id = int.Parse((obj as Button).Name);
                                                                                                           //switch (placingTowerId)
                                                                                                           //{
                                                                                                           //    case 0:
                            towerPlaces[id].tower = new Tower(MainGame.AvaliableTowers[placingTowerId].Damage, MainGame.AvaliableTowers[placingTowerId].Speed);
                            towerPlaces[id].BackgroundImage = Image.FromFile(MainGame.AvaliableTowers[placingTowerId].Image);
                            towerPlaces[id].BackgroundImageLayout = ImageLayout.Stretch;
                            towerPlaces[id].tower.Notify += NewTarget;
                            towerPlaces[id].tower.WherePlaced = id;
                            if (OnLevel.Count > 0)
                                towerPlaces[id].tower.SetTarget(OnLevel.First());
                        }
                        placingTowerId = -1;
                    };
                Controls.Add(towerPlaces[i]);
            }
        }

        public void NewTarget(Tower tower, int placeId)
        {
            lock (OnLevel)
            {
                if (OnLevel.Count > 0)
                {
                    tower.SetTarget(OnLevel.Dequeue());
                }
            }
        }

        void RepositionTowers(int width, int height)
        {
            for (var i = 1; i < 4; i++)
            {
                if (i == 2)
                    towerPlaces[i - 1].Location = new Point(Path[i * 7].Right, Path[i * 7].Top-height);
                else
                    towerPlaces[i - 1].Location = new Point(Path[i * 7].Left, Path[i * 7].Bottom);
                towerPlaces[i - 1].Size = new Size(width, height);
            }
        }
    }
}
