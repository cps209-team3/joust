////////////////////////////////////////////////////////
// filename: PlatformControl.cs
// contents: GUI element for Platforms created in-game
//
////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JoustModel;

namespace JoustClient
{
    /// <summary>
    /// a specific kind of WorldObjectControl that is used by the various kinds of platforms
    /// </summary>
    public class PlatformControl : WorldObjectControl
    {
        /// <summary>
        /// provides a positioned GUI element for a platform
        /// </summary>
        /// <param name="imagePath"></param>
        public PlatformControl(string imagePath) : base(imagePath)
        {
            Height = 30;
            Width = 200;
        }

        /// <summary>
        /// sizes the GUI platforms components to the correct dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Resize(int width, int height) {
            Width = width;
            Height = height;
        }
    }
}
