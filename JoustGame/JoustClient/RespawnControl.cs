////////////////////////////////////////////////////////
// filename: RespawnControl.cs
// contents: GUI element for respawn platforms created in-game
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
    public class RespawnControl : WorldObjectControl
    {
        /// <summary>
        /// takes the imaagePath to provide a positioned GUI representation for a respawn control
        /// </summary>
        /// <param name="imagePath"></param>
        public RespawnControl(string imagePath) : base(imagePath)
        {
            Height = 15;
            Width = 100;
        }
    }
}
