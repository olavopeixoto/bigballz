using System.Configuration;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BigBallz.Core;
using BigBallz.Helpers;
using BigBallz.Models;
using MvcContrib.UI.Html;
using StackExchange.Profiling;

namespace System.Web.Mvc {

    public static class HtmlHelpers {

        static readonly string PubDir = string.IsNullOrEmpty(Configuration.WebConfigurationManager.AppSettings["PubDir"]) ? "public" : Configuration.WebConfigurationManager.AppSettings["PubDir"];
        const string CssDir="css";
        const string ImageDir="images";
        const string ScriptDir="js";

        public static string DatePickerEnable(this HtmlHelper helper) {
            var sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript'>$(document).ready(function() {$('.date-selector').datepicker();});</script>\n");
            return sb.ToString();
        }

        public static string Friendly(this HtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.Request.Cookies["friendly"] != null ? helper.h(helper.ViewContext.HttpContext.Request.Cookies["friendly"].Value) : "";
        }

        public static string Script(this HtmlHelper helper, string fileName) {
            if (!fileName.EndsWith(".js"))
                fileName += ".js";
            var jsPath = string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>\n", PubDir.StartsWith("http") ? string.Format("{0}/{1}/{2}", PubDir, ScriptDir, helper.AttributeEncode(fileName)) : helper.ResolveUrl(string.Format("~/{0}/{1}/{2}", PubDir, ScriptDir, helper.AttributeEncode(fileName))));
            return jsPath;
        }
        public static string CSS(this HtmlHelper helper, string fileName) {
            return CSS(helper, fileName, "screen");
        }
        public static string CSS(this HtmlHelper helper, string fileName, string media) {
            if (!fileName.EndsWith(".css"))
                fileName += ".css";
            var jsPath = string.Format("<link rel='stylesheet' type='text/css' href='{0}'  media='" + media + "'/>\n", PubDir.StartsWith("http") ?  string.Format("{0}/{1}/{2}", PubDir, CssDir, helper.AttributeEncode(fileName)) : helper.ResolveUrl(string.Format("~/{0}/{1}/{2}", PubDir, CssDir, helper.AttributeEncode(fileName))));
            return jsPath;
        }
        public static string Image(this HtmlHelper helper, string fileName) {
            return Image(helper, fileName, null);
        }
        public static string Image(this HtmlHelper helper, string fileName, object htmlAttributes) {
            fileName = string.Format("{0}/{1}/{2}", PubDir, ImageDir, fileName);
            var builder = new TagBuilder("img");
            builder.Attributes["src"] = helper.AttributeEncode(fileName);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }

        public static MvcHtmlString ActionActiveLink(this HtmlHelper helper, string text, string action, string controller, object routeValues, object htmlAttributes)
        {
            var htmlAttr = new RouteValueDictionary(htmlAttributes);
            if (action.ToLowerInvariant() == helper.ViewContext.RouteData.GetRequiredString("action").ToLowerInvariant() && controller.ToLowerInvariant() == helper.ViewContext.RouteData.GetRequiredString("controller").ToLowerInvariant())
            {
                htmlAttr["class"] = htmlAttr["class"] + " active";
            }
            return helper.ActionLink(text, action, controller, new RouteValueDictionary(routeValues), htmlAttr);
        }

        public static string TeamFlag(this HtmlHelper helper, string teamId, string toolTip = null)
        {
            //return helper.Image("teams/" + teamId + ".gif", new { alt = toolTip});
            return string.Format("<img src=\"{0}\"  width=\"19\" height=\"13\" title=\"{1}\" />", new UrlHelper(helper.ViewContext.RequestContext).Content(string.Format("~/public/images/flags/{0}.png", teamId)), toolTip);
        }

