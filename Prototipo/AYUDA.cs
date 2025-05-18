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
    public partial class Ayuda : Form
    {
        public Ayuda(Form1 form1)
        {
            InitializeComponent();
        }

        private void Ayuda_Load(object sender, EventArgs e)
        {
            // Código opcional cuando cargue el formulario  
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Código opcional cuando hagan click en el label  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Instancia y abre el formulario principal  
            Principal principal = new Principal();
            principal.Show();

            // Cierra la ventana de Ayuda  
            this.Close();
        }

        private void Ayuda_Load_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }

    internal class Principal : Form // Cambiado para heredar de Form
    {
        internal new void Show()
        {
            base.Show(); // Implementación corregida para mostrar el formulario
        }
    }
}