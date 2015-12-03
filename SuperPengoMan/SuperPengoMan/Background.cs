using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPengoMan
{
    class Background: SpriteBatchObject
    {
        Game game;
        int tilesWidth;
        List<Vector2> foregroundVector, middlegroundVector;
        int fgSpacing, mgSpacing;
        float fgSpeed, mgSpeed;

        Texture2D background;
        Texture2D middleground;
        Texture2D foreground;
        Texture2D caveBackground;

        int enbaledBackgrounds;

        public Background(Game game, int tilesWidth)
        {
            this.game = game;
            this.tilesWidth = tilesWidth;
            enbaledBackgrounds = 0;
            foreground = game.Content.Load<Texture2D>(@"cloud");
            middleground = game.Content.Load<Texture2D>(@"big_cloud");
            background = game.Content.Load<Texture2D>(@"background");
            caveBackground = game.Content.Load<Texture2D>(@"snowcave");
            fgSpeed = 0.75f;
            mgSpeed = 0.3f;
            CreateLayers();
        }

        public void Update()
        {
            MoveLayer(foregroundVector, fgSpacing, fgSpeed);
            MoveLayer(middlegroundVector, mgSpacing, mgSpeed);
        }

        internal int TileWidth
        {
            get { return tilesWidth; }
            set
            {
                tilesWidth = value;
                CreateLayers();
            }
        }

        private void CreateLayers()
        {
            CreateLayer(ref foregroundVector, ref fgSpacing, foreground.Width, foreground.Height);
            CreateLayer(ref middlegroundVector, ref mgSpacing, middleground.Width, foreground.Height - middleground.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if( 0 != (enbaledBackgrounds & 1))
            {
                Draw(spriteBatch, background, new Rectangle(0, game.Window.ClientBounds.Height - background.Height - (1 * Game1.TILE_SIZE), 
                                    background.Width, background.Height), Color.White);
            }
            if (0 != (enbaledBackgrounds & 2))
            {
                Draw(spriteBatch, caveBackground, new Vector2(Game1.TILE_SIZE * 37, Game1.TILE_SIZE * 9), Color.White);
            }
            if (0 != (enbaledBackgrounds & 4))
            {
                foreach (Vector2 v in middlegroundVector)
                {
                    Draw(spriteBatch, middleground, new Vector2(v.X, 20), Color.White);
                }
            }
            if (0 != (enbaledBackgrounds & 8))
            {
                foreach (Vector2 v in foregroundVector)
                {
                    Draw(spriteBatch, foreground, new Vector2(v.X, 100), Color.White);
                }
            }
        }

        private void CreateLayer(ref List<Vector2> layerVector, ref int spacing,  int width, int height)
        {
            layerVector = new List<Vector2>();
            spacing = width + 3 * Game1.TILE_SIZE;
            for (int i = 0; i < (Game1.TILE_SIZE * (tilesWidth -1) / spacing) + 2; i++)
            {
                layerVector.Add(new Vector2(i * spacing, game.Window.ClientBounds.Height - height));
            }
        }

        private void MoveLayer(List<Vector2> layerVector, int spacing, float speed)
        {
            for (int i = 0; i < layerVector.Count; i++)
            {
                layerVector[i] = new Vector2(layerVector[i].X - speed, layerVector[i].Y);
                if (layerVector[i].X <= -spacing)
                {
                    int j = i - 1;
                    if (j < 0)
                    {
                        j = layerVector.Count - 1;
                    }
                    layerVector[i] = new Vector2(layerVector[j].X + spacing - 1, layerVector[i].Y);
                }
            }
        }

        public void EnableBackground(int bkgBits)
        {
            enbaledBackgrounds = enbaledBackgrounds | bkgBits;
        }

        public void DisableBackground(int bkgBits)
        {
            enbaledBackgrounds = enbaledBackgrounds & (0 ^ bkgBits);
        }
    }
}
