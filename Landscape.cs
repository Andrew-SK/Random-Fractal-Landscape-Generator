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
        public Landscape(Project1Game game)
        {
            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
                new[]
                    {
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange), // Front
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange), // BACK
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.OrangeRed), // Top
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.OrangeRed), // Bottom
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.DarkOrange), // Left
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.DarkOrange), // Right
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.DarkOrange),


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

            //basicEffect.World = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);
            basicEffect.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            
            // Camera controls 
            if (game.keyboardState.IsKeyDown(Keys.D))
            {

                // The D key is down so translate the camera 
                // to the right at a right angle to the target vector
                Vector3 trans = new Vector3(game.cameraTarget.Z - game.cameraEye.Z, 0f, game.cameraTarget.X - game.cameraEye.X);
                trans.Normalize();
                game.cameraEye += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * 3f;
                game.cameraTarget += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * 3f;
            }
            if (game.keyboardState.IsKeyDown(Keys.A))
            {
                // The A key is down so translate the camera 
                //  to the left at a right angle to the target vector
                Vector3 trans = new Vector3(game.cameraTarget.Z - game.cameraEye.Z, 0f, game.cameraTarget.X - game.cameraEye.X);
                trans.Normalize();
                game.cameraEye -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * 3f;
                game.cameraTarget -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * 3f;
            }
            if (game.keyboardState.IsKeyDown(Keys.W))
            {
                // Move the camera forward towards the target
                Vector3 trans = new Vector3(game.cameraTarget.X - game.cameraEye.X,
                                            game.cameraTarget.Y - game.cameraEye.Y, 
                                            game.cameraTarget.Z - game.cameraEye.Z);
                trans.Normalize();
                game.cameraEye += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * 3f;
            }
            if (game.keyboardState.IsKeyDown(Keys.S))
            {
                // Move the camera backward away from the target
                Vector3 trans = new Vector3(game.cameraTarget.X - game.cameraEye.X,
                                            game.cameraTarget.Y - game.cameraEye.Y,
                                            game.cameraTarget.Z - game.cameraEye.Z);
                trans.Normalize();
                game.cameraEye -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * 3f;
            }

        }

        public override void Draw(GameTime gameTime)
        {

            // update basicEffect View matrix 
            basicEffect.View = Matrix.LookAtLH(game.cameraEye, game.cameraTarget, game.cameraUp);
            
            // Setup the vertices
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the rotating cube
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }
    }
}
