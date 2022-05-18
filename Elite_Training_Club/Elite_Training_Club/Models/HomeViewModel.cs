using Elite_Training_Club.Data.Entities;

namespace Elite_Training_Club.Models
{

    public class HomeViewModel
    {
        public ICollection<Product> Products { get; set; }

        public float Quantity { get; set; }
    }


}
