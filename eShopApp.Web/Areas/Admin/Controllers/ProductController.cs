using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.DataAccessLayer.UOF;
using eShopApp.Models;
using eShopApp.Utility;
using eShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadHelper _fileUploadHelper;
        public ProductController(FileUploadHelper fileUploadHelper, INotyfService notifyService, IUnitOfWork unitOfWork)
        {
            _notifyService= notifyService;
            _unitOfWork= unitOfWork;
            _fileUploadHelper= fileUploadHelper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ProductViewModel productView = new ProductViewModel();
            productView.ProductList = _unitOfWork.Product.GetAll(null, includeProperties: "Category");

            return View(productView);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            SetUpProductCategory(viewModel);

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(ProductViewModel viewModel)
        {
            if (viewModel.ProductImageFile!=null)
            {
                var ext = Path.GetExtension(viewModel.ProductImageFile.FileName).ToLower();
                if (ext.Contains(".jpeg")|| ext.Contains(".png")|| ext.Contains(".jpg"))
                {
                }
                else
                {
                    _notifyService.Warning("Please upload valid product image.");
                    return View(viewModel);
                }
            }
            if (ModelState.IsValid)
            {
                string productImg = _fileUploadHelper.UploadImage(viewModel.ProductImageFile);
                if (!string.IsNullOrEmpty(productImg))
                {
                    viewModel.Product.ProductImage = productImg;

                    _unitOfWork.Product.Add(viewModel.Product);
                    _unitOfWork.Save();
                    _notifyService.Success("Product added successfully.");
                    return RedirectToAction(nameof(Index), "Product");
                }
                else
                {
                    _notifyService.Warning("Please upload product image.");
                }
                
            }
            return View(viewModel);
        }

        private void SetUpProductCategory(ProductViewModel viewModel)
        {
            var _productCategories = _unitOfWork.Category.GetAll().Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }).ToList();
            _productCategories.Insert(0, new SelectListItem()
            {
                Text = "Select Category",
                Value = ""
            });
            viewModel.Categories= _productCategories;
        }

        [HttpGet]
        public IActionResult Edit(int productId)
        {
            var product = _unitOfWork.Product.GetT(x => x.ProductId == productId);
            if (product==null)
            {
                _notifyService.Warning("Product not found");
                return RedirectToAction("Index");
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Product = product;
            SetUpProductCategory(viewModel);
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(ProductViewModel productVM)
        {
            if (productVM.ProductImageFile != null)
            {
                var ext = Path.GetExtension(productVM.ProductImageFile.FileName).ToLower().Trim();
                if (!ext.Contains(".jpeg") && !ext.Contains(".png") && !ext.Contains(".jpg"))
                {
                    _notifyService.Warning("Please upload valid product image.");
                    return View(productVM);
                }
            }
            if (ModelState.IsValid)
            {
                var product = _unitOfWork.Product.GetT(x => x.ProductId == productVM.Product.ProductId);
                string productImg = _fileUploadHelper.UploadImage(productVM.ProductImageFile);

                //update property

                product.ProductName = productVM.Product.ProductName;
                product.ProductDescription = productVM.Product.ProductDescription;
                product.Price = productVM.Product.Price;
                product.StockQty = productVM.Product.StockQty;

                if (!string.IsNullOrEmpty(productImg))
                {
                    product.ProductImage = productImg;
                }

                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();

                _notifyService.Success("Product updated successfully.");
                return RedirectToAction(nameof(Index), "Product");
            }

            return View(productVM);
        }
    }
}
