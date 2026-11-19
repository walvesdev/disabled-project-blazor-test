using Sindvamb.Utils.Extension;

namespace Sindvamb.Utils.Http;

public class HttpClientResult<T>
{
    private string? _errorMessage;

    private Exception _exception;

    public bool IsSuccessStatus { get; set; }

    public T Data { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public HttpResponseMessage HttpResponseMessage { get; set; }

    public List<object> Errors { get; set; } = new List<object>();

    public string? ErrorMessage
    {
        get
        {
            if (_errorMessage.IsNullOrEmpty() && Exception.IsNull())
            {
                return Enum.GetName(typeof(HttpStatusCode), StatusCode);
            }

            return _errorMessage;
        }
        set
        {
            _errorMessage = value;
        }
    }

    public Exception Exception
    {
        get => _exception;
        set
        {
            _exception = value;
            if (_errorMessage != null)
            {
                _errorMessage.RemoveScape();
            }
        }
    }

}
