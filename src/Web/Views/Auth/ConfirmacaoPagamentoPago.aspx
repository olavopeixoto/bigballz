<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="MainContent">
<h2>Bem-vindo ao BigBallz</h2>
<p>
Recebemos o seu pagamento da taxa de inscrição e você já está autorizado a participar.
</p>
<p>
Importante ficar claro que, participar do BigBallz, implica estar ciente e de acordo com o <%=Html.ActionLink("regulamento", "rules", "home")%>.
</p>
</asp:Content>
<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="Scripts"></asp:Content>