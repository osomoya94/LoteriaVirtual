namespace Loteria.Escritorio;

partial class dgvSorteos
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tabControl1 = new TabControl();
        tabPage1 = new TabPage();
        splitContainer1 = new SplitContainer();
        btnBuscar = new Button();
        txtBusqueda = new TextBox();
        label1 = new Label();
        btnRechazar = new Button();
        btnAprobar = new Button();
        dgvPagosPendientes = new DataGridView();
        colIdPago = new DataGridViewTextBoxColumn();
        colDni = new DataGridViewTextBoxColumn();
        colMonto = new DataGridViewTextBoxColumn();
        colComprobante = new DataGridViewTextBoxColumn();
        colFechaPago = new DataGridViewTextBoxColumn();
        tabPage2 = new TabPage();
        dataGridView1 = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colFecha = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        colPrecio = new DataGridViewTextBoxColumn();
        colModo = new DataGridViewTextBoxColumn();
        flowLayoutPanel1 = new FlowLayoutPanel();
        btnNuevo = new Button();
        btnEditar = new Button();
        btnActualizar = new Button();
        tabControl1.SuspendLayout();
        tabPage1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPagosPendientes).BeginInit();
        tabPage2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        flowLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(tabPage1);
        tabControl1.Controls.Add(tabPage2);
        tabControl1.Dock = DockStyle.Fill;
        tabControl1.Location = new Point(0, 0);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(979, 450);
        tabControl1.TabIndex = 0;
        tabControl1.Tag = "";
        // 
        // tabPage1
        // 
        tabPage1.Controls.Add(splitContainer1);
        tabPage1.Location = new Point(4, 34);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new Padding(3);
        tabPage1.Size = new Size(971, 412);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "Validacion de Pagos";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(3, 3);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(btnBuscar);
        splitContainer1.Panel1.Controls.Add(txtBusqueda);
        splitContainer1.Panel1.Controls.Add(label1);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(btnRechazar);
        splitContainer1.Panel2.Controls.Add(btnAprobar);
        splitContainer1.Panel2.Controls.Add(dgvPagosPendientes);
        splitContainer1.Size = new Size(965, 406);
        splitContainer1.SplitterDistance = 321;
        splitContainer1.TabIndex = 0;
        // 
        // btnBuscar
        // 
        btnBuscar.BackColor = Color.LightSteelBlue;
        btnBuscar.Location = new Point(88, 189);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(112, 34);
        btnBuscar.TabIndex = 2;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // txtBusqueda
        // 
        txtBusqueda.Location = new Point(67, 107);
        txtBusqueda.Name = "txtBusqueda";
        txtBusqueda.Size = new Size(181, 31);
        txtBusqueda.TabIndex = 1;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(42, 52);
        label1.Name = "label1";
        label1.Size = new Size(240, 25);
        label1.TabIndex = 0;
        label1.Text = "Ingrese DNI o Nro de Carton";
        // 
        // btnRechazar
        // 
        btnRechazar.BackColor = Color.Red;
        btnRechazar.Location = new Point(431, 337);
        btnRechazar.Name = "btnRechazar";
        btnRechazar.Size = new Size(112, 34);
        btnRechazar.TabIndex = 2;
        btnRechazar.Text = "Rechazar";
        btnRechazar.UseVisualStyleBackColor = false;
        // 
        // btnAprobar
        // 
        btnAprobar.BackColor = Color.MediumSeaGreen;
        btnAprobar.Location = new Point(239, 337);
        btnAprobar.Name = "btnAprobar";
        btnAprobar.Size = new Size(112, 34);
        btnAprobar.TabIndex = 1;
        btnAprobar.Text = "Aprobar";
        btnAprobar.UseVisualStyleBackColor = false;
        // 
        // dgvPagosPendientes
        // 
        dgvPagosPendientes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPagosPendientes.Columns.AddRange(new DataGridViewColumn[] { colIdPago, colDni, colMonto, colComprobante, colFechaPago });
        dgvPagosPendientes.Dock = DockStyle.Fill;
        dgvPagosPendientes.Location = new Point(0, 0);
        dgvPagosPendientes.Name = "dgvPagosPendientes";
        dgvPagosPendientes.RowHeadersWidth = 62;
        dgvPagosPendientes.Size = new Size(640, 406);
        dgvPagosPendientes.TabIndex = 0;
        // 
        // colIdPago
        // 
        colIdPago.HeaderText = "ID";
        colIdPago.MinimumWidth = 8;
        colIdPago.Name = "colIdPago";
        colIdPago.ReadOnly = true;
        colIdPago.Visible = false;
        colIdPago.Width = 150;
        // 
        // colDni
        // 
        colDni.HeaderText = "DNI Jugador";
        colDni.MinimumWidth = 8;
        colDni.Name = "colDni";
        colDni.ReadOnly = true;
        colDni.Width = 150;
        // 
        // colMonto
        // 
        colMonto.HeaderText = "Monto";
        colMonto.MinimumWidth = 8;
        colMonto.Name = "colMonto";
        colMonto.Width = 150;
        // 
        // colComprobante
        // 
        colComprobante.HeaderText = "Estado del Comprobante";
        colComprobante.MinimumWidth = 8;
        colComprobante.Name = "colComprobante";
        colComprobante.Width = 150;
        // 
        // colFechaPago
        // 
        colFechaPago.HeaderText = "Fecha";
        colFechaPago.MinimumWidth = 8;
        colFechaPago.Name = "colFechaPago";
        colFechaPago.Width = 150;
        // 
        // tabPage2
        // 
        tabPage2.Controls.Add(flowLayoutPanel1);
        tabPage2.Controls.Add(dataGridView1);
        tabPage2.Location = new Point(4, 34);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new Padding(3);
        tabPage2.Size = new Size(971, 412);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "Gestion de Sorteos";
        tabPage2.UseVisualStyleBackColor = true;
        // 
        // dataGridView1
        // 
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colFecha, colEstado, colPrecio, colModo });
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.Location = new Point(3, 3);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.RowHeadersWidth = 62;
        dataGridView1.Size = new Size(965, 406);
        dataGridView1.TabIndex = 0;
        dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        // 
        // colId
        // 
        colId.HeaderText = "ID";
        colId.MinimumWidth = 8;
        colId.Name = "colId";
        colId.Width = 150;
        // 
        // colNombre
        // 
        colNombre.HeaderText = "Nombre del Sorteo";
        colNombre.MinimumWidth = 8;
        colNombre.Name = "colNombre";
        colNombre.Width = 150;
        // 
        // colFecha
        // 
        colFecha.HeaderText = "Fecha y Hora";
        colFecha.MinimumWidth = 8;
        colFecha.Name = "colFecha";
        colFecha.Width = 150;
        // 
        // colEstado
        // 
        colEstado.HeaderText = "Estado";
        colEstado.MinimumWidth = 8;
        colEstado.Name = "colEstado";
        colEstado.Width = 150;
        // 
        // colPrecio
        // 
        colPrecio.HeaderText = "Precio por Carton";
        colPrecio.MinimumWidth = 8;
        colPrecio.Name = "colPrecio";
        colPrecio.Width = 150;
        // 
        // colModo
        // 
        colModo.HeaderText = "Modo Extraccion";
        colModo.MinimumWidth = 8;
        colModo.Name = "colModo";
        colModo.Width = 150;
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.AutoSize = true;
        flowLayoutPanel1.BackColor = Color.LightSteelBlue;
        flowLayoutPanel1.Controls.Add(btnNuevo);
        flowLayoutPanel1.Controls.Add(btnEditar);
        flowLayoutPanel1.Controls.Add(btnActualizar);
        flowLayoutPanel1.Dock = DockStyle.Top;
        flowLayoutPanel1.Location = new Point(3, 3);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(965, 40);
        flowLayoutPanel1.TabIndex = 1;
        // 
        // btnNuevo
        // 
        btnNuevo.Location = new Point(3, 3);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(112, 34);
        btnNuevo.TabIndex = 0;
        btnNuevo.Text = "Nuevo Sroteo";
        btnNuevo.UseVisualStyleBackColor = true;
        // 
        // btnEditar
        // 
        btnEditar.Location = new Point(121, 3);
        btnEditar.Name = "btnEditar";
        btnEditar.Size = new Size(112, 34);
        btnEditar.TabIndex = 1;
        btnEditar.Text = "Editar Seleccion";
        btnEditar.UseVisualStyleBackColor = true;
        // 
        // btnActualizar
        // 
        btnActualizar.Location = new Point(239, 3);
        btnActualizar.Name = "btnActualizar";
        btnActualizar.Size = new Size(112, 34);
        btnActualizar.TabIndex = 2;
        btnActualizar.Text = "Refrescar";
        btnActualizar.UseVisualStyleBackColor = true;
        // 
        // dgvSorteos
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(979, 450);
        Controls.Add(tabControl1);
        Name = "dgvSorteos";
        Text = "FrmGestion.cs";
        Load += dgvSorteos_Load;
        tabControl1.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel1.PerformLayout();
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPagosPendientes).EndInit();
        tabPage2.ResumeLayout(false);
        tabPage2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        flowLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TabControl tabControl1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private DataGridView dataGridView1;
    private SplitContainer splitContainer1;
    private Button btnBuscar;
    private TextBox txtBusqueda;
    private Label label1;
    private Button btnRechazar;
    private Button btnAprobar;
    private DataGridView dgvPagosPendientes;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colEstado;
    private DataGridViewTextBoxColumn colPrecio;
    private DataGridViewTextBoxColumn colModo;
    private DataGridViewTextBoxColumn colIdPago;
    private DataGridViewTextBoxColumn colDni;
    private DataGridViewTextBoxColumn colMonto;
    private DataGridViewTextBoxColumn colComprobante;
    private DataGridViewTextBoxColumn colFechaPago;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnNuevo;
    private Button btnEditar;
    private Button btnActualizar;
}
