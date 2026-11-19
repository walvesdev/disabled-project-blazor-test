using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain
{
    public class EntityBase<TKey>
    {
        [Key] public TKey Id { get; set; }

        public EntityBase()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                this.GetType().GetProperty("Id").SetValue(this, SequentialGuidGenerator.NewSequentialGuid(), null);
            }
        }
    }


    public class EntityBaseNotMaped<TKey> : EntityBase<TKey>
    {
        [Key] [NotMapped] public TKey Id { get; set; }

        public EntityBaseNotMaped()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                this.GetType().GetProperty("Id").SetValue(this, SequentialGuidGenerator.NewSequentialGuid(), null);
            }
        }
    }
}