
using System.Windows;

namespace Clustering.ViewModel.Navigation
{
    public class NavigateCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            if(parameter != null && parameter is string warning)
            {
                if(MessageBox.Show(warning, "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    _navigationService.Navigate();
                }
                return;
            }
            _navigationService.Navigate();
        }
    }
}
