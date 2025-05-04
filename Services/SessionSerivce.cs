namespace ApiGateway.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Login(string email)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", email);
        }

        public string? GetLoggedInUserEmail()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("UserEmail");
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
        
        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(GetLoggedInUserEmail());
        }
    }
}