using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace src
{
    public partial class UsersFormDialog : Form
    {
        private ComboBox comboBoxRumah = null!;
        private TextBox textBoxNama = null!;
        private TextBox textBoxUsia = null!;
        private ComboBox comboBoxJenisKelamin = null!;
        private TextBox textBoxTelepon = null!;
        private Button buttonSave = null!;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RumahId
        {
            get => comboBoxRumah?.SelectedValue != null ? Convert.ToInt32(comboBoxRumah.SelectedValue) : 0;
            set
            {
                if (comboBoxRumah != null && comboBoxRumah.Items.Count > 0)
                    comboBoxRumah.SelectedValue = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Nama
        {
            get => textBoxNama?.Text;
            set
            {
                if (textBoxNama != null)
                    textBoxNama.Text = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Usia
        {
            get => textBoxUsia?.Text;
            set
            {
                if (textBoxUsia != null)
                    textBoxUsia.Text = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? JenisKelamin
        {
            get => comboBoxJenisKelamin?.SelectedItem?.ToString();
            set
            {
                if (comboBoxJenisKelamin != null)
                    comboBoxJenisKelamin.SelectedItem = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Telepon
        {
            get => textBoxTelepon?.Text;
            set
            {
                if (textBoxTelepon != null)
                    textBoxTelepon.Text = value;
            }
        }

        public UsersFormDialog()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Form Pengguna";
            this.Size = new Size(350, 350);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 6,
                ColumnCount = 2,
                Padding = new Padding(20)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            this.Controls.Add(layout);

            // Rumah
            layout.Controls.Add(new Label { Text = "Rumah", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            comboBoxRumah = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            layout.Controls.Add(comboBoxRumah, 1, 0);

            // Nama
            layout.Controls.Add(new Label { Text = "Nama", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            textBoxNama = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(textBoxNama, 1, 1);

            // Usia
            layout.Controls.Add(new Label { Text = "Usia", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
            textBoxUsia = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(textBoxUsia, 1, 2);

            // Jenis Kelamin
            layout.Controls.Add(new Label { Text = "Jenis Kelamin", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 3);
            comboBoxJenisKelamin = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxJenisKelamin.Items.AddRange(new string[] { "Laki-laki", "Perempuan" });
            layout.Controls.Add(comboBoxJenisKelamin, 1, 3);

            // Telepon
            layout.Controls.Add(new Label { Text = "Telepon", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 4);
            textBoxTelepon = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(textBoxTelepon, 1, 4);

            // Tombol Simpan
            buttonSave = new Button
            {
                Text = "Simpan",
                Size = new Size(100, 35),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            buttonSave.FlatAppearance.BorderSize = 0;
            buttonSave.Click += ButtonSave_Click;

            Panel panelButton = new Panel { Dock = DockStyle.Fill };
            panelButton.Controls.Add(buttonSave);
            buttonSave.Location = new Point((panelButton.Width - buttonSave.Width) / 2, (panelButton.Height - buttonSave.Height) / 2);
            buttonSave.Anchor = AnchorStyles.None;
            layout.Controls.Add(panelButton, 0, 5);
            layout.SetColumnSpan(panelButton, 2);

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            this.Load += UsersFormDialog_Load;
        }

        private void UsersFormDialog_Load(object? sender, EventArgs e)
        {
            var rumahList = DatabaseHelper.GetRumah();
            comboBoxRumah.DisplayMember = "Alamat";
            comboBoxRumah.ValueMember = "RumahId";
            comboBoxRumah.DataSource = rumahList;
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNama?.Text) ||
                string.IsNullOrWhiteSpace(textBoxUsia?.Text) ||
                comboBoxJenisKelamin?.SelectedItem == null ||
                string.IsNullOrWhiteSpace(textBoxTelepon?.Text))
            {
                MessageBox.Show("Mohon isi semua field dengan lengkap!");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
