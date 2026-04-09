using System.ComponentModel;

namespace Clustering.ViewModel
{

    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose() { }

        public event EventHandler<bool>? NavigatabilityChanged;
        protected void DisableNavigation()
        {
            NavigatabilityChanged?.Invoke(this, false);
        }
        protected void EnableNavigation()
        {
            NavigatabilityChanged?.Invoke(this, true);
        }
    }
}
