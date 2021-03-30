using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChangeSqlitePassword
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSelectDb_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "sqlite db (*.db)|*.db|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    txtSourceDbPath.Text = filePath;

                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            var sourceDb = txtSourceDbPath.Text.Trim();
            var oldPassword = txtOldPassword.Text.Trim();
            var connstr = "Data Source=" + sourceDb + ";Version=3;New=False;Compress=True;Password=" + oldPassword;
            using (SQLiteConnection sQLiteConnection = new SQLiteConnection(connstr))
            {
                sQLiteConnection.Open();
                var newPass = txtNewPassword.Text.Trim();
                 sQLiteConnection.ChangePassword(newPass);
                var query = "select * from [table-name]";
                SQLiteCommand sQLiteCommand = sQLiteConnection.CreateCommand();
                sQLiteCommand.CommandText = query;
                sQLiteCommand.ExecuteNonQuery();

                SQLiteDataReader dr = sQLiteCommand.ExecuteReader();
                while (dr.Read())
                {
                    MessageBox.Show(dr["column-name"].ToString());
                }
            }
        }
    }
}
