using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace MonitorLayoutManager
{
    public partial class MainForm : Form
    {
        private static readonly string DefaultPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "layout.json");

        public MainForm()
        {
            InitializeComponent();
            RefreshMonitorList();
        }

        // ── botão Salvar ────────────────────────────────────────────────────
        private void btnSave_Click(object sender, EventArgs e)
        {
            var monitors = DisplayHelper.GetCurrentLayout();
            if (monitors.Count == 0)
            {
                MessageBox.Show("Nenhum monitor detectado.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string path = txtPath.Text.Trim();
            if (string.IsNullOrEmpty(path)) path = DefaultPath;

            try
            {
                var layout = new SavedLayout
                {
                    Name = $"Layout {DateTime.Now:yyyy-MM-dd HH:mm}",
                    Monitors = monitors
                };

                string json = Serialize(layout);
                File.WriteAllText(path, json, Encoding.UTF8);

                lblStatus.Text = $"Salvo em: {path}";
                RefreshMonitorList();
                MessageBox.Show($"Layout salvo com {monitors.Count} monitor(es).", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Erro ao salvar.";
                MessageBox.Show("Erro ao salvar:\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── botão Configurar ────────────────────────────────────────────────
        private void btnApply_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text.Trim();
            if (string.IsNullOrEmpty(path)) path = DefaultPath;

            if (!File.Exists(path))
            {
                MessageBox.Show($"Arquivo não encontrado:\n{path}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string json = File.ReadAllText(path, Encoding.UTF8);
                SavedLayout layout = Deserialize(json);

                var result = DisplayHelper.ApplyLayout(layout.Monitors);

                if (result.Success)
                {
                    string msg = "Layout aplicado com sucesso!";
                    if (result.Skipped.Count > 0)
                        msg += "\n\nMonitores não conectados (ignorados):\n  " +
                               string.Join("\n  ", result.Skipped);

                    lblStatus.Text = string.Format("Configuração '{0}' aplicada.", layout.Name);
                    RefreshMonitorList();
                    MessageBox.Show(msg, "Sucesso",
                        MessageBoxButtons.OK,
                        result.Skipped.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = "Falha ao aplicar.";
                    MessageBox.Show("Erro ao aplicar layout:\n" + result.Error, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Erro ao ler arquivo.";
                MessageBox.Show("Erro ao ler arquivo:\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── botão Brilho 100% ───────────────────────────────────────────────
        private void btnBrightness_Click(object sender, EventArgs e)
        {
            var result = BrightnessHelper.SetAllBrightness(100);

            if (result.Applied == 0 && result.Errors.Count > 0)
            {
                lblStatus.Text = "Brilho: DDC/CI não suportado.";
                MessageBox.Show(
                    "Nenhum monitor aceitou o comando de brilho via DDC/CI.\n\n" +
                    "Possíveis causas:\n" +
                    "  • Monitor não suporta DDC/CI\n" +
                    "  • Conectado via adaptador sem passagem DDC\n" +
                    "  • Driver bloqueando acesso\n\n" +
                    string.Join("\n", result.Errors),
                    "Brilho não suportado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (result.Applied == 0)
            {
                lblStatus.Text = "Brilho: nenhum monitor encontrado.";
                MessageBox.Show("Nenhum monitor físico encontrado.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string msg = string.Format("Brilho ajustado para 100% em {0} monitor(es).", result.Applied);
                if (result.Errors.Count > 0)
                    msg += "\n\nIgnorados (sem DDC/CI):\n  " + string.Join("\n  ", result.Errors);

                lblStatus.Text = string.Format("Brilho 100% aplicado em {0} monitor(es).", result.Applied);
                MessageBox.Show(msg, "Brilho",
                    MessageBoxButtons.OK,
                    result.Errors.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
        }

        // ── botão Procurar arquivo ──────────────────────────────────────────
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Selecionar arquivo de layout";
                dlg.Filter = "JSON (*.json)|*.json|Todos (*.*)|*.*";
                dlg.FileName = txtPath.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtPath.Text = dlg.FileName;
            }
        }

        // ── atualiza lista de monitores ─────────────────────────────────────
        private void RefreshMonitorList()
        {
            listMonitors.Items.Clear();
            var monitors = DisplayHelper.GetCurrentLayout();
            foreach (var m in monitors)
            {
                string primary = m.IsPrimary ? " [Principal]" : "";
                string item =
                    $"{m.DeviceName}{primary}  |  " +
                    $"{m.Width}x{m.Height} @ {m.DisplayFrequency}Hz  |  " +
                    $"Pos: ({m.PositionX}, {m.PositionY})";
                listMonitors.Items.Add(item);
            }
        }

        // ── JSON helpers (sem dependência externa) ──────────────────────────
        private static string Serialize(SavedLayout layout)
        {
            var ser = new DataContractJsonSerializer(typeof(SavedLayout),
                new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, layout);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static SavedLayout Deserialize(string json)
        {
            var ser = new DataContractJsonSerializer(typeof(SavedLayout),
                new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (SavedLayout)ser.ReadObject(ms);
        }
    }
}
