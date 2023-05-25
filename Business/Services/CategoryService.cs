using AppCore.Business.Models.Results;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {
        public RepoBase<Category, eCommerceContext> Repo { get; set; } = new Repo<Category, eCommerceContext>();

        public Result Add(CategoryModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return new ErrorResult("Category name cannot be empty!");
            if (model.Name.Length > 100)
                return new ErrorResult("The category name must be a maximum of 100 characters!");

            if (Repo.Query().Any(m => m.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("There is a record with the category name you entered!");

            Category newEntity = new Category()
            {
                Name = model.Name.Trim()
            };
            Repo.Add(newEntity);

            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Category entity = Repo.Query(m => m.Id == id, "Products").SingleOrDefault();
            if (entity.Products != null && entity.Products.Count > 0)
            {
                return new ErrorResult("There are products associated with the category to be deleted!");
            }
            Repo.Delete(m => m.Id == id);
            return new SuccessResult("The category has been successfully deleted.");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            List<CategoryModel> categories;

            categories = await Query().ToListAsync();

            return categories;
        }

        public IQueryable<CategoryModel> Query()
        {
            IQueryable<CategoryModel> query = Repo.Query("Products").OrderBy(category => category.Name).Select(category => new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            });
            return query;
        }

        public Result Update(CategoryModel model)
        {
            if (Repo.Query().Any(m => m.Name.ToUpper() == model.Name.ToUpper().Trim() && m.Id != model.Id))
                return new ErrorResult("There is a record with the category name you entered!");
            Category entity = Repo.Query(m => m.Id == model.Id).SingleOrDefault();
            if (entity == null)
                return new ErrorResult("No category record found!");
            entity.Name = model.Name.Trim();
            Repo.Update(entity);
            return new SuccessResult("The category has been successfully updated!");
        }
    }
}
