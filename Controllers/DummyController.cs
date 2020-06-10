using CityInfoAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/testdatabase")]
    public class DummyController : ControllerBase
    {
        private readonly CityInfoContext _ctx;

        //nos aseguramos que tenga acceso al CityInfoContext que esta en Context
        //sabemos que podemos usar inyeccion deinstruccion para eso.
        public DummyController(CityInfoContext ctx)
        {
            //Esto asegurará una vez que se construya el contructor DummyController, una instancia de CityInfoContext es inyectado
            //eso significa que el contructor de CityInfoContext se ha ejecutado, y eso significa que nuestra base de datos de creará.
            //ahora lo que necesitamos es una action y para eso vamos a DummyController
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        [HttpGet]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
