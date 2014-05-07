using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;

namespace BigBallz.Core.Web.MVC.Filters
{
    //http://blog.stevensanderson.com/2008/10/15/partial-output-caching-in-aspnet-mvc/

    /// <summary>
    ///* It uses reflection to access HttpResponse’s private SwitchWriter() method. That’s how it’s able to intercept 
    ///     all the output piped to Response during subsequent filters and the action method being cached. It’s 
    ///     unfortunate that SwitchWriter() is marked private, but it is. If you don’t want to bypass the “private” 
    ///     access modifier this way, or if you can’t (e.g., because you’re not hosting in full-trust mode), then you 
    ///     can download an alternative implementation that uses a filter to capture output instead. This isn’t quite 
    ///     as straightforward, but some people will prefer/need it.
    ///* It’s hard-coded to generate cache keys that vary by all incoming action method parameters and route values, 
    ///     and not by anything else. You would have to modify the code if you needed the ability to vary cache entry 
    ///     by other context parameters (such as unrelated querystring or form values).
    ///* When generating cache keys, it assumes that the action method parameter types and route value types all have 
    ///     sensible implementations of GetHashCode(). This is fine for primitive types (strings, ints, etc.), but if 
    ///     you try to use it with a custom parameter types that have no proper implementation of GetHashCode(), it will 
    ///     pick a different cache key every time and appear not to be caching. So implement GetHashCode() properly on 
    ///     any such custom parameter types.
    ///* It doesn’t attempt to cache and replay HTTP headers, so it’s not suitable for caching action methods that issue 
    ///     redirections.
    /// </summary>
    public class ActionOutputCacheAttribute : ActionFilterAttribute
    {
        // This hack is optional; I'll explain it later in the blog post
        private static readonly MethodInfo SwitchWriterMethod = typeof(HttpResponse).GetMethod("SwitchWriter", BindingFlags.Instance | BindingFlags.NonPublic);

        public ActionOutputCacheAttribute(int cacheDuration)
        {
            _cacheDuration = cacheDuration;
        }

        private readonly int _cacheDuration;
        private TextWriter _originalWriter;
        private string _cacheKey;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _cacheKey = ComputeCacheKey(filterContext);
            var cachedOutput = (string)filterContext.HttpContext.Cache[_cacheKey];
            if (cachedOutput != null)
                filterContext.Result = new ContentResult { Content = cachedOutput };
            else
                _originalWriter = (TextWriter)SwitchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { new HtmlTextWriter(new StringWriter()) });
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (_originalWriter != null) // Must complete the caching
            {
                var cacheWriter = (HtmlTextWriter)SwitchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { _originalWriter });
                var textWritten = cacheWriter.InnerWriter.ToString();
                filterContext.HttpContext.Response.Write(textWritten);

                filterContext.HttpContext.Cache.Add(_cacheKey, textWritten, null, DateTime.Now.AddSeconds(_cacheDuration), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        private string ComputeCacheKey(ActionExecutingContext filterContext)
        {
            var keyBuilder = new StringBuilder();
            foreach (var pair in filterContext.RouteData.Values)
                keyBuilder.AppendFormat("rd{0}_{1}_", pair.Key.GetHashCode(), pair.Value.GetHashCode());
            foreach (var pair in filterContext.ActionParameters)
                keyBuilder.AppendFormat("ap{0}_{1}_", pair.Key.GetHashCode(), pair.Value.GetHashCode());
            return keyBuilder.ToString();
        }
    }
}
