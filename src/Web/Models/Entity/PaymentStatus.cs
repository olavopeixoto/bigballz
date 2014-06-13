using System;
using System.ComponentModel.DataAnnotations;

namespace BigBallz
{
    public class PaymentStatus
    {
        public int Id { get; set; }

        public int User { get; set; }

        public DateTime Date { get; set; }

        public DateTime LastEventDate { get; set; }

        [Required]
        [StringLength(36)]
        public string Transaction { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(60)]
        public string SenderEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string SenderName { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public virtual User User1 { get; set; }
    }
}