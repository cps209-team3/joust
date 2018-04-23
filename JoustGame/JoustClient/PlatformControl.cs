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
    public class PlatformControl : WorldObjectControl
    {
        public PlatformControl(string imagePath) : base(imagePath)
        {
            Height = 30;
            Width = 200;
        }

        public void Resize(int width, int height) {
            Width = width;
            Height = height;
        }
    }
}
