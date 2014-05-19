<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BigBallz.Models.User>" %>
<%@ Import Namespace="System.Globalization" %>
<form method="post" target="pagseguro" action="<%=ConfigurationManager.AppSettings["pagseguro-paymentredirecturl"] %>">  
          
        <!-- Campos obrigatórios -->  
        <input name="receiverEmail" type="hidden" value="<%=ConfigurationManager.AppSettings["pagseguro-email"] %>">  
        <input name="currency" type="hidden" value="BRL">  
  
        <!-- Itens do pagamento (ao menos um item é obrigatório) -->  
        <input name="itemId1" type="hidden" value="<%=ViewData["UserId"]%>">  
        <input name="itemDescription1" type="hidden" value="Taxa <%:ViewData["UserName"]%>">  
        <input name="itemAmount1" type="hidden" value="<%=Html.Price().ToString("N2", CultureInfo.InvariantCulture)%>">  
        <input name="itemQuantity1" type="hidden" value="1">  
        <input name="itemWeight1" type="hidden" value="0">  
        
        <!-- Código de referência do pagamento no seu sistema (opcional) -->  
        <input name="reference" type="hidden" value="<%=ViewData["UserId"]%>">  
          
       <!-- Dados do comprador (opcionais) -->  
<%--        <input name="senderName" type="hidden" value="José Comprador">  
        <input name="senderAreaCode" type="hidden" value="11">  
        <input name="senderPhone" type="hidden" value="56273440">  
        <input name="senderEmail" type="hidden" value="comprador@uol.com.br"> --%> 
  
        <!-- submit do form (obrigatório) -->  
        <input alt="Pague com PagSeguro" name="submit"  type="image" src="https://p.simg.uol.com.br/out/pagseguro/i/botoes/pagamentos/120x53-pagar-azul.gif"/>  
          
</form>  