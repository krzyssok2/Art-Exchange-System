using DATA.AppConfiguration;
using DATA.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;

namespace DATA.Functions
{
    public class TestFunctions
    {
        private readonly ArtExchangeContext _context;
        private readonly UserStore<IdentityUser> _userManager;
        public TestFunctions()
        {
            _context = new ArtExchangeContext(ArtExchangeContext.Options.DatabaseOptions);

            _userManager = new UserStore<IdentityUser>(_context);
        }

        public IdentityUser GetData()
        {
            var item =_context.UserData.First();
            var item2 = _userManager.FindByNameAsync(item.DisplayName).Result;
            return item2;
        }
    }
}
