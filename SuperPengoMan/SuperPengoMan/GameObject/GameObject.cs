using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan.GameObject
{
    class GameObject : SpriteBatchObject
    {
        public Texture2D texture;
        public Vector2 pos;
        public GameObject(Texture2D texture, Vector2 pos)
        {
            this.texture = texture;
            this.pos = pos;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, texture, pos, Color.White);
        }
    }
}
