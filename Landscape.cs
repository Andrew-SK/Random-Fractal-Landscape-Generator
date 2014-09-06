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
        
        // some variables to tweak generator
        public float randScaleFactor = 1;
        public float positionScale = 2f;

        // end

        // height zones
        public float water;
        public float rock;
        public float snow;
        // end


        public int arraySize;
        public float[][] heightMap;
        public int recDepth;
        Random rand;
        
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

        public void diamondsquare()
        {
            // TODO top left is the top left corner of the 
            // sub array and subArraySize is the length of the new sub array 
            // this is all the information you need to get all the dimensions
            for (int sideLength = this.arraySize - 1; sideLength >= 2; sideLength /= 2)
            {
				// Diamond step
				// generates the middle value of the squares, making diamonds
				float avg;
				int halfSide = sideLength / 2;

                for (int y = halfSide; y <= this.arraySize - 1; y += sideLength)
                {
                    for (int x = halfSide; x <= this.arraySize - 1; x += sideLength)
                    {
                        avg = heightMap[x - halfSide][y - halfSide] // top left corner
                               + heightMap[x + halfSide][y - halfSide] // top right corner
                               + heightMap[x - halfSide][y + halfSide] // bottom left corner
                               + heightMap[x + halfSide][y + halfSide]; // bottom right corner 
                               
						avg /= 4;


						heightMap[x][y] = avg + ((rand.NextFloat(0f, 1f) - 0.5f) * (sideLength * randScaleFactor)); 

                    }
                }

                // Square step
                // generates the middle values of the diamonds, thus making squares
	

				// for loops map to the top left corner of the squares, each time the top and left 
				// sides are calculated. if the bottom or the right side are checked to be the end of the 
				// map then they are also computed.

                for (int y = 0; y < arraySize - 1; y += sideLength)
                {
                    for (int x = 0; x < arraySize - 1; x += sideLength)
                    {
						// calculate the top of the square
                        avg = heightMap[x][y] +
                              heightMap[x + sideLength][y] +
                              heightMap[x + halfSide][y + halfSide];
                        
                        if (y == 0)
                        {
							// you are on the top row 
							avg /= 3;
                        }
                        else
                        {
							// you are not on the top row 
                            avg += heightMap[x + halfSide][y - halfSide];
                            avg /= 4;
                        }

                        heightMap[x + halfSide][y] = avg + ((rand.NextFloat(0f, 1f) - 0.5f) * (sideLength * randScaleFactor));



						// calculate the left of the square
                        avg = heightMap[x][y] +
                                  heightMap[x][y + sideLength] +
                                  heightMap[x + halfSide][y + halfSide];

                        if (x == 0)
                        {
							// you are on the far left side of the map
                            avg /= 3;
                        }
                        else
                        {
							// you arn't on the far left side
                            avg += heightMap[x - halfSide][y + halfSide];
                            avg /= 4;
                        }

                        heightMap[x][y + halfSide] = avg + ((rand.NextFloat(0f, 1f) - 0.5f) * (sideLength * randScaleFactor));
						


                        // check to see if you are on the bottom row
                        if (y + sideLength == arraySize - 1)
                        {
							// on the bottom row, must calc the bottom of square
                            avg = heightMap[x][y + sideLength] +
                                  heightMap[x + sideLength][y + sideLength] +
                                  heightMap[x + halfSide][y + halfSide];
                            avg /= 3;

                            heightMap[x + halfSide][y + sideLength] = avg + ((rand.NextFloat(0f, 1f) - 0.5f) * (sideLength * randScaleFactor));
                        }

						// check to see if you are on the far right side
                        if (x + sideLength == arraySize - 1)
                        {
							// on the right side, must calc the right of the square
                            avg = heightMap[x + sideLength][y] +
                                  heightMap[x + sideLength][y + sideLength] +
                                  heightMap[x + halfSide][y + halfSide];
                            avg /= 3;

                            heightMap[x + sideLength][y + halfSide] = avg + ((rand.NextFloat(0f, 1f) - 0.5f) * (sideLength * randScaleFactor));
                        }

                    }
                }            
            }
        }

        public void calcColourZones()
        {
            // find the max and min heights
            float highest, lowest;
            lowest = Single.MaxValue;
            highest = Single.MinValue;


            for (int y = 0; y < arraySize; y++)
            {
                for (int x = 0; x < arraySize; x++)
                {
                    if (heightMap[x][y] > highest)
                    {
                        // this is the new highest height value
                        highest = heightMap[x][y];
                    }
                    if (heightMap[x][y] < lowest)
                    {
                        // this is the new lowest number
                        lowest = heightMap[x][y];
                    }
                }
            }

            float range = highest - lowest;

            // heights in the top 10 percent with be covered with snow
            snow = highest - range * 0.3f;

            // lowest 20 percent will be covered with water and coloured brown for mud.
            water = highest - range * 0.7f;

            // between the highest 20 percent and 10 percent will be rock
            rock = highest - range * 0.32f;

            // between water and rock will be grass

        }

        public VertexPositionColor createVertex(Vector3 pos)
        {
            Color col;
            if (pos.Y > this.snow)
            {
                col = Color.White;
            }
            else if (pos.Y > this.rock)
            {
                col = Color.LightGray;
            }
            else if (pos.Y > this.water)
            {
                col = Color.Green;
            }
            else
            {
                col = Color.SandyBrown;
            }

            return new VertexPositionColor(pos, col);
        }

        public Landscape(Project1Game game, float c1, float c2, float c3, float c4, int recDepth)
        {

			// setting up the random number generator
            this.rand = new Random();

            // recDepth of 0 would make a flat plane with only the 4 corner vertices
            // Determine the size of the array depending of the recursion depth
            this.arraySize = this.compArraySize(recDepth);
            this.recDepth = recDepth;

            // Creating array structure in order to store height values for all vertices
            // format will be heightMap[x][y] = z
            heightMap = new float[arraySize][];

            for (int i = 0; i < arraySize; i++){
            
                heightMap[i] = new float[arraySize];
            
            }

            // corner pattern:
            //   C1-----C2
            //   |		|
            //   C3-----C4

            // set the four corner values
            heightMap[0][0]                         = c1;
            heightMap[arraySize - 1][0]             = c2;
            heightMap[0][arraySize - 1]             = c3;
            heightMap[arraySize - 1][arraySize - 1] = c4;

			// Generate the rest of the height values 
            diamondsquare();

            // Calculate vertex colour heights
            calcColourZones();


            
            VertexPositionColor[] vertexArray = new VertexPositionColor[(arraySize - 1) * (6 + (arraySize - 2) * 6) + 4];
            
            int pos = 0;
			// Add the vertices to the array to make polygons
            for (int y = 0; y <= arraySize - 2; y++)
            {
                for (int x = 0; x <= arraySize - 1; x++)
                {

                    vertexArray[pos++] =  createVertex(new Vector3(x * positionScale, heightMap[x][y], y * positionScale));
                    if (x == 0)
                    {

                        vertexArray[pos++] =  createVertex(new Vector3(x * positionScale, heightMap[x][y + 1], (y + 1) * positionScale));
                        vertexArray[pos++] =  createVertex(new Vector3((x + 1) * positionScale, heightMap[x + 1][y], y * positionScale));
                        
                    }
                    else if (x == arraySize - 1)
                    {

                        vertexArray[pos++] = createVertex(new Vector3((x - 1) * positionScale, heightMap[x - 1][y + 1], (y + 1) * positionScale));
                        vertexArray[pos++] = createVertex(new Vector3(x * positionScale, heightMap[x][y + 1], (y + 1) * positionScale));
                        
                    }
                    else
                    {
                        vertexArray[pos++] = createVertex(new Vector3(x * positionScale, heightMap[x][y + 1], (y + 1) * positionScale));
                        vertexArray[pos++] = createVertex(new Vector3((x + 1) * positionScale, heightMap[x + 1][y], y * positionScale));


                        vertexArray[pos++] = createVertex(new Vector3(x * positionScale, heightMap[x][y], y * positionScale));
                        vertexArray[pos++] = createVertex(new Vector3((x - 1) * positionScale, heightMap[x - 1][y + 1], (y + 1) * positionScale));
                        vertexArray[pos++] = createVertex(new Vector3(x * positionScale, heightMap[x][y + 1], (y + 1) * positionScale));
                        
                    }
                }
            }

            // these vertices are for the water 
            // TODO add water vertices 
            vertexArray[pos++] = new VertexPositionNormalTexture()

                vertices = Buffer.Vertex.New(

                    game.GraphicsDevice,
                    vertexArray
                    );

            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 10000.0f),
                World = Matrix.Identity,
            };


            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
            this.game = game;
        }
        

        public override void Update(GameTime gameTime)
        {

            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            
            

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

        public override string ToString()
        {
			String outStr = "";
            for (int y = 0; y < arraySize; y++)
            {
                outStr += "[ ";
                for (int x = 0; x < arraySize; x++)
                {
                    outStr += heightMap[x][y] + ", ";
                }

				outStr += "]\n";
            }

            outStr += "arraySize = " + arraySize;

            return outStr;
        }
    }
}
