using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Form1 : Form
    {
        SqlConnection conexion = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicialización si es necesaria
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                string consulta = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario AND pass = @pass";
                SqlCommand comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@usuario", txtusuario.Text);
                comando.Parameters.AddWithValue("@pass", txtcontraseña.Text);

                int count = Convert.ToInt32(comando.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Bienvenido");
                    Form inicio = new Form4();
                    inicio.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form formulario = new Form2();
            formulario.Show();
            this.Hide();
        }

        private void btnAyuda_Click(object sender, EventArgs e)
        {
            Ayuda ayudaForm = new Ayuda(this); // Pasamos la instancia actual
            ayudaForm.Show();
            this.Hide();
        }

        // Puedes eliminar estos si no los estás usando
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { }
    }
}

