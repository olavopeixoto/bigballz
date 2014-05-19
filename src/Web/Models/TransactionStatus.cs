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
}