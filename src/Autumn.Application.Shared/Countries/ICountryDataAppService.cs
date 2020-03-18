using Abp.Application.Services;
using Autumn.Countries.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.Countries
{
    public interface ICountryDataAppService : IApplicationService
    {
        List<CountryDto> GetAllCountries();
    }
}
