using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using System.Text.RegularExpressions;
using Client.Http;
using System.Threading.Tasks;

namespace Client
{
    public partial class fMain : Form
    {
        List<int> files;
        IClientService client;
        IClientRepositoryService clientRepositoryService;
        int currentChat = -1;
        public fMain()
        {
            InitializeComponent();
            files = new List<int>();
            client = new ClientService();
            clientRepositoryService = ClientRepositoryService.GetInstance();
            ClientRepository.GetInstance().SubscribeUIUpdate(UpdateGUI);
            lbUsers.SelectedIndexChanged += lbUsers_SelectedIndexChanged;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                clientRepositoryService.SetEndPointAddress(tbIPAddress.Text, tbPort.Text);
                clientRepositoryService.SetClientName(tbName.Text);
                client.StartClient();
                tbName.ReadOnly = true;
                tbIPAddress.ReadOnly = true;
                tbPort.ReadOnly = true;
                tbMessage.ReadOnly = false;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnSend.Enabled = true;
                cbMsgToAll.Enabled = true;
                btnAttach.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Please check entered data or server is not responding!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.CloseClient();
            UpdateServerConnection();
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

        public void UpdateGUI(DataUpdateEvents.Events events)
        {
            Action action;
            switch(events)
            {
                case DataUpdateEvents.Events.MessagesUpdate:
                    action = UpdateMessages;
                    break;
                case DataUpdateEvents.Events.UsersListUpdate:
                    action = UpdateUsers;
                    break;
                case DataUpdateEvents.Events.ServerInfoUpdate:
                    action = UpdateServerInfo;
                    break;
                case DataUpdateEvents.Events.ServerConnectionUpdate:
                    action = UpdateServerConnection;
                    break;
                default:
                    action = null;
                    break;
            }
            Invoke(action);
        }

        private void UpdateServerInfo()
        {
            tbIPAddress.Text = clientRepositoryService.GetEndPointAddress().Address.ToString();
            tbPort.Text = clientRepositoryService.GetEndPointAddress().Port.ToString();
        }

        private void UpdateServerConnection()
        {
            clientRepositoryService.ClearClientRepository();
            cbMsgToAll.Enabled = false;
            cbMsgToAll.Checked = true;
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;
            tbIPAddress.ReadOnly = false;
            tbName.ReadOnly = false;
            tbPort.ReadOnly = false;
            btnSend.Enabled = false;
            btnAttach.Enabled = false;
            lbMessages.Items.Clear();
            lbUsers.Items.Clear();
        }

        private void UpdateUsers()
        {
            
            lbUsers.Items.Clear();
            foreach(string userInfo in clientRepositoryService.GetUsersList())
            {
                lbUsers.Items.Add(userInfo);
            }
        }

        private void UpdateMessages()
        {
            if (cbMsgToAll.Checked)
            {
                UpdateMessagesListBox(-1);
            }
            else
            {
                if (lbUsers.SelectedIndex != -1)
                {
                    int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                    UpdateMessagesListBox(index);
                }
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string filesNames = await GetFilesString();
            if (cbMsgToAll.Checked)
            {
                MessageSender.SendMessage(MessageCreator.CreateMessageToAll(tbMessage.Text + " " + filesNames, files));
            }
            else
            {
                if (lbUsers.SelectedIndex != -1)
                {
                    int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                    clientRepositoryService.SaveMessage(MessageCreator.CreatePrivateMessage(tbMessage.Text +  " " + filesNames , files, index), index);
                    MessageSender.SendMessage(MessageCreator.CreatePrivateMessage(tbMessage.Text, files, index));
                }
                else
                {
                    MessageBox.Show("Choose user you want to write!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            files.Clear();
            lFiles.Text = "";
            tbMessage.Clear();
        }

        private async Task<string> GetFilesString()
        {
            HttpClientService httpClientService = new HttpClientService();   
            string filesIDString = "";
            if (files != null)
            {
                foreach (int fileID in files)
                {
                    var fileInfo = await httpClientService.GetFileInfo(fileID);
                    filesIDString += fileInfo.Name + ", ";
                }
            }
            return filesIDString;
        }

        private void UpdateMessagesListBox(int id)
        {
            lbMessages.Items.Clear();
            var messages = clientRepositoryService.GetMessagesText(id);
            foreach(Common.Message message in messages)
            {
                lbMessages.Items.Add(message.message);
            }
            
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!cbMsgToAll.Checked)
            {
                int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                UpdateMessagesListBox(index);
                currentChat = index;
            }
        }

        private void cbMsgToAll_CheckedChanged(object sender, EventArgs e)
        {
            if(cbMsgToAll.Checked && cbMsgToAll.Enabled)
            {
                UpdateMessagesListBox(-1);
                currentChat = -1;
            }
        }

        private void btnFindServer_Click(object sender, EventArgs e)
        {
            client.FindServerRequest();
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            FileAttachment fileAttachment = new FileAttachment(files);
            fileAttachment.Show();
        }

        private async void fMain_Activated(object sender, EventArgs e)
        {
            HttpClientService httpClientService = new HttpClientService();
            lFiles.Text = "";
            foreach(int id in files)
            {
                var fileInfo = await httpClientService.GetFileInfo(id);
                lFiles.Text += fileInfo.Name + "\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(lbMessages.SelectedIndex != -1)
            {
                var list = clientRepositoryService.GetMessagesText(currentChat);// lbMessages.SelectedIndex
                Common.Message tempMessage = null;
                foreach(Common.Message message in list)
                {
                    if (message.message == lbMessages.Items[lbMessages.SelectedIndex].ToString())
                    {
                        tempMessage = message;
                        break;
                    }
                }
                if(tempMessage.files.Count != 0)
                {
                    ViewFiles viewFiles = new ViewFiles(tempMessage.files);
                    viewFiles.Show();
                }
                else 
                {
                    MessageBox.Show("No files attached to this message!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Choose message you want to view!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
