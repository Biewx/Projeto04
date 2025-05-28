using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace projeto4
{
    public partial class FormContato : Form
    {
        public int? ContatoId { get; set; } // null para novo, ou ID para edição

        public FormContato()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string telefone = txtTelefone.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("O nome é obrigatório.");
                return;
            }

            string conexaoString = "server=localhost;database=controle_contatos;uid=root;pwd=sua_senha;";

            using (MySqlConnection conexao = new MySqlConnection(conexaoString))
            {
                try
                {
                    conexao.Open();

                    MySqlCommand cmd;

                    if (ContatoId == null)
                    {
                        // Inserir novo contato
                        string sql = "INSERT INTO contatos (nome, telefone, email) VALUES (@nome, @telefone, @email)";
                        cmd = new MySqlCommand(sql, conexao);
                    }
                    else
                    {
                        // Atualizar contato existente
                        string sql = "UPDATE contatos SET nome = @nome, telefone = @telefone, email = @email WHERE id = @id";
                        cmd = new MySqlCommand(sql, conexao);
                        cmd.Parameters.AddWithValue("@id", ContatoId);
                    }

                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato salvo com sucesso!");
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar: " + ex.Message);
                }
            }

        }
        public void CarregarContato(int id)
        {
            string conexaoString = "server=localhost;database=controle_contatos;uid=root;pwd=sua_senha;";

            using (MySqlConnection conexao = new MySqlConnection(conexaoString))
            {
                try
                {
                    conexao.Open();

                    string sql = "SELECT * FROM contatos WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conexao);
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtNome.Text = reader["nome"].ToString();
                        txtTelefone.Text = reader["telefone"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        ContatoId = id;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar contato: " + ex.Message);
                }
            }
        }
    }
}
