using Prism.Unity;
using Prism.Ioc;
using System.Windows;
using Dany2000.Views;
using Prism.Mvvm;
using Dany2000.ViewModels;
using Dany2000.Api;
using Dany2000.Business;
using Dany2000.Models;
using MvvmDialogs;

namespace Dany2000
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
     
        protected override Window CreateShell()
        {
            return Container.Resolve<ManagementWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISongsManager, SongsManager>();
            containerRegistry.Register<IPlaylistManager, PlaylistManager>();
            containerRegistry.Register<ISongPlayer, SongPlayer>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<ManagementWindow, ManagementViewModel>();
            ViewModelLocationProvider.Register<DisplayWindow, DisplayViewModel>();
        }
    }
}
