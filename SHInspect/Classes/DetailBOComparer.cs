using System;
using System.Collections.Generic;
using System.Text;

namespace SHInspect.Classes
{
    public class DetailBOComparer : IEqualityComparer<DetailBO>
    {
        public bool Equals(DetailBO x, DetailBO y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(DetailBO obj)
        {
            return obj.GetHashCode();
        }
    }
}
