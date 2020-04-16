﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common;

namespace Client
{
    public static class MessageHandler
    {
        private static IClientRepositoryService clientRepositoryService = ClientRepositoryService.GetInstance();
        public static void HandleMessage(Message message)
        {
            switch (message.messageType)
            {
                case MessageTypes.RegResponse:
                    clientRepositoryService.SetClientID(message.id);
                    clientRepositoryService.SaveMessage(message, -1);
                    foreach (UserInfo userInfo in message.users)
                    {
                        if (clientRepositoryService.GetClientID() != userInfo.ID)
                        {
                            clientRepositoryService.AddUser(userInfo.ID, userInfo.Name);
                        }
                    }
                    break;
                case MessageTypes.UserJoinOrLeft:
                    if (message.isJoin)
                    {
                        if (message.id != clientRepositoryService.GetClientID())
                        {
                            clientRepositoryService.SaveMessage(message, -1);
                            clientRepositoryService.AddUser(message.id, message.name);
                        }
                    }
                    else
                    {
                        clientRepositoryService.SaveMessage(message, -1);
                        clientRepositoryService.DeleteUser(message.id);
                        //clientRepositoryService.DeleteMessages(id); add deleting of conversation with this user
                        //warning if user is deleted but you watching his messages how gui reacts?
                    }
                    break;
                case MessageTypes.PrivateMsg:
                    clientRepositoryService.SaveMessage(message, message.sourceID);
                    break;
                case MessageTypes.ToAllMsg:
                    clientRepositoryService.SaveMessage(message, -1);
                    break;
                /*case MessageTypes.SearchResponse:
                    ServerPort = message.port;
                    ServerIPAddress = message.ipAddress;
                    break;*/
            }
        }
    }
}
