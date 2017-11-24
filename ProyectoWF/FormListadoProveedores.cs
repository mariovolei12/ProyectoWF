using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoWF
{
    public partial class FormListadoProveedores : Form
    {

        //SqlConnection con = new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB;" +
        //    "Initial Catalog = ProyectoWF; Integrated Security = True;" +
        //    "Connect Timeout = 30; Encrypt=False;TrustServerCertificate=True;" +
        //    "ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        SqlConnection con;
        BindingSource bs;

        DataTable table;
        SqlDataAdapter adapter;
        
        public FormListadoProveedores()
        {
            InitializeComponent();
            con = Conexion.getConexion();
            bs = new BindingSource();
            splitContainer1.IsSplitterFixed = true;
            cargar();
            
            dataGridView1.Columns["ProveedorID"].Visible = false;
            dataGridView1.Columns["Logo"].Visible = false;
        }
        

        public void cargar()
        {
            try{
                table = new DataTable();
                adapter = new SqlDataAdapter("select * from Proveedores", con);
                adapter.Fill(table);
                
                dataGridView1.DataSource = table;
                bs.DataSource = table;
            } catch (Exception e)
            {
                MessageBox.Show("no se pudo llenar el datagridview "+e.ToString());
            }
        }

        private void btAbrirBusqueda_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1Collapsed)
            {
                btAbrirBusqueda.Text = "Cerrar";
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.SplitterDistance = 120;
            }
            else
            {
                btAbrirBusqueda.Text = "Abrir búsqueda";
                splitContainer1.Panel1Collapsed = true;
            }
        }
        
        private void TextBoxesFiltro_TextChanged(object sender, EventArgs e)
        {
            int iEntro = 0;
            switch (((TextBox)sender).Name)
            {
                case "tbNombre":
                    iEntro = 1;
                    bs.Filter = "ContactoNombre LIKE '%" + tbNombre.Text + "%'";
                    break;
                case "tbCompany":
                    iEntro = 2;
                    bs.Filter = "NombreCompania LIKE '%" + tbCompany.Text + "%'";
                    break;
                case "tbCiudad":
                    iEntro = 3;
                    bs.Filter = "Ciudad LIKE '%" + tbCiudad.Text + "%'";
                    break;
                case "tbTelefono":
                    iEntro = 4;
                    bs.Filter = "Telefono LIKE '%" + tbTelefono.Text + "%'";
                    break;
            }
            if ((iEntro != 1) && (tbNombre.Text != ""))
            {
                bs.Filter = bs.Filter + " AND ContactoNombre LIKE '%" + tbNombre.Text + "%'";
            }

            if ((iEntro != 2) && (tbCompany.Text != ""))
            {
                bs.Filter = bs.Filter + " AND NombreCompania LIKE '%" + tbCompany.Text + "%'";
            }

            if ((iEntro != 3) && (tbCiudad.Text != ""))
            {
                bs.Filter = bs.Filter + " AND Ciudad LIKE '%" + tbCiudad.Text + "%'";
            }

            if ((iEntro != 4) && (tbTelefono.Text != ""))
            {
                bs.Filter = bs.Filter + " AND Telefono LIKE '%" + tbTelefono.Text + "%'";
            }

            if (tbNombre.Text == "" && tbCompany.Text == "" && tbCiudad.Text == "" && tbTelefono.Text == "")
            {
                bs.Filter = "";
            }

            dataGridView1.DataSource = bs;
        }

        private void btBorrar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una fila!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else
            {
                int count = 0;
                DataGridViewRow filaborrar;
                int id;
                DialogResult dr = MessageBox.Show("¿Está seguro de borrar los datos seleccionados?", "Atención",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dr == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        filaborrar = dataGridView1.Rows[i];
                        if (filaborrar.Selected == true)
                        {
                            id = (int)dataGridView1.Rows[i].Cells["ProveedorID"].Value;
                            try
                            {
                                con.OpenAsync();
                                SqlCommand cmd = new SqlCommand("DELETE FROM Proveedores WHERE ProveedorID=" + id + "", con);
                                count += cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                    MessageBox.Show(count + " filas borradas", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cargar();
                }
            }
        }

        private void btNuevo_Click(object sender, EventArgs e)
        {
            FormProveedores formAltaPro = new FormProveedores();
            formAltaPro.FormClosed += new FormClosedEventHandler(FormProveedores_FormClosed);
            formAltaPro.ShowDialog();
            
        }

        //Actualizar dataGridView al cerrar form de Alta/Modificar
        private void FormProveedores_FormClosed(Object sender, FormClosedEventArgs args)
        {
            cargar();
        }

        private void btModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = (int)row.Cells["ProveedorID"].Value;

                FormProveedores formModificarPro = new FormProveedores(1, id);
                formModificarPro.FormClosed += new FormClosedEventHandler(FormProveedores_FormClosed);
                formModificarPro.ShowDialog();

            } else if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una fila!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("Selecciona solamente una fila para modificar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idFila = dataGridView1.SelectedCells[0].RowIndex;
            int id = (int)dataGridView1.Rows[idFila].Cells["ProveedorID"].Value;

            FormProveedores formDetallePro = new FormProveedores(2, id);
            formDetallePro.ShowDialog();
        }
    }
}
