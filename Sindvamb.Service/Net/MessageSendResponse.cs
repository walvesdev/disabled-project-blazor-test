#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Sindvamb.Service.Net
{
    public class MessageSendResponse
    {
        public string MessageId { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool Successful => !ErrorMessages.Any();

        public MessageSendResponse()
        {
            ErrorMessages = new List<string>();
        }
    }

    public class MessageSendResponse<T> : MessageSendResponse
    {
        public T Data { get; set; }
    }
}