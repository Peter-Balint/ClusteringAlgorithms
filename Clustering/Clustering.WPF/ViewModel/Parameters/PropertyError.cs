
using System.Collections.ObjectModel;

namespace Clustering.ViewModel
{
    public class PropertyError
    {
        public string PropertyName { get; }
        public ObservableCollection<string> ErrorMessages { get; }

        public PropertyError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessages = new ObservableCollection<string>([errorMessage]);
        }
    }
}
