using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Autumn.Authorization.Users
{
    [Table("UserRetirementPlans")]
    public class UserRetirementPlan: FullAuditedEntity, IMayHaveTenant
    {
        [Required]
        public long UserId { get; set; }
        public User User { get; set; }

        public RetirementGoals? RetirementGoalOptions { get; set; }

        public double? DesiredRetirementSum { get; set; }

        public double? DesiredRetirementIncome { get; set; }

        public double? DesiredLegacyAmount { get; set; }

        [MaxLength(2)]
        public int? DesiredRetirementAge { get; set; }

        public float? ReturnRate { get; set; }

        [MaxLength(15)]
        public double? InitialSaved { get; set; }

        [MaxLength(15)]
        public double? InitialOwed { get; set; }

        public double? InitialNet { get; set; }

        public double? RequiredSavings { get; set; }
         
        public double? TotalMonthlyIncome { get; set; }
         
        public double? TotalMonthlyExpences { get; set; }

        [MaxLength(15)]
        public double? TotalMonthlySavings { get; set; }

        [MaxLength(15)]
        public double? TotalLiabilities { get; set; }

        public double? LikelyRetirementSum { get; set; }

        [MaxLength(12)]
        public double? LikelyRetirementIncome { get; set; }

        public double? LikelyRetirementLegacy { get; set; }

        [MaxLength(2)]
        public double? LikelyRetirementAge { get; set; }

        public string DesiredRetirementProgress { get; set; }

        public bool FamilyView { get; set; }

        public int? TenantId { get; set; }
    }

    public enum RetirementGoals
    {
        DesiredRetirementSum = 1,
        DesiredRetirementIncome = 2,
        DesiredLegacyAmount = 3,
        DesiredRetirementAge = 4,
        PlaceToLiveAfterRetirement = 5
    }
}
