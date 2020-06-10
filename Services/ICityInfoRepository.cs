using CityInfoAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Services
{
    public interface ICityInfoRepository
    {
        //IQueryable<City> GetCities();

        //para obtener la ciudad
        IEnumerable<City> GetCities();
        City GetCity(int CityId, bool includePointOfInterest);

        //para obtener un punto de interes oara la ciudad
        IEnumerable<PointOfInterest> GetPointOfInterestForCity(int CityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
    }
}
