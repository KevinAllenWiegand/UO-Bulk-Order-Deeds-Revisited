using Npe.UO.BulkOrderDeeds.Internal;
using System;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedBook
    {
        public static readonly BulkOrderDeedBook None = new BulkOrderDeedBook("[No Book]") { Id = Guid.Empty };

        internal const string XmlRootName = "BulkOrderDeedBooks";

        private const string _XmlItemName = "BulkOrderDeedBook";
        private const string _IdAttributeName = "id";
        private const string _NameAttributeName = "name";

        public Guid Id { get; private set; }
        public string Name { get; }

        public BulkOrderDeedBook(Guid id, string name)
        {
            Guard.ArgumentNotEmpty(nameof(id), id);
            Guard.ArgumentNotNullOrEmpty(nameof(name), name);

            Id = id;
            Name = name;
        }

        public BulkOrderDeedBook(string name)
            : this(Guid.NewGuid(), name)
        {
        }

        internal void SaveToXml(XmlWriter writer)
        {
            writer.WriteStartElement(_XmlItemName);
            writer.WriteAttributeString(_IdAttributeName, Id.ToString());
            writer.WriteAttributeString(_NameAttributeName, Name);
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
