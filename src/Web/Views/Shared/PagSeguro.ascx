<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BigBallz.Models.User>" %>
<!-- INICIO FORMULARIO BOTAO PAGSEGURO -->
 <form target="pagseguro" action="<%=ConfigurationManager.AppSettings["pagseguro-checkout-url"] %>" method="post">
 <input type="hidden" name="email_cobranca" value="bigballz@asound.org">
 <input type="hidden" name="tipo" value="CP">
 <input type="hidden" name="moeda" value="BRL">
 <input type="hidden" name="item_id_1" value="<%if(Context.User != null)
                                                {%> <%= Context.User.Identity.Name%> <%
                                                }%>">
 <input type="hidden" name="item_descr_1" value="<%:"Inscrição"%>">
 <input type="hidden" name="item_quant_1" value="1">
 <input type="hidden" name="item_valor_1" value="<%=(int)(Html.Price() * 100)%>">
 <input type="hidden" name="item_frete_1" value="0">
 <input type="hidden" name="encoding" value="UTF-8">

 <input type="image" src="https://p.simg.uol.com.br/out/pagseguro/i/botoes/pagamentos/120x53-pagar-azul.gif" name="submit" alt="Pague com PagSeguro - é rápido, grátis e seguro!">
 </form>
 <!-- FINAL FORMULARIO BOTAO PAGSEGURO -->