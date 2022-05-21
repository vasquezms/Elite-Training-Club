using Elite_Training_Club.common;
using Elite_Training_Club.Data.Entities;

namespace Elite_Training_Club.Models
{

    public class HomeViewModel
    {

        public PaginatedList<Product> Products { get; set; }
        public ICollection<Category> Categories { get; set; }

        public float Quantity { get; set; }
    }


}
