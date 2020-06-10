using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Entities
{
    public class City
    {
        [Key] // considerada la primary key y el Id será una columna de identidad //también se puede afirmarexplicitamente.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //para esto necesitamos importar using System.ComponentModel.DataAnnotations.Schema;
        //Las opciones para ingresar a la base de datos generada tiene 3 posibles opciones, None= the database does not generation value, 
        //identity = The database generates a value when a row is inserted, computed= The database generates a value when a row is inserted or update 
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
             = new List<PointOfInterest>();

    }
}
