using System;
using System.Web;
using System.IO.Compression;

namespace BigBallz.Core.Web.Modules
{
    public sealed class HttpCompressionModule : IHttpModule
    {
        #region IHttpModule Members

        void IHttpModule.Dispose()
        {}

        void IHttpModule.Init(HttpApplication context)
        {
            context.PostAcquireRequestState += ContextPostAcquireRequestState;
            context.EndRequest += ContextEndRequest;
        }

        void ContextEndRequest(object sender, EventArgs e)
        {
            var context = sender as HttpApplication;
            context.PostAcquireRequestState -= ContextPostAcquireRequestState;
            context.EndRequest -= ContextEndRequest;
        }

        void ContextPostAcquireRequestState(object sender, EventArgs e)
        {
            this.RegisterCompressFilter();    
        }

        private void RegisterCompressFilter()
        {
            var context = HttpContext.Current;

            if (context.Handler is StaticFileHandler 
                || context.Handler is DefaultHttpHandler) return;

            var request = context.Request;
            
            var acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding)) return;

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            var response = HttpContext.Current.Response;

            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-Encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-Encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }

        #endregion
    }
}
