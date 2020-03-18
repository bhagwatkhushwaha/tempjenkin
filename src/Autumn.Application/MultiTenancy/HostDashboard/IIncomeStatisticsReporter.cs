using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autumn.MultiTenancy.HostDashboard.Dto;

namespace Autumn.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}