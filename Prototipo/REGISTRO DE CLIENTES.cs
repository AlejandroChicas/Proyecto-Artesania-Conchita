using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Form7 : Form
    {
        // Conexión única para el formulario
        private readonly string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            MostrarRegistros();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDireccion.Clear();
            txtDui.Clear();
            txtDepartamento.Clear();
            txtTelefono.Clear();
            dtpFecha.Value = DateTime.Now;
        }

        private void MostrarRegistros()
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT * FROM clientes";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvClientes.DataSource = tabla;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al mostrar los registros: " + ex.Message);
                }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    string query = "INSERT INTO clientes (Nombre, Direccion, DUI, Fecha, Departamento, Telefono) " +
                                   "VALUES (@Nombre, @Direccion, @DUI, @Fecha, @Departamento, @Telefono)";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    comando.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                    comando.Parameters.AddWithValue("@DUI", txtDui.Text);
                    comando.Parameters.AddWithValue("@Fecha", dtpFecha.Value.Date);
                    comando.Parameters.AddWithValue("@Departamento", txtDepartamento.Text);
                    comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Cliente registrado con éxito.");
                    MostrarRegistros();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar el cliente: " + ex.Message);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT * FROM clientes WHERE Nombre LIKE @Nombre";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conexion);
                    adapter.SelectCommand.Parameters.AddWithValue("@Nombre", "%" + txtNombre.Text + "%");
                    DataTable tabla = new DataTable();
                    adapter.Fill(tabla);
                    dgvClientes.DataSource = tabla;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar clientes: " + ex.Message);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                var row = dgvClientes.SelectedRows[0];
                string dui = row.Cells["DUI"].Value.ToString();

                DialogResult confirm = MessageBox.Show("¿Deseas eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection conexion = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conexion.Open();
                            string query = "DELETE FROM clientes WHERE DUI = @DUI";
                            SqlCommand comando = new SqlCommand(query, conexion);
                            comando.Parameters.AddWithValue("@DUI", dui);
                            comando.ExecuteNonQuery();

                            MessageBox.Show("Cliente eliminado correctamente.");
                            MostrarRegistros();
                            LimpiarCampos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para eliminar.");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                string dui = dgvClientes.SelectedRows[0].Cells["DUI"].Value.ToString();

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    try
                    {
                        conexion.Open();
                        string query = "UPDATE clientes SET Nombre = @Nombre, Direccion = @Direccion, Fecha = @Fecha, Departamento = @Departamento, Telefono = @Telefono WHERE DUI = @DUI";
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        comando.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                        comando.Parameters.AddWithValue("@Fecha", dtpFecha.Value.Date);
                        comando.Parameters.AddWithValue("@Departamento", txtDepartamento.Text);
                        comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                        comando.Parameters.AddWithValue("@DUI", dui);
                        comando.ExecuteNonQuery();

                        MessageBox.Show("Cliente modificado correctamente.");
                        MostrarRegistros();
                        LimpiarCampos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al modificar: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para modificar.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Ayuda_de_Registro_de_Cliente ayuda = new Ayuda_de_Registro_de_Cliente();
                ayuda.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir la ayuda: " + ex.Message);
            }
        }

        // Navegación
        private void label14_Click(object sender, EventArgs e) => AbrirFormulario(new Form4());
        private void label13_Click(object sender, EventArgs e) => AbrirFormulario(new Form5());
        private void label12_Click(object sender, EventArgs e) => AbrirFormulario(new Form6());
        private void label1_Click(object sender, EventArgs e) => AbrirFormulario(new Form8());
        private void label8_Click(object sender, EventArgs e) => AbrirFormulario(new Form9());
        private void label9_Click(object sender, EventArgs e) => AbrirFormulario(new Inventario());
        private void label10_Click(object sender, EventArgs e) => AbrirFormulario(new Form11());

        private void AbrirFormulario(Form formulario)
        {
            formulario.Show();
            this.Hide();
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }
    }
}

