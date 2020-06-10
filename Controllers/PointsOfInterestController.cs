using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        //private readonly LocalMailService _mailService;
        private readonly IMailService _mailService;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
               IMailService mailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //throw new Exception("Exception example.");

                var city = CitiesDataStore.Current.Cities
                    .FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{id}", Name = "GetPOintOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            // find point of interest
            var pointOfInterest = city.PointsOfInterest
                .FirstOrDefault(c => c.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public IActionResult CreatePontOInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            //Solo cuando no usamos una API controller attribute 
            //if (pointOfInterest == null)
            //{
            //    return BadRequest();
            //}

            //para controlar si envian un nombre nulo, este es el caso denviar un valor vacio.
            //if (pointOfInterest.Name == null)
            //{
            //    return BadRequest();
            //}


            //si la desccripcion coincide con el nombre manejamos este error
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Desciption",
                    "The provided description should be different from the name");
            }

            //ModelState es una propiedad valida que sera falsa, ademas de ser falsa si un valor invalido para una propiedad type es pasada, 
            //es una gran propiedad para chequear y ver si podemos continuar con nuestra action.
            //gracias al atributo ApiController no necesitamos escribir codigocomo este, cuando un ModelState es invalido, un respuesta es automaticamente sera retornada. 
            //asi que lo borramos
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //validating input working with model status
            //si el consumer sends a request to a resource URL que no existe
            //entonces debemos chequear ya sea o no una Current.Cityexiste, sino existe, return a NotFound
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            //antes de checkout we can at new point of interest, but we need to calculate the id of the new point of interest
            //and our data store currently work on a PointOfInterestDto and not on a PoibtOfInterestForCreationDto. So we need to map that DTO to a PointOfInterestDto
            //we can calculate id by running through all points of interests across cities and getting the highest value. We add one and we use it 
            //when creating the point of interest we are going to add to our in-memory data store.
            //for that, we also copy over the values of our input to our new object, and we add it to the citys PointOfInterest.
            //pero esto es muy incomodo, al mapear podemos cometer errores

            //validating input working with model status
            //demo purpose - to be improve
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId, id = finalPointOfInterest.Id },
                finalPointOfInterest);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePointOInterest(int cityId, int id,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Desciption",
                    "The provided description should be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(p => p.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent(); //no debe retornar nungun contenido
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyPointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfinterestForUpdataDto> patchDoc)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(c => c.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfinterestForUpdataDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description
                };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //colocamos nuestro custom
            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError
                    ("Description",
                      "The provided description should be different from name");
            }

            //le´t effectively trigger validation of the pointOfInteresToPatch.Todo that, we call into TryValidateModel. We pass through the pointOfInterestToPatch.
            //if this is false, the modelstate will become invalid.
            if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            //we copy over the property values, and we return NoContect
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(c => c.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send("Point of interest deleted.",
                    $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");
            return NoContent();
        }
    }
}
