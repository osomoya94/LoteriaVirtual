using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Loteria.Escritorio
{
    public partial class frmNuevoSorteo : Form
    {
        public frmNuevoSorteo()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblFecha_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones simples (opcional por ahora)
            // ...

            // Le decimos al formulario que se cierre devolviendo un resultado "OK"
            this.DialogResult = DialogResult.OK;
            this.Close();


        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        
    }
    }
}
