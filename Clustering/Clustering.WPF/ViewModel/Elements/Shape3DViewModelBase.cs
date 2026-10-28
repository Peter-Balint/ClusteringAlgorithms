
namespace Clustering.WPF.ViewModel
{
    internal abstract class Shape3DViewModelBase : ViewModelBase
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public double Z { get; protected set; }
        public int Id { get; protected set; }
    }
}
