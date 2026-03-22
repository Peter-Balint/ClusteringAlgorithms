using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model
{
    //figure out a more generic way later
    public class ParameterSet
    {
        public DisplayMode DisplayMode {  get; private set; }
        public void SetDisplayMode(DisplayMode displayMode)
        {
            DisplayMode = displayMode;
        }
    }

    internal enum ParameterType { Integer, FloatingPoint, Boolean }
    public enum DisplayMode { Spatial2D, Spatial3D, RGBA}
}
