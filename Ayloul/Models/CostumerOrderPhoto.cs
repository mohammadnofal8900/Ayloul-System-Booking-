using System.ComponentModel.DataAnnotations;

namespace Ayloul.Models
{
    public class CostumerOrderPhoto
    {
        [Key]
        public int CostumerOrderPhoto_Id { get; set; }

        [Display(Name ="Order")]
        public int Booking_Id { get; set; }

        public Booking? booking { get; set; }

        public string Fill_Name{ get; set; }
        public string Fill_Path{ get; set; }
        public string Fill_Type{ get; set; }
        public string Fill_Size{ get; set; }

        public DateTime? Fill_Date{ get; set; }
    }
}
