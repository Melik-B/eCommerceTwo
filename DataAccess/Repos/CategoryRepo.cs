using DataAccess.Contexts;
using DataAccess.Repos.Bases;

namespace DataAccess.Repos
{
    public class CategoryRepo : CategoryRepoBase
    {
        public CategoryRepo() : base()
        {

        }

        public CategoryRepo(eCommerceContext ecommerceContext) : base(ecommerceContext)
        {

        }
    }
}
