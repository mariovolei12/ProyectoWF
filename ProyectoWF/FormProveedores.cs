using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoWF
{
    public partial class FormProveedores : Form
    {
        private int modo=0;
        private int pk;

        public FormProveedores()
        {
            InitializeComponent();
        }

        public FormProveedores(int modo, int primaryKey)
        {
            InitializeComponent();
            this.modo = modo;
            this.pk = primaryKey;

            tbId.Text = ""+primaryKey;
        }
    }
}
