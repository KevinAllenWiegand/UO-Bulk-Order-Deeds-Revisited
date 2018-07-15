using Npe.UO.BulkOrderDeeds.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class Vendor
    {
        public static readonly Vendor None = new Vendor("[No Vendor]") { Id = Guid.Empty };

        internal const string XmlRootName = "Vendors";

        private const string _XmlItemName = "Vendor";
        private const string _IdAttributeName = "id";
        private const string _NameAttributeName = "name";

        public Guid Id { get; private set; }
        public string Name { get; }

        private List<BulkOrderDeedBook> _BulkOrderDeedBooks;
        public IEnumerable<BulkOrderDeedBook> BulkOrderDeedBooks => _BulkOrderDeedBooks.AsReadOnly();

        public event EventHandler<BulkOrderDeedBookEventArgs> BulkOrderDeedBookAdded;
        public event EventHandler<BulkOrderDeedBookEventArgs> BulkOrderDeedBookRemoved;

        public Vendor(Guid id, string name, IEnumerable<BulkOrderDeedBook> bulkOrderDeedBooks)
        {
            Guard.ArgumentNotEmpty(nameof(id), id);
            Guard.ArgumentNotNullOrEmpty(nameof(name), name);
            Guard.ArgumentNotNull(nameof(bulkOrderDeedBooks), bulkOrderDeedBooks);

            Id = id;
            Name = name;
            _BulkOrderDeedBooks = new List<BulkOrderDeedBook>(bulkOrderDeedBooks);
        }

        public Vendor(string name)
            : this(Guid.NewGuid(), name, new List<BulkOrderDeedBook>())
        {
        }

        public void AddBulkOrderDeedBook(BulkOrderDeedBook bulkOrderDeedBook)
        {
            lock (_BulkOrderDeedBooks)
            {
                _BulkOrderDeedBooks.Add(bulkOrderDeedBook);
            }

            OnBulkOrderDeedBookAdded(bulkOrderDeedBook);
        }

        public void RemoveBulkOrderDeedBook(BulkOrderDeedBook bulkOrderDeedBook)
        {
            lock (_BulkOrderDeedBooks)
            {
                _BulkOrderDeedBooks.Remove(bulkOrderDeedBook);
            }

            OnBulkOrderDeedBookRemoved(bulkOrderDeedBook);
        }

        internal void SaveToXml(XmlWriter writer)
        {
            writer.WriteStartElement(_XmlItemName);
            writer.WriteAttributeString(_IdAttributeName, Id.ToString());
            writer.WriteAttributeString(_NameAttributeName, Name);
            writer.WriteStartElement(BulkOrderDeedBook.XmlRootName);

            foreach (var bulkOrderDeedBook in _BulkOrderDeedBooks)
            {
                bulkOrderDeedBook.SaveToXml(writer);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        internal static IEnumerable<Vendor> LoadFromXml(XmlNode rootNode)
        {
            var retVal = new List<Vendor>();
            var nodes = rootNode.SelectNodes($"{XmlRootName}/{_XmlItemName}");

            if (nodes != null)
            {
                foreach (var node in nodes.OfType<XmlNode>())
                {
                    try
                    {
                        var idString = node.Attributes[_IdAttributeName].Value;
                        var name = node.Attributes[_NameAttributeName].Value;
                        var bulkOrderDeedBooks = BulkOrderDeedBook.LoadFromXml(node);

                        if (!String.IsNullOrEmpty(idString) && !String.IsNullOrEmpty(name))
                        {
                            var id = Guid.Parse(idString);

                            retVal.Add(new Vendor(id, name, bulkOrderDeedBooks));
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return retVal;
        }

        private void OnBulkOrderDeedBookAdded(BulkOrderDeedBook bulkOrderDeedBook)
        {
            var handler = BulkOrderDeedBookAdded;

            handler?.Invoke(this, new BulkOrderDeedBookEventArgs(this, bulkOrderDeedBook));
        }

        private void OnBulkOrderDeedBookRemoved(BulkOrderDeedBook bulkOrderDeedBook)
        {
            var handler = BulkOrderDeedBookRemoved;

            handler?.Invoke(this, new BulkOrderDeedBookEventArgs(this, bulkOrderDeedBook));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
