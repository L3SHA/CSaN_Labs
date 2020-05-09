using System;
using System.Windows.Forms;
using System.IO;
using Client.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class ViewFiles : Form
    {
        HttpClientService httpClientService;
        List<int> files;
        public ViewFiles(List<int> files)
        {
            this.files = files;
            httpClientService = new HttpClientService();
            InitializeComponent();
            UpdateFilesList(this.files);
        }

        private async void UpdateFilesList(List<int> files)
        {
            foreach(int id in files)
            {
                var info = await httpClientService.GetFileInfo(id);
                lbFiles.Items.Add(info.Name + " FileID " + id);

            }
        }

        private int GetIDFromString(string str)
        {
            int id = 0;
            var regex = new Regex(@"\d+\s");
            var matches = regex.Matches(str);
            foreach (Match match in matches)
                id = int.Parse(match.Value);
            return id;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            sfdSaveFile.ShowDialog();
            string path = sfdSaveFile.FileName;
            using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                if(lbFiles.SelectedIndex != -1)
                {
                    Stream stream = await httpClientService.GetFile(GetIDFromString((string)lbFiles.Items[lbFiles.SelectedIndex]));
                    if (stream != null)
                    {
                        stream.CopyTo(fs);
                    }
                    else
                    {
                        MessageBox.Show("You cannot get this file, it has been removed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Choose file you want to get!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
        }
    }
}
