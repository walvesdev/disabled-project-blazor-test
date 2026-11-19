namespace Sindvamb.Domain.DTO
{
    public class AccessControlDTO 
    {
        public AccessControlDTO()
        {
            Connections = new HashSet<ConnectionItemModel>();
        }

        public Guid? Id { get; set; }

        public Guid? UserId { get; set; }

        public DateTime? ConnectionTime { get; set; }

        public DateTime? LastBeatTime { get; set; }

        public TimeSpan? Dur { get; set; }

        public string? Ip { get; set; }

        public string? City { get; set; }

        public string? OS { get; set; }

        public string? Device { get; set; }

        public string? Browser { get; set; }

        public string? Language { get; set; }

        public string? Engine { get; set; }

        public string? RequestUrl { get; set; }

        public IEnumerable<ConnectionItemModel> Connections { get; set; }
        public string? UserName { get; set; }

        public IEnumerable<AccessControl> MapAccsessControl()
        {
            var newList = new List<AccessControl>();

            if (Connections.Any())
            {
                foreach (var connection in Connections)
                {
                    var connId = Guid.Parse(connection.Id!);

                    //var access = new AccessControl
                    //{
                    //    Id = connId,
                    //    UserName = UserName,
                    //    UserId = UserId,
                    //    ConnectionTime = connection.ConnectionTime.LocalDateTime,
                    //    LastBeatTime = connection.LastBeatTime.LocalDateTime,
                    //    Dur = connection.LastBeatTime - connection.ConnectionTime,
                    //    Ip = connection.ClientInfo?.Ip ?? "",
                    //    City = connection.ClientInfo?.City ?? "",
                    //    OS = connection.ClientInfo?.OS ?? "",
                    //    //Device = Enum.GetName(typeof(WebClientDeviceType), connection.ClientInfo?.Device!) ?? "",
                    //    Browser = connection.ClientInfo?.Browser ?? "",
                    //    Language = connection.ClientInfo?.Language ?? "",
                    //    Engine = connection.ClientInfo?.Engine ?? "",
                    //    RequestUrl = connection.ClientInfo?.RequestUrl ?? "",
                    //};

                    //newList.Add(access);
                } 
            }

            return newList;
        }
    }
}
