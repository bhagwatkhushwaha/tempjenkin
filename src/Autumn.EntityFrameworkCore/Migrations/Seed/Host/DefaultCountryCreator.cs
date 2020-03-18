using Autumn.Countries;
using Autumn.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Autumn.Migrations.Seed.Host
{
    public class DefaultCountryCreator
    {
        public static List<Country> InitialCountries => GetInitialData();

        private readonly AutumnDbContext _context;

        private static List<Country> GetInitialData()
        {
            List<Country> countries = new List<Country>();

            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo cul in cinfo)
            {
                RegionInfo region = new RegionInfo(cul.Name);

                Country country = new Country
                {
                    Name = region.EnglishName,
                    CountryCode = region.ThreeLetterISORegionName,
                    CurrencySymbol = region.CurrencySymbol,
                    LifeExpectancy = 85,
                    RetirementAge = 65
                };

                countries.Add(country);
            }

            return countries;
        }

        public DefaultCountryCreator(AutumnDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateProviders();
        }

        private void CreateProviders()
        {
            foreach (var country in InitialCountries)
            {
                AddCountryIfNotExists(country);
            }
        }

        private void AddCountryIfNotExists(Country data)
        {
            if (_context.Countries.IgnoreQueryFilters().Any(l => l.Name == data.Name))
            {
                return;
            }

            _context.Countries.Add(data);

            _context.SaveChanges();
        }
    }
}
