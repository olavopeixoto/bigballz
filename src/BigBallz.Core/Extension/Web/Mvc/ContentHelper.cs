using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using MvcContrib.UI.Html;
using ReadZ.AssetManagement.HtmlHelpers;

namespace BigBallz.Core.Extension.Web.Mvc
{
    public static class ContentHelper
    {
        private static readonly string DefaultThemeName = ConfigurationManager.AppSettings["DefaultThemeName"] ?? "steel";
        private static readonly string DefaultCssPathFormat = ConfigurationManager.AppSettings["DefaultCssPathFormat"] ?? "~/Content/themes/{0}/css/{1}";
        private static readonly string DefaultJsPathFormat = ConfigurationManager.AppSettings["DefaultJsPathFormat"] ?? "~/Content/js/{0}";

        private static string GetFileVersion(this HtmlHelper htmlHelper, string virtualFilePath)
        {
            var context = htmlHelper.ViewContext.HttpContext;
            var cacheKey = "fileVersion##" + virtualFilePath;
            var result = context.Cache[cacheKey] as string;
            if (result == null)
            {
                var filePath = context.Server.MapPath(virtualFilePath);
                var fileInfo = new FileInfo(filePath);
                result = fileInfo.LastWriteTime.Ticks.ToString();

                context.Cache.Add(cacheKey, result, new System.Web.Caching.CacheDependency(filePath),
                                  Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            return result;
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{DefaultThemeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string Css(this HtmlHelper htmlHelper, string cssFilePath)
        {
            return CssTheme(htmlHelper, DefaultThemeName, cssFilePath);
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{DefaultThemeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string Css(this HtmlHelper htmlHelper, params string[] cssFilePath)
        {
            var sb = new StringBuilder();
            foreach (var css in cssFilePath)
            {
                sb.AppendLine(CssTheme(htmlHelper, DefaultThemeName, css));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{DefaultThemeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string CssPrint(this HtmlHelper htmlHelper, string cssFilePath)
        {
            return CssTheme(htmlHelper, DefaultThemeName, cssFilePath, false, false, "print");
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{DefaultThemeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string CssPrint(this HtmlHelper htmlHelper, params string[] cssFilePath)
        {
            var sb = new StringBuilder();
            foreach (var css in cssFilePath)
            {
                sb.AppendLine(CssTheme(htmlHelper, DefaultThemeName, css, false, false, "print"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{DefaultThemeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string CssStrict(this HtmlHelper htmlHelper, params string[] cssFilePath)
        {
            var sb = new StringBuilder();
            foreach (var css in cssFilePath)
            {
                sb.AppendLine(CssTheme(htmlHelper, DefaultThemeName, css, true));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{DefaultThemeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string CssPrintStrict(this HtmlHelper htmlHelper, params string[] cssFilePath)
        {
            var sb = new StringBuilder();
            foreach (var css in cssFilePath)
            {
                sb.AppendLine(CssTheme(htmlHelper, DefaultThemeName, css, true, false, "print"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{themeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="themeName"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string CssTheme(this HtmlHelper htmlHelper, string themeName, string cssFilePath)
        {
            return CssTheme(htmlHelper, themeName, cssFilePath, false);
        }

        public static string CssTheme(this HtmlHelper htmlHelper, string themeName, string cssFilePath, bool xhtml)
        {
            return CssTheme(htmlHelper, themeName, cssFilePath, xhtml, false, "screen, projection");
        }

        /// <summary>
        /// Renders a link tag referencing the cascade stylesheet.  Assumes the CSS is in the /content/themes/{themeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="themeName"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <param name="xhtml">indicates if should close the link tag</param>
        /// <returns></returns>
        public static string CssTheme(this HtmlHelper htmlHelper, string themeName, string cssFilePath, bool xhtml, bool alternate, string media)
        {
            cssFilePath = cssFilePath.EndsWith(".css") ? cssFilePath : cssFilePath + ".css";

            var virtualCssFilePath = cssFilePath.StartsWith("~") ? string.Format(cssFilePath, themeName) : string.Format(DefaultCssPathFormat, themeName, cssFilePath);
            var version = htmlHelper.GetFileVersion(virtualCssFilePath);

            var builder = new TagBuilder("link");
            builder.Attributes["type"] = "text/css";
            builder.Attributes["rel"] = alternate ? "alternate stylesheet" : "stylesheet";
            builder.Attributes["media"] = media;
            builder.Attributes["title"] = themeName;
            builder.Attributes["href"] = htmlHelper.ResolveUrl(string.Format("{0}?v={1}", virtualCssFilePath, version));

            return builder.ToString(xhtml ? TagRenderMode.SelfClosing : TagRenderMode.StartTag);
        }

        /// <summary>
        /// Renders a link tag referencing an alternate cascade stylesheet.
        /// Alternate Stylesheets are used as an alternative theme to the mains style.
        /// Usually requires browser support to change it but it can be done by JavaScript.
        /// Assumes the alternate CSS is in the /content/themes/{DefaultThemeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="cssFilePath">css fileName or full relative url</param>
        /// <returns></returns>
        public static string AlternateCss(this HtmlHelper htmlHelper, string cssFilePath)
        {
            return AlternateCss(htmlHelper, DefaultThemeName, cssFilePath);
        }

        public static string AlternateCss(this HtmlHelper htmlHelper, string themeName, string cssFilePath)
        {
            return AlternateCss(htmlHelper, themeName, cssFilePath, "screen, projection");
        }

        /// <summary>
        /// Renders a link tag referencing an alternate cascade stylesheet.
        /// Alternate Stylesheets are used as an alternative theme to the mains style.
        /// Usually requires browser support to change it but it can be done by JavaScript.
        /// Assumes the alternate CSS is in the /content/themes/{themeName}/css directory.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="themeName"></param>
        /// <param name="cssFilePath">css fileName</param>
        /// <param name="media">media type: "screen, projection, print, etc"</param>
        /// <returns></returns>
        public static string AlternateCss(this HtmlHelper htmlHelper, string themeName, string cssFilePath, string media)
        {
            return CssTheme(htmlHelper, themeName, cssFilePath, false, true, media);
        }

        public static string Favicon(this HtmlHelper htmlHelper, bool xhtml)
        {
            return xhtml ? htmlHelper.Favicon() : htmlHelper.Favicon().Replace("/>", ">");
        }

        public static string ScriptInclude(this HtmlHelper htmlHelper, params string[] jsPaths)
        {
            var sb = new StringBuilder();
            foreach (var jsPath in jsPaths)
            {
                var path = jsPath.StartsWith("http") || jsPath.StartsWith("~")
                    ? jsPath
                    : string.Format(DefaultJsPathFormat, jsPath);

                var version = path.StartsWith("http")
                    ? string.Empty
                    : "?v=" + htmlHelper.GetFileVersion(path);

                sb.AppendLine(MvcContrib.UI.Html.HtmlHelperExtensions.ScriptInclude(htmlHelper, string.Format("{0}{1}", path, version)));
            }
            return sb.ToString();
        }

        /// <summary>
        /// jQuery Script Manager:
        /// O gerenciador de scripts jquery permite que sejam incluidos scripts em qualquer ordem de hierarquia, tanto na master quanto na view quanto nos controles e eles serão sempre incluidos na ordem correta no final do arquivo.
        /// Para isso na sua master deve ter uma unica chamada no final do arquivo imediatamente antes de fechar a tag </BODY> do tipo:
        /// <%=Html.DumpRegisteredScripts()%>
        /// E nos outros pontos onde deseja registrar os seus scripts chamadas do tipo:
        /// <%Html.RegisterScript("CAMINHO RELATIVO A content/js DO SCRIPT JS OU CAMINHO NO FORMATO ~/PATH/SCRIPT.JS");%>
        /// ou do tipo:
        /// <%Html.RegisterAsset("NOME DO ASSET CONFIGURADO NO WEB.CONFIG");%>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="jsPaths"></param>
        /// <returns></returns>
        public static string RegisterScript(this HtmlHelper helper, params string[] jsPaths)
        {
            return RegisterScript(helper, true, jsPaths);
        }

        /// <summary>
        /// jQuery Script Manager:
        /// O gerenciador de scripts jquery permite que sejam incluidos scripts em qualquer ordem de hierarquia, tanto na master quanto na view quanto nos controles e eles serão sempre incluidos na ordem correta no final do arquivo.
        /// Para isso na sua master deve ter uma unica chamada no final do arquivo imediatamente antes de fechar a tag </BODY> do tipo:
        /// <%=Html.DumpRegisteredScripts()%>
        /// E nos outros pontos onde deseja registrar os seus scripts chamadas do tipo:
        /// <%Html.RegisterScript("CAMINHO RELATIVO A content/js DO SCRIPT JS OU CAMINHO NO FORMATO ~/PATH/SCRIPT.JS");%>
        /// ou do tipo:
        /// <%Html.RegisterAsset("NOME DO ASSET CONFIGURADO NO WEB.CONFIG");%>
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ajaxFallback"></param>
        /// <param name="jsPaths"></param>
        /// <returns></returns>
        public static string RegisterScript(this HtmlHelper helper, bool ajaxFallback, params string[] jsPaths)
        {
            if (ajaxFallback && helper.ViewContext.HttpContext.Request.IsAjaxRequest())
            {
                return helper.ScriptInclude(jsPaths);
            }

            var scriptManager = new jQueryScriptHtmlHelper(helper);

            foreach (var jsPath in jsPaths)
            {
                if (jsPath.StartsWith("http://") || jsPath.StartsWith("https://"))
                {
                    scriptManager.Scripts(script => script.Source(jsPath));
                }
                else if (jsPath.StartsWith("~"))
                {
                    var version = helper.GetFileVersion(jsPath);
                    scriptManager.Scripts(script => script.Source(string.Format("{0}?v={1}", jsPath, version)));
                }
                else
                {
                    var virtualJsFilePath = string.Format(DefaultJsPathFormat, jsPath);
                    var version = helper.GetFileVersion(virtualJsFilePath);
                    scriptManager.Scripts(script => script.Source(string.Format("{0}?v={1}", virtualJsFilePath, version)));
                }
            }

            return string.Empty;
        }

        public static void RegisterAsset(this HtmlHelper helper, params string[] assetNames)
        {
            var scriptManager = new jQueryScriptHtmlHelper(helper);

            foreach (var assetName in assetNames)
            {
                scriptManager.Scripts(script => script.Asset(assetName));
            }
        }

        public static string RegisterScriptAndDumpRegisteredScripts(this HtmlHelper helper, params string[] jsPaths)
        {
            helper.RegisterScript(jsPaths);
            return helper.DumpRegisteredScripts();
        }

        public static string RegisterAssetAndDumpRegisteredScripts(this HtmlHelper helper, params string[] assetNames)
        {
            helper.RegisterAsset(assetNames);
            return helper.DumpRegisteredScripts();
        }

        public static string DumpRegisteredScripts(this HtmlHelper helper)
        {
            var scriptManager = new jQueryScriptHtmlHelper(helper);
            return new jQueryScriptHtmlHelper(helper).ToString();
        }

        public static void RegisterOnPageLoadScript(this HtmlHelper helper, string script)
        {
            var scriptManager = new jQueryScriptHtmlHelper(helper);
            scriptManager.OnPageLoad(script);
        }

        public static void RegisterOnPageUnloadScript(this HtmlHelper helper, string script)
        {
            var scriptManager = new jQueryScriptHtmlHelper(helper);
            scriptManager.OnPageUnload(script);
        }

        public static string GoogleAnalytics(this HtmlHelper helper)
        {
            var googleAnalyticsId = ConfigurationManager.AppSettings["GoogleAnalyticsId"];
            if (string.IsNullOrEmpty(googleAnalyticsId)) return string.Empty;
            return GoogleAnalytics(helper, googleAnalyticsId);
        }

        public static string GoogleAnalytics(this HtmlHelper helper, string googleAnalyticsId)
        {
            var builder = new TagBuilder("script");
            builder.Attributes["type"] = "text/javascript";            
            var sb = new StringBuilder("var _gaq = _gaq || [];");
            sb.AppendLine(string.Format("_gaq.push(['_setAccount', '{0}']);", googleAnalyticsId));
            sb.AppendLine("_gaq.push(['_trackPageview']);");
            sb.AppendLine("(function() {");
            sb.AppendLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
            sb.AppendLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
            sb.AppendLine("(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(ga);");
            sb.AppendLine("})();");
            
            builder.SetInnerText(sb.ToString());
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}