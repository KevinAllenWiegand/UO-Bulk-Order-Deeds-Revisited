namespace Npe.UO.BulkOrderDeeds.Plugins
{
    public abstract class ImportPlugin
    {
        internal virtual bool Trusted { get; }

        public abstract string DisplayName { get; }

        public abstract void Import();
    }
}
