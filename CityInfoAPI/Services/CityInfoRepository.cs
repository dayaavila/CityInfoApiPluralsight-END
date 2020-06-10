using CityInfoAPI.Contexts;
using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();            
        }

        public City GetCity(int CityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == CityId).FirstOrDefault();
            }
            //solo devuelve el primer filtro de la ciudad por Id
            return _context.Cities
                .Where(c => c.Id == CityId).FirstOrDefault(); 
        }

        public IEnumerable<PointOfInterest> GetPointOfInterestForCity(int CityId)
        {
            //obtenemos el punto de interes por una ciudad, retornara todos los que coincidan con CityId
            return _context.PointOfInterests
                .Where(p => p.CityId == CityId).ToList();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            //filtramos por CityId y pointOfInterestId y retornari el 1ero verificar, no estoy segura
            return _context.PointOfInterests
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefault();
        }
    }
}
