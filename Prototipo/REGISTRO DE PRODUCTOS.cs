using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Form5 : Form
    {
        // Cadena de conexión a la base de datos
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        // Método para cargar los datos de la tabla "productos" en el DataGridView
        private void CargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM productos";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvProductos.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message);
                }
            }
        }

        // Botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO productos (Codigo, Nombre, Descripcion, Cantidad, PrecioUnitario, PrecioTotal) VALUES (@Codigo, @Nombre, @Descripcion, @Cantidad, @PrecioUnitario, @PrecioTotal)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Codigo", txtcodigo.Text);
                    command.Parameters.AddWithValue("@Nombre", txtnombre.Text);
                    command.Parameters.AddWithValue("@Descripcion", txtdescripcion.Text);
                    command.Parameters.AddWithValue("@Cantidad", int.Parse(txtcantidad.Text));
                    command.Parameters.AddWithValue("@PrecioUnitario", decimal.Parse(txtpreciounitario.Text));
                    command.Parameters.AddWithValue("@PrecioTotal", decimal.Parse(textBox2.Text)); // Asegúrate de que textBox2 tenga el precio total
                    command.ExecuteNonQuery();
                    MessageBox.Show("Producto guardado correctamente.");
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el producto: " + ex.Message);
                }

            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Validar que el campo Código no esté vacío
            if (string.IsNullOrWhiteSpace(txtcodigo.Text))
            {
                MessageBox.Show("Por favor, selecciona un producto para editar.");
                return;
            }

            // Validar y convertir campos numéricos
            if (!int.TryParse(txtcantidad.Text, out int cantidad))
            {
                MessageBox.Show("La cantidad debe ser un número entero válido.");
                return;
            }

            if (!decimal.TryParse(txtpreciounitario.Text, out decimal precioUnitario))
            {
                MessageBox.Show("El precio unitario debe ser un número decimal válido.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE productos 
                             SET Nombre = @Nombre, 
                                 Descripcion = @Descripcion, 
                                 Cantidad = @Cantidad, 
                                 PrecioUnitario = @PrecioUnitario
                                 PrecioTotal = @PrecioTotal
                             WHERE Codigo = @Codigo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Codigo", txtcodigo.Text.Trim());
                        command.Parameters.AddWithValue("@Nombre", txtnombre.Text.Trim());
                        command.Parameters.AddWithValue("@Descripcion", txtdescripcion.Text.Trim());
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                        command.Parameters.AddWithValue("@PrecioTotal", decimal.Parse(textBox2.Text)); // Asegúrate de que textBox2 tenga el precio total

                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Producto actualizado correctamente.");
                            CargarDatos();  // Actualiza el DataGridView
                            LimpiarCampos(); // Opcional: Limpia los campos

                        }
                        else
                        {
                            MessageBox.Show("No se encontró el producto con ese código.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el producto: " + ex.Message);
                }
            }
        }
        // Otros métodos como btnEditar_Click, btnEliminar_Click, etc.

        private void LimpiarCampos()
        {
            txtcodigo.Clear();
            txtnombre.Clear();
            txtdescripcion.Clear();
            txtcantidad.Clear();
            txtpreciounitario.Clear();
            textBox2.Clear(); // Limpiar el campo de precio total

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validación del campo código
            if (string.IsNullOrWhiteSpace(txtcodigo.Text))
            {
                MessageBox.Show("Debe ingresar un código de producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtcodigo.Focus();
                return;
            }

            // Confirmación antes de eliminar
            DialogResult confirmacion = MessageBox.Show(
                $"¿Está seguro que desea eliminar el producto con código: {txtcodigo.Text}?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM productos WHERE Codigo = @Codigo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Codigo", txtcodigo.Text.Trim());

                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarDatos();
                            LimpiarCampos(); // Método opcional para limpiar los campos después de eliminar
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el código especificado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error de base de datos al eliminar el producto:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inesperado:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Botón Nuevo
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            txtcodigo.Clear();
            txtnombre.Clear();
            txtdescripcion.Clear();
            txtcantidad.Clear();
            txtpreciounitario.Clear();
            txtcodigo.Focus();
            textBox2.Clear(); // Limpiar el campo de precio total
        }

        // Botón Imprimir (ejemplo simple, puede adaptarse)
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función de impresión no implementada.");
        }

        // Seleccionar registro del DataGridView
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                txtcodigo.Text = row.Cells["Codigo"].Value.ToString();
                txtnombre.Text = row.Cells["Nombre"].Value.ToString();
                txtdescripcion.Text = row.Cells["Descripcion"].Value.ToString();
                txtcantidad.Text = row.Cells["Cantidad"].Value.ToString();
                txtpreciounitario.Text = row.Cells["PrecioUnitario"].Value.ToString();
                textBox2.Text = row.Cells["PrecioTotal"].Value.ToString(); // Asegúrate de que este campo esté en el DataGridView
            }
        }

        // Navegación entre formularios
        private void label14_Click(object sender, EventArgs e)
        {
            Form inicio = new Form4();
            inicio.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Form registrodefacturacion = new Form6();
            registrodefacturacion.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Form registrodecliente = new Form7();
            registrodecliente.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
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

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void txtcodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtpreciounitario_TextChanged(object sender, EventArgs e)
        {
            PrecioTotal();
        }

        private void PrecioTotal()
        {
            // Validar que los valores de cantidad y precio unitario sean numéricos
            if (decimal.TryParse(txtcantidad.Text, out decimal cantidad) &&
                decimal.TryParse(txtpreciounitario.Text, out decimal precioUnitario))
            {
                // Calcular el precio total
                decimal precioTotal = cantidad * precioUnitario;

                // Mostrar el resultado en textBox2
                textBox2.Text = precioTotal.ToString("F2"); // Formato con 2 decimales
            }
            else
            {
                // Si los valores no son válidos, mostrar 0.00
                textBox2.Text = "$ 0.00";
            }
        }


        private void txtcantidad_TextChanged(object sender, EventArgs e)
        {
            PrecioTotal();
        }
    }
}
