using System;
using System.Drawing;
using System.Windows.Forms;

namespace src
{
    public partial class FormRumah : Form
    {
        private DataGridView? dgv;

        public FormRumah()
        {
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Text = "Manajemen Rumah";
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
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(layout);

            Panel panelTop = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240),
                // Padding = new Padding(10),
                // Margin = new Padding(0, 0, 0, 10) // Tambahkan jarak bawah 10 pixel
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

            buttonPanel.Controls.Add(CreateButton("Tambah", BtnAdd_Click));
            buttonPanel.Controls.Add(CreateButton("Edit", BtnEdit_Click));
            buttonPanel.Controls.Add(CreateButton("Hapus", BtnDelete_Click));

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

            dgv.Columns.Add("rumahId", "ID");
            dgv.Columns.Add("alamat", "Alamat");
            dgv.Columns.Add("status", "Status");
        }

        private Button CreateButton(string text, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(80, 30),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
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
            var rumahList = DatabaseHelper.GetRumah();

            foreach (var r in rumahList)
            {
                dgv?.Rows.Add(r.RumahId, r.Alamat, r.Status);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using RumahFormDialog dialog = new RumahFormDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DatabaseHelper.InsertRumah(new DatabaseHelper.Rumah
                {
                    Alamat = dialog.Alamat,
                    Status = dialog.Status
                });

                LoadData();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                var row = dgv.SelectedRows[0];
                RumahFormDialog dialog = new RumahFormDialog
                {
                    Alamat = row.Cells["alamat"]?.Value?.ToString(),
                    Status = row.Cells["status"]?.Value?.ToString()
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (int.TryParse(row.Cells["rumahId"].Value?.ToString(), out int id))
                    {
                        DatabaseHelper.UpdateRumah(new DatabaseHelper.Rumah
                        {
                            RumahId = id,
                            Alamat = dialog.Alamat,
                            Status = dialog.Status
                        });

                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih salah satu baris untuk diedit.");
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                var row = dgv.SelectedRows[0];
                var confirm = MessageBox.Show("Yakin ingin menghapus?", "Konfirmasi", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    if (int.TryParse(row.Cells["rumahId"].Value?.ToString(), out int id))
                    {
                        DatabaseHelper.DeleteRumah(id);
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih salah satu baris untuk dihapus.");
            }
        }
    }
}
