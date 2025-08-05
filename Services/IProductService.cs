namespace Product_Catalog.Services
{
    public interface IProductService 
    {
        IEnumerable<Product> GetProducts();
        Task<bool> CreateProductAsync(ProductFormViewModel model);
        ProductFormViewModel GetEditFormModelAsync(int id);
        Task<bool> UpdateProductAsync(ProductFormViewModel model);
        Task<bool> ToggleProductStatusAsync(int id);
    }
}
