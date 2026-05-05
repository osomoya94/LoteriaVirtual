namespace Loteria.Escritorio;

public partial class dgvSorteos : Form
{
    public dgvSorteos()
    {
        InitializeComponent();
    }

    private void dgvSorteos_Load(object sender, EventArgs e)
    {

    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {

    }

    private void btnNuevo_Click(object sender, EventArgs e)
    {
        // 1. Creamos la instancia del formulario nuevo
        frmNuevoSorteo ventanaNuevo = new frmNuevoSorteo();

        // 2. Lo abrimos como cuadro de diálogo (bloquea la ventana de atrás hasta que se cierre)
        if (ventanaNuevo.ShowDialog() == DialogResult.OK)
        {
            // 3. Si el usuario tocó "Guardar", por ahora solo avisamos
            MessageBox.Show("Sorteo configurado correctamente. (Aquí conectaremos la API pronto)");
        }
    }


}
