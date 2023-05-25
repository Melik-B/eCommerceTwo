using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace DataAccess.Repos.Bases
{
    public class CategoryRepoBase : RepoBase<Category, eCommerceContext>
    {
        protected CategoryRepoBase() : base()
        {

        }

        protected CategoryRepoBase(eCommerceContext ecommerceContext) : base(ecommerceContext)
        {

        }
    }
}
