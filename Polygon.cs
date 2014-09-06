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

    public struct coords
    {
        public float x, y, z;

        public coords(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

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
                vertexList.Add(new coords(x, y, z));
            }
        }

        public VertexPositionColor output(int i)
        {
            coords pos = (coords)vertexList[i];
            Color col;

            if (pos.z > landscape.snow) {
                col = Color.White;
            } else if (pos.z > landscape.rock) {
                col = Color.LightGray;
            } else if (pos.z > landscape.water) {
                col = Color.LightGreen;
            } else {
                col = Color.Brown;
            }

            return new VertexPositionColor(new Vector3(pos.x, pos.y, pos.z), col);
        }


    }
}
