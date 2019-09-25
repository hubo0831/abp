namespace Volo.Abp.Domain.Entities
{
    /// <summary>有并行版本</summary>
    public interface IHasConcurrencyVersion
    {
        /// <summary>并行版本</summary>
        int Version { get; }
        /// <summary>增加并行版本</summary>
        void IncrementVersion();
    }
}