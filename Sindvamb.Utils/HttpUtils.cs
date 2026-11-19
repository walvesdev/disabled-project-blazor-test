namespace Sindvamb.Utils
{
    public class HttpUtils
    {
        private static HttpClient _client;
        private static readonly object _locker = new object();

        public static HttpClient GetHttpClient()
        {
            lock (_locker)
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                }
            }
            return _client;
        }
    }
}
