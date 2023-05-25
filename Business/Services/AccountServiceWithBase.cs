using AppCore.Business.Models.Results;
using Business.Enums;
using Business.Models;

namespace Business.Services
{
    public interface IAccountService
    {
        Result<UserModel> Login(UserLoginModel model);
        Result Register(UserRegistrationModel model);
    }

    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result<UserModel> Login(UserLoginModel model)
        {
            UserModel user = _userService.Query().SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.IsActive);
            if (user == null)
                return new ErrorResult<UserModel>("Invalid username and password!");
            return new SuccessResult<UserModel>(user);
        }

        public Result Register(UserRegistrationModel model)
        {
            var user = new UserModel()
            {
                IsActive = true,
                RoleId = (int)Role.User,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password,
                UserDetails = new UserDetailModel()
                {
                    Gender = model.UserDetails.Gender,
                    Email = model.UserDetails.Email.Trim(),
                    CityId = model.UserDetails.CityId,
                    CountryId = model.UserDetails.CountryId
                }
            };
            return _userService.Add(user);
        }
    }
}
