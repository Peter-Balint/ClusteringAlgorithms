using Clustering.Model;
using Clustering.ViewModel;
using Clustering.ViewModel.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Clustering
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MainModel _model;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainModel>();
            services.AddSingleton<NavigationStore>();

            services.AddTransient<ParametersViewModel>();
            services.AddTransient<SetupViewModel>();
            services.AddTransient <ResultViewModel>();
            services.AddTransient<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            CreateParametersNavigationService(_serviceProvider).Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateParametersNavigationService(serviceProvider),
                CreateSetupNavigationService(serviceProvider),
                CreateResultNavigationService(serviceProvider));
        }

        private INavigationService CreateParametersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<ParametersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ParametersViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>()
                );
        }

        private INavigationService CreateSetupNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<SetupViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SetupViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>()
                );
        }

        private INavigationService CreateResultNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<ResultViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ResultViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>()
                );
        }
        
    }

}
