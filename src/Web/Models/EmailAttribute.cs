using System.ComponentModel.DataAnnotations;

namespace BigBallz.Models
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute() : base("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-])*\\.([a-z]{2,4})(\\.[a-z]{2,4})?$")
        {
        }
    }
}