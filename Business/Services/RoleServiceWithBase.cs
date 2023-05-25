using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    //ROLÜ ENUMS DAN DEĞİL ENTİTY DEN ÇEK
    //USİNG ENUMS KULLANMA!!!
    public interface IRoleService : IService<RoleModel, Role, eCommerceContext>
    {
        Result<List<RoleModel>> GetRoles();
        Result<RoleModel> GetRole(int id);
    }

    public class RoleService : IRoleService
    {
        public RepoBase<Role, eCommerceContext> Repo { get; set; } = new Repo<Role, eCommerceContext>();

        public Result Add(RoleModel model)
        {
            if (Repo.Query().Any(r => r.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("There is a record with the role name you entered!");
            Role entity = new Role()
            {
                Name = model.Name.Trim()
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Role entity = Repo.Query(r => r.Id == id, "Users").SingleOrDefault();
            if (entity.Users != null && entity.Users.Count > 0)
                return new ErrorResult("There are users associated with the role to be deleted!");
            Repo.Delete(entity);
            return new SuccessResult();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public Result<RoleModel> GetRole(int id)
        {
            var role = Query().SingleOrDefault(r => r.Id == id);
            if (role == null)
                return new ErrorResult<RoleModel>("Role not found!");
            return new SuccessResult<RoleModel>(role);
        }

        public Result<List<RoleModel>> GetRoles()
        {
            var roles = Query().ToList();
            if (roles.Count == 0)
                return new ErrorResult<List<RoleModel>>("Role not found!");
            return new SuccessResult<List<RoleModel>>(roles.Count + " Role found!", roles);
        }

        public IQueryable<RoleModel> Query()
        {
            return Repo.Query("Users").OrderBy(r => r.Name).Select(r => new RoleModel()
            {
                Id = r.Id,
                Name = r.Name,
                UsersDisplay = r.Users.Select(k => k.Username).ToList()
            });
        }

        public Result Update(RoleModel model)
        {
            if (Repo.Query().Any(r => r.Name.ToUpper() == model.Name.ToUpper().Trim() && r.Id != model.Id))
                return new ErrorResult("There is a record with the role name you entered!");
            Role entity = Repo.Query(r => r.Id == model.Id).SingleOrDefault();
            entity.Name = model.Name.Trim();
            Repo.Update(entity);
            return new SuccessResult();
        }
    }
}
