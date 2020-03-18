using System;
using System.Collections.Generic;
using System.Text;

namespace Autumn.Authorization.Users.Dto
{
    public enum GenderEnumDto
    {
        Male = 1,
        Female = 2,
        Neither = 3,
        Other = 4,
        NotDisclosed = 5
    }

    public enum RetirementGoalsEnumDto
    {
        DesiredRetirementSum = 1,
        DesiredRetirementIncome = 2,
        DesiredLegacyAmount = 3,
        DesiredRetirementAge = 4,
        PlaceToLiveAfterRetirement = 5
    }
}
