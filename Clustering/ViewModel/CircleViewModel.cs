
namespace Clustering.ViewModel
{
    class CircleViewModel : ViewModelBase
    {
        //itt sem direkt referencia lesz valszeg a modellbeli pontra
        //lehet itt nem kell notification, mert mindig változáskor új pont lesz
        public double Diameter { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public void Scale(double deltaX, double deltaY, double? newDiameter = null)
        {
            //math it out how to translate the model's coordinate to the canvases, including zooming
            //otherwise simply offsetting it would work
            if(newDiameter is null)
            {
                X += deltaX;
                Y += deltaY;
                return;
            }
        }
    }
}
