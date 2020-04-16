using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using Common;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class fMain : Form
    {
        IClientService client;
        IClientRepositoryService clientRepositoryService;
        public fMain()
        {
            InitializeComponent();
            client = new ClientService();
            clientRepositoryService = ClientRepositoryService.GetInstance();
            ClientRepository.GetInstance().SubscribeUIUpdate(UpdateGUI);
            lbUsers.SelectedIndexChanged += lbUsers_SelectedIndexChanged;
        }

        public void UpdateGUI(DataUpdateEvents.Events events)
        {
            Action action;
            if (events == DataUpdateEvents.Events.MessagesUpdate)
            {
                action = UpdateMessages;
            }
            else
            {
                action = UpdateUsers;
            }
            //action += UpdateServerInfo;
            Invoke(action);
        }

        /*private void UpdateServerInfo()
        {
            //tbIPAddress.Text = client.ServerIPAddress;
            //tbPort.Text = client.ServerPort.ToString();
        }*/

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
            }
            catch
            {
                MessageBox.Show("Please check entered data or server is not responding!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.CloseClient();
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;
            tbIPAddress.ReadOnly = false;
            tbName.ReadOnly = false;
            tbPort.ReadOnly = false;
            btnSend.Enabled = false;
            tbMessages.Clear();
            lbUsers.Items.Clear();
            cbMsgToAll.Checked = true;
            cbMsgToAll.Enabled = false;
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
                tbMessages.Text = clientRepositoryService.GetMessagesText(-1);
            }
            else
            {
                if (lbUsers.SelectedIndex != -1)
                {
                    int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                    tbMessages.Text = clientRepositoryService.GetMessagesText(index);
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (cbMsgToAll.Checked)
            {
                MessageSender.SendMessage(MessageCreator.CreateMessageToAll(tbMessage.Text));
            }
            else
            {
                if (lbUsers.SelectedIndex != -1)
                {
                    int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                    clientRepositoryService.SaveMessage(MessageCreator.CreatePrivateMessage(tbMessage.Text, index), index);
                    MessageSender.SendMessage(MessageCreator.CreatePrivateMessage(tbMessage.Text, index));
                }
                else
                {
                    MessageBox.Show("Choose user you want to write!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            tbMessage.Clear();
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!cbMsgToAll.Checked)
            {
                int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                tbMessages.Text = clientRepositoryService.GetMessagesText(index);
            }
        }

        private void cbMsgToAll_CheckedChanged(object sender, EventArgs e)
        {
            if(cbMsgToAll.Checked)
            {
                tbMessages.Text = clientRepositoryService.GetMessagesText(-1);
            }
        }

        private void btnFindServer_Click(object sender, EventArgs e)
        {
            //client.UdpBroadcastRequest();
        }
    }
}
