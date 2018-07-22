using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Deployment.WindowsInstaller.Package;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace Npe.UO.BulkOrderDeeds.BuiltInImportPlugins
{
    public class ImportViewModel : ViewModelBase
    {
        private const string _TailorProfession = "Tailor";
        private const string _BlacksmithProfession = "Blacksmith";

        private readonly object _SyncRoot = new object();
        private readonly Dictionary<Guid, string> _BodBooks;

        private ObservableCollection<ImportableBulkOrderDeed> _ImportableBulkOrderDeeds;
        public ObservableCollection<ImportableBulkOrderDeed> ImportableBulkOrderDeeds
        {
            get { return _ImportableBulkOrderDeeds; }
            set
            {
                _ImportableBulkOrderDeeds = value;
                CommandManager.InvalidateRequerySuggested();
                NotifyPropertyChanged(nameof(ImportableBulkOrderDeeds));
            }
        }

        private string _CollectionFile;
        public string CollectionFile
        {
            get { return _CollectionFile; }
            set
            {
                _CollectionFile = value;
                NotifyPropertyChanged(nameof(CollectionFile));
            }
        }

        private int _TotalBulkOrderDeeds;
        public int TotalBulkOrderDeeds
        {
            get { return _TotalBulkOrderDeeds; }
            set
            {
                _TotalBulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(TotalBulkOrderDeeds));
            }
        }

        private int _TotalTailorBulkOrderDeeds;
        public int TotalTailorBulkOrderDeeds
        {
            get { return _TotalTailorBulkOrderDeeds; }
            set
            {
                _TotalTailorBulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(TotalTailorBulkOrderDeeds));
            }
        }

        private int _TotalTailorSmallBulkOrderDeeds;
        public int TotalTailorSmallBulkOrderDeeds
        {
            get { return _TotalTailorSmallBulkOrderDeeds; }
            set
            {
                _TotalTailorSmallBulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(TotalTailorSmallBulkOrderDeeds));
            }
        }

        private int _TotalTailorLargeBulkOrderDeeds;
        public int TotalTailorLargeBulkOrderDeeds
        {
            get { return _TotalTailorLargeBulkOrderDeeds; }
            set
            {
                _TotalTailorLargeBulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(TotalTailorLargeBulkOrderDeeds));
            }
        }

        private int _TotalBlacksmithBulkOrderDeeds;
        public int TotalBlacksmithBulkOrderDeeds
        {
            get { return _TotalBlacksmithBulkOrderDeeds; }
            set
            {
                _TotalBlacksmithBulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(TotalBlacksmithBulkOrderDeeds));
            }
        }

        private int _TotalBlacksmithSmallBulkOrderDeeds;
        public int TotalBlacksmithSmallBulkOrderDeeds
        {
            get { return _TotalBlacksmithSmallBulkOrderDeeds; }
            set
            {
                _TotalBlacksmithSmallBulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(TotalBlacksmithSmallBulkOrderDeeds));
            }
        }

        private int _TotalBlacksmithLargeBulkOrderDeeds;
        public int TotalBlacksmithLargeBulkOrderDeeds
        {
            get { return _TotalBlacksmithLargeBulkOrderDeeds; }
            set
            {
                _TotalBlacksmithLargeBulkOrderDeeds = value;
                NotifyPropertyChanged(nameof(TotalBlacksmithLargeBulkOrderDeeds));
            }
        }

        private bool _ResetCollection;
        public bool ResetCollection
        {
            get { return _ResetCollection; }
            set
            {
                _ResetCollection = value;
                NotifyPropertyChanged(nameof(ResetCollection));
            }
        }

        private bool _SuccessfullyLoaded;
        public bool SuccessfullyLoaded
        {
            get { return _SuccessfullyLoaded; }
            set
            {
                _SuccessfullyLoaded = value;
                NotifyPropertyChanged(nameof(SuccessfullyLoaded));
            }
        }

        private bool _IsImportActive;
        public bool IsImportActive
        {
            get { return _IsImportActive; }
            set
            {
                _IsImportActive = value;
                CommandManager.InvalidateRequerySuggested();
                NotifyPropertyChanged(nameof(IsImportActive));
            }
        }

        private int _ImportedCount;
        public int ImportedCount
        {
            get { return _ImportedCount; }
            set
            {
                _ImportedCount = value;
                NotifyPropertyChanged(nameof(ImportedCount));
            }
        }

        public ICommand ImportCommand { get; }
        public ICommand CancelCommand { get; }

        public ImportViewModel()
        {
            ImportCommand = new RelayCommand(OnImportCommand, ImportCommandEnabled);
            CancelCommand = new RelayCommand(OnCancelCommand, CancelCommandEnabled);

            _BodBooks = new Dictionary<Guid, string>();
            _ImportableBulkOrderDeeds = new ObservableCollection<ImportableBulkOrderDeed>();

            var previousVersionInstallPath = GetPreviousVersionInstallPath();
            var collectionFile = Path.Combine(previousVersionInstallPath, "My BODs.xml");

            if (!File.Exists(collectionFile)) return;

            _CollectionFile = collectionFile;
            LoadCollection();
            _SuccessfullyLoaded = true;
        }

        private bool ImportCommandEnabled()
        {
            return !_IsImportActive && _ImportableBulkOrderDeeds.Count > 0;
        }

        private bool CancelCommandEnabled()
        {
            return !IsImportActive;
        }

        private void LoadCollection()
        {
            LoadCollectionFile();
            TotalBulkOrderDeeds = _ImportableBulkOrderDeeds.Count;

            var tailorBulkOrderDeeds = _ImportableBulkOrderDeeds.Where(b => b.Profession == _TailorProfession);

            TotalTailorBulkOrderDeeds = tailorBulkOrderDeeds.Count();
            TotalTailorSmallBulkOrderDeeds = tailorBulkOrderDeeds.Where(b => b is ImportableSmallBulkOrderDeed).Count();
            TotalTailorLargeBulkOrderDeeds = tailorBulkOrderDeeds.Where(b => b is ImportableLargeBulkOrderDeed).Count();

            var BlacksmithBulkOrderDeeds = _ImportableBulkOrderDeeds.Where(b => b.Profession == _BlacksmithProfession);

            TotalBlacksmithBulkOrderDeeds = BlacksmithBulkOrderDeeds.Count();
            TotalBlacksmithSmallBulkOrderDeeds = BlacksmithBulkOrderDeeds.Where(b => b is ImportableSmallBulkOrderDeed).Count();
            TotalBlacksmithLargeBulkOrderDeeds = BlacksmithBulkOrderDeeds.Where(b => b is ImportableLargeBulkOrderDeed).Count();
        }

        private void OnImportCommand(object parameter)
        {
            ImportedCount = 0;
            IsImportActive = true;

            if (_ResetCollection)
            {
                BulkOrderDeedManager.Instance.ClearBulkOrderDeedBooks();
                BulkOrderDeedManager.Instance.ClearCollection();
            }
            
            foreach (var bulkOrderDeedBook in _BodBooks)
            {
                BulkOrderDeedManager.Instance.AddBulkOrderDeedBook(new BulkOrderDeedBook(bulkOrderDeedBook.Key, bulkOrderDeedBook.Value));
            }

            var bulkOrderDeeds = new List<CollectionBulkOrderDeed>(_ImportableBulkOrderDeeds.Count);

            foreach (var bulkOrderDeed in _ImportableBulkOrderDeeds)
            {
                var collectionBulkOrderDeed = CreateCollectionBulkOrderDeed(bulkOrderDeed);

                ImportedCount++;

                if (collectionBulkOrderDeed == null) continue;

                bulkOrderDeeds.Add(collectionBulkOrderDeed);
            }

            BulkOrderDeedManager.Instance.AddBulkOrderDeeds(bulkOrderDeeds);
            IsImportActive = false;
            ((Window)parameter).Close();
        }

        private CollectionBulkOrderDeed CreateCollectionBulkOrderDeed(ImportableBulkOrderDeed bulkOrderDeed)
        {
            CollectionBulkOrderDeed retVal = null;

            var bulkOrderDeedType = bulkOrderDeed is ImportableSmallBulkOrderDeed ? BulkOrderDeedType.Small : BulkOrderDeedType.Large;
            string professionName = null;

            if (bulkOrderDeed.Profession == "Tailor")
            {
                professionName = "Tailoring";
            }
            else if (bulkOrderDeed.Profession == "Blacksmith")
            {
                professionName = "Blacksmithing";
            }

            var materialName = bulkOrderDeed.Material;

            if (materialName == "Golden")
            {
                materialName = "Gold";
            }

            if (professionName != null)
            {
                var profession = BulkOrderDeedManager.Instance.Professions.FirstOrDefault(p => String.Compare(p.Name, professionName, true) == 0);
                var bulkOrderDeedDefinition = BulkOrderDeedManager.Instance.GetBulkOrderDeedDefinition(profession.Name, bulkOrderDeed.Name, bulkOrderDeedType);
                var material = profession.BulkOrderDeedMaterials.Materials.FirstOrDefault(m => String.Compare(m.Name, materialName, true) == 0);

                if (bulkOrderDeed is ImportableSmallBulkOrderDeed smallBulkOrderDeed)
                {
                    retVal = new SmallCollectionBulkOrderDeed(smallBulkOrderDeed.Id, profession.Name, bulkOrderDeedDefinition.DisplayName, smallBulkOrderDeed.Quantity, smallBulkOrderDeed.Exceptional, material.Name, Guid.Empty, smallBulkOrderDeed.BulkOrderDeedBook, smallBulkOrderDeed.CompletedCount);
                }
                else if (bulkOrderDeed is ImportableLargeBulkOrderDeed largeBulkOrderDeed)
                {
                    var combined = new List<CollectionBulkOrderDeedItem>();

                    foreach (var smallBulkOrderDeedDefinition in ((LargeBulkOrderDeedDefinition)bulkOrderDeedDefinition).SmallBulkOrderDeedDefinitions)
                    {
                        var combinedSmallBulkOrderDeed = BulkOrderDeedManager.Instance.GetBulkOrderDeedDefinition(profession.Name, smallBulkOrderDeedDefinition.Name, BulkOrderDeedType.Small);
                        var isCombined = (largeBulkOrderDeed.Combined.Contains(combinedSmallBulkOrderDeed?.DisplayName));

                        combined.Add(new CollectionBulkOrderDeedItem(smallBulkOrderDeedDefinition.Name, largeBulkOrderDeed.Quantity, isCombined));
                    }

                    retVal = new LargeCollectionBulkOrderDeed(largeBulkOrderDeed.Id, profession.Name, bulkOrderDeedDefinition.DisplayName, largeBulkOrderDeed.Quantity, largeBulkOrderDeed.Exceptional, material.Name, Guid.Empty, largeBulkOrderDeed.BulkOrderDeedBook, combined);
                }
            }

            return retVal;
        }

        private void OnCancelCommand(object parameter)
        {
            ((Window)parameter).Close();
        }

        private void LoadCollectionFile()
        {
            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(_CollectionFile);
                LoadCollectionFileImpl(xmlDocument);
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCollectionFileImpl(XmlNode xmlNode)
        {
            lock (_SyncRoot)
            {
                LoadBODBooksInLock(xmlNode);
                LoadSmallBulkOrderDeedsInLock(xmlNode);
                LoadLargeBulkOrderDeedsInLock(xmlNode);


            }
        }

        private void LoadBODBooksInLock(XmlNode xmlNode)
        {
            var bodBooks = new Dictionary<Guid, string>();
            var bodBookNodes = xmlNode.SelectNodes("MyBODs/BODBooks/Book");

            foreach (var bodBookNode in bodBookNodes.OfType<XmlNode>())
            {
                try
                {
                    var id = Guid.Parse(bodBookNode.Attributes["ID"].Value);
                    var name = bodBookNode.Attributes["Name"].Value;

                    _BodBooks.Add(id, name);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void LoadSmallBulkOrderDeedsInLock(XmlNode xmlNode)
        {
            var bodNodes = xmlNode.SelectNodes("MyBODs/BODs/Small/BOD");

            foreach (var bodNode in bodNodes.OfType<XmlNode>())
            {

                try
                {
                    var id = Guid.Parse(bodNode.Attributes["ID"].Value);
                    var type = bodNode.Attributes["Type"].Value;  // Blacksmith or Tailor
                    var exceptional = bool.Parse(bodNode.Attributes["Exceptional"].Value);
                    var material = bodNode.Attributes["Material"].Value;
                    var name = bodNode.Attributes["Name"].Value;
                    var totalCount = int.Parse(bodNode.Attributes["TotalCount"].Value);
                    var completedCount = int.Parse(bodNode.Attributes["CompletedCount"].Value);
                    var bodBookString = bodNode.Attributes["BODBook"].Value;
                    var bodBookId = String.IsNullOrEmpty(bodBookString) ? Guid.Empty : Guid.Parse(bodBookString);

                    _ImportableBulkOrderDeeds.Add(new ImportableSmallBulkOrderDeed(id, type, exceptional, material, name, totalCount, completedCount, bodBookId));
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void LoadLargeBulkOrderDeedsInLock(XmlNode xmlNode)
        {
            var bodNodes = xmlNode.SelectNodes("MyBODs/BODs/Large/BOD");

            foreach (var bodNode in bodNodes.OfType<XmlNode>())
            {
                try
                {
                    var id = Guid.Parse(bodNode.Attributes["ID"].Value);
                    var type = bodNode.Attributes["Type"].Value;  // Blacksmith or Tailor
                    var exceptional = bool.Parse(bodNode.Attributes["Exceptional"].Value);
                    var material = bodNode.Attributes["Material"].Value;
                    var name = bodNode.Attributes["Name"].Value;
                    var totalCount = int.Parse(bodNode.Attributes["TotalCount"].Value);
                    var bodBookString = bodNode.Attributes["BODBook"].Value;
                    var bodBookId = String.IsNullOrEmpty(bodBookString) ? Guid.Empty : Guid.Parse(bodBookString);
                    var combinedNodes = bodNode.SelectNodes("Combined/BOD");
                    var combined = new List<string>();

                    foreach (var combinedNode in combinedNodes.OfType<XmlNode>())
                    {
                        var combinedName = combinedNode.Attributes["Name"].Value;

                        combined.Add(combinedName);
                    }

                    _ImportableBulkOrderDeeds.Add(new ImportableLargeBulkOrderDeed(id, type, exceptional, material, name, totalCount, bodBookId, combined));
                }
                catch (Exception ex)
                {
                }
            }
        }

        private static string GetPreviousVersionInstallPath()
        {
            var retVal = String.Empty;

            var products = ProductInstallation.GetProducts("{944871E7-9F8D-47B7-BE04-103E4C8F2254}", null, UserContexts.All);
            var product = products?.FirstOrDefault();
            var location = product?.LocalPackage;

            if (location != null)
            {
                var package = new InstallPackage(location, DatabaseOpenMode.ReadOnly);
                var componentId = package.Files.FirstOrDefault(kvp => kvp.Value.SourceName == "UO Bulk Order Deeds.exe").Key;

                if (componentId != null)
                {
                    var results = package.ExecuteQuery($"SELECT `ComponentId` FROM `Component` WHERE `Component` = 'C_{componentId}'");
                    var result = results?.Count > 0 ? results[0].ToString() : null;

                    if (result != null)
                    {
                        var componentInstallation = new ComponentInstallation(result);
                        var componentPath = componentInstallation.Path;

                        retVal = Path.GetDirectoryName(componentPath);
                    }
                }
            }

            return retVal;
        }
    }
}
