

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PfisterGameEngine
{
    public class AnimatedTexture
    {
        // Properties
        public int FrameCount
        {
            get;
            set;
        }
        public int CurrentFrame
        {
            get;
            set;
        }
        public Double FrameDelay
        {
            get;
            set;
        }
        public int FrameWidth
        {
            get;
            protected set;
        }
        public int FrameHeight
        {
            get;
            protected set;
        }
        public bool IsAnimated
        {
            get;
            set;
        }
        private float animationTimer;
        private Texture2D texture;
        // 

        // Constructors/Deconstructors
        public AnimatedTexture(Texture2D texture)
        {
            
            this.texture = texture;
            FrameWidth = this.texture.Width;
            FrameHeight = this.texture.Height;
            FrameCount = this.texture.Width / FrameWidth;
            CurrentFrame = 0;
            IsAnimated = false;
            FrameDelay = 0;
        }
        public AnimatedTexture(Texture2D texture, int frameWidth, int frameHeight, bool doAnimation, float frameDelay)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture), "Texture cannot be null!");
            else
                this.texture = texture;

            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            FrameCount = this.texture.Bounds.Width / FrameWidth;
            CurrentFrame = 0;
            IsAnimated = doAnimation;
            FrameDelay = frameDelay;
        }
        public AnimatedTexture(Texture2D texture, int width, int height, bool doAnimation)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture), "Texture cannot be null!");
            else
                this.texture = texture;

            FrameWidth = width;
            FrameHeight = height;
            FrameCount = this.texture.Bounds.Width / width;
            CurrentFrame = 0;
            IsAnimated = doAnimation;
            FrameDelay = 0;
        }
        // 

        // Methods
        public Texture2D ExtractFrame()
        {
            return ExtractFrame(CurrentFrame);
        }

        public Texture2D ExtractFrame(int frame)
        {
            if (frame >= FrameCount || frame < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(frame),"Specified frame is higher than max frames or lower than zero!");
            }
            Rectangle frameBounds = new Rectangle(frame * FrameWidth, 0, FrameWidth, FrameHeight);

            Color[] textureData = new Color[FrameWidth * FrameHeight];
            texture.GetData(0, frameBounds, textureData, 0, frameBounds.Width * frameBounds.Height);
            Texture2D returnTexture = new Texture2D(texture.GraphicsDevice, frameBounds.Width, frameBounds.Height);
            returnTexture.SetData(textureData);
            return returnTexture;

        }

        public void DrawFrame(SpriteBatch spriteBatch, Rectangle destinationRectangle, DrawingArguments drawArgs)
        {
            DrawFrame(spriteBatch, destinationRectangle, 0, drawArgs);
        }
        public void DrawFrame(SpriteBatch spriteBatch, Rectangle destinationRectangle, int frame, DrawingArguments drawArgs)
        {
            if (drawArgs == null)
            {
                throw new ArgumentNullException(nameof(drawArgs), "Drawing arguments cannot be null");
            }
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch), "SpriteBatch cannot be null");
            }

            spriteBatch.Draw(ExtractFrame(frame), destinationRectangle, null, drawArgs.DrawingColor, drawArgs.Rotation, drawArgs.Origin, drawArgs.SpriteEffects, drawArgs.Depth);
        }
        public void DrawAnimation(SpriteBatch spriteBatch, Rectangle destinationRectangle, float elapsed, DrawingArguments drawArgs)
        {
            if (drawArgs == null)
                throw new ArgumentNullException(nameof(drawArgs), "Drawing arguments cannot be null");
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch), "SpriteBatch cannot be null");
            }
            if (!IsAnimated)
            {
                throw new InvalidOperationException("Texture is not animated");
            }

            if (animationTimer >= FrameDelay)
            {
                animationTimer = 0;
                CurrentFrame++;
                if (CurrentFrame >= FrameCount)
                {
                    CurrentFrame = 0;
                }

            }
            else
            {
                animationTimer += elapsed;
            }
            spriteBatch.Draw(ExtractFrame(CurrentFrame), destinationRectangle, null, drawArgs.DrawingColor, drawArgs.Rotation, drawArgs.Origin, drawArgs.SpriteEffects, drawArgs.Depth);


        }
        // 

    }
}
