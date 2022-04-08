using System.ComponentModel.DataAnnotations;

namespace Elite_Training_Club.Data.Entities
{
    //,,,
    public class Country
    {
        public int Id { get; set; }

        [Display(Name ="País")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres.")]
        [Required(ErrorMessage ="El campo {0} es obligatorio.")]
        public String Name { get; set; }
    }
}
