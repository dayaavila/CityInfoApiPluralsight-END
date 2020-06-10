using Microsoft.AspNetCore.Mvc; //para heredar de ControllerBase
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        //[HttpGet] //asi passamos el Routing Template
        //public JsonResult GetCities()
        //{
        //     return new JsonResult(CitiesDataStore.Current.Cities);

        //    // 1 //return new JsonResult(
        //    //    new List<object>()
        //    //    { //son 2 objetos anonimos ya que aun no tenemos la clase CitiesModel Class
        //    //Para retornar una lista de cities, corremos la app, aparece error pero vamos al POSTMAN y ejecutamos el GET y 
        //    //obtenemos los 2 objetos de abajo y asi hemos creado nuestra primera API action que funciona.
        //    //new { id = 1, Name = "New YOrk City" },
        //    //new { id = 2, Name = "Antwerp" } ////Este codigo es remplazado en el CitiesDataStore y agregamos el codigo de arriba
        //    //});

        //    //Cuando consumimos una HTTP services services to get data, necesitamos enviar un HTTP request con el HTTP ,metodo GET en la URL that router to this method.   
        //    //Any type of client that consumes an API send an HTTP request to that API.
        //    //podemos usar un browser para eso, esto para que el browser sends to a URL.
        //    ////run into restrinctuions quite fast when we want to start sending requests to create or update resources.      
        //}


        //[HttpGet("{id}")]
        //public JsonResult GetCity(int id)
        //{
        //    return new JsonResult(
        //        CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
        //}


        [HttpGet] //asi passamos el Routing Template
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            // Find city
            var cityToReturn = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == id);

            if (cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);
        }
    }
}
