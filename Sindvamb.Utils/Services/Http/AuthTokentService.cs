#region

#endregion

namespace Sindvamb.Utils.Services.Http;

public abstract class AuthTokentService
{
    private int _expiresIn;

    public string Username { get; set; }

    public string Password { get; set; }

    public string BasicCredentials
    {
        get
        {
            var byteArray = new UTF8Encoding().GetBytes($"{Username}:{Password}");
            return $"{Convert.ToBase64String(byteArray)}";
        }
    }

    public string Token { get; set; }

    public string BaseAddress { get; set; }

    public int ExpiresIn
    {
        get => _expiresIn;
        set
        {
            _expiresIn = value;
            ExpirationDate = DateTime.UtcNow.AddSeconds(_expiresIn);
        }
    }

    public DateTime ExpirationDate { get; private set; }

    public bool Expired() => ExpirationDate <= DateTime.UtcNow;
}
