

namespace Product_Catalog.Controllers
{

    [Authorize]
    public class ProductController(IProductService productService) : Controller
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public IActionResult Index()
        {
            var product = _productService.GetProducts();
            return View(product);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View( "Form", new ProductFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", model);
            }

            var success = await _productService.CreateProductAsync(model);
            if (!success)
            {
                ModelState.AddModelError(nameof(model.Image), "Invalid image file!");
                return View("Form", model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _productService.GetEditFormModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View("Form", model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", model);
            }

            var success = await _productService.UpdateProductAsync(model);
            if (!success)
            {
                ModelState.AddModelError(nameof(model.Image), "Invalid image file or product not found!");
                return View("Form", model);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ToggleStatus(int id)
        {
            var success = await _productService.ToggleProductStatusAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
