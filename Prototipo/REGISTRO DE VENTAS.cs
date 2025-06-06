using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Form9 : Form
    {

        // Cadena de conexión a la base de datos
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;";
        private TextBox txtCódigo;
        private TextBox txtDescripción;
        private TextBox txtCantidad;
        private TextBox txtPrecioUnitario;
        private TextBox txtPrecioTotal;
        private TextBox txtTotalFinal;
        private DateTimePicker dtpFecha;
        private TextBox txtNombreProducto;


        public Form9()
        {
            InitializeComponent();
            dtpFecha = new DateTimePicker(); // Initialize the DateTimePicker
            dtpFecha.Format = DateTimePickerFormat.Short; // Optional: Set the format
            Controls.Add(dtpFecha); // Add it to the form if not already added in the designer
            CargarDatos();
        }

        // Método para cargar los datos en el DataGridView
        private void CargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM RegistroVentas", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }




        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM ventas WHERE Nombre LIKE @Nombre";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", "%" + txtNombreProducto.Text + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
        }

        // Updating the LimpiarCampos method to ensure it works correctly with the updated types.
        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombreProducto.Clear();
            txtDescripcion.Clear();
            txtCantidad.Clear();
            txtPrecioUnitario.Clear();
            txtPrecioTotal.Clear();
            txtTotalFinal.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }


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
            try
            {
                Form registrodeventa = new Form9();
                registrodeventa.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el formulario: {ex.ToString()}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form registrodecompra = new Form8();
            registrodecompra.Show();
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

        private void label10_Click(object sender, EventArgs e)
        {
            Form cerrarsesion = new Form11();
            cerrarsesion.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Form registrodeproducto = new Form5();
            registrodeproducto.Show();
            this.Hide();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void PrecioUnitario_TextChanged(object sender, EventArgs e)
        {

        }

        private void PrecioUnitario_Click(object sender, EventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Eliminar_Click(object sender, EventArgs e)
        {

        }

        private void Modificar_Click(object sender, EventArgs e)
        {

        }

        private void txtCantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNombreProducto_TextChanged(object sender, EventArgs e)
        {

        }

        private void PrecioTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void TotalFinal_TextChanged(object sender, EventArgs e)
        {

        }

        private void Nombre_Click_1(object sender, EventArgs e)
        {

        }

        private void Codigo_Click(object sender, EventArgs e)
        {

        }

        private void Cantidad_Click(object sender, EventArgs e)
        {

        }

        private void Descripcion_Click(object sender, EventArgs e)
        {

        }

        private void PrecioTotal_Click(object sender, EventArgs e)
        {

        }

        private void FechaVenta_Click(object sender, EventArgs e)
        {

        }

        private void TotalFinal_Click(object sender, EventArgs e)
        {

        }

        private void buttonGuardar_Click_1(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO ventas (Codigo, Nombre, Descripcion, Cantidad, PrecioUnitario, PrecioTotal, TotalFinal, Fecha) " +
                                   "VALUES (@Codigo, @Nombre, @Descripcion, @Cantidad, @PrecioUnitario, @PrecioTotal, @TotalFinal, @Fecha)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Codigo", SqlDbType.VarChar).Value = txtCodigo.Text;
                        command.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = txtNombreProducto.Text;
                        command.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = txtDescripcion.Text;
                        command.Parameters.Add("@Cantidad", SqlDbType.Int).Value = int.Parse(txtCantidad.Text);
                        command.Parameters.Add("@PrecioUnitario", SqlDbType.Decimal).Value = decimal.Parse(txtPrecioUnitario.Text);
                        command.Parameters.Add("@PrecioTotal", SqlDbType.Decimal).Value = decimal.Parse(txtPrecioTotal.Text);
                        command.Parameters.Add("@TotalFinal", SqlDbType.Decimal).Value = decimal.Parse(txtTotalFinal.Text);
                        command.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = dtpFecha.Value;

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Venta registrada correctamente.");
                    CargarDatos();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al registrar la venta: {ex.Message}");
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Ingrese el código del producto.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreProducto.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Ingrese la descripción del producto.");
                return false;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida (número entero mayor a 0).");
                return false;
            }

            if (!decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("Ingrese un precio unitario válido (número decimal mayor a 0).");
                return false;
            }

            if (!decimal.TryParse(txtPrecioTotal.Text, out decimal precioTotal) || precioTotal <= 0)
            {
                MessageBox.Show("Ingrese un precio total válido.");
                return false;
            }

            if (!decimal.TryParse(txtTotalFinal.Text, out decimal totalFinal) || totalFinal <= 0)
            {
                MessageBox.Show("Ingrese un total final válido.");
                return false;
            }

            if (dtpFecha.Value == null)
            {
                MessageBox.Show("Seleccione una fecha válida.");
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un nuevo formulario de ayuda
                Ayuda_de_Registro_de_Ventas ayudaForm = new Ayuda_de_Registro_de_Ventas();
                ayudaForm.ShowDialog(); // Mostrar el formulario como modal
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

    }
}