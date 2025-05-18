using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Inventario : Form
    {
        private SqlConnection connection;

        public Inventario()
        {
            InitializeComponent();

            // Configuración de la conexión
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=prototipo;Trusted_Connection=True;";
            connection = new SqlConnection(connectionString);

            // Llenar el combobox de filtros
            cboFiltros.Items.Add("Todos");
            cboFiltros.Items.Add("Categoría");
            cboFiltros.Items.Add("Estado");
            cboFiltros.SelectedIndex = 0; // Seleccionar el primer filtro por defecto

            // Cargar el inventario al iniciar
            CargarInventario();
        }

        private void CargarInventario(string filtro = "", string busqueda = "")
        {
            try
            {
                string query = "SELECT * FROM inventario";

                // Si se aplica un filtro y una búsqueda
                if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrEmpty(busqueda))
                {
                    query += $" WHERE {filtro} LIKE @Busqueda";
                }

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrEmpty(busqueda))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@Busqueda", $"%{busqueda}%");
                }

                DataTable table = new DataTable();
                adapter.Fill(table);

                // Mostrar los datos en el DataGridView
                dgvInventario.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el inventario: {ex.Message}");
            }

        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = "";
            if (cboFiltros.SelectedItem.ToString() == "Categoría")
            {
                filtro = "categoria";
            }
            else if (cboFiltros.SelectedItem.ToString() == "Estado")
            {
                filtro = "estado";
            }

            CargarInventario(filtro, txtBusqueda.Text);
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

        private void label10_Click(object sender, EventArgs e)
        {
            Form cerrarsesion = new Form11();
            cerrarsesion.Show();
            this.Hide();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            // Optionally, you can load the inventory here too if needed
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

        }
    }
}
