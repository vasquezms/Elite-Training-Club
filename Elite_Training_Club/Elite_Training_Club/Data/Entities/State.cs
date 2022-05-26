using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elite_Training_Club.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Departamento/Estado")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }
        
        [JsonIgnore]
        public Country Country { get; set; }

        public ICollection <City> Cities  { get; set; }

        [Display(Name = "Ciudades")]

        public int CitiesNumber => Cities == null ? 0 : Cities.Count;

      }
   

}
