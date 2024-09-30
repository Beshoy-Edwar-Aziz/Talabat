using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}