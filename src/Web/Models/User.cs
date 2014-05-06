using System.ComponentModel.DataAnnotations;

namespace BigBallz.Models
{
    [MetadataType(typeof(UserMD))]
    public partial class User
    {
        public class UserMD
        {
            [Required(ErrorMessage = "Apelido é obrigatório")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "E-Mail é obrigatório")]
            [EmailAttribute(ErrorMessage = "Informe um E-Mail válido")]
            public string EmailAddress { get; set; }
        }
    }
}