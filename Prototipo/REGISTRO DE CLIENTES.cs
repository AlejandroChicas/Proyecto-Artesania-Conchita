using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Form7 : Form
    {
        // Conexión a la base de datos
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";
        private object dataGridView1;
        private object dateTimePicker1;
        private object date;

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            // Mostrar registros al cargar el formulario
            MostrarRegistros();
        }

        // Función para limpiar los campos
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDireccion.Clear();
            txtDui.Clear();
            txtDepartamento.Clear();
            txtTelefono.Clear();
            dtpFecha.Value = DateTime.Now;
        }

        // Función para mostrar los registros en el DataGridView
        private void MostrarRegistros()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM clientes";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvClientes.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al mostrar los registros: " + ex.Message);
                }
            }
        }

        // Botón Nuevo: Limpia los campos
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // Botón Registro: Inserta un nuevo cliente
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO clientes (Nombre, Direccion, DUI, Fecha, Departamento, Telefono) " +
                                   "VALUES (@Nombre, @Direccion, @DUI, @Fecha, @Departamento, @Telefono)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                    command.Parameters.AddWithValue("@DUI", txtDui.Text);
                    command.Parameters.AddWithValue("@Fecha", dtpFecha.Value);
                    command.Parameters.AddWithValue("@Departamento", txtDepartamento.Text);
                    command.Parameters.AddWithValue("@Telefono", txtTelefono.Text);

                    command.ExecuteNonQuery();
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

        // Botón Buscar: Filtra los registros por nombre
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM clientes WHERE Nombre LIKE @Nombre";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@Nombre", "%" + txtNombre.Text + "%");
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvClientes.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar clientes: " + ex.Message);
                }
            }
        }

        // Navegación entre formularios (Código existente)
        private void label14_Click(object sender, EventArgs e)
        {
            Form inicio = new Form4();
            inicio.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Form registrodeproducto = new Form5();
            registrodeproducto.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Form registrodefacturacion = new Form6();
            registrodefacturacion.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form registrodecompra = new Form8();
            registrodecompra.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form registrodeventa = new Form9();
            registrodeventa.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Form inventario = new Inventario();
            inventario.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Form cerrarsesion = new Form11();
            cerrarsesion.Show();
            this.Hide();
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Asegúrate de que estás usando el DataGridView correcto y que está declarado como DataGridView
            if (dgvClientes is DataGridView dataGridView && dataGridView.SelectedRows.Count > 0)
            {
                // Obtener los datos de la fila seleccionada
                string nombre = dataGridView.SelectedRows[0].Cells["Nombre"].Value.ToString();
                string direccion = dataGridView.SelectedRows[0].Cells["Direccion"].Value.ToString();
                DateTime fecha = Convert.ToDateTime(dataGridView.SelectedRows[0].Cells["Fecha"].Value);
                string departamento = dataGridView.SelectedRows[0].Cells["Departamento"].Value.ToString();
                string telefono = dataGridView.SelectedRows[0].Cells["Telefono"].Value.ToString();
                string dui = dataGridView.SelectedRows[0].Cells["Dui"].Value.ToString();

                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este registro?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Clientes WHERE Nombre = @Nombre AND Direccion = @Direccion AND Fecha = @Fecha " +
                                       "AND Departamento = @Departamento AND Telefono = @Telefono AND Dui = @Dui";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Direccion", direccion);
                            command.Parameters.AddWithValue("@Fecha", fecha.Date); // solo fecha, sin hora
                            command.Parameters.AddWithValue("@Departamento", departamento);
                            command.Parameters.AddWithValue("@Telefono", telefono);
                            command.Parameters.AddWithValue("@Dui", dui);

                            command.ExecuteNonQuery();

                            MessageBox.Show("Registro eliminado correctamente.");
                        }
                    }

                    MostrarRegistros();
                    LimpiarCampos();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona una fila para eliminar.");
            }
        }


        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Asegúrate de que estás usando el DataGridView correcto y que está declarado como DataGridView
            if (dgvClientes is DataGridView dataGridView && dataGridView.SelectedRows.Count > 0)
            {
                // Obtiene el DUI de la fila seleccionada (clave única)
                string duiSeleccionado = dataGridView.SelectedRows[0].Cells["DUI"].Value.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Clientes SET Nombre = @Nombre, Direccion = @Direccion, Fecha = @Fecha, " +
                                   "Departamento = @Departamento, Telefono = @Telefono WHERE DUI = @DUI";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        command.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                        command.Parameters.AddWithValue("@Fecha", dtpFecha.Value.Date);
                        command.Parameters.AddWithValue("@Departamento", txtDepartamento.Text);
                        command.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                        command.Parameters.AddWithValue("@DUI", duiSeleccionado);  // Se usa para el WHERE

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registro modificado correctamente.");

                MostrarRegistros();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un registro para modificar.");
            }
        }


    }
}

