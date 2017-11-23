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
        SqlDataAdapter adapter;
        DataTable table;
        SqlCommand command;
        BindingSource bs;

        public FormListadoProveedores()
        {
            InitializeComponent();
            con = Conexion.getConexion();
            bs = new BindingSource();
            splitContainer1.IsSplitterFixed = true;
            consulta();
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


        public void consulta()
        {
            string sql = "SELECT * FROM Proveedores";
            command = new SqlCommand(sql, con);
            adapter = new SqlDataAdapter(command);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            //dataGridView1.AutoSize = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1Collapsed)
            {
                button5.Text = "Cerrar";
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.SplitterDistance = 100;
            }
            else
            {
                button5.Text = "Abrir búsqueda";
                splitContainer1.Panel1Collapsed = true;

            }
            
        }
    }
}
