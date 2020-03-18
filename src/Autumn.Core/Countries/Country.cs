using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Autumn.Countries
{
    [Table("Country")]
    public class Country : FullAuditedEntity
    {
        [Required]
        [MaxLength(3)]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required] 
        public string CurrencySymbol { get; set; }

        [Required]
        [MaxLength(3)]
        public int RetirementAge { get; set; }

        [Required]
        [MaxLength(2)]
        public int LifeExpectancy { get; set; }
    }
}
