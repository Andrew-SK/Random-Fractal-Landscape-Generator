﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;


using SharpDX;
using SharpDX.Toolkit;

namespace Project1
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class Project1Game : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public Landscape model;

        public Camera camera;

        public KeyboardManager keyboardManager;
        public KeyboardState keyboardState;

        public MouseManager mouseManager;
        public MouseState mouseState;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Project1Game" /> class.
        /// </summary>
        public Project1Game()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Creating the keyboard manager for keyboard Camera controls 
            keyboardManager = new KeyboardManager(this);
            
            // Creating the mouse manager for mouse Camera controls
            mouseManager = new MouseManager(this);
            
        
            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

        }

        protected override void LoadContent()
        {
            model = new Landscape(this, 1, 1, 1, 1, 11);
            
            // Camera object containing all Camera specific controls and info
            int camX = 50;
            int camZ = 50;
            Vector3 pos = new Vector3(camX, model.heightMap[camX][camX] + 50, camZ);
            Vector3 target = pos + new Vector3(1, 0, 1);
            this.camera = new Camera(pos, target, Vector3.UnitY, this);

            // Create an input layout from the vertices

            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Project 1";
            
            this.IsMouseVisible = false;
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {

            // Getting input device states
            keyboardState = keyboardManager.GetState();
            mouseState = mouseManager.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            model.Update(gameTime);
            camera.Update(gameTime);

            
            // Handle base.Update
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);

            model.Draw(gameTime);

            
            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
