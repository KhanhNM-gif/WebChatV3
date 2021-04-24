using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

public class ChatHub : Hub
{
    public static Dictionary<string, long> ConnectedUsers = new Dictionary<string, long>();
    public static ListGroupHub listGroup = new ListGroupHub();

    public void Connect(string tokenkey)
    {
        
        if (!CacheUserToken.GetResultUserToken(tokenkey)) return;

        bool isFind = ConnectedUsers.TryGetValue(Context.ConnectionId, out long IDUser);
        if (!isFind)
        {
            CacheUserToken.GetUserToken(tokenkey, out UserToken token);

            ConnectedUsers.Add(Context.ConnectionId, token.UserID);
        }
    }

    public override Task OnDisconnected(bool stopCalled)
    {
        bool isFind = ConnectedUsers.TryGetValue(Context.ConnectionId, out long IDUser);
        if (!isFind)
        {
            ConnectedUsers.Remove(Context.ConnectionId);

            var list = listGroup.GetListGroupChatByIdUserConnect(Context.ConnectionId);
            Clients.Groups(list).onUserDisconnected(IDUser);

            foreach(var roomName in list)
            {
                Groups.Add(Context.ConnectionId, roomName);
            }

        }
        return base.OnDisconnected(stopCalled);
    }

    public override Task OnConnected()
    {

        return base.OnConnected();
    }
}
