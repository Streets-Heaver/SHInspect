using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SHInspect.Classes
{
    public class MyVirtualizingStackPanel : VirtualizingStackPanel
    {
        /// <summary>
        /// Publically expose BringIndexIntoView.
        /// </summary>
        public void BringIntoView(int index)
        {
            this.BringIndexIntoView(index);
        }
    }
}
