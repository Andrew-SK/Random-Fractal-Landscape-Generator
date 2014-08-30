using System;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace Project1
{
    public class Camera : GameObject
    {

        public Vector3 eye;
        public Vector3 target;
        public Vector3 up;
        public Vector3 reference;
        public float pitch;
        public float yaw;


        private readonly float speed = 10f;

        public Camera(Vector3 eye, Vector3 Target, Vector3 Up, Project1Game game) {
            this.eye = eye;
            this.target = Target;
            this.up = Up;
            this.reference = new Vector3(0, 0, 1);

            this.pitch = 0;
            this.yaw = 0;

            this.game = game;
        }

        public override void Update(GameTime gameTime)
        {
           

            // Camera mouse controls
            #region Camera mouse controls 

            
            float angleX = (float)Math.Asin(game.mouseState.X - 0.5f);
            float angleY = (float)Math.Asin(game.mouseState.Y - 0.5f);
            
            this.yaw += angleX;
            this.pitch += angleY;

            // limit the pitch to straight up and down

            if (this.pitch > Math.PI / 2)
            {
                this.pitch = (float)Math.PI / 2;
            }
            else if (this.pitch < -Math.PI / 2)
            {
                this.pitch = (float)-Math.PI / 2;
            }
	    
	        // wrap around for X rotations to avoid complications

	        if (this.yaw > 2 * Math.PI) {
		        this.yaw -= (float)(2 * Math.PI);
	        }

            Matrix cameraRotation = Matrix.RotationX(this.pitch) * Matrix.RotationY(this.yaw);
            Vector4 rotatedReferenceV4 = Vector3.Transform(this.reference, cameraRotation);
            Vector3 rotatedReference = new Vector3(
                                                       rotatedReferenceV4.X,
                                                       rotatedReferenceV4.Y,
                                                       rotatedReferenceV4.Z
                                                  );

            this.target = this.eye + rotatedReference;




            game.mouseManager.SetPosition(new Vector2(0.5f, 0.5f));
            #endregion

            // Camera keyboard controls 
            #region Camera keyboard controls
            Vector3 forward = (Vector3)Vector3.Transform(this.reference, Matrix.RotationY(this.yaw));

            if (game.keyboardState.IsKeyDown(Keys.D))
            {

                // The D key is down so translate the camera 
                // to the right at a right angle to the target vector

                Vector3 translationVector = (Vector3)Vector3.Transform(forward, Matrix.RotationY((float)Math.PI / 2));
                translationVector.Normalize();
                translationVector /= 4f;
                
                this.eye = (Vector3)Vector3.Transform(this.eye, Matrix.Translation(translationVector));
                this.target = (Vector3)Vector3.Transform(this.target, Matrix.Translation(translationVector));

            }
            if (game.keyboardState.IsKeyDown(Keys.A))
            {
                // The A key is down so translate the camera 
                //  to the left at a right angle to the forward vector
                Vector3 translationVector = (Vector3)Vector3.Transform(forward, Matrix.RotationY(-(float)Math.PI / 2));
                translationVector.Normalize();
                translationVector /= 4f;

                this.eye = (Vector3)Vector3.Transform(this.eye, Matrix.Translation(translationVector));
                this.target = (Vector3)Vector3.Transform(this.target, Matrix.Translation(translationVector));
            }
            if (game.keyboardState.IsKeyDown(Keys.W))
            {
                // Move the camera forward towards the target
                Vector3 trans = new Vector3(this.target.X - this.eye.X,
                                            this.target.Y - this.eye.Y, 
                                            this.target.Z - this.eye.Z);
                trans.Normalize();
                this.eye += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                this.target += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            if (game.keyboardState.IsKeyDown(Keys.S))
            {
                // Move the camera backward away from the target
                Vector3 trans = new Vector3(this.target.X - this.eye.X,
                                            this.target.Y - this.eye.Y,
                                            this.target.Z - this.eye.Z);
                trans.Normalize();
                this.eye -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                this.target -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            #endregion


            // output the fps to console
             Console.Out.WriteLine("FPS: " + ((float)gameTime.FrameCount / gameTime.TotalGameTime.Seconds + 0.001f));
            
        }
        
        public override void Draw(GameTime gameTime) {   }
    }
}
