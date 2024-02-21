using Credito.ContraCheque.API.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Credito.ContraCheque.API.Domain.Response.Base
{
    [ExcludeFromCodeCoverage]
    public class ResponseContract<TDados>
    {
        readonly ICollection<string> _mensagens = new List<string>();
        readonly bool possuiErro;

        public string DetalheErro { get; set; }
        public TDados Dados { get; set; }
        public bool PossuiErro => !string.IsNullOrWhiteSpace(DetalheErro) || possuiErro;
        public MotivoErro? MotivoErro { get; private set; }

        #region Construtores
        public ResponseContract()
        {
            Dados = default;
            MotivoErro = null;
        }
        public ResponseContract(TDados dados)
        {
            Dados = dados;
            DetalheErro = string.Empty;
            MotivoErro = null;
        }

        public ResponseContract(MotivoErro motivoFalha, string mensagemErro)
        {
            MotivoErro = motivoFalha;

            _mensagens.Add(string.IsNullOrWhiteSpace(mensagemErro)
                ? motivoFalha.ToString()
                : mensagemErro);

            DetalheErro = _mensagens.Any() ? string.Join(" | ", _mensagens.ToList()) : string.Empty;
        }
        public ResponseContract(MotivoErro motivoFalha)
        {
            Dados = default;
            MotivoErro = motivoFalha;

            possuiErro = true;
        }
        #endregion

        public static ResponseContract<TDados> ComSucesso(TDados dados)
            => new ResponseContract<TDados>(dados);
        public static ResponseContract<TDados> ComErro(MotivoErro motivoErro)
            => new ResponseContract<TDados>(motivoErro);

        public static ResponseContract<TDados> ComDescricaoErro(MotivoErro motivoErro, string descricaoErro)
            => new ResponseContract<TDados>(motivoErro, descricaoErro);
    }
}
