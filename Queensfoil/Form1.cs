using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Queensfoil
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Util.OnDestiny2FolderSet += LoadDestinyFolder;
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string Destiny2Folder = dialog.FileName;
                if(!Util.IsValidDestiny2Folder(Destiny2Folder))
                {
                    MessageBox.Show("Invalid Destiny 2 folder, select the folder with destiny2.exe in it", "Error", MessageBoxButtons.OK);
                    return;
                }
                Util.SetDestiny2Folder(Destiny2Folder);
            }           
            
        }

        

        private void Form1_Shown(object sender, EventArgs e)
        {
            if(Util.IsValidDestiny2Folder(Properties.Settings.Default.DestinyPath))
            {
                Util.SetDestiny2Folder(Properties.Settings.Default.DestinyPath);
            }
        }

        private void LoadDestinyFolder()
        {
            string PackagesPath = Path.Combine(Properties.Settings.Default.DestinyPath, "packages");

            foreach (string pkgfile in Directory.EnumerateFiles(PackagesPath))
            {
                TreeNode node = new TreeNode(pkgfile);
                treeView1.Nodes.Add(node);
            }
        }
    }
}
