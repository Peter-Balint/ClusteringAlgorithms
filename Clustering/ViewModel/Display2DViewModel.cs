
using System.Collections.ObjectModel;

namespace Clustering.ViewModel
{
    class Display2DViewModel : ViewModelBase
    {
        public int CanvasHeight { get; set; }
        public int CanvasWidth { get; set; }

        //mivel observable collection nem adhatja vissza egyből a model egy collectionjét
        //akkor a model pont létrehozásáról és törléséről jelez eventtel?
        public ObservableCollection<CircleViewModel> Points {  get; set; }


        private void ScalePoints(double deltaX, double deltaY, double? newDiameter = null)
        {
            foreach (CircleViewModel point in Points)
            {
                point.Scale(deltaX, deltaY, newDiameter);
            }
        }

    }
}
