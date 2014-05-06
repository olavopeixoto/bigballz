namespace BigBallz.Core.Security
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Verifica se determinado usuário tem acesso de leitura a uma dada funcionalidade SCAFplus.NET
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="funcionalidade">Número da funcionalidade</param>
        /// <returns>Se o usuário tem ou não acesso de leitura a funcionalidade</returns>
        bool TemAcessoFuncionalidade(int userId, int funcionalidade);

        /// <summary>
        /// Verifica se determinado usuário tem acesso de escrita a uma dada funcionalidade SCAFplus.NET
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="funcionalidade">Número da funcionalidade</param>
        /// <returns>Se o usuário tem ou não acesso de escrita a funcionalidade</returns>
        bool TemAcessoEscritaFuncionalidade(int userId, int funcionalidade);

        /// <summary>
        /// Verifica se determinado usuário possui um determinado papel associado ao seu perfil
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="papelId">Codigo numérico de identificação do papel no sistema SCAFplus.NET</param>
        /// <returns>Se o usuário tem ou não o papel associado ao seu perfil</returns>
        bool TemPapel(int userId, int papelId);

        /// <summary>
        /// Verifica se determinado usuário possui acesso de leitura a um determinado pacote funcional
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="pacoteId">Codigo numérico de identificação do pacote funcional no sistema SCAFplus.NET</param>
        /// <returns>Se o usuário tem ou não acesso de leitura no pacote funcional</returns>
        bool TemLeituraPacoteFuncional(int userId, int pacoteId);

        /// <summary>
        /// Verifica se determinado usuário possui acesso de escrita a um determinado pacote funcional
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="pacoteId">Codigo numérico de identificação do pacote funcional no sistema SCAFplus.NET</param>
        /// <returns>Se o usuário tem ou não acesso de escrita no pacote funcional</returns>
        bool TemEscritaPacoteFuncional(int userId, int pacoteId);

        /// <summary>
        /// Verifica se determinado usuário possui direito de acesso a determinado objetosistema
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="objetoSistemaId">Codigo numérico de identificação do pacote funcional no sistema SCAFplus.NET</param>
        /// <returns>Se o usuário tem ou não acesso de escrita no pacote funcional</returns>
        bool TemAcessoObjetoSistema(int userId, int objetoSistemaId);

        /// <summary>
        /// Verifica se determinado usuário possui direito de excluir determinado objetosistema
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="objetoSistemaId"></param>
        /// <returns></returns>
        bool PodeExcluirObjetoSistema(int userId, int objetoSistemaId);

        /// <summary>
        /// Verifica se determinado usuário possui direito de executar determinado objetosistema
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="objetoSistemaId"></param>
        /// <returns></returns>
        bool PodeExecutarObjetoSistema(int userId, int objetoSistemaId);

        /// <summary>
        /// Verifica se determinado usuário possui direito de incluir determinado objetosistema
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="objetoSistemaId"></param>
        /// <returns></returns>
        bool PodeIncluirObjetoSistema(int userId, int objetoSistemaId);

        /// <summary>
        /// Verifica se determinado usuário possui direito de modificar determinado objetosistema
        /// </summary>
        /// <param name="userId">Codigo numérico de identificação do usuário no sistema SCAFplus.NET</param>
        /// <param name="objetoSistemaId"></param>
        /// <returns></returns>
        bool PodeModificarObjetoSistema(int userId, int objetoSistemaId);
    }
}