using Clustering.Model;
using Clustering.ViewModel;
using Clustering.ViewModel.Navigation;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<CloseModalNavigationService>();

            services.AddTransient<ParametersViewModel>(s =>
                new ParametersViewModel(s.GetRequiredService<MainModel>(), CreateSetupNavigationService(s)));
            services.AddTransient<SetupViewModel>(s => 
                new SetupViewModel(s.GetRequiredService<MainModel>(), CreateProgressNavigationService(s)));
            services.AddTransient<ResultsViewModel>();
            services.AddTransient<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddTransient<ProgressViewModel>(CreateProgressViewModel);
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
                CreateResultsNavigationService(serviceProvider));
        }

        private ProgressViewModel CreateProgressViewModel(IServiceProvider serviceProvider)
        {
            CompositeNavigationService navigationService = new CompositeNavigationService(
                serviceProvider.GetRequiredService<CloseModalNavigationService>(),
                CreateResultsNavigationService(serviceProvider));
            return new ProgressViewModel(serviceProvider.GetRequiredService<MainModel>(), navigationService);
        }

        private INavigationService CreateProgressNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<ProgressViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<ProgressViewModel>());
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

        private INavigationService CreateResultsNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<ResultsViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ResultsViewModel>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>()
                );
        }
        
    }

}
