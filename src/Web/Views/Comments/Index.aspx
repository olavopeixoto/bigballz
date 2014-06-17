<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IList<BigBallz.Models.Comment>>" %>
<%@ Import Namespace="BigBallz.Core" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"> - Comentários</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Mensagens BigBallz</h2>
<div style="display:block;clear:both;padding-top:20px;padding-bottom:20px;">
<%using(Html.BeginForm("post", "comments")){%>
    <span style="display:block"><%=Html.TextArea("comment", new {rows=3, style="width:95%"})%></span>
    <input type="submit" value="Comentar" />
<%}%>
</div>
<%foreach(var comment in Model) {%>
<div class="comment-container" style="clear:both;display:block;padding-bottom:10px;">
<span class="comment-photo" style="float:left;padding-right:10px;"><%=Html.GetUserPhoto(comment.User)%></span>
<div class="comment-contents" style="margin-left:60px;display:block;">
<span class="comment-timestamp" style="font-weight:bold;"><%:comment.CommentedOn.FormatDateTime()%> - <%:comment.User.UserName%></span>
<span class="comment-text" style="display:block"><%=HttpUtility.HtmlEncode(comment.Comments).ToHtml()%></span>
</div>
</div>
<%}%>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>