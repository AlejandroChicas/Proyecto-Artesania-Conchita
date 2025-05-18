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
    public partial class Form2 : Form
    {
        public string NombreCompleto => txtnombre.Text;
        public string CorreoElectronico => txtcorreo.Text;
        public string Contraseña => txtcontraseña.Text;
        public string Telefono => txttelefono.Text;
        public string Cargo => txtcargo.Text;


        public Form2()
        {
            InitializeComponent();
        }




        public class UsuarioManager
        {
            // Cambia esta cadena de conexión a la configuración de tu base de datos
            private string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";

            public void CrearUsuario(string usuario, string correo, string pass, string telefono, string cargo)
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    try
                    {
                        conexion.Open();

                        string consulta = "INSERT INTO usuarios (usuario, correo, pass, telefono, cargo) " +
                                          "VALUES (@usuario, @correo, @pass, @telefono, @cargo)";

                        using (SqlCommand comando = new SqlCommand(consulta, conexion))
                        {
                            // Uso de parámetros para evitar SQL Injection
                            comando.Parameters.AddWithValue("@usuario", usuario);
                            comando.Parameters.AddWithValue("@correo", correo);
                            comando.Parameters.AddWithValue("@pass", pass);
                            comando.Parameters.AddWithValue("@telefono", telefono);
                            comando.Parameters.AddWithValue("@cargo", cargo);

                            int filasAfectadas = comando.ExecuteNonQuery();

                            if (filasAfectadas > 0)
                            {
                                MessageBox.Show("Usuario creado exitosamente.");
                            }
                            else
                            {
                                MessageBox.Show("Error al crear el usuario.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error: " + ex.Message);
                    }
                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            // Crear una instancia de UsuarioManager
            UsuarioManager usuarioManager = new UsuarioManager();

            // Obtener los valores de los campos de texto del formulario
            string nombreCompleto = txtnombre.Text;
            string correoElectronico = txtcorreo.Text;
            string contraseña = txtcontraseña.Text;
            string telefono = txttelefono.Text;
            string cargo = txtcargo.Text;

            // Llamar al método CrearUsuario con los valores obtenidos
            usuarioManager.CrearUsuario(nombreCompleto, correoElectronico, contraseña, telefono, cargo);
        }









        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form formulario = new Form1();
            formulario.Show();
            this.Hide();
        }





        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtnombre_TextChanged(object sender, EventArgs e)
        {

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            // Aquí puedes agregar cualquier código que quieras ejecutar cuando se cargue Form2
        }

    }
}

