using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IUserService : IService<UserModel, User, eCommerceContext>
    {
        Result<List<UserModel>> GetUsers();
        Result<UserModel> GetUser(int id);
    }

    public class UserService : IUserService
    {
        public RepoBase<User, eCommerceContext> Repo { get; set; } = new Repo<User, eCommerceContext>();

        private readonly RepoBase<UserDetail, eCommerceContext> _userDetailRepo;
        private readonly RepoBase<Role, eCommerceContext> _roleRepo;
        private readonly RepoBase<Country, eCommerceContext> _countryRepo;
        private readonly RepoBase<City, eCommerceContext> _cityRepo;

        public UserService()
        {
            eCommerceContext dbContext = new eCommerceContext();
            Repo = new Repo<User, eCommerceContext>(dbContext);
            _userDetailRepo = new Repo<UserDetail, eCommerceContext>(dbContext);
            _roleRepo = new Repo<Role, eCommerceContext>(dbContext);
            _countryRepo = new Repo<Country, eCommerceContext>(dbContext);
            _cityRepo = new Repo<City, eCommerceContext>(dbContext);
        }

        public Result Add(UserModel model)
        {
            if (Repo.Query().Any(k => k.Username.ToUpper() == model.Username.ToUpper().Trim()))
                return new ErrorResult("A user record with the entered username already exists!");
            if (Repo.Query("UserDetail").Any(k => k.UserDetail.Email.ToUpper() == model.UserDetails.Email.ToUpper().Trim()))
                return new ErrorResult("A user record with the entered email already exists!");
            var entity = new User()
            {
                IsActive = model.IsActive,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password,
                RoleId = model.RoleId.Value,
                UserDetail = new UserDetail()
                {
                    Gender = model.UserDetails.Gender,
                    Email = model.UserDetails.Email.Trim(),
                    CityId = model.UserDetails.CityId.Value,
                    CountryId = model.UserDetails.CountryId.Value
                }
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            return new SuccessResult("Delete");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public Result<UserModel> GetUser(int id)
        {
            var user = Query().SingleOrDefault(k => k.Id == id);
            if (user == null)
                return new ErrorResult<UserModel>("User not found!");
            return new SuccessResult<UserModel>(user);
        }

        public Result<List<UserModel>> GetUsers()
        {
            var users = Query().ToList();
            if (users.Count == 0)
                return new ErrorResult<List<UserModel>>("No users found!");
            return new SuccessResult<List<UserModel>>(users.Count + " users found.", users);
        }

        public IQueryable<UserModel> Query()
        {
            var userQuery = Repo.Query();
            var userDetailQuery = _userDetailRepo.Query();
            var roleQuery = _roleRepo.Query();
            var countryQuery = _countryRepo.Query();
            var cityQuery = _cityRepo.Query();

            //inner join
            var query = from user in userQuery
                        join userDetail in userDetailQuery
                        on user.Id equals userDetail.UserId
                        join role in roleQuery
                        on user.RoleId equals role.Id
                        join country in countryQuery
                        on userDetail.CountryId equals country.Id
                        join city in cityQuery
                        on userDetail.CityId equals city.Id
                        orderby role.Name, user.Username
                        select new UserModel()
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Username = user.Username,
                            Password = user.Password,
                            IsActive = user.IsActive,
                            UserDetails = new UserDetailModel()
                            {
                                Gender = userDetail.Gender,
                                Email = userDetail.Email,
                                CountryId = userDetail.CountryId,
                                CountryNameDisplay = country.Name,
                                CityId = userDetail.CityId,
                                CityNameDisplay = city.Name,
                            },
                            RoleId = user.RoleId,
                            RoleNameDisplay = role.Name,
                            IsActiveDisplay = user.IsActive ? "Yes" : "No"
                        };
            return query;
        }

        public Result Update(UserModel model)
        {
            if (Repo.Query().Any(k => k.Username.ToUpper() == model.Username.ToUpper().Trim() && k.Id != model.Id))
                return new ErrorResult("A user record with the entered username already exists!");
            if (Repo.Query("UserDetail").Any(k => k.UserDetail.Email.ToUpper() == model.UserDetails.Email.ToUpper().Trim() && k.Id != model.Id))
                return new ErrorResult("A user record with the entered email already exists!");
            var entity = Repo.Query(k => k.Id == model.Id, "UserDetail").SingleOrDefault();
            entity.IsActive = model.IsActive;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Username = model.Username;
            entity.Password = model.Password;
            entity.RoleId = model.RoleId.Value;
            entity.UserDetail.Gender = model.UserDetails.Gender;
            entity.UserDetail.Email = model.UserDetails.Email.Trim();
            entity.UserDetail.CityId = model.UserDetails.CityId.Value;
            entity.UserDetail.CountryId = model.UserDetails.CountryId.Value;
            Repo.Update(entity);
            return new SuccessResult();
        }
    }
}
