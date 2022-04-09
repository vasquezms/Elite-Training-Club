﻿using System.ComponentModel.DataAnnotations;

namespace Elite_Training_Club.Data.Entities
{
    public class Plan
    {
        public int Id { get; set; }

        [Display(Name = "Plan")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String Name { get; set; }
    }
}
