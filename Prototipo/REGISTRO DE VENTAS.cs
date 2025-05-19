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
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";
        // Replacing the problematic object declarations with appropriate types for text boxes.
        private TextBox txtCódigo;
        private TextBox txtNombre;
        private TextBox txtDescripción;
        private TextBox txtCantidad;
        private TextBox txtPrecioUnitario;
        private TextBox txtPrecioTotal;
        private TextBox txtTotalFinal;

        public Form9()
        {
            InitializeComponent();
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO RegistroVentas (Codigo , Fecha , Nombre , Descripcion , Cantidad , PrecioUnitario , PrecioTotal ,TotalFinal) VALUES (@Codigo, @Fecha, @Nombre, @Descripcion, @Cantidad, @PrecioUnitario, @PrecioTotal, @TotalFinal)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Codigo", txtCodigo.Text);
                    command.Parameters.AddWithValue("@Fecha", dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                    command.Parameters.AddWithValue("@Cantidad", txtCantidad.Text);
                    command.Parameters.AddWithValue("@PrecioUnitario", txtPrecioUnitario.Text);
                    command.Parameters.AddWithValue("@PrecioTotal", txtPrecioTotal.Text);
                    command.Parameters.AddWithValue("@TotalFinal", txtTotalFinal.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Registro de venta guardado correctamente.");
                }
            }
            CargarDatos();
            LimpiarCampos();
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
                string query = "SELECT * FROM RegistroVentas WHERE Nombre LIKE @Nombre";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", "%" + txtNombre.Text + "%");
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
            txtNombre.Clear();
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

        private void txtNombre_TextChanged(object sender, EventArgs e)
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
    }
}