using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NewsTella.Models.Database;

namespace NewsTella.Services
{
    public class AppUserManager: UserManager<User>
    {
        public AppUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
              keyNormalizer, errors, services, logger)
        {
        }

        public override Task<User> FindByIdAsync(string userId)
        {
            return Users.SingleOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return Users.SingleOrDefaultAsync(u => u.UserName == userName && !u.IsDeleted);
        }

        public override Task<User> FindByEmailAsync(string email)
        {
            return Users.SingleOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
}
