using System;

namespace BigBallz.Core.Compatibility
{
    public interface IDat
    {
        string TipoServidor  { get; }
        string StringConexao  { get; }
        string Login  { get; }
        string Senha  { get; }
        int MaxObjectPool  { get; }
        bool EnableObjectPool  { get; }
        DateTime PoolRefreshTime  { get; }
    }
}