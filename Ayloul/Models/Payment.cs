using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ayloul.Models
{
    public class Payment
    {
        [Key]
        public int Payment_Id { get; set; }

        public int Booking_Id { get; set; }

        [ForeignKey(nameof(Booking_Id))]
        public Booking Booking { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "JOD";

        
        public string Transaction_Id { get; set; }

       

        public DateTime Payment_Date { get; set; } = DateTime.Now;

        public PaymentMethod Payment_Method { get; set; }

        public PaymentStatus Status { get; set; }

        public enum PaymentMethod
        {
            Cash,
            Visa
        }

        public enum PaymentStatus
        {
            Pending,
            Paid,
            Failed
        }


    }




}
