using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWebPortal.ViewModels
{
    public class PriceViewModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
        public string Response { get; set; }
    }
}
