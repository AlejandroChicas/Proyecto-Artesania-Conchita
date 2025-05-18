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

        public Form8()
        {
            InitializeComponent();

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";
            connection = new SqlConnection(connectionString);

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
            try
            {
                // Validar entradas antes de ejecutar la consulta
                if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                    !int.TryParse(txtCantidad.Text, out int cantidad) ||
                    !decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) ||
                    !decimal.TryParse(txtPrecioTotal.Text, out decimal precioTotal))

                {
                    MessageBox.Show("Por favor, complete todos los campos correctamente.");
                    return;
                }

                string query = "INSERT INTO productos (Codigo, Nombre, Descripcion, Cantidad, PrecioUnitario, PrecioTotal) " +
                               "VALUES (@Codigo, @Nombre, @Descripcion, @Cantidad, @PrecioUnitario, @PrecioTotal)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Codigo", txtCodigo.Text);
                    command.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                    command.Parameters.AddWithValue("@Cantidad", cantidad);
                    command.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    command.Parameters.AddWithValue("@PrecioTotal", precioTotal);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Registro guardado correctamente.");
                    CargarRegistros();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
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
    }
}
