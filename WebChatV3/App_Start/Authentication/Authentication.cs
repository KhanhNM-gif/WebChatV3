using BSS;
using System.Web.Http;

public class Authentication : ApiController
{
    
    public Result ResultCheckToken;
    public UserToken UserToken = new UserToken();

    public Authentication()
    {
        ResultCheckToken = CacheUserToken.GetResultUserToken(out UserToken);
    }
}