using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scambio.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Field must be filled")]
        [StringLength(256, ErrorMessage = "The field should have a length of more than 4", MinimumLength = 4)]
        public string UserName { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Field must be filled")]
        [StringLength(256, ErrorMessage = "The field should have a length of more than 3", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Field must be filled")]
        [StringLength(256, ErrorMessage = "The field should have a length of more than 3", MinimumLength = 3)]
        public string LastName{ get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Field must be filled")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect email address")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Field must be filled")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm")]
        [Required(ErrorMessage = "Field must be filled")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password don`t match")]
        public string ConfirmPassword { get; set; }

        
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
    }
}
