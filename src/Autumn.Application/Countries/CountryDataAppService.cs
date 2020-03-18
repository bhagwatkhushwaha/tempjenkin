using Abp.Domain.Repositories;
using Autumn.Countries.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autumn.Countries
{
    public class CountryDataAppService : AutumnAppServiceBase, ICountryDataAppService
    {
        private readonly IRepository<Country> _CountryRepo;

        public CountryDataAppService(IRepository<Country> CountryRepo)
        {
            _CountryRepo = CountryRepo;
        }

        public List<CountryDto> GetAllCountries()
        {
            var query = ObjectMapper.Map<List<CountryDto>>(_CountryRepo.GetAll().ToList().OrderBy(c=>c.Name));
            var idx = query.FindIndex(c => c.Name == "Singapore");
            var item = query[idx];
            query.RemoveAt(idx);
            query.Insert(0, item);
            return query;
        }

    }
}
