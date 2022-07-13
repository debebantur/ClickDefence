using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ClickDefend
{
    public partial class Menue : Form
    {
        public Button ReadMe;
        public Button ChooseLevel;
        public Button Close;
        public Menue()
        {
            ChooseLevel = new Button();
            ChooseLevel.Click += OpenGame;
            ChooseLevel.Text = "Старт";
            Controls.Add(ChooseLevel);

            ReadMe = new Button();
            ReadMe.Click += ShowReadme;
            ReadMe.Text = "Руководство";
            Controls.Add(ReadMe);

            Close = new Button();
            Close.Click += (obj, args)=> {
                var answer = MessageBox.Show("Вы хотите выйти?", "Неужто!?", MessageBoxButtons.YesNo);
                if(answer == DialogResult.Yes) this.Close(); 
            };
            Close.Text = "Выход";
            Controls.Add(Close);

            SizeChanged += RepositionControls;
            Load += RepositionControls;

            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        void RepositionControls(object sender, EventArgs args)
        {
            ChooseLevel.Location = new Point(ClientSize.Width / 4, ClientSize.Height / 7);
            ChooseLevel.Size = new Size(ClientSize.Width / 2, ClientSize.Height / 7);

            ReadMe.Location = new Point(ChooseLevel.Left, ChooseLevel.Bottom+ ClientSize.Height / 7);
            ReadMe.Size = ChooseLevel.Size;

            Close.Location = new Point(ChooseLevel.Left, ReadMe.Bottom + ClientSize.Height / 7);
            Close.Size = ChooseLevel.Size;
        }

        public void ShowReadme(object sender, EventArgs args)
        {
            var place = new Panel();
            place.Location = new Point(0, 0);
            place.Size = this.Size;
            place.BackgroundImage = Image.FromFile("data\\b1.png");
            place.BackgroundImageLayout = ImageLayout.Stretch;

            var closePanel = new Button();
            closePanel.Click += (obj, args2)=> { place.Dispose(); };
            closePanel.Text = "Ок";
            closePanel.Location = new Point(place.Width/2- place.Width / 20, place.Height-place.Height/6);
            closePanel.Size = new Size(place.Width / 10, place.Height / 15);
            closePanel.BringToFront();
            place.Controls.Add(closePanel);

            var info = new Label();
            info.Location = new Point(60, 0);
            info.Size = new Size( place.Width-120, place.Height - place.Height / 6);
            info.BackColor = Color.Transparent;
            info.Text = System.IO.File.ReadAllText(@"data\ReadMe.TXT");
            info.Font = new Font("Comic Sans MS", 14);
            info.TextAlign = ContentAlignment.MiddleCenter;           
            place.Controls.Add(info);

            this.Controls.Add(place);
            place.BringToFront();

            SizeChanged += (obg, args3) => {
                if (place == null)
                    return;
                place.Size = this.Size;
                closePanel.Location = new Point(place.Width / 2 - place.Width / 20, place.Height - place.Height / 6);
                closePanel.Size = new Size(place.Width / 10, place.Height / 15);
                info.Location = new Point(60, 0);
                info.Size = new Size(place.Width - 120, place.Height - place.Height / 6);
            };
        }

        public void OpenGame(object sender, EventArgs args)
        {
            var newForm = new MainWindow(this);
            newForm.Show();
            this.Hide();
        }
    }
}
