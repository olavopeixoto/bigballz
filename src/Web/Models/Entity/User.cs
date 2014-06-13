using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BigBallz.Models;

namespace BigBallz
{
    [Table("User")]
    [MetadataType(typeof(UserMD))]
    public class User
    {
        public User()
        {
            Bets = new HashSet<Bet>();
            BonusBets = new HashSet<BonusBet>();
            Comments = new HashSet<Comment>();
            PaymentStatus = new HashSet<PaymentStatus>();
            UserMappings = new HashSet<UserMapping>();
            Roles = new HashSet<Role>();
        }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        public bool EmailAddressVerified { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(250)]
        public string PhotoUrl { get; set; }

        public bool Authorized { get; set; }

        [StringLength(50)]
        public string AuthorizedBy { get; set; }

        public bool PagSeguro { get; set; }

        public bool HelpShown { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }

        public virtual ICollection<BonusBet> BonusBets { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<PaymentStatus> PaymentStatus { get; set; }

        public virtual ICollection<UserMapping> UserMappings { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        private bool? _isAdmin;

        public bool IsAdmin
        {
            get
            {
                return
                    (bool)
                        (_isAdmin =
                            _isAdmin ??
                            Roles.Any(x => x.Name.ToLowerInvariant() == "admin"));
            }
            set { _isAdmin = value; }
        }

        public class UserMD
        {
            [Required(ErrorMessage = "Apelido é obrigatório")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "E-Mail é obrigatório")]
            [Email(ErrorMessage = "Informe um E-Mail válido")]
            public string EmailAddress { get; set; }
        }
    }
}