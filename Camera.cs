﻿using System;

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


        private readonly float speed = 750f;

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

            this.pitch = Math.Min(this.pitch, (float)Math.PI / 2 - 0.01f);
            this.pitch = Math.Max(this.pitch, -(float)Math.PI / 2 - 0.01f);

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
            // TODO fix the speed of the left and right movement
            #region Camera keyboard controls
            Vector3 forward = (Vector3)Vector3.Transform(this.reference, Matrix.RotationY(this.yaw));
            Vector3 tempEye = this.eye;
            Vector3 tempTarget = this.target;

            if (game.keyboardState.IsKeyDown(Keys.D))
            {

                // The D key is down so translate the camera 
                // to the right at a right angle to the target vector

                Vector3 translationVector = (Vector3)Vector3.Transform(forward, Matrix.RotationY((float)Math.PI / 2));
                translationVector.Normalize();

                translationVector *= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

                tempEye = (Vector3)Vector3.Transform(tempEye, Matrix.Translation(translationVector));
                tempTarget = (Vector3)Vector3.Transform(tempTarget, Matrix.Translation(translationVector));

            }
            if (game.keyboardState.IsKeyDown(Keys.A))
            {
                // The A key is down so translate the camera 
                //  to the left at a right angle to the forward vector
                Vector3 translationVector = (Vector3)Vector3.Transform(forward, Matrix.RotationY(-(float)Math.PI / 2));
                translationVector.Normalize();

                translationVector *= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

                tempEye = (Vector3)Vector3.Transform(tempEye, Matrix.Translation(translationVector));
                tempTarget = (Vector3)Vector3.Transform(tempTarget, Matrix.Translation(translationVector));
            }
            if (game.keyboardState.IsKeyDown(Keys.W))
            {
                // Move the camera forward towards the target
                Vector3 trans = new Vector3(tempTarget.X - tempEye.X,
                                            tempTarget.Y - tempEye.Y, 
                                            tempTarget.Z - tempEye.Z);
                trans.Normalize();
                tempEye += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                tempTarget += trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            if (game.keyboardState.IsKeyDown(Keys.S))
            {
                // Move the camera backward away from the target
                Vector3 trans = new Vector3(tempTarget.X - tempEye.X,
                                            tempTarget.Y - tempEye.Y,
                                            tempTarget.Z - tempEye.Z);
                trans.Normalize();
                tempEye -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                tempTarget -= trans * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            #endregion


            // find the cameras relative position on the heightMap
            // therefore finding the vertex it is above
            int camIndexX, camIndexZ;
            camIndexX = (int)(tempEye.X / 2f);
            camIndexZ = (int)(tempEye.Z / 2f);

           

            if (tempEye.X > 0 &&
                tempEye.Z > 0 &&
                tempEye.X < game.model.positionScale * game.model.arraySize &&
                tempEye.Z < game.model.positionScale * game.model.arraySize
                )
            {

                if (tempEye.Y < (game.model.heightMap[camIndexX][camIndexZ] + 1))
                {
                    tempEye += new Vector3(0, (game.model.heightMap[camIndexX][camIndexZ] + 1) - tempEye.Y + 5, 0);
                    tempTarget += new Vector3(0, (game.model.heightMap[camIndexX][camIndexZ] + 1) - tempEye.Y + 5, 0);
                }


                this.eye = tempEye;
                this.target = tempTarget;

            }




            
            // output the fps to console
            // Console.Out.WriteLine("FPS: " + ((float)gameTime.FrameCount / gameTime.TotalGameTime.Seconds + 0.001f));

            
        }
        
        public override void Draw(GameTime gameTime) {   }
    }
}
