using Sindvamb.Web.Shared.Utils;

namespace Sindvamb.Web.Shared.Utils
{
    public class StateContainer
    {
        private JwtAccessToken? _jwtAccessToken;

        public JwtAccessToken? JwtAccessToken
        {
            get => _jwtAccessToken ?? null;
            set
            {
                _jwtAccessToken = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
