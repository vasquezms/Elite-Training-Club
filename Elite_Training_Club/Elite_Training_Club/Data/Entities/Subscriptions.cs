using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elite_Training_Club.Data.Entities
{
    public class Subscriptions
    {
            public int Id { get; set; }

            [Display(Name = "Nombre")]
            [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public string Name { get; set; }

            [DataType(DataType.MultilineText)]
            [Display(Name = "Descripción")]
            [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
            public string Description { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            [DisplayFormat(DataFormatString = "{0:C2}")]
            [Display(Name = "Precio")]
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public decimal Price { get; set; }

            public ICollection<SubscriptionsPlan> SubscriptionsPlans { get; set; }

            [Display(Name = "Categorías")]
            public int SubscriptionsNumber => SubscriptionsPlans == null ? 0 : SubscriptionsPlans.Count;
        }

    
}
