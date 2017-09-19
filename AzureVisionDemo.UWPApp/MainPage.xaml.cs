using AzureVisionDemo.UWPApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace AzureVisionDemo.UWPApp
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel _viewModel;

        public MainPage()
        {
            DataContext = _viewModel = new MainPageViewModel();
            this.InitializeComponent();
        }

        private async void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();

            var photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {
                NavigateToAnalisys(photo);
            }
        }
        
        private async void PickButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var photo = await picker.PickSingleFileAsync();
            if (photo != null)
            {
                NavigateToAnalisys(photo);
            }
        }

        private void NavigateToAnalisys(StorageFile file)
        {
            var parameters = new AnalisysParameters { StorageFile = file };
            foreach (var item in _viewModel.VisualFeaturesList.Where(f => f.IsSelected))
            {
                parameters.VisualFeatures |= (VisualFeatures)item.Value;
            }

            foreach (var item in _viewModel.VisualDetailsList.Where(f => f.IsSelected))
            {
                parameters.VisualDetails |= (VisualDetails)item.Value;
            }

            Frame.Navigate(typeof(AnalisysView), parameters);
        }
    }
}
