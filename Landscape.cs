using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

using SharpDX.Toolkit.Input;

namespace Project1
{
    using SharpDX.Toolkit.Graphics;
    class Landscape : ColoredGameObject
    {
        public int arraySize;
        public int compArraySize(int rDepth)
        {
            if (rDepth == 0)
            {
                return 2;
            }
            else
            {
                return (compArraySize(rDepth - 1) * 2) - 1;
            }
        }

        public void diamondsquare(ref float[][] heightMap, int topLeft, int topRight, int botLeft, int botRight)
        {

        }

        public Landscape(Project1Game game, float c1, float c2, float c3, float c4, int recDepth)
        {

            // recDepth of 0 would make a flat plane with only the 4 corner vertices
            // Determine the size of the array depending of the recursion depth
            arraySize = this.compArraySize(recDepth);

            // Creating array structure in order to store height values for all vertices
            // format will be heightMap[x][y] = z
            float[][] heightMap = new float[arraySize][];

            for (int i = 0; i < arraySize; i++){
            
                heightMap[i] = new float[arraySize];
            
            }

            // set the four corner values
            heightMap[0][0]                         = c1;
            heightMap[arraySize - 1][0]             = c2;
            heightMap[0][arraySize - 1]             = c3;
            heightMap[arraySize - 1][arraySize - 1] = c4;



            vertices = Buffer.Vertex.New(
                           
                game.GraphicsDevice,
                new[]
                    {
                        // E.g.
                        //new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange), // Front
                        //new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.Orange),

                    });

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
                World = Matrix.Identity
            };

            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            this.game = game;
        }
        

        public override void Update(GameTime gameTime)
        {
            // Rotate the cube.
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            basicEffect.World = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
            basicEffect.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            
            

        }

        public override void Draw(GameTime gameTime)
        {

            // update basicEffect View matrix 
            basicEffect.View = Matrix.LookAtLH(game.camera.eye, game.camera.target, game.camera.up);

            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the rotating cube
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }


    }
}
