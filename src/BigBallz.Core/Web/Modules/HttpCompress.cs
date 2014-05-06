/// 
/// As a starting point for this module i used the Blowery module written by Ben Lowery.
/// his blog is located at : http://www.blowery.org/blog/
/// After studying the module that he wrote I couldn't see the point of all the inherited streams
/// because the one we use is already limited with a bunch of constraints.
/// http://www.blowery.org/code/HttpCompressionModule.html
/// The version he has for .net 2.0 is ported from .net 1.1 and the configuration properties don't follow 
/// the new schema anymore. 
/// Because of the change from #ziplib to the native .net 2.0 everything that has to do with quality 
/// doesn't count anymore.
/// I decided to rewrite his library to a .net 2.0 version of it which turned out to be much shorter.
/// So this module is copyrighted by Ivan Porto Carrero, Flanders International Marketing Ltd. 2006
/// Blog : http://www.flanders.co.nz/blog
/// You are free to use this module as long as you keep this notice in the source files
/// 
/// 23/01/2006
/// I changed the module to do work at the post release request state event instead of at begin request.
/// When fired with begin request the parameters get zipped and form values get affected which kills the correct processing of the page.
/// The compression now takes place after the entire content has been created.
/// 
/// 
using System;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace BigBallz.Core.Web.Modules
{
    /// <summary>
    /// The Http Module that will compress the outputstream to the browser if it is supported by the browser.
    /// </summary>
    public class HttpCompressModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            /* The Post Release Request State is the event most fitted for the task of adding a filter
             * Everything else is too soon or too late. At this point in the execution phase the entire 
             * response content is created and the page has fully executed but still has a few modules to go through
             * from an asp.net perspective.  We filter the content here and all of the javascript renders correctly.
             * I would like to append headers to my output but that has already been created and there is no way of appending headers
             * to the response anymore.  I can append headers before the actual zipping has been done. but that only 
             * shows that the page should be zipped and that the compression module has been installed.*/
            context.PostReleaseRequestState += new EventHandler(context_PostReleaseRequestState);
        }

        private void context_PostReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication) sender;

            // fix to handle caching appropriately
            // see http://www.pocketsoap.com/weblog/2003/07/1330.html
            // Note, this header is added only when the request
            // has the possibility of being compressed...
            // i.e. it is not added when the request is excluded from
            // compression by CompressionLevel, Path, or MimeType
            app.Context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

            string acceptedTypes = app.Request.Headers["Accept-Encoding"];

            // if we couldn't find the header, bail out
            if (acceptedTypes == null)
                return;

            // Current response stream
            Stream baseStream = app.Response.Filter;

            // If there are more than one possibility offered by the browser default to GZip
            acceptedTypes = acceptedTypes.ToLower();

            if ((acceptedTypes.Contains("gzip") || acceptedTypes.Contains("x-gzip") || acceptedTypes.Contains("*")))
            {
                app.Response.Filter = new GZipStream(baseStream, CompressionMode.Compress);
                //This won't show up in a trace log but if you use fiddler or nikhil kothari's web dev helper BHO you can see it appended
                app.Response.AppendHeader("Content-Encoding", "gzip");
            }
            else if (acceptedTypes.Contains("deflate"))
            {
                app.Response.Filter = new DeflateStream(baseStream, CompressionMode.Compress);
                //This won't show up in a trace log but if you use fiddler or nikhil kothari's web dev helper BHO you can see it appended
                app.Response.AppendHeader("Content-Encoding", "deflate");
            }
        }

        #endregion
    }
}