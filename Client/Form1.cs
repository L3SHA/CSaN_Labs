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
    public partial class Form1 : Form
    {
        ClientService client;
        public Form1()
        {
            InitializeComponent();
            client = new ClientService();
            client.Subscribe(UpdateGUI);
            lbUsers.SelectedIndexChanged += lbUsers_SelectedIndexChanged;
        }

        public void UpdateGUI()
        {
            Action action = UpdateMessages;
            action += UpdateUsers;
            action += UpdateServerInfo;
            Invoke(action);
        }

        private void UpdateServerInfo()
        {
            tbIPAddress.Text = client.ServerIPAddress;
            tbPort.Text = client.ServerPort.ToString();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.StartClient(IPAddress.Parse(tbIPAddress.Text), int.Parse(tbPort.Text));
                client.SendMessage(new Common.Message(MessageTypes.RegRequest, tbName.Text));
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

        private void UpdateMessages()
        {
            if(cbMsgToAll.Checked)
            {
                tbMessages.Text = client.Conversations[-1];
            }
            else
            {
                if(lbUsers.SelectedIndex != -1)
                {
                    int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                    tbMessages.Text = client.Conversations[index];
                }
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

        private void UpdateUsers()
        {
            
            lbUsers.Items.Clear();
            foreach(int id in client.Users.Keys)
            {
                if (id != -1)
                {
                    lbUsers.Items.Add("User id:" + id + " " + client.Users[id]);
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.SendMessage(new Common.Message(MessageTypes.UserJoinOrLeft, client.ID, tbName.Text, tbName.Text + " left the chat", false));
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (cbMsgToAll.Checked)
            {
                client.SendMessage(new Common.Message(MessageTypes.ToAllMsg, -1, "[Public Message]" + DateTime.Now + " From " + tbName.Text + ": " + tbMessage.Text));
            }
            else
            {
                if (lbUsers.SelectedIndex != -1)
                {
                    int index = GetIDFromString((string)lbUsers.Items[lbUsers.SelectedIndex]);
                    client.Conversations[index] += tbMessage.Text;
                    client.SendMessage(new Common.Message(MessageTypes.PrivateMsg, client.ID, index, "[Private Message]" + DateTime.Now + " From " + tbName.Text + ": " + tbMessage.Text));
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
                tbMessages.Text = client.Conversations[index];
            }
        }

        private void cbMsgToAll_CheckedChanged(object sender, EventArgs e)
        {
            if(cbMsgToAll.Checked)
            {
                tbMessages.Text = client.Conversations[-1];
            }
        }

        private void btnFindServer_Click(object sender, EventArgs e)
        {
            client.UdpBroadcastRequest();
        }
    }
}
