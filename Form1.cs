using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicBeePlugin;
using MySql.Data.MySqlClient;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{ 

    public partial class Form1 : Form
    {

        public MusicBeeApiInterface mbApiInterface;
        public Form1(Plugin.MusicBeeApiInterface pApi)
        {
            mbApiInterface = pApi;
            InitializeComponent();
            


        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
            label1.Text = "OK";
        }
        public void button1_Click(object sender, EventArgs e)
        {

        }
           
          
    }
}
          