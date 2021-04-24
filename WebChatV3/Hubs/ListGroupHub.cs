using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ListGroupHub
{
    private Dictionary<string, GroupHub> listGroupHub;
    public ListGroupHub()
    {
        listGroupHub = new Dictionary<string, GroupHub>();
    }

    public List<string> GetListGroupChatByIdUserConnect(string IdUserConnect)
    {
        List<string> ListIdGroup = new List<string>();
        foreach (var item in listGroupHub)
        {
            if (item.Value.ltIdUserConnect.FirstOrDefault(x => x == IdUserConnect) != null)
            {
                ListIdGroup.Add(item.Key);
            }
        }
        return ListIdGroup;
    }

    public void CreatePrivateGroupHub(FriendShip outFriendShip, out string NameGroup)
    {
        GroupHub groupHub = new GroupHub();

        var list = ChatHub.ConnectedUsers.Where(x => x.Value == outFriendShip.IdUser1 || x.Value == outFriendShip.IdUser2).ToDictionary(p => p.Key, p => p.Value);

        foreach (var IDUserConnect in list)
        {
            groupHub.ltIdUserConnect.Add(IDUserConnect.Key);
        }

        NameGroup = "private " + outFriendShip.ObjectGuid.ToString();

        listGroupHub.Add(NameGroup, groupHub);
    }
    public void CreatePublicGroupHub(Guid ObjectGuid, out string NameGroup)
    {
        GroupHub groupHub = new GroupHub();
        NameGroup = "";

        string msg = UserAccount.GetListByGuidServer(ObjectGuid,out List<UserAccount> outUserAccount);
        if (msg.Length > 0) return;

        var list = ChatHub.ConnectedUsers.Where(x => outUserAccount.Exists(y => y.Id == x.Value));

        foreach (var IDUserConnect in list)
        {
            groupHub.ltIdUserConnect.Add(IDUserConnect.Key);
        }

        NameGroup = "pubic " + ObjectGuid.ToString();

        listGroupHub.Add(NameGroup, groupHub);
    }
    public GroupHub GetGroupHub(string GroupID,bool isPrivate)
    {
        listGroupHub.TryGetValue((isPrivate ? "private " : "public ") + GroupID, out GroupHub outGroupHub);

        return outGroupHub;
    }

    public bool CheckExistIDConnectInGroup(string NameGroup, string IdConnect)
    {
        bool isFind = listGroupHub.TryGetValue(NameGroup, out GroupHub groupHub);
        if (isFind)
        {
            foreach (var item in groupHub.ltIdUserConnect)
            {
                if (item == IdConnect) return true;
            }
        }
        return false;
    }

    public bool CheckExistGroup(string NameGroup, out GroupHub groupHub)
    {
        return listGroupHub.TryGetValue(NameGroup, out groupHub);
    }
    public List<KeyValuePair<string, string>> AddUserIntoGroup(long IdUser, string IDUserConnect, Dictionary<string, long> listUserConnect)
    {
        try
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

            UserAccount.GetListFriendById(IdUser, out List<UserAccount> outListFriend);
            foreach (var item in outListFriend)
            {
                var friend = listUserConnect.FirstOrDefault(x => x.Value == item.Id);
                if (!friend.Equals(default(KeyValuePair<string, long>)))
                {
                    FriendShip.GetOne(IdUser, friend.Value, out FriendShip outFriendShip);

                    GroupHub groupHub = new GroupHub();
                    groupHub.ltIdUserConnect.Add(friend.Key);
                    groupHub.ltIdUserConnect.Add(IDUserConnect);

                    list.Add(new KeyValuePair<string, string>("private" + outFriendShip.ObjectGuid.ToString(), friend.Key));
                    list.Add(new KeyValuePair<string, string>("private" + outFriendShip.ObjectGuid.ToString(), IDUserConnect));

                    listGroupHub.Add("private" + outFriendShip.ObjectGuid.ToString(), groupHub);
                }
            }
            /*
                            Server.GetListByIdUser(IdUser, out List<Server> outListServer);
                            foreach(var item in outListServer)
                            {
                                string keyServer = item.ObjectGuid.ToString();
                                if (listGroupHub[keyServer] != null)
                                {
                                    listGroupHub[keyServer].ltIdUserConnect.Add(IDUserConnect);
                                }
                                else
                                {
                                    GroupHub groupHub = new GroupHub();
                                    groupHub.ltIdUserConnect.Add(IDUserConnect);

                                    list.Add(keyServer);
                                    listGroupHub.Add(keyServer, groupHub);
                                }
                            }*/

            return list;
        }
        catch
        {

        }
        return null;
    }
    public void Disconnect(string IDUserConnect)
    {
        try
        {
            foreach (var item in listGroupHub.Values)
            {
                item.ltIdUserConnect.Remove(IDUserConnect);
            }
        }
        catch
        {

        }
    }
}
public class GroupHub
{
    public GroupHub()
    {
        ltIdUserConnect = new List<string>();
    }
    public string IdGroup { get; set; }
    public List<string> ltIdUserConnect;
}
