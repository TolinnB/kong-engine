using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
            LoadCollisionRectanglesFromCsv("Content/JumpLand_Collisions.csv");
        }

        private void LoadCollisionRectanglesFromCsv(string filePath)
        {
            var collisionData = CsvHelper.LoadCsv(filePath);
            var width = collisionData.GetLength(0);
            var height = collisionData.GetLength(1);

            Console.WriteLine($"Loading collision data from {filePath}");
            Console.WriteLine($"CSV Width: {width}, Height: {height}");

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (collisionData[x, y] != -1)
                    {
                        var rectangle = new Rectangle(
                            (int)(x * tileWidth * scale),
                            (int)(y * tileHeight * scale),
                            (int)(tileWidth * scale),
                            (int)(tileHeight * scale)
                        );
                        CollisionRectangles.Add(rectangle);
                        Console.WriteLine($"Collision rectangle added at ({rectangle.X}, {rectangle.Y}, {rectangle.Width}, {rectangle.Height})");
                    }
                }
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
            // If necessary, reload or recalculate collision rectangles to apply the new scale
            CollisionRectangles.Clear();
            LoadCollisionRectanglesFromCsv("Content/JumpLand_Collisions.csv");
        }
    }
}
