using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elite_Training_Club.Models
{
    public class AddSubscriptionPlansViewModel
    {
        public int SubscriptionId { get; set; }

        [Display(Name = "Plan")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un Plan.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PlanId { get; set; }

        public IEnumerable<SelectListItem> Plans { get; set; }

    }
}
