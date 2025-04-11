using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Server.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; }
    }

    public interface IFromId<T>
    {
        public static abstract T FromId(int id);
    }
}
