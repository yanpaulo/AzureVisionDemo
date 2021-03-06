﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace AzureVisionDemo.UWPApp
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AnalisysView : Page
    {
        private const string
            AzureLocation = "eastus2",
            AzureKey = "df1b001a919943b9b0195fd29cadf2e8";

        private AnalisysParameters _parameters;

        public string QueryResult { get; set; }
        public BitmapImage ImageBitmap { get; set; }

        public AnalisysView()
        {
            this.InitializeComponent();
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var ws = new WebService(AzureLocation, AzureKey);
            using (var stream = await _parameters.StorageFile.OpenAsync(FileAccessMode.Read))
            {
                ImageBitmap = new BitmapImage();
                await ImageBitmap.SetSourceAsync(stream);
                Bindings.Update();

            }
            try
            {
                using (var stream = await _parameters.StorageFile.OpenStreamForReadAsync())
                {

                    var result = await ws.AnalyzeImageAsync(stream, _parameters.VisualFeatures, _parameters.VisualDetails);

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.Indented
                    };
                    QueryResult = JsonConvert.SerializeObject(result, settings);
                    LoadingProgressRing.Visibility = Visibility.Collapsed;
                    Bindings.Update();
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _parameters = (AnalisysParameters)e.Parameter;
            Bindings.Update();

            base.OnNavigatedTo(e);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var currentState = VisualStateManager.GetVisualStateGroups(RootGrid).First().CurrentState;

            if (currentState?.Name == "WideView")
            {
                PhotoGrid.Width = e.NewSize.Width / 2;
                PhotoGrid.Height = double.NaN;
            }
            else
            {
                PhotoGrid.Width = double.NaN;
                PhotoGrid.Height = e.NewSize.Height / 3;
            }
        }

    }
}
