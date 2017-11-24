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

        int a = 0; //Tomará valores 1,2,3 o 4 dependiendo del textBox de filtrado en el que se haya escrito primero.

        private void TextBoxesFiltro_TextChanged(object sender, EventArgs e)
        {
            string nombre = ((TextBox)sender).Name; //nombre del textBox que llama al método

            int a = 0;
            if (tbCompany.Text=="" && tbNombre.Text=="" && tbCiudad.Text=="" && tbTelefono.Text=="")
            {
                a = 0;
            }

            if (a==0 && nombre == tbCompany.Name)
            {
                a = 1;
                bs.Filter = string.Format("NombreCompania LIKE '%{0}%'", tbNombre.Text);
            }

            if (a==0 && nombre == tbNombre.Name)
            {
                a = 2;
                bs.Filter = string.Format("ContactNombre LIKE '%{0}%'", tbCompany.Text);
                
            }

            if (a==0 && nombre == tbCiudad.Name)
            {
                a = 3;
                bs.Filter = string.Format("Ciudad LIKE '%{0}%'", tbCiudad.Text);
            }

            if (a==0 && nombre == tbTelefono.Name)
            {
                a = 4;
                bs.Filter = string.Format("Telefono LIKE '%{0}%'", tbTelefono.Text);
            }


        }


        //public void consulta()
        //{
        //    string sql = "SELECT * FROM Proveedores";
        //    command = new SqlCommand(sql, con);
        //    adapter = new SqlDataAdapter(command);
        //    table = new DataTable();
        //    adapter.Fill(table);
        //    dataGridView1.DataSource = table;
        //    //dataGridView1.AutoSize = true;
        //}
    }
}
