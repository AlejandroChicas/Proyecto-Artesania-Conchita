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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Prototipo
{
    public partial class Form6 : Form
    {

        // Cadena de conexión a la base de datos
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;";
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
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Facturacion", connection);
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

                // Validaciones seguras
                if (!int.TryParse(textBox3.Text.Trim(), out int cantidad))
                {
                    MessageBox.Show("Ingrese una cantidad válida (solo números enteros).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(textBox1.Text.Trim(), out decimal precio))
                {
                    MessageBox.Show("Ingrese un precio válido (ej. 10.50).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!decimal.TryParse(textBox5.Text.Trim(), out decimal precioTotal))
                {
                    MessageBox.Show("Ingrese un precio total válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal totalFinal = cantidad * precio;

                using (SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;"))
                {
                    connection.Open();

                    string query = "UPDATE Facturacion SET Fecha = @Fecha, NIT_DUI = @NIT_DUI, NombreCliente = @NombreCliente, Direccion = @Direccion, Cuenta = @Cuenta, " +
                                   "Cantidad = @Cantidad, Descripcion = @Descripcion, Precio = @Precio, PrecioTotal = @PrecioTotal, TotalFinal = @TotalFinal WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Fecha", dateTimePicker1.Value.Date);
                        command.Parameters.AddWithValue("@NIT_DUI", txtNitDui.Text.Trim());
                        command.Parameters.AddWithValue("@NombreCliente", txtNombreCliente.Text.Trim());
                        command.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                        command.Parameters.AddWithValue("@Cuenta", txtCuenta.Text.Trim());
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@Descripcion", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@Precio", precio);
                        command.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                        command.Parameters.AddWithValue("@TotalFinal", totalFinal);
                        command.Parameters.AddWithValue("@Id", id);

                        command.ExecuteNonQuery();
                    }
                }

                CargarDatos();
                LimpiarCampos();
                MessageBox.Show("Registro modificado correctamente", "Modificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Seleccione una fila para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para imprimir.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string rutaArchivo = @"C:\Users\josue\OneDrive\Escritorio\ARTESANIA CONCHITA\Facturacion" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                Document doc = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));
                doc.Open();

                // Agregar logo 
                string rutaLogo = @"C:\Users\josue\OneDrive\Escritorio\ARTESANIA CONCHITA\Prototipo\Resources\WhatsApp Image 2025-03-29 at 9.07.14 PM.jpeg";
                PdfPTable encabezado = new PdfPTable(2);
                encabezado.WidthPercentage = 100;
                float[] anchoCols = { 80f, 20f };
                encabezado.SetWidths(anchoCols);

                // Título
                Paragraph titulo = new Paragraph("Facturacion - Artesanías Conchita", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18));
                PdfPCell celdaTitulo = new PdfPCell(titulo)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                encabezado.AddCell(celdaTitulo);

                // Logo (para saber si el logo existe)
                if (File.Exists(rutaLogo))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                    logo.ScaleAbsolute(100, 50);
                    PdfPCell celdaLogo = new PdfPCell(logo)
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    encabezado.AddCell(celdaLogo);
                }
                else
                {
                    encabezado.AddCell(new PdfPCell(new Phrase("")) { Border = iTextSharp.text.Rectangle.NO_BORDER });
                }

                doc.Add(encabezado);
                doc.Add(new Paragraph("Fecha: " + DateTime.Now.ToShortDateString()));
                doc.Add(new Paragraph("Cliente:"));
                doc.Add(new Paragraph("NIT / DUI:"));
                doc.Add(new Paragraph("Dirección:"));
                doc.Add(new Paragraph("Cuenta:"));
                doc.Add(new Paragraph(" "));

                // Crear tabla
                PdfPTable tabla = new PdfPTable(10);
                tabla.WidthPercentage = 100;
                string[] columnas = { "Fecha", "NIT/DUI", "Nombre Cliente", "Dirección", "Cuenta", "Cantidad", "Descripción", "Precio", "Precio Total", "Total Final" };
                foreach (string columna in columnas)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(columna))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    };
                    tabla.AddCell(celda);
                }

                // Agregar datos
                foreach (DataGridViewRow fila in dataGridView1.Rows)
                {
                    if (!fila.IsNewRow)
                    {
                        tabla.AddCell(Convert.ToDateTime(fila.Cells["Fecha"].Value).ToShortDateString());
                        tabla.AddCell(fila.Cells["NIT_DUI"].Value?.ToString());
                        tabla.AddCell(fila.Cells["NombreCliente"].Value?.ToString());
                        tabla.AddCell(fila.Cells["Direccion"].Value?.ToString());
                        tabla.AddCell(fila.Cells["Cuenta"].Value?.ToString());
                        tabla.AddCell(fila.Cells["Cantidad"].Value?.ToString());
                        tabla.AddCell(fila.Cells["Descripcion"].Value?.ToString());
                        tabla.AddCell(fila.Cells["Precio"].Value?.ToString());
                        tabla.AddCell(fila.Cells["PrecioTotal"].Value?.ToString());
                        tabla.AddCell(fila.Cells["TotalFinal"].Value?.ToString());
                    }
                }

                doc.Add(tabla);
                doc.Add(new Paragraph(" "));

                // Mensaje de agradecimiento
                Paragraph agradecimiento = new Paragraph("Gracias por su compra", FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 12));
                agradecimiento.Alignment = Element.ALIGN_LEFT;
                doc.Add(agradecimiento);

                doc.Close();

                MessageBox.Show("PDF generado con éxito en:\n" + rutaArchivo, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(rutaArchivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un nuevo formulario de ayuda
                Ayuda_de_Registro_de_Facturacion ayudaForm = new Ayuda_de_Registro_de_Facturacion();
                ayudaForm.ShowDialog(); // Mostrar el formulario como modal
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