        public static string MatchReminder(this HtmlHelper helper, int matchId, DateTime matchStartTime, int pointsEarned)
        {
            var datetimeLeft = matchStartTime.Subtract(new TimeSpan(0, 1, 0, 0)).Subtract(DateTime.Now.BrazilTimeZone());
            var hoursLeft = datetimeLeft.TotalHours;
            var daysLeft = datetimeLeft.TotalDays;
            var minutesLeft = datetimeLeft.TotalMinutes;
            var secondsLeft = datetimeLeft.TotalSeconds;
            var unitName = hoursLeft > 24 ? "dia" : minutesLeft > 60 ? "hora" : secondsLeft > 60 ? "minuto" : "segundo";
            var unit = (int)(hoursLeft > 24 ? daysLeft : minutesLeft > 60 ? hoursLeft : secondsLeft > 60 ? minutesLeft : secondsLeft);
            var plural = unit > 1;
            return secondsLeft < 0 ? string.Format("<div class=\"match-reminder bet-times-up\">{0} ponto{1}<span class=\"reminder-explanation\">{2}</span></div>", pointsEarned, pointsEarned == 1 ? "" : "s", helper.ActionLink("ver demais apostas", "matchbets", "bet", new {id=matchId}, null)) : string.Format("<div class=\"match-reminder\"><span class=\"ui-icon ui-icon-clock\" style=\"float: left;margin-right:.3em;\"></span>Falta{0} {1} {2}{3}<span class=\"reminder-explanation\">para encerrar a aposta</span></div>", plural ? "m" : "", unit, unitName, plural ? "s" : "");
        }

        public static string CountDown(this HtmlHelper helper, DateTime startTime)
        {
            var datetimeLeft = startTime.Subtract(new TimeSpan(0, 1, 0, 0)).Subtract(DateTime.Now.BrazilTimeZone());
            var hoursLeft = datetimeLeft.TotalHours;
            var daysLeft = datetimeLeft.TotalDays;
            var minutesLeft = datetimeLeft.TotalMinutes;
            var secondsLeft = datetimeLeft.TotalSeconds;
            var unitName = hoursLeft > 24 ? "dia" : minutesLeft > 60 ? "hora" : secondsLeft > 60 ? "minuto" : "segundo";
            var unit = (int)(hoursLeft > 24 ? daysLeft : minutesLeft > 60 ? hoursLeft : secondsLeft > 60 ? minutesLeft : secondsLeft);
            var plural = unit > 1;
            return secondsLeft > 0 ? string.Format("<h4>Falta{0} {1} {2}{3}</h4>", plural ? "m" : "", unit, unitName, plural ? "s" : "") : string.Empty;
        }

        public static string BonusReminder(this HtmlHelper helper, DateTime bonusStartTime, int pointsEarned)
        {
            var datetimeLeft = bonusStartTime.Subtract(new TimeSpan(0, 1, 0, 0)).Subtract(DateTime.Now.BrazilTimeZone());
            var hoursLeft = datetimeLeft.TotalHours;
            var daysLeft = datetimeLeft.TotalDays;
            var minutesLeft = datetimeLeft.TotalMinutes;
            var secondsLeft = datetimeLeft.TotalSeconds;
            var unitName = hoursLeft > 24 ? "dia" : minutesLeft > 60 ? "hora" : secondsLeft > 60 ? "minuto" : "segundo";
            var unit = (int)(hoursLeft > 24 ? daysLeft : minutesLeft > 60 ? hoursLeft : secondsLeft > 60 ? minutesLeft : secondsLeft);
            var plural = unit > 1;
            return secondsLeft < 0 ? string.Format("<div class=\"match-reminder bet-times-up\">{0} ponto{1}<span class=\"reminder-explanation\">Aposta Encerrada</span></div>", pointsEarned, pointsEarned == 1 ? "" : "s") : string.Format("<div class=\"match-reminder\"><span class=\"ui-icon ui-icon-clock\" style=\"float: left;margin-right:.3em;\"></span>Falta{0} {1} {2}{3}<span class=\"reminder-explanation\">para encerrar a aposta</span></div>", plural ? "m" : "", unit, unitName, plural ? "s" : "");
        }

        public static string GetUserPhoto(this HtmlHelper htmlHelper, User user)
        {
            string photoUrl;

            if (!string.IsNullOrEmpty(user.PhotoUrl))
                photoUrl = user.PhotoUrl;
            else
            {
                var gravatar = new GravatarHelper {email = user.EmailAddress};
                photoUrl = gravatar.GetGravatarUrl();
            }

            return "<img style=\"margin:5px 0 0;padding:0;\" width=\"50\" height=\"50\" src=" + photoUrl.Replace("https:", "").Replace("http:", "") +
                   " alt=\"\" />";
        }

        public static decimal Price(this HtmlHelper helper)
        {
            return ConfigurationHelper.Price;
        }

        public static IHtmlString MiniProfiler(this HtmlHelper helper)
        {
            return StackExchange.Profiling.MiniProfiler.RenderIncludes(RenderPosition.Left, showTrivial: false, showTimeWithChildren: false, useExistingjQuery: true, showControls: true);
        }
    }
}