using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projeto4
{
    public partial class TelaPrincipal : Form
    {
        string conexaoString = "server=localhost;database=controle_contatos;uid=root;pwd=sua_senha;";
        MySqlConnection conexao;
        public TelaPrincipal()
        {
            InitializeComponent();
            conexao = new MySqlConnection(conexaoString);
            CarregarContatos();
        }
        private void CarregarContatos(string filtro = "")
        {
            try
            {
                conexao.Open();

                string sql = "SELECT id, nome, telefone, email FROM contatos";

                if (!string.IsNullOrEmpty(filtro))
                {
                    sql += " WHERE nome LIKE @filtro";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conexao);

                if (!string.IsNullOrEmpty(filtro))
                {
                    cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvContatos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar contatos: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void TelaPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBusca.Text.Trim();
            CarregarContatos(filtro);
        }

        private void TelaPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            FormContato fc = new FormContato();

            if (fc.ShowDialog() == DialogResult.OK)
            {
                CarregarContatos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvContatos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um contato para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvContatos.CurrentRow.Cells["id"].Value);

            FormContato fc = new FormContato();
            fc.CarregarContato(id);

            if (fc.ShowDialog() == DialogResult.OK)
            {
                CarregarContatos();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvContatos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um contato para excluir.");
                return;
            }

            if (MessageBox.Show("Tem certeza que deseja excluir?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvContatos.CurrentRow.Cells["id"].Value);
                string conexaoString = "server=localhost;database=controle_contatos;uid=root;pwd=sua_senha;";

                using (MySqlConnection conexao = new MySqlConnection(conexaoString))
                {
                    try
                    {
                        conexao.Open();
                        string sql = "DELETE FROM contatos WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(sql, conexao);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Contato excluído com sucesso!");
                        CarregarContatos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir: " + ex.Message);
                    }
                }
            }
        }
    }
}
