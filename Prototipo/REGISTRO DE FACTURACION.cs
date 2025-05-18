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
    public partial class Form6 : Form
    {

        // Cadena de conexión a la base de datos
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";
        private object totalFinal;

        public Form6()
        {
            InitializeComponent();
            CargarDatos();
        }

        // Método para cargar los datos de la base en el DataGridView
        private void CargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM facturacion", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }




        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE facturacion SET Fecha = @Fecha, NIT_DUI = @NIT_DUI, NombreCliente = @NombreCliente, Direccion = @Direccion, Cuenta = @Cuenta , " +
                        "Cantidad = @Cantidad , Descripcion = @Descripcion , Precio = @Precio , PrecioTotal = @PrecioTotal , TotalFinal = TotalFinal WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Fecha", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@NIT_DUI", txtNitDui.Text);
                        command.Parameters.AddWithValue("@NombreCliente", txtNombreCliente.Text);
                        command.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                        command.Parameters.AddWithValue("@Cuenta", txtCuenta.Text);
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Cantidad", textBox3.Text);
                        command.Parameters.AddWithValue("@Descripcion", textBox1.Text);
                        command.Parameters.AddWithValue("@Precio", textBox1.Text);
                        command.Parameters.AddWithValue("@PrecioTotal", textBox5.Text);
                        // Aquí puedes calcular el TotalFinal si es necesario
                        command.Parameters.AddWithValue("@TotalFinal", totalFinal); // Asumiendo que tienes una variable para TotalFinal

                        command.ExecuteNonQuery();
                    }
                }
                CargarDatos();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM facturacion WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))

                    {

                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                CargarDatos();
            }
        }

        private void LimpiarCampos()
        {
            txtNitDui.Clear();
            txtNombreCliente.Clear();
            txtDireccion.Clear();
            txtCuenta.Clear();
            dateTimePicker1.Value = DateTime.Now;
            textBox3.Clear();
            textBox1.Clear();
            textBox5.Clear();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función de impresión no implementada.");
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



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

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            Form registrodecliente = new Form7();
            registrodecliente.Show();
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {


        }

        private void label4_Click(object sender, EventArgs e)
        {


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtNombreCliente_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            // Este puede quedar vacío si el usuario no debe modificarlo directamente
        }

        // Método para calcular el total
        private void CalcularTotal()
        {
            // Intentar convertir los valores de cantidad y precio
            if (decimal.TryParse(textBox3.Text, out decimal cantidad) && decimal.TryParse(textBox1.Text, out decimal precio))
            {
                decimal total = cantidad * precio;
                textBox5.Text = total.ToString("0.00"); // Mostrar con dos decimales
            }
            else
            {
                textBox5.Text = "0.00"; // Si no se puede calcular, mostrar 0.00
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
