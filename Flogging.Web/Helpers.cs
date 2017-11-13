namespace Flogging.Web
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Security.Claims;

    public static class Helpers
    {
        //private IHttpContextAccessor _request;
        //public Helpers(IHttpContextAccessor httpContextAccessor)
        //{
        //    this._request = httpContextAccessor;
        //}

        public static Dictionary<string, object> GetWebFloggingData (
            HttpRequest request,
            out string userId,
            out string userName,
            out string location )
        {
            var data = new Dictionary<string, object>();
            GetRequestData(request, data, out location);
            GetuserData(data, out userId, out userName);
            GetSessionData(request.HttpContext, data);
            // get cookies?

            return data;
        }


        private static void GetRequestData (
            HttpRequest httpRequest,
            Dictionary<string, object> data,
            out string location )
        {
            location = "";

            var request = httpRequest;
            if (request != null)
            {
                location = request.Path;
                string type, version;
                // MS Edge Require special detection logic
                GetBrowserInfo(request, out type, out version);
                data.Add("Browser", $"{type}{version}");
                data.Add("UserAgent", $"{request.Headers["User-Agent"]}");
                //data.Add("Languages", $"{request.UserLanguages}");
                var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(request.QueryString.Value);
                foreach (var qsKey in queryDictionary)
                {
                    data.Add($"QueryString-{qsKey}", queryDictionary[qsKey.ToString()]);
                }
            }
        }

        private static void GetBrowserInfo(
             HttpRequest httpRequest,
             out string type,
             out string version)
        {
            var userAgent = "";
            if (string.IsNullOrEmpty(userAgent))
            {
                userAgent = httpRequest.Headers["User-Agent"];
            }

            UserAgent.UserAgent ua = new UserAgent.UserAgent(userAgent);

            type = ua.Browser.Name;
            version = string.Format(" (v {0} )", ua.Browser.Version);

            //if (type.StartsWith("Chrome") && request..UserAgent.Contains("Edge/"))
            //{
            //    type = "Edge";
            //    version = string.Format(" (v {0} )", request.UserAgent.Substring(request.UserAgent.IndexOf("Edge/"), +5));
            //}
            //else
            //{
            //    version = string.Format(" (v {0} )", request.Browser.MajorVersion + "." + request.Browser.MinorVersion);
            //}
        }

        private static void GetuserData(
            Dictionary<string, object> data,
            out string userId,
            out string userName
            )
        {
            userId = "";
            userName = "";
            var user = ClaimsPrincipal.Current;
            if (user != null)
            {
                var i = 1;
                foreach (var claim in user.Claims)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                        userId = claim.Value;
                    else if (claim.Type == ClaimTypes.Name)
                        userName = claim.Value;
                    else
                        // example diction key: UserClaim-4-role
                        data.Add($"UserClaim-{i++}-{claim.Type}", claim.Value);
                }
            }
        }

        private static void GetSessionData(
            HttpContext context,
            Dictionary<string, object> data
            )
        {
            /*
                if(context.Session != null)
                {
                    foreach (var key in context.Session.Keys)
                    {
                        var keyName = key.ToString();
                        if (context.Session.Get(keyName) != null)
                        {
                            data.Add($"Session-{keyName}", context.Session.Get(keyName).ToString());
                        }
                    }
                    data.Add("SessionId", context.Session.Id);
                }
            */
        }



    }
}
