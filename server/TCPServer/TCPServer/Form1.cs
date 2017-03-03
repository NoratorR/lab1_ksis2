using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
         {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form1 frm = new Form1();
            SocetListner scl = new SocetListner();
           
            label2.Text = "Status: Online";

            scl.Start();
           // label2.Text = "Status: Offline";
        }
    }
}
