using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototipo
{
    public partial class Ayuda_de_Cerrar_Sesion : Form
    {
        public Ayuda_de_Cerrar_Sesion()
        {
            InitializeComponent();
        }

        private void Ayuda_de_Cerrar_Sesion_Load(object sender, EventArgs e)
        {
            try
            {
                // Aquí puedes agregar cualquier lógica adicional que necesites al cargar el formulario
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la ayuda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
