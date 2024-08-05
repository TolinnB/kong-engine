using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TiledSharp;

namespace Kong_Engine
{
    public class TileMapManager
    {
        private SpriteBatch spriteBatch;
        private TmxMap map;
        private Texture2D tileset;
        private int tilesetTilesWide;
        private int tileWidth;
        private int tileHeight;
        private float scale;
        public List<Rectangle> CollisionRectangles { get; private set; }

        public TileMapManager(SpriteBatch _spriteBatch, TmxMap _map, Texture2D _tileset, int _tilesetTilesWide, int _tileWidth, int _tileHeight, float _scale = 1.0f)
        {
            spriteBatch = _spriteBatch;
            map = _map;
            tileset = _tileset;
            tilesetTilesWide = _tilesetTilesWide;
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
            scale = _scale;
            CollisionRectangles = new List<Rectangle>();
            LoadCollisionRectangles();
        }

        private void LoadCollisionRectangles()
        {
            Console.WriteLine("Available Object Layers:");
            foreach (var layer in map.ObjectGroups)
            {
                Console.WriteLine($"- {layer.Name}");
            }

            TmxObjectGroup objectGroup = map.ObjectGroups.FirstOrDefault(og => og.Name == "Object Layer 1") ??
                                         map.ObjectGroups.FirstOrDefault(og => og.Name == "Collisions") ??
                                         map.ObjectGroups.FirstOrDefault();

            if (objectGroup != null)
            {
                Console.WriteLine($"Using object layer: {objectGroup.Name}");
                foreach (var obj in objectGroup.Objects)
                {
                    var rectangle = new Rectangle((int)obj.X, (int)obj.Y, (int)obj.Width, (int)obj.Height);
                    CollisionRectangles.Add(rectangle);
                }
            }
            else
            {
                var availableLayers = string.Join(", ", map.ObjectGroups.Select(og => og.Name));
                throw new KeyNotFoundException($"No suitable object layer found. Available layers: {availableLayers}");
            }
        }

        public void Draw(Matrix matrix)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                transformMatrix: matrix);

            foreach (var layer in map.Layers)
            {
                for (var j = 0; j < layer.Tiles.Count; j++)
                {
                    int gid = layer.Tiles[j].Gid;
                    if (gid != 0)
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = tileFrame / tilesetTilesWide;
                        float x = (j % map.Width) * map.TileWidth * scale;
                        float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight * scale;
                        Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                        spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, (int)(tileWidth * scale), (int)(tileHeight * scale)), tilesetRec, Color.White);
                    }
                }
            }

            spriteBatch.End();
        }

        public void SetScale(float newScale)
        {
            scale = newScale;
        }
    }
}
