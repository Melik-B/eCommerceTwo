using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface ICountryService : IService<CountryModel, Country, eCommerceContext>
    {
        Result<List<CountryModel>> GetCountries();
    }

    public class CountryService : ICountryService
    {
        public RepoBase<Country, eCommerceContext> Repo { get ; set ; } = new Repo<Country, eCommerceContext>();

        public Result Add(CountryModel model)
        {
            if (Repo.Query().Any(u => u.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("There is a record found for the country name you entered!");
            Country entity = new Country()
            {
                Name = model.Name.Trim()
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Country entity = Repo.Query(u => u.Id == id, "Cities", "UserDetails").SingleOrDefault();
            if (entity.Cities != null && entity.Cities.Count > 0)
                return new ErrorResult("There are cities associated with the country to be deleted!");
            if (entity.UserDetails != null && entity.UserDetails.Count > 0)
                return new ErrorResult("There are users associated with the country to be deleted!");
            Repo.Delete(u => u.Id == id);
            return new SuccessResult("The country has been successfully deleted.");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public Result<List<CountryModel>> GetCountries()
        {
            var countries = Query().ToList();
            if (countries.Count == 0)
                return new ErrorResult<List<CountryModel>>("Country not found!");
            return new SuccessResult<List<CountryModel>>(countries);
        }

        public IQueryable<CountryModel> Query()
        {
            return Repo.Query().OrderBy(u => u.Name).Select(u => new CountryModel()
            {
                Id = u.Id,
                Name = u.Name
            });
        }

        public Result Update(CountryModel model)
        {
            if (Repo.Query().Any(u => u.Name.ToUpper() == model.Name.ToUpper().Trim() && u.Id != model.Id))
                return new ErrorResult("There is a record found for the country name you entered!");
            Country entity = Repo.Query(u => u.Id == model.Id).SingleOrDefault();
            entity.Name = model.Name.Trim();
            Repo.Update(entity);
            return new SuccessResult();
        }
    }
}
