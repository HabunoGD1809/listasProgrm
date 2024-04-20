using System;
using System.Windows.Forms;
using System.IO;

namespace listasProgrm
{
    // Define el modelo Empleado
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int Edad { get; set; }
        public decimal Salario { get; set; }

        public Empleado(int idEmpleado, string nombre, string direccion, string telefono, int edad, decimal salario)
        {
            IdEmpleado = idEmpleado;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            Edad = edad;
            Salario = salario;
        }

        public override string ToString()
        {
            return $"ID: {IdEmpleado}, Nombre: {Nombre}, Dirección: {Direccion}, Teléfono: {Telefono}, Edad: {Edad}, Salario: {Salario:C}";
        }
    }

    // Define los nodos de la lista enlazada
    public class Nodo
    {
        public Empleado Empleado { get; set; }
        public Nodo Siguiente { get; set; }

        public Nodo(Empleado empleado)
        {
            Empleado = empleado;
        }
    }

    // Define la lista enlazada
    public class ListaEnlazada
    {
        public Nodo Cabeza { get; set; }

        public void AgregarEmpleado(Empleado empleado)
        {
            Nodo nuevoNodo = new Nodo(empleado) { Siguiente = Cabeza };
            Cabeza = nuevoNodo;
        }

        public Empleado ConsultarEmpleado(int idEmpleado)
        {
            Nodo actual = Cabeza;
            while (actual != null)
            {
                if (actual.Empleado.IdEmpleado == idEmpleado)
                    return actual.Empleado;
                actual = actual.Siguiente;
            }
            return null;
        }

        public bool ModificarEmpleado(int idEmpleado, string nombre, string direccion, string telefono, int edad, decimal salario)
        {
            Nodo actual = Cabeza;
            while (actual != null)
            {
                if (actual.Empleado.IdEmpleado == idEmpleado)
                {
                    actual.Empleado.Nombre = nombre;
                    actual.Empleado.Direccion = direccion;
                    actual.Empleado.Telefono = telefono;
                    actual.Empleado.Edad = edad;
                    actual.Empleado.Salario = salario;
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }

        public bool EliminarEmpleado(int idEmpleado)
        {
            Nodo actual = Cabeza;
            Nodo anterior = null;
            while (actual != null)
            {
                if (actual.Empleado.IdEmpleado == idEmpleado)
                {
                    if (anterior == null)
                        Cabeza = actual.Siguiente;
                    else
                        anterior.Siguiente = actual.Siguiente;
                    return true;
                }
                anterior = actual;
                actual = actual.Siguiente;
            }
            return false;
        }

        public void GuardarEnArchivo(string path)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                Nodo actual = Cabeza;
                while (actual != null)
                {
                    file.WriteLine($"{actual.Empleado.IdEmpleado},{actual.Empleado.Nombre},{actual.Empleado.Direccion},{actual.Empleado.Telefono},{actual.Empleado.Edad},{actual.Empleado.Salario}");
                    actual = actual.Siguiente;
                }
            }
        }

        public void CargarDesdeArchivo(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader file = new StreamReader(path))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        AgregarEmpleado(new Empleado(
                            Convert.ToInt32(data[0]),
                            data[1],
                            data[2],
                            data[3],
                            Convert.ToInt32(data[4]),
                            Convert.ToDecimal(data[5])));
                    }
                }
            }
        }
    }

    // Define el formulario principal
    public class MainForm : Form
    {
        private DataGridView dgvEmpleados;
        private Button btnAgregar, btnConsultar, btnModificar, btnEliminar, btnGuardar, btnSalir;
        private ListaEnlazada listaEmpleados;

        public MainForm()
        {
            InitializeComponent();
            listaEmpleados = new ListaEnlazada();
            try
            {
                listaEmpleados.CargarDesdeArchivo("empleados.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
            CargarDatos();
        }

        private void InitializeComponent()
        {
            this.dgvEmpleados = new DataGridView();
            this.btnAgregar = new Button();
            this.btnConsultar = new Button();
            this.btnModificar = new Button();
            this.btnEliminar = new Button();
            this.btnGuardar = new Button();
            this.btnSalir = new Button();

            // Configura DataGridView
            this.dgvEmpleados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmpleados.Location = new System.Drawing.Point(12, 12);
            this.dgvEmpleados.Name = "dgvEmpleados";
            this.dgvEmpleados.Size = new System.Drawing.Size(760, 250);
            this.dgvEmpleados.AllowUserToAddRows = false; 
            this.dgvEmpleados.ReadOnly = true; 
            this.dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; 
            this.dgvEmpleados.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; 
            this.dgvEmpleados.DefaultCellStyle.Font = new Font("Cascadia code", 15); 
            this.dgvEmpleados.DefaultCellStyle.ForeColor = Color.Black; 
            this.dgvEmpleados.DefaultCellStyle.BackColor = Color.White; 
            this.dgvEmpleados.RowHeadersVisible = false;
            this.dgvEmpleados.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.Controls.Add(this.dgvEmpleados);

            // Configura botones
            this.btnAgregar.Location = new System.Drawing.Point(12, 270);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += AgregarEmpleado;

            this.btnConsultar.Location = new System.Drawing.Point(93, 270);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(75, 23);
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += ConsultarEmpleado;

            this.btnModificar.Location = new System.Drawing.Point(174, 270);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(75, 23);
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += ModificarEmpleado;

            this.btnEliminar.Location = new System.Drawing.Point(255, 270);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += EliminarEmpleado;

            this.btnGuardar.Location = new System.Drawing.Point(336, 270);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += GuardarDatos;


            this.btnSalir.Location = new System.Drawing.Point(580, 270);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += Salir;
            this.Controls.Add(this.btnSalir);

            // Agrega los botones al formulario
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnConsultar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnGuardar);

            // Configuraciones del formulario
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Name = "MainForm";
            this.Text = "Gestión de Empleados";

            // Agrega columnas al DataGridView
            dgvEmpleados.Columns.Add("IdEmpleado", "ID");
            dgvEmpleados.Columns.Add("Nombre", "Nombre");
            dgvEmpleados.Columns.Add("Direccion", "Dirección");
            dgvEmpleados.Columns.Add("Telefono", "Teléfono");
            dgvEmpleados.Columns.Add("Edad", "Edad");
            dgvEmpleados.Columns.Add("Salario", "Salario");

        }

        private void Salir(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AgregarEmpleado(object sender, EventArgs e)
        {
            try
            {
                using (EmpleadoForm ef = new EmpleadoForm())
                {
                    if (ef.ShowDialog() == DialogResult.OK)
                    {
                        listaEmpleados.AgregarEmpleado(ef.Empleado);
                        CargarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar empleado: {ex.Message}");
            }
        }

        private void ConsultarEmpleado(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmpleados.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvEmpleados.CurrentRow.Cells["IdEmpleado"].Value);
                    Empleado empleado = listaEmpleados.ConsultarEmpleado(id);
                    if (empleado != null)
                        MessageBox.Show(empleado.ToString());
                    else
                        MessageBox.Show("Empleado no encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar empleado: {ex.Message}");
            }
        }

        private void ModificarEmpleado(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmpleados.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvEmpleados.CurrentRow.Cells["IdEmpleado"].Value);
                    Empleado empleado = listaEmpleados.ConsultarEmpleado(id);
                    if (empleado != null)
                    {
                        using (EmpleadoForm ef = new EmpleadoForm(empleado))
                        {
                            if (ef.ShowDialog() == DialogResult.OK)
                            {
                                listaEmpleados.ModificarEmpleado(id, ef.Empleado.Nombre, ef.Empleado.Direccion, ef.Empleado.Telefono, ef.Empleado.Edad, ef.Empleado.Salario);
                                CargarDatos();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Empleado no encontrado para modificar.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar empleado: {ex.Message}");
            }
        }

        private void EliminarEmpleado(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmpleados.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dgvEmpleados.CurrentRow.Cells["IdEmpleado"].Value);
                    if (listaEmpleados.EliminarEmpleado(id))
                    {
                        CargarDatos();
                        MessageBox.Show("Empleado eliminado correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el empleado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar empleado: {ex.Message}");
            }
        }

        private void GuardarDatos(object sender, EventArgs e)
        {
            try
            {
                listaEmpleados.GuardarEnArchivo("empleados.txt");
                MessageBox.Show("Datos guardados correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar datos: {ex.Message}");
            }
        }

        private void CargarDatos()
        {
            dgvEmpleados.Rows.Clear();
            Nodo actual = listaEmpleados.Cabeza;
            while (actual != null)
            {
                dgvEmpleados.Rows.Add(new object[]
                {
                    actual.Empleado.IdEmpleado,
                    actual.Empleado.Nombre,
                    actual.Empleado.Direccion,
                    actual.Empleado.Telefono,
                    actual.Empleado.Edad,
                    actual.Empleado.Salario.ToString("C")
                });
                actual = actual.Siguiente;
            }

            dgvEmpleados.AutoResizeColumns();
            dgvEmpleados.AutoResizeRows();
        }
    }
}
