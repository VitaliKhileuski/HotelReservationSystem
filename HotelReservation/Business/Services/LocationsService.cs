using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace Business.Services
{
    public class LocationsService
    {
        public LocationRepository _locationRepository;
        public IMapper _mapper;
        public LocationsService(LocationRepository locationRepository, MapConfiguration cfg)
        {
            _locationRepository = locationRepository;
            _mapper = new Mapper(cfg.LocationConfiguration);
        }
        public List<string> GetCountries()
        {
            List<string> countries = new List<string>();
            var countriesEntities = _locationRepository.GetAll().ToList();
            if (countriesEntities.Count == 0)
            {
                throw new NotFoundException("no data about countries");
            }
            foreach (var country in countriesEntities)
            {
                countries.Add(country.Country);
            }
            List<string> uniqueCounries = countries.Distinct().OrderBy(x => x).ToList();
            return uniqueCounries;
        }
        public List<string> GetCities(string country)
        {
            List<string> cities = new List<string>();
            var locationsEntities = _locationRepository.GetAll().ToList();
            if (locationsEntities.Count == 0)
            {
                throw new NotFoundException("no data about countries");
            }
            locationsEntities = locationsEntities.Where(x => x.Country == country).ToList();
            foreach(var location in locationsEntities)
            {
                cities.Add(location.City);
            }
            return cities.Distinct().OrderBy(x => x).ToList();
        }
    }
}