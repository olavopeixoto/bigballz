using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BigBallz.Core.Helper;
using BigBallz.Core.Web.MVC;

namespace BigBallz.Core.Extension.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static string DefaultImagePathFormat = ConfigurationManager.AppSettings["DefaultImagePathFormat"] ?? "~/Content/themes/{0}/images/{1}";

        public static MvcHtmlString ActionConditionalLink(this HtmlHelper htmlHelper, string linkText, string action, object routeValues, object htmlAttributes, bool linkConditionTrue)
        {
            return linkConditionTrue ? htmlHelper.ActionLink(linkText, action, routeValues, htmlAttributes) : MvcHtmlString.Empty;
        }

        public static MvcHtmlString ActionConditionalLink(this HtmlHelper htmlHelper, string linkText, string action, string controller, object routeValues, object htmlAttributes, bool linkConditionTrue)
        {
            return linkConditionTrue ? htmlHelper.ActionLink(linkText, action, controller, routeValues, htmlAttributes) : MvcHtmlString.Empty;
        }

        public static MvcHtmlString ActionLinkOrSpanSelectedByController<TController>(this HtmlHelper htmlHelper, string linkText, string action, string controller, object htmlAttributes, bool linkConditionTrue)
        {
            var htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);
            var controllerName = typeof(TController).Name;

            var selected = controllerName.ToLowerInvariant() ==
                           htmlHelper.ViewContext.RouteData.GetRequiredString("controller").ToLowerInvariant() + "controller";
            if (selected)
                if (htmlAttributesDictionary.ContainsKey("class"))
                    htmlAttributesDictionary["class"] += " selected";
                else
                    htmlAttributesDictionary["class"] = "selected";

            if (linkConditionTrue && !selected)
            {
                return htmlHelper.ActionLink(action, linkText, htmlAttributes);
            }

            var builder = new TagBuilder("span") { InnerHtml = htmlHelper.Encode(linkText) };
            builder.MergeAttributes(htmlAttributesDictionary);
            if (!selected) builder.AddCssClass("disabled");
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ActionLinkOrSpan<TController>(this HtmlHelper htmlHelper, string linkText, string action, string controller, object htmlAttributes, bool linkConditionTrue)
        {
            var htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);

            var selected = action.ToLowerInvariant() ==
                           htmlHelper.ViewContext.RouteData.GetRequiredString("action").ToLowerInvariant() &&
                           controller.ToLowerInvariant() ==
                           htmlHelper.ViewContext.RouteData.GetRequiredString("controller").ToLowerInvariant() +
                           "controller";
            if (selected)
                if (htmlAttributesDictionary.ContainsKey("class"))
                {
                    if (((string) htmlAttributesDictionary["class"]).IndexOf("selected") < 0)
                        htmlAttributesDictionary["class"] += " selected";
                }
                else
                    htmlAttributesDictionary["class"] = "selected";

            if (linkConditionTrue && !selected)
            {
                return htmlHelper.ActionLink(action, linkText, htmlAttributes);
            }

            var builder = new TagBuilder("span") { InnerHtml = htmlHelper.Encode(linkText) };
            builder.MergeAttributes(htmlAttributesDictionary);
            if (!selected) builder.AddCssClass("disabled");
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }


        public static MvcHtmlString ActionLinkOrSpan(this HtmlHelper htmlHelper, string linkText, string controllerName, object values, object htmlAttributes, bool linkConditionTrue)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentNullException("controllerName");
            }

            var htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);
            var valuesDictionary = new RouteValueDictionary(values);

            var selected = controllerName.ToLowerInvariant() ==
                           htmlHelper.ViewContext.RouteData.GetRequiredString("controller").ToLowerInvariant();
            if (selected)
                if (htmlAttributesDictionary.ContainsKey("class"))
                    htmlAttributesDictionary["class"] += " selected";
                else
                    htmlAttributesDictionary["class"] = "selected";

            if (linkConditionTrue && !selected)
                return htmlHelper.ActionLink(linkText, "Index", controllerName, valuesDictionary, htmlAttributesDictionary);

            var builder = new TagBuilder("span") { InnerHtml = htmlHelper.Encode(linkText) };
            builder.MergeAttributes(htmlAttributesDictionary);
            if (!selected) builder.AddCssClass("disabled");
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ActionLinkOrSpan(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object values, object htmlAttributes, bool linkConditionTrue)
        {
            Check.Argument.IsNotEmpty(actionName, "actionName");
            Check.Argument.IsNotEmpty(controllerName, "controllerName");

            var htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);
            var valuesDictionary = new RouteValueDictionary(values);

            var selected = actionName.ToLowerInvariant() ==
                           htmlHelper.ViewContext.RouteData.GetRequiredString("action").ToLowerInvariant()
                           &&
                           controllerName.ToLowerInvariant() ==
                           htmlHelper.ViewContext.RouteData.GetRequiredString("controller").ToLowerInvariant();

            if (selected)
                if (htmlAttributesDictionary.ContainsKey("class"))
                    htmlAttributesDictionary["class"] += " selected";
                else
                    htmlAttributesDictionary["class"] = "selected";

            if (linkConditionTrue && !selected)
                return htmlHelper.ActionLink(linkText, actionName, controllerName, valuesDictionary, htmlAttributesDictionary);

            var builder = new TagBuilder("span") { InnerHtml = htmlHelper.Encode(linkText) };
            builder.MergeAttributes(htmlAttributesDictionary);
            if (!selected) builder.AddCssClass("disabled");
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString SameActionLink(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            return SameActionLink(htmlHelper, linkText, routeValues, null);
        }

        public static MvcHtmlString SameActionLink(this HtmlHelper htmlHelper, string linkText)
        {
            return SameActionLink(htmlHelper, linkText, null, null);
        }

        public static MvcHtmlString SameActionLink(this HtmlHelper htmlHelper, string linkText, object routeValues, object htmlAttributes)
        {
            var actionName = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var newRouteValues = new RouteValueDictionary(routeValues);
            var qs = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            foreach (string qsKey in qs.Keys)
            {
                if (!newRouteValues.ContainsKey(qsKey))
                    newRouteValues.Add(qsKey, qs[qsKey]);
            }
            return htmlHelper.ActionLink(linkText, actionName, newRouteValues, new RouteValueDictionary(htmlAttributes));
        }

        public static void RenderPartialRequest(this HtmlHelper html, string controller, string action, object routeValues)
        {
            var routeValuesDictionary = new RouteValueDictionary(routeValues);
            routeValuesDictionary["action"] = action;
            routeValuesDictionary["controller"] = controller;
            var partial = new PartialRequest(routeValuesDictionary);
            partial.Invoke(html.ViewContext);
        }

        public static void RenderPartialRequest(this HtmlHelper html, string action, object routeValues)
        {
            var routeValuesDictionary = new RouteValueDictionary(routeValues);
            routeValuesDictionary["action"] = action;
            routeValuesDictionary["controller"] = html.ViewContext.RouteData.GetRequiredString("controller");
            var partial = new PartialRequest(routeValuesDictionary);
            partial.Invoke(html.ViewContext);
        }

        public static void RenderPartialRequest(this HtmlHelper html, string controller, string action)
        {
            var partial = new PartialRequest(new { controller, action });
            partial.Invoke(html.ViewContext);
        }

        public static void RenderPartialRequest(this HtmlHelper html, string action)
        {
            var partial = new PartialRequest(new { controller = html.ViewContext.RouteData.GetRequiredString("controller"), action });
            partial.Invoke(html.ViewContext);
        }
    }
}