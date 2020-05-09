using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Http;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class FileAttachment : Form
    {
        HttpClientService httpClientService;
        List<int> files;
        List<string> extensions;
        const int MaxFileSize = 30;
        const int Megabyte = 1000000;

        public FileAttachment(List<int> files)
        {
            InitializeComponent();
            httpClientService = new HttpClientService();
            this.files = files;
            extensions = new List<string>();
            extensions.Add(".pdf");
            extensions.Add(".exe");
            UpdateFilesList();
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

        private async void btnLoadFile_Click(object sender, EventArgs e)
        {
            ofdOpenFile.ShowDialog();
            string filePath = ofdOpenFile.FileName;
            var fileWrapper = new System.IO.FileInfo(filePath);
            if (! (fileWrapper.Length / Megabyte > MaxFileSize))
            {
                if (!extensions.Contains(fileWrapper.Extension))
                {
                    if (await httpClientService.LoadFile(filePath) != -1)
                    {
                        MessageBox.Show("You cannot add such file, it is already added!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("File successfully added", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("You cannot add such file, extension is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You cannot add such file, it is too big. Max size is 30MB!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private async void btnDeleteStorage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!await httpClientService.DeleteFile(int.Parse(tbFileID.Text)))
                {
                    MessageBox.Show("There is no file with such id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("File successfully deleted", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Some shit happened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private async void btnGetInfo_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = await httpClientService.GetFileInfo(int.Parse(tbFileID.Text));
            try
            {
                if (fileInfo == null)
                {
                    MessageBox.Show("There is no file with such id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("File name - " + fileInfo.Name + "\n File size - " + fileInfo.Size, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Some shit happened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAddMsg_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = await httpClientService.GetFileInfo(int.Parse(tbFileID.Text));
            try
            {
                if (fileInfo == null)
                {
                    MessageBox.Show("There is no file with such id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if(!files.Contains(int.Parse(tbFileID.Text)))
                    {
                        files.Add(int.Parse(tbFileID.Text));
                        UpdateFilesList();
                        MessageBox.Show("You added file to message.\n" + "File name - " + fileInfo.Name + "\n File size - " + fileInfo.Size, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("You've already added this file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Some shit happened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void UpdateFilesList()
        {
            lbFiles.Items.Clear();
            foreach(int id in files)
            {
                var info = await httpClientService.GetFileInfo(id);
                lbFiles.Items.Add(info.Name + " id " + id);
            }
        }

        private void btnDeleteMsg_Click(object sender, EventArgs e)
        {
            if (lbFiles.SelectedIndex != -1)
            {
                int fileID = GetIDFromString((string)lbFiles.Items[lbFiles.SelectedIndex]);
                files.Remove(fileID);
                UpdateFilesList();
            }
            else
            {
                MessageBox.Show("Choose file id!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
