using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class Channel
{
    public long Id { get; set; }
    public Guid ObjectGuid { get; set; }
    public long IdServer { get; set; }
    public string Name { get; set; }
    public bool IsDelete { get; set; }
    public bool IsActive { get; set; }
    public DateTime Create_Date { get; set; }
    public DateTime LastUpdate { get; set; }

    static public string GetOneByObjectGuidChannelAndServer(Guid ServerGuid, Guid ChannelGuid, out Channel outChannel)
    {
        return DBM.GetOne("usp_Channel_GetOneByGuidChannelAndServer", new { ServerGuid, ChannelGuid }, out outChannel);
    }

    static public string GetList(long ServerID, out List<Channel> outLtChannel)
    {
        return DBM.GetList("usp_Channel_GetListByServerID", new { ServerID }, out outLtChannel);
    }
}
