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

            CargarRegistros();
        }

        private void CargarRegistros()
        {
            try
            {
                string query = "SELECT * FROM compra";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // ✅ Cadena de conexión: asegúrate de poner la correcta según tu base de datos
            string connectionString = "Data Source=JOSUE;Initial Catalog=Prtotipo;Integrated Security=True";

            // ✅ Validación de campos (evita errores si un TextBox es null)
            if ((txtCodigo?.Text ?? "").Trim() == "" ||
                (txtNombre?.Text ?? "").Trim() == "" ||
                (txtDescripcion?.Text ?? "").Trim() == "" ||
                (txtCantidad?.Text ?? "").Trim() == "" ||
                (txtPrecioUnitario?.Text ?? "").Trim() == "" ||
                (txtPrecioTotal?.Text ?? "").Trim() == "" ||
                (txtTotalFinal?.Text ?? "").Trim() == "")
            {
                MessageBox.Show("Por favor complete todos los campos.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO RegistroVentas 
                             (Codigo, Fecha, Nombre, Descripcion, Cantidad, PrecioUnitario, PrecioTotal, TotalFinal)
                             VALUES (@Codigo, @Fecha, @Nombre, @Descripcion, @Cantidad, @PrecioUnitario, @PrecioTotal, @TotalFinal)";

                    SqlCommand command = new SqlCommand(query, connection);

                    // ✅ Conversión segura de datos
                    int codigo = int.TryParse(txtCodigo.Text, out int c) ? c : 0;
                    decimal cantidad = decimal.TryParse(txtCantidad.Text, out decimal cant) ? cant : 0;
                    decimal precioUnitario = decimal.TryParse(txtPrecioUnitario.Text, out decimal pu) ? pu : 0;
                    decimal precioTotal = decimal.TryParse(txtPrecioTotal.Text, out decimal pt) ? pt : 0;
                    decimal totalFinal = decimal.TryParse(txtTotalFinal.Text, out decimal tf) ? tf : 0;

                    // ✅ Agregar parámetros a la consulta
                    command.Parameters.AddWithValue("@Codigo", codigo);
                    command.Parameters.AddWithValue("@Fecha", dtpFecha.Value.Date); // DateTimePicker
                    command.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    command.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text.Trim());
                    command.Parameters.AddWithValue("@Cantidad", cantidad);
                    command.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    command.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                    command.Parameters.AddWithValue("@TotalFinal", totalFinal);

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Registro guardado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }



        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                    string query = "UPDATE compra SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, Cantidad = @Cantidad, PrecioUnitario = @PrecioUnitario, PrecioTotal = @PrecioTotal WHERE id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Codigo", txtCodigo.Text);
                    command.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                    command.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(txtCantidad.Text));
                    command.Parameters.AddWithValue("@PrecioUnitario", Convert.ToDecimal(txtPrecioUnitario.Text));
                    command.Parameters.AddWithValue("@PrecioTotal", Convert.ToDecimal(txtPrecioTotal.Text));
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Registro actualizado correctamente.");
                    CargarRegistros();
                }
                else
                {
                    MessageBox.Show("Selecciona un registro para editar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar: {ex}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
                    string query = "DELETE FROM compra WHERE id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Registro eliminado correctamente.");
                    CargarRegistros();
                }
                else
                {
                    MessageBox.Show("Selecciona un registro para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
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
            CargarRegistros();
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
    }
}

