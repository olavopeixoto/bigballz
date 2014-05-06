using System.Collections.Generic;

namespace BigBallz.Core.Security
{
    public interface IProcessAuthorizationService
    {
        /// <summary>
        /// Verifica se o usuário tem permissão para aprovação do processo
        /// </summary>
        /// <param name="processoID">ProcessoID</param>
        /// <param name="deltaNivelAutorizacao">Variação de níveis para o processo</param>
        /// Exception, com mensagem customizada caso não possua autorização
        void PodeAutorizarProcesso(int processoID, int? deltaNivelAutorizacao);

        /// <summary>
        /// Verifica se o usuário tem permissão para execução do processo
        /// </summary>
        /// <param name="processoID">ProcessoID</param>
        /// <param name="deltaNivelAutorizacao">Variação de níveis para o processo</param>
        /// Exception, com mensagem customizada caso não possua autorização
        void PodeExecutarProcesso(int processoID, int? deltaNivelAutorizacao);

        /// <summary>
        /// Verifica se o usuário tem permissão para aprovação do processos compostos (processo que possui outros processos)
        /// </summary>
        /// <param name="processosNiveisAutorizacao">Dictionary com ProcessoID e DeltaNívelAutorizacao de cada processo</param>
        /// <returns>Exception, com mensagem customizada caso não possua autorização</returns>
        void PodeAutorizarProcessos(IDictionary<int, int> processosNiveisAutorizacao);

        /// <summary>
        /// Verifica se o usuário tem permissão para execução do processos compostos (processo que possui outros processos)
        /// </summary>
        /// <param name="processosNiveisAutorizacao">Dictionary com ProcessoID e DeltaNívelAutorizacao de cada processo</param>
        /// <returns>Exception, com mensagem customizada caso não possua autorização</returns>
        void PodeExecutarProcessos(IDictionary<int, int> processosNiveisAutorizacao);

        ///// <summary>
        ///// Retorna as alçadas do usuário e do processo, para verificar se o nível da alçada do usuário está acima do nível necessário para aprovação do processo
        ///// </summary>
        ///// <param name="processoID">ProcessoID</param>
        ///// <param name="deltaNivelAutorizacao">Variação de níveis para o processo</param>
        ///// <returns>Alçadas, sendo a primeira a alcada do usuário e a segunda a alçada do processo</returns>
        //bool AlcadaSuperiorNecessarioAprovacao(int processoID, int deltaNivelAutorizacao);

        ///// <summary>
        ///// Retorna as alçadas do usuário e dos processos, para verificar se o nível da alçada do usuário está acima do nível necessário para aprovação dos processos
        ///// </summary>
        ///// <param name="deltaNivelAutorizacao">Dicionário com ProcessoID e deltaNivelAutorizacao de cada processo</param>
        ///// <returns>Alçadas, sendo a primeira a alçada do usuário e as demais as alçadas dos processos</returns>
        //bool AlcadaSuperiorNecessarioAprovacao(Dictionary<int, int> processosNiveisAutorizacao);

        /// <summary>
        /// Autoriza o Item Processo no respectivo nível de autorização do usuário
        /// </summary>
        /// <param name="processoID">ProcessoID</param>
        /// <param name="itemProcessoID">ItemProcessoID</param>
        /// <returns>SituacaoID do itemProcesso</returns>
        int AutorizaItemProcesso(int processoID, int itemProcessoID);

        /// <summary>
        /// Autoriza os Item Processos, no respectivo nível de autorização do usuário
        /// </summary>
        /// <param name="processoID">ProcessoID</param>
        /// <param name="itensProcessosID">String com separador "," de todos os ItemProcessoID's</param>
        /// <returns>SituacaoID dos itemProcessos</returns>
        int AutorizaItensProcessos(int processoID, string itemProcessosID);

        /// <summary>
        /// Autoriza os Item Processos, no respectivo nível de autorização do usuário
        /// </summary>
        /// <param name="processoID">ProcessoID</param>
        /// <param name="itensProcessosID">String com separador "," de todos os ItemProcessoID's</param>
        /// <param name="situacao">Situacao para a qual se deseja atualizar os itens processos</param>
        /// <returns>SituacaoID dos itemProcessos</returns>
        int AutorizaItensProcessos(int processoID, string itensProcessosID, int? situacao);
    }
}