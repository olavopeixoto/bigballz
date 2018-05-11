using System.Web.Mvc.Html;
using System.Web.Routing;
using BigBallz.Core;
using BigBallz.Helpers;
using BigBallz.Models;
using StackExchange.Profiling;

namespace System.Web.Mvc {

    public static class HtmlHelpers {
 
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
            return string.Format("<div class=\"match-reminder bet-times-up hide\" data-bind=\"visible: expired\">{0} ponto{1}<span class=\"reminder-explanation\">{2}</span></div>", pointsEarned, pointsEarned == 1 ? "" : "s", helper.ActionLink("ver demais apostas", "matchbets", "bet", new {id=matchId}, null)) +
                string.Format("<div class=\"match-reminder hide\" data-bind=\"visible: !expired()\"><span class=\"ui-icon ui-icon-clock\" style=\"float: left;margin-right:.3em;\"></span><span data-bind=\"text: expirationDate\"></span><span class=\"reminder-explanation\">para encerrar a aposta</span></div>");
        }

        public static string CountDown(this HtmlHelper helper, DateTime startTime)
        {
            var datetimeLeft = startTime.Subtract(new TimeSpan(0, 1, 0, 0)).Subtract(DateTime.Now.BrazilTimeZone());
            var hoursLeft = datetimeLeft.TotalHours;
            var daysLeft = Math.Round(datetimeLeft.TotalDays);
            var minutesLeft = datetimeLeft.TotalMinutes;
            var secondsLeft = datetimeLeft.TotalSeconds;
            var unitName = hoursLeft > 24 ? "dia" : minutesLeft > 60 ? "hora" : secondsLeft > 60 ? "minuto" : "segundo";
            var unit = (int)(hoursLeft > 24 ? daysLeft : minutesLeft > 60 ? hoursLeft : secondsLeft > 60 ? minutesLeft : secondsLeft);
            var plural = unit > 1;
            return secondsLeft > 0 ? string.Format("<h4>Falta{0} {1} {2}{3}</h4>", plural ? "m" : "", unit, unitName, plural ? "s" : "") : string.Empty;
        }

        public static string GetUserPhoto(this HtmlHelper htmlHelper, User user)
        {
            string photoUrl;

            if (!string.IsNullOrEmpty(user.PhotoUrl))
                photoUrl = user.PhotoUrl;
            else
            {
                var gravatar = new GravatarHelper {Email = user.EmailAddress};
                photoUrl = gravatar.GetGravatarUrl();
            }

            return "<img class=\"profile-pic\" style=\"margin:5px 0 0;padding:0;\" width=\"50\" height=\"50\" src=" + photoUrl.Replace("https:", "").Replace("http:", "") +
                   " alt=\"\" />";
        }

        public static decimal Price(this HtmlHelper helper)
        {
            return ConfigurationHelper.Price;
        }

        public static decimal PriceNet(this HtmlHelper helper)
        {
            return ConfigurationHelper.Revenue(true);
        }

        public static IHtmlString MiniProfiler(this HtmlHelper helper)
        {
            return StackExchange.Profiling.MiniProfiler.RenderIncludes(RenderPosition.Left, showTrivial: false, showTimeWithChildren: false, useExistingjQuery: true, showControls: true);
        }
    }
}