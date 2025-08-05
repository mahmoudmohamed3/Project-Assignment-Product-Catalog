
namespace Product_Catalog.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly List<string> _allowedExtensions = new() { ".jpg", ".png", ".jpeg" };
        private readonly int _maxSize = 2097152;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.AsNoTracking().ToList();
        }

        public async Task<bool> CreateProductAsync(ProductFormViewModel model)
        {
            if (model.Image != null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    return false;
                }

                if (model.Image.Length > _maxSize)
                {
                    return false;
                }

                var imageName = $"{Guid.NewGuid()}{extension}";
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                var path = Path.Combine(imagePath, imageName);
                using var stream = File.Create(path);
                await model.Image.CopyToAsync(stream);
                model.ImageUrl = imageName;
            }

            var product = new Product
            {
                ProductId = model.ProductId,
                ProductCode = model.ProductCode,
                ProductName_EN = model.ProductName_EN,
                SKU = model.SKU,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public ProductFormViewModel GetEditFormModelAsync(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return null;
            }

            return new ProductFormViewModel
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName_EN = product.ProductName_EN,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                SKU = product.SKU,
            };
        }
        public async Task<bool> UpdateProductAsync(ProductFormViewModel model)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == model.ProductId);
            if (product == null)
            {
                return false;
            }

            if (model.Image != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", product.ImageUrl);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    return false;
                }

                if (model.Image.Length > _maxSize)
                {
                    return false;
                }

                var imageName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", imageName);
                using var stream = File.Create(path);
                await model.Image.CopyToAsync(stream);
                model.ImageUrl = imageName;
            }
            else if (model.Image == null && !string.IsNullOrEmpty(product.ImageUrl))
            {
                model.ImageUrl = product.ImageUrl;
            }


            product.ProductId = model.ProductId;
            product.ProductCode = model.ProductCode;
            product.ImageUrl = model.ImageUrl;
            product.Price = model.Price;
            product.SKU = model.SKU;
            product.ProductName_EN = model.ProductName_EN;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> ToggleProductStatusAsync(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return false;
            }

            product.IsDeleted = !product.IsDeleted;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
