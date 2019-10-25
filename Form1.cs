using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASONParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Node root = SyntaxAnalyser.Parse(TokenHelper.ReadTokens("JASON_ScannerOutput.txt"));
            treeView1.Nodes.Add(SyntaxAnalyser.PrintParseTree(root));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
