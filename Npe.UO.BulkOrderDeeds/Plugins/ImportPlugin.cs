namespace Npe.UO.BulkOrderDeeds.Plugins
{
    public abstract class ImportPlugin
    {
        public abstract string DisplayName { get; }

        public abstract void Import();
    }
}
