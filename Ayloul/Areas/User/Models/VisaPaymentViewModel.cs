using System.ComponentModel.DataAnnotations;

namespace Ayloul.Areas.User.Models
{
    public class VisaPaymentViewModel
    {

        [Required(ErrorMessage = "Please enter card holder name.")]
        [Display(Name = "Card Holder Name")]
        public string CardHolderName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter card number.")]
        [CreditCard(ErrorMessage = "Please enter a valid card number.")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; } = null!;

        [Required(ErrorMessage = "Please enter expiry date.")]
        [Display(Name = "Expiry Date")]
        public string ExpiryDate { get; set; } = null!;

        [Required(ErrorMessage = "Please enter CVV.")]
        [StringLength(4, MinimumLength = 3)]
        [Display(Name = "CVV")]
        public string CVV { get; set; } = null!;
    }
}
