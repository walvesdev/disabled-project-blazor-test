

using Sindvamb.Domain;
using Sindvamb.Domain.Entities;



namespace Sindvamb.Domain.Entities
{

    public class Address : EntityBase<Guid>
    {
        public string? ZipCode { get; set; }

        public string? Neighborhood { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? Complement { get; set; }

        public long? Number { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? UF { get; set; }

        public string? Housing { get; set; }
    }

}
