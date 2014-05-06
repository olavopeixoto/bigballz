using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BigBallz.Core.Helper;

namespace BigBallz.Core.Extension.Web.Mvc
{
    public static class InputExtentions
    {
        public static string SingleCheckBox(this HtmlHelper htmlHelper, string name)
        {
            return htmlHelper.SingleCheckBox(name, null, false, true, null);
        }

        public static string SingleCheckBox(this HtmlHelper htmlHelper, string name, string value)
        {
            return htmlHelper.SingleCheckBox(name, value, false, true, null);
        }

        public static string SingleCheckBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return htmlHelper.SingleCheckBox(name, value, false, true, htmlAttributes);
        }

        public static string SingleCheckBox(this HtmlHelper htmlHelper, string name, object value, bool inputChecked)
        {
            return htmlHelper.SingleCheckBox(name, value, inputChecked, true, null);
        }

        public static string SingleCheckBox(this HtmlHelper htmlHelper, string name, object value, bool inputChecked, object htmlAttributes)
        {
            return htmlHelper.SingleCheckBox(name, value, inputChecked, true, htmlAttributes);
        }

        public static string SingleCheckBox(this HtmlHelper htmlHelper, string name, object value, bool inputChecked, bool inputEnabled, object htmlAttributes)
        {
            var htmlAttributesDictionary = new RouteValueDictionary(htmlAttributes);

            var builder = new TagBuilder("input");
            builder.MergeAttributes(htmlAttributesDictionary);
            builder.Attributes["type"] = "checkbox";
            builder.MergeAttribute("name", htmlHelper.Encode(name));

            var stringValue = Convert.ToString(value);

            if (!string.IsNullOrEmpty(stringValue))
                builder.MergeAttribute("value", stringValue);
            if (!inputEnabled)
                builder.Attributes["disabled"] = "disabled";
            if (inputChecked)
                builder.Attributes["checked"] = "checked";
            
            return builder.ToString(TagRenderMode.SelfClosing);
        }

        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool>> action, string trueLabel, string falseLabe) where TModel : class
        {
            return BooleanRadioButton(html, model, action, trueLabel, falseLabe, null, null, null);
        }

        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool>> action, string trueLabel, string falseLabe, string objectName) where TModel : class
        {
            return BooleanRadioButton(html, model, action, trueLabel, falseLabe, null, null, objectName);
        }

        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool>> action, string trueLabel, string falseLabe, object htmlAttributesTrueRadio, object htmlAttributesFalseRadio) where TModel : class
        {
            return BooleanRadioButton(html, model, action, trueLabel, falseLabe, htmlAttributesTrueRadio, htmlAttributesFalseRadio, null);
        }

        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool>> action, string trueLabel, string falseLabe, object htmlAttributesTrueRadio, object htmlAttributesFalseRadio, string objectName) where TModel : class
        {
            var expression = (MemberExpression)action.Body;
            var name = objectName == null ? expression.Member.Name : objectName + "." + expression.Member.Name;

            var itemChecked = action.Compile().Invoke(model);

            var trueRadio = html.RadioButton(name, true, itemChecked, htmlAttributesTrueRadio) + trueLabel;
            var falseRadio = html.RadioButton(name, false, !itemChecked, htmlAttributesFalseRadio) + falseLabe;

            return trueRadio + falseRadio;
        }

        
        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool?>> action, string trueLabel, string falseLabe) where TModel : class
        {
            return BooleanRadioButton(html, model, action, trueLabel, falseLabe, null, null, null);
        }

        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool?>> action, string trueLabel, string falseLabe, string objectName) where TModel : class
        {
            return BooleanRadioButton(html, model, action, trueLabel, falseLabe, null, null, objectName);
        }

        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool?>> action, string trueLabel, string falseLabe, object htmlAttributesTrueRadio, object htmlAttributesFalseRadio) where TModel : class
        {
            return BooleanRadioButton(html, model, action, trueLabel, falseLabe, htmlAttributesTrueRadio, htmlAttributesFalseRadio, null);
        }

        public static string BooleanRadioButton<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, bool?>> action, string trueLabel, string falseLabe, object htmlAttributesTrueRadio, object htmlAttributesFalseRadio, string objectName) where TModel : class
        {
            var expression = (MemberExpression)action.Body;
            var name = objectName == null ? expression.Member.Name : objectName + "." + expression.Member.Name;

            var itemChecked = action.Compile().Invoke(model);

            var trueRadio = html.RadioButton(name, true, itemChecked.HasValue && itemChecked.Value, htmlAttributesTrueRadio) + trueLabel;
            var falseRadio = html.RadioButton(name, false, itemChecked.HasValue && !itemChecked.Value, htmlAttributesFalseRadio) + falseLabe;

            return trueRadio + falseRadio;
        }

        public static string Label(this HtmlHelper htmlHelper, string forname, string label)
        {
            return htmlHelper.Label(forname, label, TagRenderMode.Normal);
        }

        public static string Label(this HtmlHelper htmlHelper, string forname, string label, TagRenderMode tagRenderMode)
        {
            var builder = new TagBuilder("label");
            builder.Attributes.Add("for", forname);
            builder.SetInnerText(label);
            return builder.ToString(tagRenderMode);
        }

        public static string TextBoxWithLabel(this HtmlHelper htmlHelper, string name, string label)
        {
            return string.Concat(htmlHelper.Label(name, label), htmlHelper.TextBox(name));
        }

        public static string TextBoxWithLabel(this HtmlHelper htmlHelper, string name, string label, object value)
        {
            return string.Concat(htmlHelper.Label(name, label), htmlHelper.TextBox(name, value));
        }

        public static string TextBoxWithLabel(this HtmlHelper htmlHelper, string name, string label, object value, IDictionary<string, object> htmAttributes)
        {
            return string.Concat(htmlHelper.Label(name, label), htmlHelper.TextBox(name, value, htmAttributes));
        }

        public static string TextBoxWithLabel(this HtmlHelper htmlHelper, string name, string label, object value, object htmAttributes)
        {
            return string.Concat(htmlHelper.Label(name, label), htmlHelper.TextBox(name, value, htmAttributes));
        }

        public static MvcHtmlString TextBox<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, object>> action) where TModel : class
        {
            return TextBox(html, model, action, string.Empty, null);
        }

        public static MvcHtmlString TextBox<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, object>> action, string objectName) where TModel : class
        {
            return TextBox(html, model, action, objectName, null);
        }

        public static MvcHtmlString TextBox<TModel>(this HtmlHelper html, TModel model, Expression<Func<TModel, object>> action, string objectName, object htmlAttributes) where TModel : class
        {
            var expression = (UnaryExpression)action.Body;
            var property = (((System.Linq.Expressions.MemberExpression)(expression.Operand)).Member).Name;
            var classe = ((((System.Linq.Expressions.MemberExpression)(expression.Operand)).Member).DeclaringType).Name;

            var name = objectName == null ? classe + "." + property : objectName.Trim().Length > 0 ? objectName + "." + property : property;
            var value = action.Compile().Invoke(model);

            return html.TextBox(name, value, htmlAttributes);
        }

        public static string[] RadioButtonList(this HtmlHelper html, string name, IEnumerable<SelectListItem> selectList, Expression<Func<SelectListItem, string>> labelExpression, object htmlAttributes)
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotNull(selectList, "selectList");

            var exprCompiled = labelExpression.Compile();
            var radioButtons = selectList.Select(item => "<label for=\"" + name + "\">" + exprCompiled.Invoke(item) + "</label>" + html.RadioButton(name, item.Value, item.Selected, htmlAttributes));

            return radioButtons.ToArray();
        }
    }
}
