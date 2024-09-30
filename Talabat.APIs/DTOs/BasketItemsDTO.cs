using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        [Range(0.1,double.MaxValue,ErrorMessage ="Price must be more than 0.1")]
        public decimal Price { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Quantity must be 1 or more")]
        public int Quantity { get; set; }
    }
}
