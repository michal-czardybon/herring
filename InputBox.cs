using System;
using System.Windows.Forms;

namespace Herring
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public string InputText
        {
            get
            {
                return textBox1.Text;
            }
        }

        private void InputBox_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
