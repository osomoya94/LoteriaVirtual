namespace Loteria.Escritorio
{
    partial class frmNuevoSorteo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            cmbModo = new ComboBox();
            nudPrecio = new NumericUpDown();
            lblModo = new Label();
            dtpFecha = new DateTimePicker();
            lblPrecio = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblFecha = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnGuardar = new Button();
            btnCancelar = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(cmbModo, 1, 3);
            tableLayoutPanel1.Controls.Add(nudPrecio, 1, 2);
            tableLayoutPanel1.Controls.Add(lblModo, 0, 3);
            tableLayoutPanel1.Controls.Add(dtpFecha, 1, 1);
            tableLayoutPanel1.Controls.Add(lblPrecio, 0, 2);
            tableLayoutPanel1.Controls.Add(lblNombre, 0, 0);
            tableLayoutPanel1.Controls.Add(txtNombre, 1, 0);
            tableLayoutPanel1.Controls.Add(lblFecha, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 51.04895F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 48.95105F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // cmbModo
            // 
            cmbModo.Dock = DockStyle.Fill;
            cmbModo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModo.FormattingEnabled = true;
            cmbModo.Location = new Point(169, 202);
            cmbModo.Name = "cmbModo";
            cmbModo.Size = new Size(615, 33);
            cmbModo.TabIndex = 4;
            // 
            // nudPrecio
            // 
            nudPrecio.DecimalPlaces = 2;
            nudPrecio.Dock = DockStyle.Fill;
            nudPrecio.Location = new Point(169, 124);
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(615, 31);
            nudPrecio.TabIndex = 3;
            // 
            // lblModo
            // 
            lblModo.AutoSize = true;
            lblModo.Location = new Point(16, 199);
            lblModo.Name = "lblModo";
            lblModo.Size = new Size(91, 50);
            lblModo.TabIndex = 1;
            lblModo.Text = "Modo Extraccion";
            lblModo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpFecha
            // 
            dtpFecha.Dock = DockStyle.Fill;
            dtpFecha.Location = new Point(169, 71);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(615, 31);
            dtpFecha.TabIndex = 2;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(16, 121);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(98, 50);
            lblPrecio.TabIndex = 1;
            lblPrecio.Text = "Precio por Carton";
            lblPrecio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(16, 13);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(112, 50);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre del Sorteo:";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNombre
            // 
            txtNombre.Dock = DockStyle.Fill;
            txtNombre.Location = new Point(169, 16);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(615, 31);
            txtNombre.TabIndex = 1;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(16, 68);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(57, 25);
            lblFecha.TabIndex = 1;
            lblFecha.Text = "Fecha";
            lblFecha.TextAlign = ContentAlignment.MiddleLeft;
            lblFecha.Click += lblFecha_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btnGuardar);
            flowLayoutPanel1.Controls.Add(btnCancelar);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(169, 364);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(615, 70);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.MediumSeaGreen;
            btnGuardar.DialogResult = DialogResult.OK;
            btnGuardar.Location = new Point(500, 3);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(112, 34);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Red;
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(382, 3);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(112, 34);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // frmNuevoSorteo
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "frmNuevoSorteo";
            Text = "FrmNuevoSorteo";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPrecio).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private NumericUpDown nudPrecio;
        private Label lblPrecio;
        private Label lblModo;
        private ComboBox cmbModo;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnGuardar;
        private Button btnCancelar;
    }
}