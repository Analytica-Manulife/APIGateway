using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ApiGateway.Data;

namespace ApiGateway.Repositories
{
    public class AuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(string name, string email, string password)
        {
            var nameParam = new SqlParameter("@name", name);
            var emailParam = new SqlParameter("@email", email);
            var passwordParam = new SqlParameter("@password", password);

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC RegisterUser @name, @email, @password",
                    nameParam, emailParam, passwordParam);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Account?> LoginUserAsync(string email, string password)
        {
            var emailParam = new SqlParameter("@email", email);
            var passwordParam = new SqlParameter("@password", password);

            var result = await _context.Accounts
                .FromSqlRaw("EXEC LoginUser @email, @password", emailParam, passwordParam)
                .ToListAsync();

            return result.FirstOrDefault();
        }
    }
}