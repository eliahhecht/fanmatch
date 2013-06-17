using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Security.Principal;
using System.Text;
using FanMatch;

public class HttpCustomBasicUnauthorizedResult : HttpUnauthorizedResult
{
    // the base class already assigns the 401.
    // we bring these constructors with us to allow setting status text
    public HttpCustomBasicUnauthorizedResult() : base() { }
    public HttpCustomBasicUnauthorizedResult(string statusDescription) : base(statusDescription) { }

    public override void ExecuteResult(ControllerContext context)
    {
        if (context == null) throw new ArgumentNullException("context");

        // this is really the key to bringing up the basic authentication login prompt.
        // this header is what tells the client we need basic authentication
        context.HttpContext.Response.AddHeader("WWW-Authenticate", "Basic");
        base.ExecuteResult(context);
    }
}

public class CustomBasicAuthorizeAttribute : AuthorizeAttribute
{
    bool _RequireSsl = true;
    public bool RequireSsl
    {
        get { return _RequireSsl; }
        set { _RequireSsl = value; }
    }


    private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
    {
        validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
    }

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        if (filterContext == null) throw new ArgumentNullException("filterContext");

        if (!Authenticate(filterContext.HttpContext))
        {
            // HttpCustomBasicUnauthorizedResult inherits from HttpUnauthorizedResult and does the
            // work of displaying the basic authentication prompt to the client
            filterContext.Result = new HttpCustomBasicUnauthorizedResult();
        }
        else
        {
            // AuthorizeCore is in the base class and does the work of checking if we have
            // specified users or roles when we use our attribute
            if (Authenticate(filterContext.HttpContext))
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
            else
            {
                // auth failed, display login

                // HttpCustomBasicUnauthorizedResult inherits from HttpUnauthorizedResult and does the
                // work of displaying the basic authentication prompt to the client
                filterContext.Result = new HttpCustomBasicUnauthorizedResult();
            }
        }
    }

    // from here on are private methods to do the grunt work of parsing/verifying the credentials

    private bool Authenticate(HttpContextBase context)
    {
        if (_RequireSsl && !context.Request.IsSecureConnection && !context.Request.IsLocal) return false;

        if (!context.Request.Headers.AllKeys.Contains("Authorization")) return false;

        string authHeader = context.Request.Headers["Authorization"];

        return CheckAuth(authHeader);
    }

    private bool CheckAuth(string authHeader)
    {
        var creds = ParseAuthHeader(authHeader);
        if (creds != null)
        {
            return CheckAuth(creds[0], creds[1]);
        }
       
        return false;
    }

    private string[] ParseAuthHeader(string authHeader)
    {
        // Check this is a Basic Auth header
        if (authHeader == null || authHeader.Length == 0 || !authHeader.StartsWith("Basic")) return null;

        // Pull out the Credentials with are seperated by ':' and Base64 encoded
        string base64Credentials = authHeader.Substring(6);
        string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(base64Credentials)).Split(new char[] { ':' });

        if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[0])) return null;

        // Okay this is the credentials
        return credentials;
    }

    private bool CheckAuth(string userName, string password)
    {
        return userName == "pod_together" && password == "podficplease";
    }
}