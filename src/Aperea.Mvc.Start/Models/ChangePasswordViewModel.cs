﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ApereaStart.Annotations;

namespace ApereaStart.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(128)]
        [HiddenInput]
        public string LoginName { get; set; }

        [Required]
        [StringLength(1024)]
        [AllowHtml]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(1024)]
        [LabelName("Password")]
        [DataType(DataType.Password)]
        [AllowHtml]
        public string Password { get; set; }

        [Required]
        [StringLength(1024)]
        [Compare("Password", ErrorMessageResourceType = typeof (ResourceStrings),
            ErrorMessageResourceName = "Error_The_password_and_confirmation_password_do_not_match")]
        [LabelName("ConfirmPassword")]
        [DataType(DataType.Password)]
        [AllowHtml]
        public string ConfirmPassword { get; set; }
    }
}