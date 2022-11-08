using System;
using System.ComponentModel.DataAnnotations;

namespace KeyAcademy_2.Models
{
    public class RequestHoliday
    {
        public string processInstanceKey = "holiday_request_key";

        [Display(Name = "Employee name")]
        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(maximumLength:20,MinimumLength =3)]
        public string employeeName { get; set; }

        [Display(Name = "Number of holidays")]
        [Required(ErrorMessage = "Number of holidays id required")]
        [Range(minimum:1,maximum:20, ErrorMessage = "U can't take more than 20 days of holidays")]
        public int numberOfHolidays { get; set; }

        [Display(Name = "Description")]
        [StringLength(maximumLength:100,MinimumLength =10)]
        [Required(ErrorMessage = "Description is required")]
        public string description { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone number is required")]
        public string phoneNumber { get; set; }
    }
}