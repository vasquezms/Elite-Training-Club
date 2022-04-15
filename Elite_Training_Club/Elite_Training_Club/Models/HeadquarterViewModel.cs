using System.ComponentModel.DataAnnotations;

namespace Elite_Training_Club.Models
{
    public class HeadquarterViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sede")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }

        public int CityId { get; set; }
    }
}
