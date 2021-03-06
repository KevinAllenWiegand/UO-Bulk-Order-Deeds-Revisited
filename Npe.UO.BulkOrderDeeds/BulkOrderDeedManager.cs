﻿using Npe.UO.BulkOrderDeeds.Filters;
using Npe.UO.BulkOrderDeeds.Internal;
using Npe.UO.BulkOrderDeeds.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds
{
    public class BulkOrderDeedManager : IDisposable
    {
        #region Singleton

        private static readonly object _SingletonLock = new object();
        private static volatile BulkOrderDeedManager _Instance;

        public static BulkOrderDeedManager Instance
        {
            get
            {
                // Quick return if it's already set.
                if (_Instance != null)
                {
                    return _Instance;
                }

                lock (_SingletonLock)
                {
                    // Check again inside the lock.
                    if (_Instance == null)
                    {
                        _Instance = new BulkOrderDeedManager();
                    }
                }

                return _Instance;
            }
        }

        // Private for singleton pattern.
        private BulkOrderDeedManager()
        {
            _Professions = new List<Profession>();
            _Collection = new List<CollectionBulkOrderDeed>();
            _Vendors = new List<Vendor>();
            _BulkOrderDeedBooks = new List<BulkOrderDeedBook>();
            _ImportPlugins = new List<ImportPlugin>();

            _CollectionFullPath = _RootSaveLocation + _CollectionFilename;
            _VendorsFullPath = _RootSaveLocation + _VendorsFilename;
            _BulkOrderDeedBooksFullPath = _RootSaveLocation + _BulkOrderDeedBooksFilename;

            _XmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    "
            };

            LoadBuiltInPlugins();
            LoadPlugins();
    }

        #endregion

        private const string _ProfessionsFolderPath = "Professions";
        private const int _SaveThresholdMilliseconds = 500;
        private const string _XmlProcessingName = "xml";
        private const string _XmlProcessingText = "version=\"1.0\"";

        private static XmlWriterSettings _XmlWriterSettings;

        private readonly string _CollectionFilename = "Collection.xml";
        private readonly string _VendorsFilename = "Vendors.xml";
        private readonly string _BulkOrderDeedBooksFilename = "BulkOrderDeedBooks.xml";

        private readonly string _RootSaveLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NinjaPuffer Enterprises\\UO Bulk Order Deeds Revisited\\");
        private readonly string _ImportPluginsLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NinjaPuffer Enterprises\\UO Bulk Order Deeds Revisited\\Plugins\\Import");
        private readonly string _BuiltInImportPluginsLocation = Path.Combine(Internals.GetRootLocation(), "Plugins\\Import");
        private readonly object _ImportPluginsSync = new object();
        private readonly List<ImportPlugin> _ImportPlugins;
        private readonly object _ProfessionsSync = new object();
        private readonly List<Profession> _Professions;
        private readonly object _CollectionSync = new object();
        private readonly List<CollectionBulkOrderDeed> _Collection;
        private readonly object _VendorsSync = new object();
        private readonly List<Vendor> _Vendors;
        private readonly object _BulkOrderDeedBooksSync = new object();
        private readonly List<BulkOrderDeedBook> _BulkOrderDeedBooks;

        private readonly string _CollectionFullPath;
        private readonly string _VendorsFullPath;
        private readonly string _BulkOrderDeedBooksFullPath;
        private Timer _SaveCollectionTimer;
        private Timer _SaveVendorsTimer;
        private Timer _SaveBulkOrderDeedBooksTimer;

        public static readonly int[] PossibleQuantities = { 10, 15, 20 };

        public event EventHandler<BulkOrderDeedEventArgs> BulkOrderDeedCollectionItemsAdded;
        public event EventHandler<BulkOrderDeedEventArgs> BulkOrderDeedCollectionItemsRemoved;
        public event EventHandler<VendorEventArgs> VendorAdded;
        public event EventHandler<VendorEventArgs> VendorRemoved;
        public event EventHandler<BulkOrderDeedBookEventArgs> BulkOrderDeedBookAdded;
        public event EventHandler<BulkOrderDeedBookEventArgs> BulkOrderDeedBookRemoved;

        public IEnumerable<Profession> Professions => _Professions.AsReadOnly();

        public IEnumerable<ImportPlugin> ImportPlugins
        {
            get
            {
                var retVal = new List<ImportPlugin>();

                lock (_ImportPluginsSync)
                {
                    retVal.AddRange(_ImportPlugins);
                }

                return retVal;
            }
        }

        public IEnumerable<CollectionBulkOrderDeed> Collection
        {
            get
            {
                var retVal = new List<CollectionBulkOrderDeed>();

                lock (_CollectionSync)
                {
                    retVal.AddRange(_Collection);
                }

                return retVal;
            }
        }

        public IEnumerable<Vendor> Vendors
        {
            get
            {
                var retVal = new List<Vendor>();

                lock (_VendorsSync)
                {
                    retVal.AddRange(_Vendors);
                }

                return retVal;
            }
        }

        public IEnumerable<BulkOrderDeedBook> BulkOrderDeedBooks
        {
            get
            {
                var retVal = new List<BulkOrderDeedBook>();

                lock (_BulkOrderDeedBooksSync)
                {
                    retVal.AddRange(_BulkOrderDeedBooks);
                }

                return retVal;
            }
        }

        public void LoadProfessions()
        {
            var professionFolders = Directory.GetDirectories(_ProfessionsFolderPath);

            foreach (var professionFolder in professionFolders)
            {
                _Professions.Add(new Profession(professionFolder));
            }
        }

        public void ClearCollection()
        {
            List<CollectionBulkOrderDeed> collectionBulkOrderDeeds;

            lock (_CollectionSync)
            {
                collectionBulkOrderDeeds = new List<CollectionBulkOrderDeed>(_Collection);
            }

            if (collectionBulkOrderDeeds.Any())
            {
                RemoveBulkOrderDeeds(collectionBulkOrderDeeds);
            }
        }

        public void ClearVendors()
        {
            List<Vendor> vendors;

            lock (_VendorsSync)
            {
                vendors = new List<Vendor>(_Vendors);
            }

            foreach (var vendor in vendors)
            {
                RemoveVendor(vendor);
            }
        }

        public void ClearBulkOrderDeedBooks()
        {
            List<BulkOrderDeedBook> bulkOrderDeedBooks;

            lock (_BulkOrderDeedBooksSync)
            {
                bulkOrderDeedBooks = new List<BulkOrderDeedBook>(_BulkOrderDeedBooks);
            }

            foreach (var bulkOrderDeedBook in bulkOrderDeedBooks)
            {
                RemoveBulkOrderDeedBook(bulkOrderDeedBook);
            }
        }

        public void LoadCollection()
        {
            lock (_CollectionSync)
            {
                LoadCollectionInLock();
            }

            lock (_VendorsSync)
            {
                LoadVendorsInLock();
            }

            lock (_BulkOrderDeedBooksSync)
            {
                LoadBulkOrderDeedBooksInLock();
            }
        }

        public IEnumerable<CollectionBulkOrderDeed> GetFilteredCollection(CollectionFilterParameters parameters)
        {
            var parametersCopy = parameters.Clone();
            var retVal = new List<CollectionBulkOrderDeed>();

            foreach (var bulkOrderDeed in _Collection)
            {
                if (!ApplyCollectionFilterImpl(parametersCopy, bulkOrderDeed)) continue;

                retVal.Add(bulkOrderDeed);
            }

            return retVal.AsReadOnly();
        }

        public bool ApplyCollectionFilter(CollectionFilterParameters parameters, CollectionBulkOrderDeed bulkOrderDeed)
        {
            var parametersCopy = parameters.Clone();

            return ApplyCollectionFilterImpl(parametersCopy, bulkOrderDeed);
        }

        private bool ApplyCollectionFilterImpl(CollectionFilterParameters parameters, CollectionBulkOrderDeed bulkOrderDeed)
        {
            var retVal = true;

            foreach (var parameter in parameters.GetAppliedFilters())
            {
                if (!parameter.ApplyFilter(bulkOrderDeed))
                {
                    retVal = false;
                    break;
                }
            }

            return retVal;
        }

        public void AddVendor(Vendor vendor)
        {
            Guard.ArgumentNotNull(nameof(vendor), vendor);

            vendor.BulkOrderDeedBookAdded += OnVendorBulkOrderDeedBookAdded;
            vendor.BulkOrderDeedBookRemoved += OnVendorBulkOrderDeedBookRemoved;

            lock (_VendorsSync)
            {
                _Vendors.Add(vendor);
                SaveVendors();
            }

            OnVendorAdded(vendor);
        }

        public void RemoveVendor(Vendor vendor)
        {
            Guard.ArgumentNotNull(nameof(vendor), vendor);

            vendor.BulkOrderDeedBookAdded -= OnVendorBulkOrderDeedBookAdded;
            vendor.BulkOrderDeedBookRemoved -= OnVendorBulkOrderDeedBookRemoved;

            lock (_VendorsSync)
            {
                _Vendors.Remove(vendor);

                foreach (var collectionBulkOrderDeed in _Collection)
                {
                    if (collectionBulkOrderDeed.Location.Vendor.Id == vendor.Id)
                    {
                        collectionBulkOrderDeed.Location.Vendor = Vendor.None;
                        collectionBulkOrderDeed.Location.BulkOrderDeedBook = BulkOrderDeedBook.None;
                    }
                }

                SaveVendors();
            }

            OnVendorRemoved(vendor);
        }

        public void AddBulkOrderDeedBook(BulkOrderDeedBook bulkOrderDeedBook)
        {
            Guard.ArgumentNotNull(nameof(bulkOrderDeedBook), bulkOrderDeedBook);

            lock (_BulkOrderDeedBooksSync)
            {
                _BulkOrderDeedBooks.Add(bulkOrderDeedBook);
                SaveBulkOrderDeedBooks();
            }

            OnBulkOrderDeedBookAdded(bulkOrderDeedBook);
        }

        public void RemoveBulkOrderDeedBook(BulkOrderDeedBook bulkOrderDeedBook)
        {
            Guard.ArgumentNotNull(nameof(bulkOrderDeedBook), bulkOrderDeedBook);

            lock (_BulkOrderDeedBooksSync)
            {
                _BulkOrderDeedBooks.Remove(bulkOrderDeedBook);

                foreach (var collectionBulkOrderDeed in _Collection)
                {
                    if (collectionBulkOrderDeed.Location.BulkOrderDeedBook.Id == bulkOrderDeedBook.Id)
                    {
                        collectionBulkOrderDeed.Location.BulkOrderDeedBook = BulkOrderDeedBook.None;
                    }
                }

                SaveBulkOrderDeedBooks();
            }

            OnBulkOrderDeedBookRemoved(bulkOrderDeedBook);
        }

        public void AddBulkOrderDeeds(IEnumerable<CollectionBulkOrderDeed> collectionBulkOrderDeeds)
        {
            Guard.ArgumentCollectionNotNullOrEmpty(nameof(collectionBulkOrderDeeds), collectionBulkOrderDeeds);

            lock (_CollectionSync)
            {
                _Collection.AddRange(collectionBulkOrderDeeds);
                SaveCollection();
            }

            OnBulkOrderDeedCollectionItemsAdded(collectionBulkOrderDeeds);
        }

        public void RemoveBulkOrderDeeds(IEnumerable<CollectionBulkOrderDeed> collectionBulkOrderDeeds)
        {
            Guard.ArgumentCollectionNotNullOrEmpty(nameof(collectionBulkOrderDeeds), collectionBulkOrderDeeds);

            lock (_CollectionSync)
            {
                foreach (var collectionBulkOrderDeed in collectionBulkOrderDeeds)
                {
                    _Collection.Remove(collectionBulkOrderDeed);
                }

                SaveCollection();
            }

            OnBulkOrderDeedCollectionItemsRemoved(collectionBulkOrderDeeds);
        }

        private void OnVendorBulkOrderDeedBookAdded(object sender, BulkOrderDeedBookEventArgs e)
        {
            SaveVendors();
        }

        private void OnVendorBulkOrderDeedBookRemoved(object sender, BulkOrderDeedBookEventArgs e)
        {
            SaveVendors();
        }

        private void OnBulkOrderDeedCollectionItemsAdded(IEnumerable<CollectionBulkOrderDeed> collectionBulkOrderDeeds)
        {
            var handler = BulkOrderDeedCollectionItemsAdded;

            handler?.Invoke(this, new BulkOrderDeedEventArgs(collectionBulkOrderDeeds));
        }

        private void OnBulkOrderDeedCollectionItemsRemoved(IEnumerable<CollectionBulkOrderDeed> collectionBulkOrderDeeds)
        {
            var handler = BulkOrderDeedCollectionItemsRemoved;

            handler?.Invoke(this, new BulkOrderDeedEventArgs(collectionBulkOrderDeeds));
        }

        private void OnBulkOrderDeedBookAdded(BulkOrderDeedBook bulkOrderDeedBook)
        {
            var handler = BulkOrderDeedBookAdded;

            handler?.Invoke(this, new BulkOrderDeedBookEventArgs(bulkOrderDeedBook));
        }

        private void OnBulkOrderDeedBookRemoved(BulkOrderDeedBook bulkOrderDeedBook)
        {
            var handler = BulkOrderDeedBookRemoved;

            handler?.Invoke(this, new BulkOrderDeedBookEventArgs(bulkOrderDeedBook));
        }

        private void OnVendorAdded(Vendor vendor)
        {
            var handler = VendorAdded;

            handler?.Invoke(this, new VendorEventArgs(vendor));
        }

        private void OnVendorRemoved(Vendor vendor)
        {
            var handler = VendorRemoved;

            handler?.Invoke(this, new VendorEventArgs(vendor));
        }

        private void LoadCollectionInLock()
        {
            if (!File.Exists(_CollectionFullPath)) return;

            var xmlDocument = new XmlDocument();

            xmlDocument.Load(_CollectionFullPath);

            var bulkOrderDeeds = CollectionBulkOrderDeed.LoadFromXml(xmlDocument);

            _Collection.AddRange(bulkOrderDeeds);
        }

        private void SaveCollection()
        {
            if (_SaveCollectionTimer == null)
            {
                _SaveCollectionTimer = new Timer(OnSaveCollectionTimer, null, Timeout.Infinite, Timeout.Infinite);
            }

            _SaveCollectionTimer.Change(_SaveThresholdMilliseconds, Timeout.Infinite);
        }

        private void OnSaveCollectionTimer(object state)
        {
            lock (_CollectionSync)
            {
                EnsureRootSaveLocationExists();

                var output = new StringBuilder();
                var writer = XmlWriter.Create(output, _XmlWriterSettings);

                writer.WriteProcessingInstruction(_XmlProcessingName, _XmlProcessingText);
                writer.WriteStartElement(CollectionBulkOrderDeed.XmlRootName);

               foreach (var bulkOrderDeed in _Collection)
               {
                    bulkOrderDeed.SaveToXml(writer);
                }

                writer.WriteEndElement();
                writer.Close();

                File.WriteAllText(_CollectionFullPath, output.ToString(), Encoding.UTF8);
            }
        }

        private void LoadVendorsInLock()
        {
            if (!File.Exists(_VendorsFullPath)) return;

            var xmlDocument = new XmlDocument();

            xmlDocument.Load(_VendorsFullPath);

            var vendors = Vendor.LoadFromXml(xmlDocument);

            _Vendors.AddRange(vendors);
        }

        private void SaveVendors()
        {
            if (_SaveVendorsTimer == null)
            {
                _SaveVendorsTimer = new Timer(OnSaveVendorsTimer, null, Timeout.Infinite, Timeout.Infinite);
            }

            _SaveVendorsTimer.Change(_SaveThresholdMilliseconds, Timeout.Infinite);
        }

        private void OnSaveVendorsTimer(object state)
        {
            lock (_VendorsSync)
            {
                EnsureRootSaveLocationExists();

                var output = new StringBuilder();
                var writer = XmlWriter.Create(output, _XmlWriterSettings);

                writer.WriteProcessingInstruction(_XmlProcessingName, _XmlProcessingText);
                writer.WriteStartElement(Vendor.XmlRootName);

                foreach (var vendor in _Vendors)
                {
                    vendor.SaveToXml(writer);
                }

                writer.WriteEndElement();
                writer.Close();

                File.WriteAllText(_VendorsFullPath, output.ToString(), Encoding.UTF8);
            }
        }

        private void LoadBulkOrderDeedBooksInLock()
        {
            if (!File.Exists(_BulkOrderDeedBooksFullPath)) return;

            var xmlDocument = new XmlDocument();

            xmlDocument.Load(_BulkOrderDeedBooksFullPath);

            var bulkOrderDeedBooks = BulkOrderDeedBook.LoadFromXml(xmlDocument);

            _BulkOrderDeedBooks.AddRange(bulkOrderDeedBooks);
        }

        private void SaveBulkOrderDeedBooks()
        {
            if (_SaveBulkOrderDeedBooksTimer == null)
            {
                _SaveBulkOrderDeedBooksTimer = new Timer(OnSaveBulkOrderDeedBooksTimer, null, Timeout.Infinite, Timeout.Infinite);
            }

            _SaveBulkOrderDeedBooksTimer.Change(_SaveThresholdMilliseconds, Timeout.Infinite);
        }

        private void OnSaveBulkOrderDeedBooksTimer(object state)
        {
            lock (_BulkOrderDeedBooksSync)
            {
                EnsureRootSaveLocationExists();

                var output = new StringBuilder();
                var writer = XmlWriter.Create(output, _XmlWriterSettings);

                writer.WriteProcessingInstruction(_XmlProcessingName, _XmlProcessingText);
                writer.WriteStartElement(BulkOrderDeedBook.XmlRootName);

                foreach (var bulkOrderDeedBook in _BulkOrderDeedBooks)
                {
                    bulkOrderDeedBook.SaveToXml(writer);
                }

                writer.WriteEndElement();
                writer.Close();

                File.WriteAllText(_BulkOrderDeedBooksFullPath, output.ToString(), Encoding.UTF8);
            }
        }

        private void EnsureRootSaveLocationExists()
        {
            if (!Directory.Exists(_RootSaveLocation))
            {
                Directory.CreateDirectory(_RootSaveLocation);
            }
        }

        private void LoadBuiltInPlugins()
        {
            LoadPluginsImpl(_BuiltInImportPluginsLocation, true);
        }

        private void LoadPlugins()
        {
            LoadPluginsImpl(_ImportPluginsLocation, false);
        }

        private void LoadPluginsImpl(string location, bool mustBeTrusted)
        {
            if (!Directory.Exists(location)) return;

            var files = Directory.GetFiles(location, "*.dll");

            foreach (var file in files)
            {
                try
                {
                    var pluginAssembly = Assembly.LoadFrom(file);
                    var pluginTypes = pluginAssembly.GetTypes().Where(t => t.BaseType == typeof(ImportPlugin));

                    foreach (var pluginType in pluginTypes)
                    {
                        var instance = (ImportPlugin)Activator.CreateInstance(pluginType);

                        if (mustBeTrusted && !instance.Trusted) continue;

                        _ImportPlugins.Add(instance);
                    }
                }
                catch
                {
                }
            }
        }

        internal BulkOrderDeedDefinition GetBulkOrderDeedDefinition(string professionName, string bulkOrderDeedNameMatch, BulkOrderDeedType bulkOrderDeedType)
        {
            var profession = _Professions.FirstOrDefault(p => String.Compare(p.Name, professionName, true) == 0);

            if (profession == null)
            {
                throw new Exception($"Unable to find the \"{professionName}\" profession.");
            }

            var retVal = profession.BulkOrderDeedDefinitions.Definitions.FirstOrDefault(b => (String.Compare(b.DisplayName, bulkOrderDeedNameMatch, true) == 0)
                && ((bulkOrderDeedType == BulkOrderDeedType.Large && b is LargeBulkOrderDeedDefinition) || (bulkOrderDeedType == BulkOrderDeedType.Small && b is SmallBulkOrderDeedDefinition)));

            if (retVal == null)
            {
                throw new Exception($"Unable to find an \"{professionName}\" Bulk Order Deed for \"{bulkOrderDeedNameMatch}\".");
            }

            return retVal;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _Disposed;

        private void Dispose(bool disposing)
        {
            if (_Disposed) return;

            if (disposing)
            {
                try
                {
                    _SaveBulkOrderDeedBooksTimer?.Dispose();
                    _SaveCollectionTimer?.Dispose();
                    _SaveVendorsTimer?.Dispose();
                }
                catch
                {
                }
            }

            _Disposed = true;
        }
    }
}
