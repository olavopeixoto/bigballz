using System.ComponentModel;

namespace BigBallz.Models
{
    public enum PagSeguroTransactionStatus
    {
        [Description("Aguardando pagamento: o comprador iniciou a transação, mas até o momento o PagSeguro não recebeu nenhuma informação sobre o pagamento.")]
        AguardandoPagamento = 1,
        [Description("Em análise: o comprador optou por pagar com um cartão de crédito e o PagSeguro está analisando o risco da transação.")]
        EmAnalise = 2,
        [Description("Paga: a transação foi paga pelo comprador e o PagSeguro já recebeu uma confirmação da instituição financeira responsável pelo processamento.")]
        Paga = 3,
        [Description("Disponível: a transação foi paga e chegou ao final de seu prazo de liberação sem ter sido retornada e sem que haja nenhuma disputa aberta.")]
        Disponivel = 4,
        [Description("Em disputa: o comprador, dentro do prazo de liberação da transação, abriu uma disputa.")]
        EmDisputa = 5,
        [Description("Devolvida: o valor da transação foi devolvido para o comprador.")]
        Devolvida = 6,
        [Description("Cancelada: a transação foi cancelada sem ter sido finalizada.")]
        Cancelada = 7
    }

    public enum PagSeguroPaymentMethod
    {
        Cartao_de_credito_Visa				= 101,
        Cartao_de_credito_MasterCard		= 102,
        Cartao_de_credito_American_Express 	= 103,
        Cartao_de_credito_Diners			= 104,
        Cartao_de_credito_Hipercard 		= 105,
        Cartao_de_credito_Aura				= 106,
        Cartao_de_credito_Elo 				= 107,
        Cartao_de_credito_PLENOCard 		= 108,
        Cartao_de_credito_PersonalCard 		= 109,
        Cartao_de_credito_JCB 				= 110,
        Cartao_de_credito_Discover 			= 111,
        Cartao_de_credito_BrasilCard 		= 112,
        Cartao_de_credito_FORTBRASIL 		= 113,
        Cartao_de_credito_CARDBAN 			= 114,
        Cartao_de_credito_VALECARD 			= 115,
        Cartao_de_credito_Cabal 			= 116,
        Cartao_de_credito_Mais 				= 117,
        Cartao_de_credito_Avista 			= 118,
        Cartao_de_credito_GRANDCARD 		= 119,
        Cartao_de_credito_Sorocred 			= 120,
        Boleto_Bradesco 					= 201,
        Boleto_Santander 					= 202,
        Debito_online_Bradesco 				= 301,
        Debito_online_Itau 					= 302,
        Debito_online_Unibanco 				= 303,
        Debito_online_Banco_do_Brasil 		= 304,
        Debito_online_Banco_Real 			= 305,
        Debito_online_Banrisul 				= 306,
        Debito_online_HSBC 					= 307,
        Saldo_PagSeguro 					= 401,
        Oi_Paggo 							= 501,
        Deposito_em_conta_Banco_do_Brasil = 701,
        Deposito_em_conta_HSBC 			= 702
    }
}