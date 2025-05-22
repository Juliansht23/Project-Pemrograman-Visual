using System;
using System.Drawing;
using System.Windows.Forms;

namespace src
{
    public partial class FormUsers : Form
    {
        private DataGridView? dgv;

        public FormUsers()
        {
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Text = "Manajemen Pengguna";
            this.Size = new Size(700, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;
            this.WindowState = FormWindowState.Maximized;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(layout);

            Panel panelTop = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(10)
            };
            layout.Controls.Add(panelTop, 0, 0);

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true
            };
            panelTop.Controls.Add(buttonPanel);

            Button btnAdd = CreateButton("Tambah", BtnAdd_Click);
            buttonPanel.Controls.Add(btnAdd);

            Button btnEdit = CreateButton("Edit", BtnEdit_Click);
            buttonPanel.Controls.Add(btnEdit);

            Button btnDelete = CreateButton("Hapus", BtnDelete_Click);
            buttonPanel.Controls.Add(btnDelete);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowTemplate = { Height = 35 },
                Font = new Font("Segoe UI", 10),
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(200, 200, 200),
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                EnableHeadersVisualStyles = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(245, 245, 245)
                }
            };
            layout.Controls.Add(dgv, 0, 1);

            dgv?.Columns.Add("usersId", "ID");
            dgv?.Columns.Add("rumahId", "Rumah ID");
            dgv?.Columns.Add("nama", "Nama");
            dgv?.Columns.Add("usia", "Usia");
            dgv?.Columns.Add("jenis_kelamin", "Jenis Kelamin");
            dgv?.Columns.Add("telepon", "Telepon");
        }

        private Button CreateButton(string text, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(80, 30),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(5)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.DodgerBlue;
            btn.MouseLeave += (s, e) => btn.BackColor = Color.LightBlue;

            return btn;
        }

        private void LoadData()
        {
            dgv?.Rows.Clear();
            var users = DatabaseHelper.GetUsers();

            foreach (var u in users)
            {
                dgv?.Rows.Add(u.UsersId, u.RumahId, u.Nama, u.Usia, u.JenisKelamin, u.Telepon);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var dialog = new UsersFormDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DatabaseHelper.InsertUser(new DatabaseHelper.Users
                {
                    RumahId = dialog.RumahId,
                    Nama = dialog.Nama,
                    Usia = dialog.Usia,
                    JenisKelamin = dialog.JenisKelamin,
                    Telepon = dialog.Telepon
                });
                LoadData();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                var row = dgv.SelectedRows[0];
                var dialog = new UsersFormDialog
                {
                    RumahId = int.TryParse(row.Cells["rumahId"].Value?.ToString(), out var rId) ? rId : 0,
                    Nama = row.Cells["nama"].Value?.ToString(),
                    Usia = row.Cells["usia"].Value?.ToString(),
                    JenisKelamin = row.Cells["jenis_kelamin"].Value?.ToString(),
                    Telepon = row.Cells["telepon"].Value?.ToString()
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    int.TryParse(row.Cells["usersId"].Value?.ToString(), out var id);
                    DatabaseHelper.UpdateUser(new DatabaseHelper.Users
                    {
                        UsersId = id,
                        RumahId = dialog.RumahId,
                        Nama = dialog.Nama,
                        Usia = dialog.Usia,
                        JenisKelamin = dialog.JenisKelamin,
                        Telepon = dialog.Telepon
                    });
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Pilih satu baris yang mau diedit.");
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                var row = dgv.SelectedRows[0];
                var confirm = MessageBox.Show("Yakin mau hapus?", "Konfirmasi", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    int.TryParse(row.Cells["usersId"].Value?.ToString(), out var id);
                    DatabaseHelper.DeleteUser(id);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Pilih satu baris yang mau dihapus.");
            }
        }
    }
}
