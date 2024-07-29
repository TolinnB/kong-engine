using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

        public TileMapManager(SpriteBatch _spriteBatch, TmxMap _map, Texture2D _tileset, int _tilesetTilesWide, int _tileWidth, int _tileHeight, float _scale = 1.0f)
        {
            spriteBatch = _spriteBatch;
            map = _map;
            tileset = _tileset;
            tilesetTilesWide = _tilesetTilesWide;
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
            scale = _scale;
        }

        public void Draw(Matrix matrix)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                transformMatrix: matrix);

            for (var i = 0; i < map.Layers.Count; i++)
            {
                for (var j = 0; j < map.Layers[i].Tiles.Count; j++)
                {
                    int gid = map.Layers[i].Tiles[j].Gid;
                    if (gid == 0)
                    {
                        // do nothing
                    }
                    else
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
