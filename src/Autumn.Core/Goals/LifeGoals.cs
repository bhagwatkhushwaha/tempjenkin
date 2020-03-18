using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Autumn.Goals
{
    [Table("LifeGoals")]
    public class LifeGoals: FullAuditedEntity
    {
        public string Name { get; set; }

        public DateTime PurchaseDate { get; set; }

        public double Value { get; set; }

        public float Interest { get; set; }

        public int Term { get; set; }

        public double Lumpsum { get; set; }

        public double Downpayment { get; set; }

        public double Instalment { get; set; }

        public double MonthlyMortgagePayment { get; set; }

        public int MortgageTerm { get; set; }

        public float MortgageRate { get; set; }

        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
    }
}
