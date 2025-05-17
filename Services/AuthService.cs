using APIGateWay.Model;
using ApiGateway.Repositories;
using ApiGateway.Services;

namespace ApiGateway.Services
{
    public class AuthService
    {
        private readonly AuthRepository _repository;
        private readonly SessionService _sessionService;

        public AuthService(AuthRepository repository, SessionService sessionService)
        {
            _repository = repository;
            _sessionService = sessionService;
        }

        public async Task<(bool Success, string Message)> Register(string name, string email, string password)
        {
            var result = await _repository.RegisterUserAsync(name, email, password);
            return result ? (true, "Registration successful") : (false, "Registration failed (maybe email already exists)");
        }

        public async Task<(bool Success, string Message, string? Email, string? Name, decimal? balance, Guid? accountId)> Login(string email, string password)
        {
            var account = await _repository.LoginUserAsync(email, password);
            if (account == null)
                return (false, "Invalid credentials", null, null, 0, new Guid());

            _sessionService.Login(account.Email);
            return (true, "Login successful", account.Email, account.Name, account.Balance, account.AccountId);
        }
        public async Task<(bool Success, string Message)> InsertKycAsync(KycModel model, string accountId, DateTime recordDate)
        {
            var result = await _repository.InsertKycAsync(model, accountId, recordDate);
            return result ? (true, "KYC inserted successfully") : (false, "Failed to insert KYC");
        }

    }
}