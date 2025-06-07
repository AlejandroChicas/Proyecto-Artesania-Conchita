using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Inventario : Form
    {
        private SqlConnection conexion = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=Prototipo;Trusted_Connection=True;");

        public Inventario()
        {
            InitializeComponent();

            // Inicializar filtros
            cboFiltros.Items.AddRange(new string[] { "Todos", "Categoría", "Estado" });
            cboFiltros.SelectedIndex = 0;

            // Cargar el inventario completo
            CargarInventario();
        }

        private void CargarInventario(string filtro = "", string busqueda = "")
        {
            try
            {
                conexion.Open();

                string query = "SELECT * FROM inventario";
                SqlCommand comando;

                if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrEmpty(busqueda))
                {
                    query += $" WHERE {filtro} LIKE @Busqueda";
                    comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Busqueda", $"%{busqueda}%");
                }
                else
                {
                    comando = new SqlCommand(query, conexion);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(comando);
                DataTable tabla = new DataTable();
                adapter.Fill(tabla);

                dgvInventario.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el inventario: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = "";
            string seleccion = cboFiltros.SelectedItem.ToString();

            if (seleccion == "Categoría")
                filtro = "categoria";
            else if (seleccion == "Estado")
                filtro = "estado";

            if (seleccion == "Todos")
                CargarInventario();
            else
                CargarInventario(filtro, txtBusqueda.Text.Trim());
        }

        private void label14_Click(object sender, EventArgs e)
        {
            new Form4().Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            new Form5().Show();
            this.Hide();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            new Form6().Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            new Form7().Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            new Form8().Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            new Form9().Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            new Form11().Show();
            this.Hide();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            // Si deseas recargar el inventario al abrir, puedes llamarlo aquí
            // CargarInventario();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Aquí puedes colocar la lógica para agregar productos si es necesario
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // Código opcional si necesitas funcionalidad en este label
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un nuevo formulario de ayuda
                AYUDA_DE_INVENTARIO ayudaForm = new AYUDA_DE_INVENTARIO();
                ayudaForm.ShowDialog(); // Mostrar el formulario como modal
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
    }
}

