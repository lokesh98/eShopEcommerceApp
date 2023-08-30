using AspNetCoreHero.ToastNotification.Abstractions;
using eShopApp.DataAccessLayer.UOF;
using eShopApp.Models;
using eShopApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _notifyService = notifyService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var category = _unitOfWork.Category.GetAll().Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                CategoryDescription = x.CategoryDescription,
                CategoryName = x.CategoryName,
                DisplayOrder = x.DisplayOrder,
            });
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Category entity = new Category()
                {
                    ApplicationUserId = "1",
                    CategoryName = model.CategoryName,
                    CategoryDescription = model.CategoryDescription,
                    DisplayOrder = model.DisplayOrder,
                    CreatedDate = DateTime.Now
                };

                _unitOfWork.Category.Add(entity);
                _unitOfWork.Save();

                _notifyService.Success("Category created successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int? categoryId)
        {
            if (categoryId == null)
            {
                _notifyService.Warning("Category ID not found.");
                return RedirectToAction(nameof(Index));
            }
            var category = _unitOfWork.Category.GetT(x => x.Id == categoryId);
            if (category == null)
            {
                _notifyService.Warning("Category not found.");
            }
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Id = category!.Id,
                CategoryDescription = category.CategoryDescription,
                DisplayOrder = category.DisplayOrder,
                CategoryName = category.CategoryName,
            };


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = _unitOfWork.Category.GetT(x => x.Id == categoryViewModel.Id);

                category.CategoryName = categoryViewModel.CategoryName;
                category.CategoryDescription= categoryViewModel.CategoryDescription;
                category.DisplayOrder = categoryViewModel.DisplayOrder;

                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();

                _notifyService.Success("Category updated successfully.");
                return RedirectToAction(nameof(Index));
            }
            return View(categoryViewModel);
        }
    }
}
