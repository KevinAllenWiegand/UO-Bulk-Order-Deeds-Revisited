using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Npe.UO.BulkOrderDeeds.SampleImportPlugin
{
    public class ImportViewModel : ViewModelBase
    {
        private const int _DefaultCount = 1000;

        private string _Count = _DefaultCount.ToString();
        public string Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                NotifyPropertyChanged(nameof(Count));
            }
        }

        public ICommand AddRandomItemsCommand { get; }

        public ImportViewModel()
        {
            AddRandomItemsCommand = new RelayCommand(OnAddRandomItemsCommand);
        }

        private void OnAddRandomItemsCommand(object parameter)
        {
            var collection = new List<CollectionBulkOrderDeed>(BulkOrderDeedManager.Instance.Collection);

            if (collection.Count > 0)
            {
                BulkOrderDeedManager.Instance.RemoveBulkOrderDeeds(collection);
            }

            var vendors = new List<Vendor>(BulkOrderDeedManager.Instance.Vendors);

            foreach (var vendor in vendors)
            {
                BulkOrderDeedManager.Instance.RemoveVendor(vendor);
            }

            var bulkOrderDeedBooks = new List<BulkOrderDeedBook>(BulkOrderDeedManager.Instance.BulkOrderDeedBooks);

            foreach (var bulkOrderDeedBook in bulkOrderDeedBooks)
            {
                BulkOrderDeedManager.Instance.RemoveBulkOrderDeedBook(bulkOrderDeedBook);
            }

            if (!int.TryParse(_Count, out int count))
            {
                count = _DefaultCount;
            }

            // Add a standalone BOD Book.
            var unsortedBook = new BulkOrderDeedBook("Unsorted");
            BulkOrderDeedManager.Instance.AddBulkOrderDeedBook(unsortedBook);

            // Add a Vendor with a BOD Book.
            var chloeVendor = new Vendor("Chloe");
            var forSaleBook = new BulkOrderDeedBook("For Sale");

            chloeVendor.AddBulkOrderDeedBook(forSaleBook);
            BulkOrderDeedManager.Instance.AddVendor(chloeVendor);

            var bulkOrderDeeds = new List<CollectionBulkOrderDeed>();
            var random = new Random();

            for (var index = 0; index < count; index++)
            {
                var professionIndex = random.Next(BulkOrderDeedManager.Instance.Professions.Count());
                var profession = BulkOrderDeedManager.Instance.Professions.ToArray()[professionIndex];
                var bulkOrderDeedDefinitionIndex = random.Next(profession.BulkOrderDeedDefinitions.Definitions.Count());
                var bulkOrderDeedDefinition = profession.BulkOrderDeedDefinitions.Definitions.ToArray()[bulkOrderDeedDefinitionIndex];
                var exceptional = bulkOrderDeedDefinition.CanBeExceptional ? random.Next(2) == 1 : false;
                var bulkOrderDeedMaterial = profession.BulkOrderDeedMaterials?.Materials?.FirstOrDefault();
                var quantity = BulkOrderDeedManager.PossibleQuantities[random.Next(BulkOrderDeedManager.PossibleQuantities.Length)];
                var vendor = Vendor.None;
                var bulkOrderDeedBook = BulkOrderDeedBook.None;

                var onVendor = random.Next(2) == 1;

                if (onVendor)
                {
                    vendor = chloeVendor;

                    var inBook = random.Next(2) == 1;

                    if (inBook)
                    {
                        bulkOrderDeedBook = chloeVendor.BulkOrderDeedBooks.First();
                    }
                }
                else
                {
                    var standaloneBook = random.Next(2) == 1;

                    if (standaloneBook)
                    {
                        bulkOrderDeedBook = unsortedBook;
                    }
                }

                if (bulkOrderDeedDefinition.CanHaveMaterial)
                {
                    var bulkOrderDeedMaterialIndex = random.Next(profession.BulkOrderDeedMaterials.Materials.Count());

                    bulkOrderDeedMaterial = profession.BulkOrderDeedMaterials.Materials.ToArray()[bulkOrderDeedMaterialIndex];
                }

                if (bulkOrderDeedDefinition is SmallBulkOrderDeedDefinition smallBulkOrderDeedDefinition)
                {
                    var collectionBulkOrderDeed = new SmallCollectionBulkOrderDeed(profession, smallBulkOrderDeedDefinition, quantity, exceptional, bulkOrderDeedMaterial, vendor, bulkOrderDeedBook, random.Next(quantity));

                    bulkOrderDeeds.Add(collectionBulkOrderDeed);
                }

                if (bulkOrderDeedDefinition is LargeBulkOrderDeedDefinition largeBulkOrderDeedDefinition)
                {
                    var completedStates = new Dictionary<SmallBulkOrderDeedDefinition, bool>();

                    foreach (var largeBulkOrderDeedDefinitionItem in largeBulkOrderDeedDefinition.SmallBulkOrderDeedDefinitions)
                    {
                        completedStates[largeBulkOrderDeedDefinitionItem] = random.Next(2) == 1;
                    }

                    var collectionBulkOrderDeed = new LargeCollectionBulkOrderDeed(profession, largeBulkOrderDeedDefinition, quantity, exceptional, bulkOrderDeedMaterial, vendor, bulkOrderDeedBook, completedStates);

                    bulkOrderDeeds.Add(collectionBulkOrderDeed);
                }
            }

            BulkOrderDeedManager.Instance.AddBulkOrderDeeds(bulkOrderDeeds);
            ((Window)parameter).Close();
        }
    }
}
