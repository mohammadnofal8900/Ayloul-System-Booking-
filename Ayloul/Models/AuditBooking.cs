using Ayloul.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class AuditDBooking
    {
        [Key]
        public int AuditBooking_Id { get; set; }
        [ValidateNever]
        public int Booking_Id { get; set; }

        [ValidateNever]
        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;

        //public string Deleted_By { get; set; }

        [ValidateNever]
        public string UserId { get; set; }
        [ValidateNever]

        public string User_Name { get; set; }
        [ValidateNever]
        public DateTime Delete_Date { get; set; }
    }
}
