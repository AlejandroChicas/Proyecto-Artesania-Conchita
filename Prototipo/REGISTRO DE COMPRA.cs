using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Http.Headers;



namespace Prototipo
{
    public partial class Form8 : Form
    {
        private SqlConnection connection;

        private TextBox txtCodigo;
        private TextBox txtNombre;
        private TextBox txtDescripcion;
        private TextBox txtCantidad;
        private TextBox txtPrecioUnitario;
        private TextBox txtPrecioTotal;
        private TextBox txtTotalFinal; // Update the type of txtTotalFinal from object to TextBox
        private DateTimePicker dtpFecha; // Change the type of dtpFecha from object to DateTimePicker

        public Form8()
        {
            InitializeComponent();

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";
            connection = new SqlConnection(connectionString);

            // Initialize the DateTimePicker
            dtpFecha = new DateTimePicker();
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Value = DateTime.Now; // Set a default value

            CargarDatos();
        }
        private void CargarDatos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;"))
                {
                    string query = "SELECT id, Codigo, Nombre, Descripcion, Cantidad, PrecioUnitario, PrecioTotal FROM productos";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;

                    // Ocultar columna ID si no deseas mostrarla
                    dataGridView1.Columns["id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar campos vacíos
            if (string.IsNullOrWhiteSpace(txtcodigo.Text) ||
                string.IsNullOrWhiteSpace(txtnombre.Text) ||
                string.IsNullOrWhiteSpace(txtdescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtcantidad.Text) ||
                string.IsNullOrWhiteSpace(txtpreciounitario.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text)) // textBox2 = Precio Total
            {
                MessageBox.Show("Por favor complete todos los campos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación y conversión segura
            if (!int.TryParse(txtcodigo.Text.Trim(), out int codigo))
            {
                MessageBox.Show("El código debe ser un número entero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtcantidad.Text.Trim(), out decimal cantidad))
            {
                MessageBox.Show("La cantidad debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtpreciounitario.Text.Trim(), out decimal precioUnitario))
            {
                MessageBox.Show("El precio unitario debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(textBox2.Text.Trim(), out decimal precioTotal))
            {
                MessageBox.Show("El precio total debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal totalFinal = cantidad * precioUnitario;

            try
            {
                using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;"))
                {
                    connection.Open();

                    string query = @"INSERT INTO productos 
                            (Codigo, Nombre, Descripcion, Cantidad, PrecioUnitario, PrecioTotal)
                             VALUES (@Codigo, @Nombre, @Descripcion, @Cantidad, @PrecioUnitario, @PrecioTotal)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Codigo", codigo);
                    command.Parameters.AddWithValue("@Nombre", txtnombre.Text.Trim());
                    command.Parameters.AddWithValue("@Descripcion", txtdescripcion.Text.Trim());
                    command.Parameters.AddWithValue("@Cantidad", cantidad);
                    command.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    command.Parameters.AddWithValue("@PrecioTotal", precioTotal);

                    command.ExecuteNonQuery();
                }
                CargarDatos();

                MessageBox.Show("Compra registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    object value = dataGridView1.SelectedRows[0].Cells["id"].Value;
                    if (value == null || !int.TryParse(value.ToString(), out int id))
                    {
                        MessageBox.Show("ID inválido.");
                        return;
                    }

                    // Validar los campos numéricos
                    if (!decimal.TryParse(txtcantidad.Text.Trim(), out decimal cantidad) ||
                        !decimal.TryParse(txtpreciounitario.Text.Trim(), out decimal precioUnitario) ||
                        !decimal.TryParse(textBox2.Text.Trim(), out decimal precioTotal))
                    {
                        MessageBox.Show("Ingrese valores numéricos válidos en Cantidad, Precio Unitario y Precio Total.");
                        return;
                    }

                    using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;"))
                    {
                        connection.Open();

                        string query = @"UPDATE productos 
                             SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, 
                                 Cantidad = @Cantidad, PrecioUnitario = @PrecioUnitario, 
                                 PrecioTotal = @PrecioTotal 
                             WHERE id = @Id";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Codigo", txtcodigo.Text.Trim());
                        command.Parameters.AddWithValue("@Nombre", txtnombre.Text.Trim());
                        command.Parameters.AddWithValue("@Descripcion", txtdescripcion.Text.Trim());
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                        command.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                        command.Parameters.AddWithValue("@Id", id);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registro actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Selecciona un registro para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                    using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;"))
                    {
                        connection.Open();
                        string query = "DELETE FROM productos WHERE id = @Id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Registro eliminado correctamente.");
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show("Selecciona un registro para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}");
            }
        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            CalcularPrecioTotal();
        }

        private void txtPrecioUnitario_TextChanged(object sender, EventArgs e)
        {
            CalcularPrecioTotal();
        }

        private void CalcularPrecioTotal()
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad) &&
                decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario))
            {
                decimal total = cantidad * precioUnitario;
                txtPrecioTotal.Text = total.ToString("0.00");
            }
            else
            {
                txtPrecioTotal.Text = "";
            }
        }

        // Eventos para navegación (pueden mantenerse como los tenías)
        private void label14_Click(object sender, EventArgs e)
        {
            Form inicio = new Form4();
            inicio.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Form inventario = new Inventario();
            inventario.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form registrodeventa = new Form9();
            registrodeventa.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Form registrodecliente = new Form7();
            registrodecliente.Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Form registrodefacturacion = new Form6();
            registrodefacturacion.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Form registrodeproducto = new Form5();
            registrodeproducto.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            try
            {
                Form cerrarsesion = new Form11();
                cerrarsesion.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar cerrar sesión: {ex}");
            }
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función de impresión no implementada.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un nuevo formulario de ayuda
                Ayuda_de_Registro_de_Compra ayudaForm = new Ayuda_de_Registro_de_Compra();
                ayudaForm.ShowDialog(); // Mostrar el formulario como modal
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
          
        }
    }
}

