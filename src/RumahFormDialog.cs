using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace src
{
    public partial class RumahFormDialog : Form
    {
        private TextBox? txtAlamat, txtStatus;
        private Button? btnSave;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Alamat { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Status { get; set; }

        public RumahFormDialog()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Detail Rumah";
            this.Size = new Size(350, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            this.Controls.Add(layout);

            layout.Controls.Add(new Label { Text = "Alamat", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            txtAlamat = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(txtAlamat, 1, 0);

            layout.Controls.Add(new Label { Text = "Status", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 1);
            txtStatus = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(txtStatus, 1, 1);

            btnSave = new Button
            {
                Text = "Simpan",
                Dock = DockStyle.None,
                Size = new Size(100, 35),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnSave.MouseEnter += (s, e) => btnSave.BackColor = Color.MediumSeaGreen;
            btnSave.MouseLeave += (s, e) => btnSave.BackColor = Color.LightGreen;

            Panel panelButton = new Panel { Dock = DockStyle.Fill };
            panelButton.Controls.Add(btnSave);
            btnSave.Location = new Point((panelButton.Width - btnSave.Width) / 2, (panelButton.Height - btnSave.Height) / 2);
            btnSave.Anchor = AnchorStyles.None;
            layout.Controls.Add(panelButton, 0, 2);
            layout.SetColumnSpan(panelButton, 2);

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            Alamat = txtAlamat?.Text;
            Status = txtStatus?.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (txtAlamat != null) txtAlamat.Text = Alamat ?? "";
            if (txtStatus != null) txtStatus.Text = Status ?? "";
        }
    }
}
