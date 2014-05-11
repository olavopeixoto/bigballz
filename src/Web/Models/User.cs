﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using BigBallz.Core.Helper;

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

        private bool? _isAdmin;
        public bool IsAdmin
        {
            get
            {
                return (bool) (_isAdmin = _isAdmin ?? UserRoles.Any(x => x.Role.Name.ToLower() == "admin"));
            }
            set { _isAdmin = value; }
        }
    }
}