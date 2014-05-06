using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using BigBallz.Core.Security;

namespace BigBallz.Core.Compatibility
{
    /// <summary>
    /// Classe que le e salva arquivos de configuracoes
    /// </summary>
    [XmlRoot("Configuracoes")]
    public class XMLDAT : IDat
    {
        private const string PRIVATEKEY = "12345678";

        [XmlElement("TipoServidor")]
        public string tipoServidor;
        [XmlElement("StringConexao")]
        public string stringConexao;
        [XmlElement("Login")]
        public string login;
        [XmlElement("Senha")]
        public string senha;
        [XmlElement("MaxObjectPool")]
        public int maxObjectPool;
        [XmlElement("EnableObjectPool")]
        public bool enableObjectPool;
        [XmlElement("PoolRefreshTime")]
        public DateTime poolRefreshTime;

        /// <summary>
        /// Usado para checar se o arquivo foi alterado externamente
        /// </summary>
        [XmlElement("SecurityKey")]
        public string securityKey;

        public static XMLDAT GetDat(string datPath)
        {
            var s = new XmlSerializer(typeof(XMLDAT));
            using (TextReader r = new StreamReader(datPath))
            {
                var dat = s.Deserialize(r) as XMLDAT;
                r.Close();

                if (dat == null) return new XMLDAT();

                var cripto = new Cripto();

                dat.stringConexao = cripto.DecriptarRC2(dat.stringConexao, PRIVATEKEY);
                dat.tipoServidor = cripto.DecriptarRC2(dat.tipoServidor, PRIVATEKEY);
                dat.login = cripto.DecriptarRC2(dat.login, PRIVATEKEY);
                dat.senha = cripto.DecriptarRC2(dat.senha, PRIVATEKEY);

                if (dat.securityKey != BuildSecurityKey(dat))
                {
                    throw new Exception("Arquivo de cofiguração inválido.");
                }
                return dat;
            }
        }

        private static string BuildSecurityKey(XMLDAT dat)
        {
            var cripto = new Cripto();

            var sb = new StringBuilder(dat.stringConexao);
            sb.Append(dat.tipoServidor);
            sb.Append(dat.login);
            sb.Append(dat.senha);

            return cripto.EncriptarMD5(sb.ToString());
        }

        public string TipoServidor
        {
            get { return tipoServidor; }
        }

        public string StringConexao
        {
            get { return stringConexao; }
        }

        public string Login
        {
            get { return login;  }
        }

        public string Senha
        {
            get { return senha; }
        }

        public int MaxObjectPool
        {
            get { return maxObjectPool; }
        }

        public bool EnableObjectPool
        {
            get { return enableObjectPool; }
        }

        public DateTime PoolRefreshTime
        {
            get { return poolRefreshTime; }
        }
    }
}
