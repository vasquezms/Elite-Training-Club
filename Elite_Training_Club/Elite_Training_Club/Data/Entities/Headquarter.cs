using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elite_Training_Club.Data.Entities
{
    public class Headquarter
    {
        public int Id { get; set; }

        [Display(Name = "Sede")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

        [JsonIgnore]
        public City City { get; set; }
    }
}
