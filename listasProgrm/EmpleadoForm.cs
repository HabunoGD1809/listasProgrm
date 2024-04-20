namespace listasProgrm
{
    public class EmpleadoForm : Form
    {
        private TextBox txtId, txtNombre, txtDireccion, txtTelefono, txtEdad, txtSalario;
        private Button btnSave;
        private Label lblId, lblNombre, lblDireccion, lblTelefono, lblEdad, lblSalario;

        public Empleado Empleado { get; private set; }

        public EmpleadoForm(Empleado emp = null)
        {
            InitializeComponents();
            Empleado = emp ?? new Empleado(0, "", "", "", 0, 0m);

            txtId.Text = Empleado.IdEmpleado.ToString();
            txtNombre.Text = Empleado.Nombre;
            txtDireccion.Text = Empleado.Direccion;
            txtTelefono.Text = Empleado.Telefono;
            txtEdad.Text = Empleado.Edad.ToString();
            txtSalario.Text = Empleado.Salario.ToString();

            if (emp != null)
            {
                txtId.Enabled = false;  // El ID no debería ser editable si ya existe el empleado
            }
        }

        private void InitializeComponents()
        {
            // Initialize labels
            lblId = new Label { Text = "ID:" };
            lblNombre = new Label { Text = "Nombre:" };
            lblDireccion = new Label { Text = "Dirección:" };
            lblTelefono = new Label { Text = "Teléfono:" };
            lblEdad = new Label { Text = "Edad:" };
            lblSalario = new Label { Text = "Salario:" };

            // Initialize text boxes
            txtId = new TextBox();
            txtNombre = new TextBox();
            txtDireccion = new TextBox();
            txtTelefono = new TextBox();
            txtEdad = new TextBox();
            txtSalario = new TextBox();

            // Initialize button
            btnSave = new Button { Text = "Guardar" };
            btnSave.Click += btnSave_Click;

            // Layout using a TableLayoutPanel for simplicity
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.AutoSize = true;
            panel.Controls.Add(lblId, 0, 0);
            panel.Controls.Add(txtId, 1, 0);
            panel.Controls.Add(lblNombre, 0, 1);
            panel.Controls.Add(txtNombre, 1, 1);
            panel.Controls.Add(lblDireccion, 0, 2);
            panel.Controls.Add(txtDireccion, 1, 2);
            panel.Controls.Add(lblTelefono, 0, 3);
            panel.Controls.Add(txtTelefono, 1, 3);
            panel.Controls.Add(lblEdad, 0, 4);
            panel.Controls.Add(txtEdad, 1, 4);
            panel.Controls.Add(lblSalario, 0, 5);
            panel.Controls.Add(txtSalario, 1, 5);
            panel.Controls.Add(btnSave, 1, 6);

            Controls.Add(panel);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // EmpleadoForm
            // 
            ClientSize = new Size(556, 261);
            Name = "EmpleadoForm";
            ResumeLayout(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Attempt to update the Empleado object
                int id = int.Parse(txtId.Text);
                string nombre = txtNombre.Text;
                string direccion = txtDireccion.Text;
                string telefono = txtTelefono.Text;
                int edad = int.Parse(txtEdad.Text);
                decimal salario = decimal.Parse(txtSalario.Text);

                Empleado = new Empleado(id, nombre, direccion, telefono, edad, salario);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
