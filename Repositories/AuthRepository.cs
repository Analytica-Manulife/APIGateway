using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ApiGateway.Data;
using APIGateWay.Model;

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
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
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
        
        public async Task<bool> InsertKycAsync(KycModel model, string accountId, DateTime recordDate)
        {
            var parameters = new[]
            {
                new SqlParameter("@BRCode", model.BRCode ?? (object)DBNull.Value),
                new SqlParameter("@BankAccountNumber", model.BankAccountNumber ?? (object)DBNull.Value),
                new SqlParameter("@BankCode", model.BankCode ?? (object)DBNull.Value),
                new SqlParameter("@BankName", model.BankName ?? (object)DBNull.Value),
                new SqlParameter("@BirthCountry", model.BirthCountry ?? (object)DBNull.Value),
                new SqlParameter("@BranchCode", model.BranchCode ?? (object)DBNull.Value),
                new SqlParameter("@DateOfBirth", model.DateOfBirth),
                new SqlParameter("@FirstName", model.FirstName ?? (object)DBNull.Value),
                new SqlParameter("@LastName", model.LastName ?? (object)DBNull.Value),
                new SqlParameter("@PostalCode", model.PostalCode ?? (object)DBNull.Value),
                new SqlParameter("@ProposalNumber", model.ProposalNumber ?? (object)DBNull.Value),
                new SqlParameter("@FormUrl", model.FormUrl ?? (object)DBNull.Value),
                new SqlParameter("@AccountId", accountId ?? (object)DBNull.Value),
                new SqlParameter("@RecordDate", recordDate)
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC InsertKycRecord @BRCode, @BankAccountNumber, @BankCode, @BankName, @BirthCountry, @BranchCode, @DateOfBirth, @FirstName, @LastName, @PostalCode, @ProposalNumber, @FormUrl, @AccountId, @RecordDate",
                    parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InsertKycAsync error: {ex.Message}");
                return false;
            }
        }

    }
    
    
}