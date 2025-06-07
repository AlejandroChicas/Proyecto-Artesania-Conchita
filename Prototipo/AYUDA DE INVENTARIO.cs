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
    public partial class AYUDA_DE_INVENTARIO : Form
    {
        public AYUDA_DE_INVENTARIO()
        {
            InitializeComponent();
        }

        private void AYUDA_DE_INVENTARIO_Load(object sender, EventArgs e)
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
    }
}
