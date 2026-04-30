namespace MonitorLayoutManager
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMonitors = new System.Windows.Forms.Label();
            this.listMonitors = new System.Windows.Forms.ListBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnBrightness = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Text = "Ajuste de Tela";

            // lblMonitors
            this.lblMonitors.AutoSize = true;
            this.lblMonitors.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMonitors.Location = new System.Drawing.Point(12, 50);
            this.lblMonitors.Text = "Monitores detectados:";

            // listMonitors
            this.listMonitors.Font = new System.Drawing.Font("Consolas", 9F);
            this.listMonitors.FormattingEnabled = true;
            this.listMonitors.ItemHeight = 18;
            this.listMonitors.Location = new System.Drawing.Point(15, 72);
            this.listMonitors.Size = new System.Drawing.Size(770, 112);
            this.listMonitors.TabIndex = 0;

            // lblFile
            this.lblFile.AutoSize = true;
            this.lblFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFile.Location = new System.Drawing.Point(12, 202);
            this.lblFile.Text = "Arquivo de layout:";

            // txtPath
            this.txtPath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPath.Location = new System.Drawing.Point(15, 222);
            this.txtPath.Size = new System.Drawing.Size(660, 23);
            this.txtPath.TabIndex = 1;
            this.txtPath.Text = System.IO.Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory, "layout.json");

            // btnBrowse
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBrowse.Location = new System.Drawing.Point(685, 220);
            this.btnBrowse.Size = new System.Drawing.Size(100, 27);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Procurar...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(15, 265);
            this.btnSave.Size = new System.Drawing.Size(180, 40);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Salvar Layout Atual";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnApply
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(16, 137, 62);
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(210, 265);
            this.btnApply.Size = new System.Drawing.Size(180, 40);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Configurar Monitores";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);

            // btnBrightness
            this.btnBrightness.BackColor = System.Drawing.Color.FromArgb(136, 68, 0);
            this.btnBrightness.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightness.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBrightness.ForeColor = System.Drawing.Color.White;
            this.btnBrightness.Location = new System.Drawing.Point(405, 265);
            this.btnBrightness.Size = new System.Drawing.Size(180, 40);
            this.btnBrightness.TabIndex = 5;
            this.btnBrightness.Text = "Brilho 100%";
            this.btnBrightness.UseVisualStyleBackColor = false;
            this.btnBrightness.Click += new System.EventHandler(this.btnBrightness_Click);

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(12, 318);
            this.lblStatus.Size = new System.Drawing.Size(400, 19);
            this.lblStatus.Text = "Pronto.";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 350);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMonitors);
            this.Controls.Add(this.listMonitors);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnBrightness);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajuste de Tela — Monitor Layout Manager";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMonitors;
        private System.Windows.Forms.ListBox listMonitors;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnBrightness;
        private System.Windows.Forms.Label lblStatus;
    }
}
