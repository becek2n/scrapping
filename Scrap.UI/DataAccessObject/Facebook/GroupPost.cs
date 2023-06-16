//using Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Scrap.UI.DataAccessObject.Facebook
{
    public class GroupPost
    {
        private const string AuthenticationUrlFormat =
            "https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials&scope=manage_pages,offline_access,publish_stream";

        //static string GetAccessToken(string apiId, string apiSecret)
        //{
        //    string accessToken = string.Empty;
        //    string url = string.Format(AuthenticationUrlFormat, apiId, apiSecret);

        //    WebRequest request = WebRequest.Create(url);
        //    WebResponse response = request.GetResponse();

        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
        //        String responseString = reader.ReadToEnd();

        //        var responseFacebook = JsonConvert.DeserializeObject<FacebookAuthResponse>(responseString);

        //        accessToken = responseFacebook.access_token;
        //    }

        //    if (accessToken.Trim().Length == 0)
        //        throw new Exception("There is no Access Token");

        //    return accessToken;
        //}
        //public void Post(){
        //    string appId = "1141021233198939",
        //            appSecret = "7be8a4680482b67829b2b2e35989b579",
        //            pageId = "415834792649867";
        //    string accessToken = GetAccessToken(appId, appSecret);

        //    dynamic messagePost = new ExpandoObject();
        //    messagePost.access_token = accessToken;
        //    messagePost.picture = "http://www.c-sharpcorner.com/UploadFile/AuthorImage/raj1979.gif";
        //    messagePost.link = "https://www.infolooker.id";
        //    messagePost.name = "Here is post name";
        //    messagePost.caption = "Djanuar " + "Here is message"; //<---{*actor*} is the user (i.e.: Aaron)
        //    messagePost.description = "here is description";

        //    FacebookClient app = new FacebookClient(accessToken);

        //    try
        //    {
        //        var result = app.Post("/" + pageId + "/feed", messagePost);
        //    }
        //    catch (FacebookOAuthException ex)
        //    {
        //        //handle something
        //    }
        //    catch (FacebookApiException ex)
        //    {
        //        //handle something else
        //    }
        //}
    }


    public class FacebookAuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}