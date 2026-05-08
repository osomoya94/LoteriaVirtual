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

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LblFecha_Click(object sender, EventArgs e)
        {

        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Limpiamos errores previos
            errorProvider1.SetError(txtNombre, "");

            // Validamos si el nombre está vacío
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "El nombre del sorteo es obligatorio.");
                txtNombre.Focus(); // Ponemos el cursor ahí para que el usuario escriba
                return; // Cortamos la ejecución aquí, no se cierra el formulario
            }

            // Si pasó la validación, avisamos que todo OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void NudPrecio_KeyPress(object sender, KeyPressEventArgs e)
        { 
            // Solo permitimos números, la coma (o punto según tu configuración) y borrar
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true; // Bloquea cualquier otra tecla (letras, símbolos, etc.)
            }
        
    }
        }
    }
    

