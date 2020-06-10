using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Entities
{
    public class PointOfInterest //esta es la clase dependiente de la clase City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //para esto necesitamos importar using System.ComponentModel.DataAnnotations.Schema;  
        public int Id { get; set; } //esta seria la primary key
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [ForeignKey("CityId")] //no es obligatorio hacerlo explicitamente pero es buena practica
        public City City { get; set; } 
        public int CityId { get; set; } //esta seria la llave foranea con la tabla City.
    }
}
