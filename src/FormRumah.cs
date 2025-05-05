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
            LoadData(); // Load from database
        }

        private void SetupUI()
        {
            this.Text = "Rumah Management";
            this.Size = new Size(700, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

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

            dgv?.Columns.Add("rumahId", "Rumah ID");
            dgv?.Columns.Add("alamat", "Alamat");
            dgv?.Columns.Add("status", "Status");
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
            var rumah = Database.GetRumah();

            foreach (var r in rumah)
            {
                dgv?.Rows.Add(r.RumahId, r.Alamat ?? "", r.Status ?? "");

            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using RumahFormDialog dialog = new RumahFormDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Database.InsertRumah(new Database.Rumah
                {
                    Alamat = dialog.Alamat,
                    Status = dialog.Status,
                });

                LoadData();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                var selectedRow = dgv.SelectedRows[0];
                if (selectedRow != null)
                {
                    RumahFormDialog dialog = new RumahFormDialog
                    {
                        // ProductId = selectedRow.Cells["productId"]?.Value?.ToString(),
                        Alamat = selectedRow.Cells["alamat"]?.Value?.ToString(),
                        Status = selectedRow.Cells["status"]?.Value?.ToString(),
                    };

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Database.UpdateRumah(new Database.Rumah
                        {
                            Alamat = dialog.Alamat,
                            Status = dialog.Status,
                        });

                        LoadData();
                    }
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
                var selectedRow = dgv.SelectedRows[0];
                if (selectedRow != null)
                {
                    var confirm = MessageBox.Show("Yakin mau hapus?", "Konfirmasi", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        string? rumahId = selectedRow.Cells["rumahId"]?.Value?.ToString();
                        // if (!string.IsNullOrEmpty(productId))
                        // {
                        //     Database.DeleteProduct(productId);
                        //     LoadData();
                        // }
                        if (int.TryParse(rumahId, out int id))
                        {
                            Database.DeleteRumah(id);
                            LoadData();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih satu baris yang mau dihapus.");
            }
        }

    }
}