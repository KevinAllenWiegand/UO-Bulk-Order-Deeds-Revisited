# Introduction
This program is the successor to the UO Bulk Order Deeds program I wrote years ago that supported Blacksmithing and Tailoring bulk order deeds.  Since Broadsword has added additional professions and changed the way rewards worked, this new version is set to handle those new changes.

# Getting Started
Not much to do here.  It's a standalone app.  If you download the source, the WPF application is the starting point.

# Build and Test
The code was written with Visual Studio 2017.  I don't know if Visual Studio Code will work or not.  You're on on your own on this part, sorry.

# Importing Bulk Order Deeds
A framework for importing Bulk Order Deeds exists.  This is done via loadable plugins.  For developers, you can look at the Npe.UO.BulkOrderDeeds.SampleImportPlugin to see what is available to you to import from your own format.  At this time, there are no built-in import plugins, but there will be an "Import from Previous Version" plugin at a future date.
The basic procedure for creating an import plugin is as follows:
1) Create a new .NET project, and reference Npe.UO.BulkOrderDeeds.  Create a class that inherits from Npe.UO.BulkOrderDeeds.Plugins.ImportPlugin.  Implement the DisplayName property and the Import() method.
2) Once your plugin is built and available, copy it to "%AppData%\NinjaPuffer Enterprises\UO Bulk Order Deeds Revisited\Plugins".  When opening the application, your plugin should be located and available from the "Import" button from the "Collections" view.

As stated, there is a sample import plugin that you can look at/use.  Please be aware that if you use the sample plugin, it will OVERWRITE your Vendors / Bulk Order Deed Books / Collection, so please use it with caution.

# Contribute
I am not taking direct contributions to the code, but if you have suggestions, please email me at greatpumpkinator@hotmail.com.

# License
This project is licensed under the MIT License - see the LICENSE.md file for details.  Note...the provisions of the license states that you can "sell copies of the Software".  Lemme say one thing:  This is free software - if you try and sell it, you're a douchebag, and you will burn in hell for eternity.  Don't sell it.  Not that anyone would pay for it, but don't do it, you're a fucktard if you do.