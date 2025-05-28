using MySql.Data.MySqlClient;

namespace projeto4
{
    public partial class TelaLogin : Form
    {
        string conexaoString = "server=localhost;database=controle_contatos;uid=root;pwd='';";
        MySqlConnection conexao;
        public TelaLogin()
        {
            InitializeComponent();
            conexao = new MySqlConnection(conexaoString);
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string senha = txtSenha.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Digite usuário e senha!");
                return;
            }

            try
            {
                conexao.Open();

                string query = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario AND senha = @senha";
                MySqlCommand cmd = new MySqlCommand(query, conexao);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@senha", senha);

                int resultado = Convert.ToInt32(cmd.ExecuteScalar());

                if (resultado > 0)
                {
                    MessageBox.Show("Login realizado com sucesso!");
                    if (resultado > 0)
                    {
                        this.Hide();
                        TelaPrincipal fp = new TelaPrincipal();
                        fp.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Usuário ou senha inválidos!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar com banco: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
      }
    }
