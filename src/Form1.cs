using System.Drawing;
using System.Windows.Forms;

namespace src
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Size = new Size(1400, 1000);
            SetupUI();
        }

        private void SetupUI()
        {
            // Header
            Panel header = new Panel
            {
                Size = new Size(this.Width, 80),
                BackColor = Color.FromArgb(50, 50, 50),
                Dock = DockStyle.Top
            };
            this.Controls.Add(header);

            // Icon toko (kiri)
            PictureBox shopIcon = new PictureBox
            {
                ImageLocation = "./assets/store.png",
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(50, 50),
                Location = new Point(10, 15)
            };
            header.Controls.Add(shopIcon);

            // Icon profile (kanan)
            PictureBox profileIcon = new PictureBox
            {
                ImageLocation = "./assets/user_1.png",
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(40, 40),
                Location = new Point(header.Width - 60, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            header.Controls.Add(profileIcon);

            // FlowLayoutPanel untuk card
            FlowLayoutPanel panelCards = new FlowLayoutPanel
            {
                Padding = new Padding(30),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Anchor = AnchorStyles.None,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            Panel containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            containerPanel.Controls.Add(panelCards);

            containerPanel.Resize += (s, e) =>
            {
                panelCards.Location = new Point(
                    (containerPanel.ClientSize.Width - panelCards.PreferredSize.Width) / 2,
                    (containerPanel.ClientSize.Height - panelCards.PreferredSize.Height) / 2
                );
            };

            this.Load += (s, e) =>
            {
                panelCards.Location = new Point(
                    (containerPanel.ClientSize.Width - panelCards.PreferredSize.Width) / 2,
                    (containerPanel.ClientSize.Height - panelCards.PreferredSize.Height) / 2
                );
            };

            this.Controls.Add(containerPanel);

            // Card Rumah
            panelCards.Controls.Add(CreateCard("Rumah", Color.LightSkyBlue, RumahCard_Click));

            // Card Users
            panelCards.Controls.Add(CreateCard("Users", Color.LightGoldenrodYellow, UsersCard_Click));
        }

        private Panel CreateCard(string title, Color color, EventHandler onClick)
        {
            Panel card = new Panel
            {
                Size = new Size(200, 150),
                BackColor = color,
                Margin = new Padding(20),
                Cursor = Cursors.Hand
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            card.Controls.Add(lblTitle);

            // Event click
            card.Click += onClick;
            lblTitle.Click += onClick;

            return card;
        }

        private void RumahCard_Click(object? sender, EventArgs e)
        {
            FormRumah rumahForm = new FormRumah();
            rumahForm.ShowDialog();
        }

        private void UsersCard_Click(object? sender, EventArgs e)
        {
            FormUsers usersForm = new FormUsers();
            usersForm.ShowDialog();
        }
    }
}

