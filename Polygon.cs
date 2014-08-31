using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project1
{
    using SharpDX.Toolkit.Graphics;
    class Polygon
    {
        ArrayList vertexList;
        Landscape landscape;


        public Polygon(Landscape landscape)
        {
            vertexList = new ArrayList();
            this.landscape = landscape;

            

        }

        public void Add(float x, float y, float z)
        {
            if (vertexList.Count < 3)
            {

            }
        }


    }
}
