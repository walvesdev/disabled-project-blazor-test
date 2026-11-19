using Sindvamb.Utils;

namespace Sindvamb.Application.Core.FIlters
{
    public class CustomExceptionFilterResult
    {
        public string? Message { get; set; }
        public string? SistemMessage { get; set; }
        public string? InnerMessage { get; set; }
        public List<ValidationExceptionResult>? Errors { get; set; }
        public object? Data { get; set; }
    }
}
