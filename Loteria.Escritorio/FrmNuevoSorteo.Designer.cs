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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblPrecio = new Label();
            nudPrecio = new NumericUpDown();
            lblModo = new Label();
            cmbModo = new ComboBox();
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
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(cmbModo, 1, 3);
            tableLayoutPanel1.Controls.Add(nudPrecio, 1, 2);
            tableLayoutPanel1.Controls.Add(lblModo, 0, 3);
            tableLayoutPanel1.Controls.Add(dtpFecha, 1, 1);
            tableLayoutPanel1.Controls.Add(lblPrecio, 0, 2);
            tableLayoutPanel1.Controls.Add(lblNombre, 0, 0);
            tableLayoutPanel1.Controls.Add(txtNombre, 1, 0);
            tableLayoutPanel1.Controls.Add(lblFecha, 0, 1);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 5);
            tableLayoutPanel1.Location = new Point(427, 28);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 38.51351F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 61.48649F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.Size = new Size(338, 273);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(3, 0);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(83, 36);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre del Sorteo:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(103, 3);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(150, 31);
            txtNombre.TabIndex = 2;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(3, 36);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(57, 25);
            lblFecha.TabIndex = 1;
            lblFecha.Text = "Fecha";
            lblFecha.Click += lblFecha_Click;
            // 
            // dtpFecha
            // 
            dtpFecha.Location = new Point(103, 39);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(300, 31);
            dtpFecha.TabIndex = 2;
            // 
            // lblPrecio
            // 
            lblPrecio.AutoSize = true;
            lblPrecio.Location = new Point(3, 93);
            lblPrecio.Name = "lblPrecio";
            lblPrecio.Size = new Size(93, 50);
            lblPrecio.TabIndex = 1;
            lblPrecio.Text = "Precio por Carton";
            // 
            // nudPrecio
            // 
            nudPrecio.DecimalPlaces = 2;
            nudPrecio.Location = new Point(103, 96);
            nudPrecio.Name = "nudPrecio";
            nudPrecio.Size = new Size(180, 31);
            nudPrecio.TabIndex = 2;
            // 
            // lblModo
            // 
            lblModo.AutoSize = true;
            lblModo.Location = new Point(3, 157);
            lblModo.Name = "lblModo";
            lblModo.Size = new Size(91, 50);
            lblModo.TabIndex = 1;
            lblModo.Text = "Modo Extraccion";
            // 
            // cmbModo
            // 
            cmbModo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModo.FormattingEnabled = true;
            cmbModo.Location = new Point(103, 160);
            cmbModo.Name = "cmbModo";
            cmbModo.Size = new Size(182, 33);
            cmbModo.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btnGuardar);
            flowLayoutPanel1.Controls.Add(btnCancelar);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(103, 252);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 18);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.DialogResult = DialogResult.OK;
            btnGuardar.Location = new Point(185, 3);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(112, 34);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Location = new Point(67, 3);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(112, 34);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
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