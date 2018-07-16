namespace Npe.UO.BulkOrderDeeds.Internal
{
    internal interface ICloneable<T>
        where T : class
    {
        T Clone();
    }
}
