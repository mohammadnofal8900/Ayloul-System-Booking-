using Ayloul.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ayloul.Models
{
    public class Booking
    {
        [Key]
        public int Booking_Id { get; set; }


        [Required]
        [Display(Name="Your Name")]
        public int Customer_Id { get; set; }

        [ForeignKey(nameof(Customer_Id))]
        public Customer? Customer { get; set; }

        [ValidateNever]
        public int Booking_Number { get; set; } = Random.Shared.Next(10000000, 99999999);

        public const decimal Service_Fees = 0.05m;

        public int BasePrice { get; set; } = 20;

        [ValidateNever]
        [Precision(6,2)]
        public decimal TotalPrice { get; set; }

        [Display(Name ="Name")]
        [Required(ErrorMessage = "please Enter Name .")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 20 char .")]
        public string Name { get; set; }


        [Required(ErrorMessage = "please Enter Description .")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 50 char .")]
        public string Description { get; set; }


        [Display(Name = "Special date or event date")]
        public string? Event_Date { get; set; }

        [Display(Name = "Delivery date")]
        [Required(ErrorMessage = "please Enter The Delivery date .")]
        public DateTime Date_Time { get; set; }


        [StringLength(500, MinimumLength = 3, ErrorMessage = "Please Enter between 3 to 500 char .")]
        public string? Note { get; set; }

        public string? Note_Image { get; set; }

        [NotMapped]
        [Display(Name = "Note Image")]
        public IFormFile? ImageFile { get; set; }

        [ValidateNever]
        [DefaultValue(true)]
        public bool Is_Active { get; set; } = true;

        

        [Required(ErrorMessage = "Please Enter Your Phone .")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Phone number must contain numbers only.")]
        public string Phone_Number { get; set; }

        [ValidateNever]
        public DateTime Order_Date { get; set; }=DateTime.Now;


        [ValidateNever]
        public int? Background_Id { get; set; }

        [ForeignKey("Background_Id")]
        public Background? Background { get; set; }

        [ValidateNever]
        public int FrameColor_Id { get; set; }

        [ValidateNever]
        [ForeignKey("FrameColor_Id")]
        public FrameColor FrameColor { get; set; }

        [ValidateNever]
        public int Location_Id { get; set; }

        [ValidateNever]
        [ForeignKey("Location_Id")]
        public Location Location { get; set; }

        

        [Range(1, int.MaxValue, ErrorMessage = "Please select the first rose color.")]
        public int RoseColor1_Id { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(RoseColor1_Id))]
        public RoseColor RoseColor1 { get; set; }

        public int? RoseColor2_Id { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(RoseColor2_Id))]
        public RoseColor? RoseColor2 { get; set; }


        [ValidateNever]
        public int RoseType_Id { get; set; }

        [ValidateNever]
        [ForeignKey("RoseType_Id")]
        public RoseType RoseType { get; set; }


        [Display(Name = "Status Booking")]
        public BookingStatus bookingStatus { get; set; } = BookingStatus.pending;
        

        [ValidateNever]
        public ICollection<BookingService>? BookingServices { get; set; } = new List<BookingService>();

        
        public Rating? Rating { get; set; }

        //[ValidateNever]
        //public ICollection<CostumerOrderPhoto>? costumerOrderPhotos { get; set; }

        public Booking()
        {

            Description = "no Description";
            Note = "No Note";
            Is_Active = true;
        }
        public PaymentMethodType? paymentMethod { get; set; }

        public enum PaymentMethodType
        {
            Cash = 0,
            Visa = 1
        }



        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum BookingStatus{
            [Display(Name = "Pending")]
            pending =1,

            [Display(Name = "Accepted")]
            accepted =2,


            [Display(Name = "Completed")]
            completed =4,
            [Display(Name = "Cancelled")]
            cancelled =5
        }


    }
}
