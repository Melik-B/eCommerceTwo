using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface ICityService : IService<CityModel, City, eCommerceContext>
    {
        Result<List<CityModel>> GetCities(int countryId);
    }

    public class CityService : ICityService
    {
        public RepoBase<City, eCommerceContext> Repo { get; set; } = new Repo<City, eCommerceContext>();

        public Result Add(CityModel model)
        {
            if (Repo.Query().Any(s => s.Name.ToUpper() == model.Name.ToUpper().Trim()))
            {
                return new ErrorResult("There is a record with the city name you entered!");
            }
            City entity = new City()
            {
                Name = model.Name.Trim(),
                CountryId = model.CountryId.Value
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            City entity = Repo.Query(s => s.Id == id, "UserDetails").SingleOrDefault();
            if (entity.UserDetails != null && entity.UserDetails.Count > 0)
                return new ErrorResult("There are users associated with the city to be deleted!");
            Repo.Delete(s => s.Id == id);
            return new SuccessResult("The city has been successfully deleted.");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public Result<List<CityModel>> GetCities(int countryId)
        {
            var cities = Query().Where(s => s.CountryId == countryId).ToList();
            return new SuccessResult<List<CityModel>>(cities);
        }

        public IQueryable<CityModel> Query()
        {
            return Repo.Query("Country").OrderBy(s => s.Name).Select(s => new CityModel()
            {
                Id = s.Id,
                Name = s.Name,
                CountryId = s.CountryId,
                Country = new CountryModel()
                {
                    Id = s.Country.Id,
                    Name = s.Country.Name
                }
            });
        }

        public Result Update(CityModel model)
        {
            if (Query().Any(s => s.Name.ToUpper() == model.Name.ToUpper().Trim() && s.Id != model.Id))
                return new ErrorResult("There is a record found for the city name you entered!");
            City entity = Repo.Query(s => s.Id == model.Id).SingleOrDefault();
            entity.Name = model.Name.Trim();
            entity.CountryId = model.CountryId.Value;
            Repo.Update(entity);
            return new SuccessResult();
        }
    }
}
