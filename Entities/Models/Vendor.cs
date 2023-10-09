using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Vendor
    {
        [Column("VenderoId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Vendor name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is a required field.")]
        [ForeignKey(nameof(Market))]
        public Guid MarketId { get; set; }
        public Market Market { get; set; }
    }
}
