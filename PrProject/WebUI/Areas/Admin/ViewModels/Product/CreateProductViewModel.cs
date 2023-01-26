using Core.Entities;

namespace WebUI.Areas.Admin.ViewModels.Product
{
    public class CreateProductViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Raiting { get; set; }
    }
}
