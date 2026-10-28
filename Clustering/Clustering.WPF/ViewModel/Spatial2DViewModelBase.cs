

using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    internal abstract class Spatial2DViewModelBase : ViewModelBase
    {
        protected double _offsetX = 0;
        protected double _offsetY = 0;

        protected double _dragStartX;
        protected double _dragStartY;

        protected int _baseDiameter = 30;

        protected double _zoomFactor = 1;

        public ICommand? CanvasClickedCommand { get; protected set; }
        public ICommand? MouseMoveCommand { get; protected set; }
        public ICommand? CanvasReleasedCommand { get; protected set; }
        public ICommand? ZoomCommand { get; protected set; }

        protected virtual void OnCanvasClickedDrag(MouseButtonEventArgs? m)
        {
            if (m is null || m.Source is not Canvas) return;

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseDrag);
            OnPropertyChanged(nameof(MouseMoveCommand));

            _dragStartX = m.GetPosition((IInputElement)m.Source).X;
            _dragStartY = m.GetPosition((IInputElement)m.Source).Y;
        }

        protected void OnMouseDrag(MouseEventArgs? m)
        {
            if (m is null || m.Source is not Canvas) return;

            var currentLocation = m.GetPosition((IInputElement)m.Source);

            double offsetIncrementX = currentLocation.X - _dragStartX;
            double offsetIncrementY = currentLocation.Y - _dragStartY;
            _offsetX -= offsetIncrementX;
            _offsetY -= offsetIncrementY;
            ScalePoints();
            _dragStartX = currentLocation.X;
            _dragStartY = currentLocation.Y;
        }

        protected void OnDragReleased()
        {
            MouseMoveCommand = null;
            OnPropertyChanged(nameof(MouseMoveCommand));
        }

        protected void OnCanvasScrolling(MouseWheelEventArgs? m)
        {
            if (m is null) return;

            if (m.Delta > 0 && _zoomFactor <= 2.9)
            {
                _zoomFactor += 0.1;
                ScalePoints();
            }
            else if (m.Delta < 0 && _zoomFactor >= 0.2)
            {
                _zoomFactor -= 0.1;
                ScalePoints();
            }
        }

        protected abstract void ScalePoints();
    }
}
