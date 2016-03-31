using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SourceForgeHTTPSvnDownload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //https://sourceforge.net/p/benderbot/svn/HEAD/tree/
            GatherPic gp = new GatherPic(webBrowser1, "https://sourceforge.net/p/benderbot/svn/HEAD/tree/", "C:\\Users\\Administrator\\Desktop\\百家乐(手机端\\开始编码\\DownBenderBot");
            gp.start();
        }
    }
}
