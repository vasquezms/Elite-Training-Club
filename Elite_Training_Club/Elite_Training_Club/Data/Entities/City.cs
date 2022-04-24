using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elite_Training_Club.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

        [JsonIgnore]
        public State State  { get; set; }

        public ICollection<Headquarter> Headquarters { get; set; }

        public ICollection<User> Users { get; set; }

        [Display(Name = "Sedes")]
        public int HeadquartersNumber => Headquarters == null ? 0 : Headquarters.Count;
    }
}
