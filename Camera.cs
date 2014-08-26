using System;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;

namespace Project1
{
    public class Camera : GameObject
    {

        public Vector3 eye;
        public Vector3 target;
        public Vector3 up;

        private readonly float speed = 5f;

        public Camera(Vector3 eye, Vector3 Target, Vector3 Up, Project1Game game) {
            this.eye = eye;
            this.target = Target;
            this.up = Up;
            
            this.game = game;
        }

        public override void Update(GameTime gameTime)
        {
            // Camera keyboard controls 
            #region Camera keyboard controls
            if (game.keyboardState.IsKeyDown(Keys.D))
            {

                // The D key is down so translate the camera 
                // to the right at a right angle to the target vector
                Vector3 trans = new Vector3(this.target.Z - this.eye.Z,
                                            0f, 
                                            this.target.X - this.eye.X);
                trans.Normalize();
                this.eye += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                this.target += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            if (game.keyboardState.IsKeyDown(Keys.A))
            {
                // The A key is down so translate the camera 
                //  to the left at a right angle to the target vector
                Vector3 trans = new Vector3(this.target.Z - this.eye.Z, 
                                            0f, 
                                            this.target.X - this.eye.X);
                trans.Normalize();
                this.eye -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                this.target -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            if (game.keyboardState.IsKeyDown(Keys.W))
            {
                // Move the camera forward towards the target
                Vector3 trans = new Vector3(this.target.X - this.eye.X,
                                            this.target.Y - this.eye.Y, 
                                            this.target.Z - this.eye.Z);
                trans.Normalize();
                this.eye += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            if (game.keyboardState.IsKeyDown(Keys.S))
            {
                // Move the camera backward away from the target
                Vector3 trans = new Vector3(this.target.X - this.eye.X,
                                            this.target.Y - this.eye.Y,
                                            this.target.Z - this.eye.Z);
                trans.Normalize();
                this.eye -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            #endregion
          

        }
        
        public override void Draw(GameTime gameTime) {   }
    }
}
