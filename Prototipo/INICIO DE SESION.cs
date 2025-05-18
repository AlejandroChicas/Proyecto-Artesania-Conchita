using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Org.BouncyCastle.Cms;
using System.Diagnostics.Eventing.Reader;
using System.Data;


namespace Prototipo
{
    public partial class Form1 : Form
    {
        SQLcontrol sQLcontrol = new SQLcontrol();

        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conexion = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;");



        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form formulario = new Form2();
            formulario.Show();
            this.Hide();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conexion.Open();
            string consulta = "select * from usuarios where usuario='" + txtusuario.Text + "' and pass='" + txtcontraseña.Text + "'";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            SqlDataReader lector;
            lector = comando.ExecuteReader();

            if (lector.HasRows == true)
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
            conexion.Close();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BtnAyuda_Click(object sender, EventArgs e)
        {
            this.Hide(); // Opcional: oculta el formulario actual
            Ayuda ayuda = new Ayuda(this); // Le pasas el formulario actual
            ayuda.Show(); // Puedes usar ShowDialog() si prefieres que sea modal
        }

        private void btnAyuda_Click(object sender, EventArgs e)
        {
            Ayuda ayudaForm = new Ayuda(this); // Pasamos la instancia actual
            this.Hide();
            ayudaForm.Show();
        }
    }
}
