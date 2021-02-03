﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Tick42.Windows;

namespace Contacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RegisterToStickyWindows();
            ContactInfoGrid.ItemsSource = GetContacts();
        }

        private void RegisterToStickyWindows()
        {
            //  1. Try to get startup options passed from GD. Create our default options if there aren't any passed options. Make your app a sticky flat window with title Clients
            //  Hint - you can use the placement object for your default config and you can get the startup options from Glue.StickyWindows.GetStartupOptions();
          
            var gwOptions = App.Glue.GlueWindows.GetStartupOptions();
            if (gwOptions == null)
            {
               
                gwOptions = new GlueWindowOptions();
                var placement = new GlueWindowScreenPlacement();
                var bounds = new GlueWindowBounds
                {
                    Width = 510,
                    Height = 450
                };
                placement.WithBounds(bounds);
                gwOptions
                    .WithId(Guid.NewGuid().ToString())
                    .WithPlacement(placement);
            }
          
            gwOptions
                .WithType(GlueWindowType.Flat)
                .WithTitle("Contacts");

            App.Glue.GlueWindows.RegisterWindow(this, gwOptions);
        }

        private IEnumerable<ContactInfo> GetContacts()
        {
            IEnumerable<ContactInfo> contacts = new ObservableCollection<ContactInfo>();
            // 5 Invoke the FindWhoToCall method and use the returned value to populate the grid
            var contactsProxy = App.Glue.Interop.CreateServiceProxy<IContactsService>();

            if (App.Glue.Interop.IsServiceAvailable(contactsProxy))
            {
                contacts = contactsProxy.FindWhoToCall();
            }

            return contacts;
        }

    }
}
