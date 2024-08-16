using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Kong_Engine
{
    public class TileMapManager
    {
        //Variables

        private SpriteBatch spriteBatch;
        private TmxMap map;
        private Texture2D tileset;
        private int tilesetTilesWide;
        private int tileWidth;
        private int tileHeight;
        private float scale;
        public int TileWidth => tileWidth;
        public TmxMap Map => map;
        public float ScaleFactor => scale;


        public List<Rectangle> CollisionRectangles { get; private set; }
        private List<Texture2D> backgroundTextures;
        private List<Vector2> backgroundPositions;
        private ContentManager contentManager;

        //Constructor Method - Initialises TileMapManager with data, tileset and rendering commands
        //Sets up background assets and creates collision rectangles
        public TileMapManager(ContentManager _contentManager, SpriteBatch _spriteBatch, TmxMap _map, Texture2D _tileset, int _tilesetTilesWide, int _tileWidth, int _tileHeight, float _scale = 1.0f)
        {
            contentManager = _contentManager;
            spriteBatch = _spriteBatch;
            map = _map;
            tileset = _tileset;
            tilesetTilesWide = _tilesetTilesWide;
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
            scale = _scale;
            CollisionRectangles = new List<Rectangle>();
            backgroundTextures = new List<Texture2D>();
            backgroundPositions = new List<Vector2>();

            LoadBackgrounds();
            LoadCollisionRectanglesFromCsv("Content/JumpLand_Collisions.csv");
        }

        
        private void LoadBackgrounds()
        {
           
            var background3 = contentManager.Load<Texture2D>("Background3");
            var background2 = contentManager.Load<Texture2D>("Background2"); 
            var background1 = contentManager.Load<Texture2D>("Background1"); 

            backgroundTextures.Add(background3);
            backgroundTextures.Add(background2);
            backgroundTextures.Add(background1);

            //The order of background assets. Topmost = furthest back
            backgroundPositions.Add(Vector2.Zero);
            backgroundPositions.Add(Vector2.Zero);
            backgroundPositions.Add(Vector2.Zero); 
        }

        /**/
        //Processes Collision Data from a CSV file and generates and stores 'rectangle' objects where collisions should occur
        /**/
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

        //Renders background textures, tiling them in both directions. 

        public void Draw(Matrix matrix)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                transformMatrix: matrix);

            
            for (int i = 0; i < backgroundTextures.Count; i++)
            {
                var backgroundTexture = backgroundTextures[i];
                var position = backgroundPositions[i];
                var scaledWidth = backgroundTexture.Width * scale;
                var scaledHeight = backgroundTexture.Height * scale;

                //Outer Loop tiles background images on the X axis, and inner loop tiles it on the Y axis
                for (float x = 0; x < map.Width * map.TileWidth * scale; x += scaledWidth)
                {
                    for (float y = 0; y < map.Height * map.TileHeight * scale; y += scaledHeight)
                    {
                        spriteBatch.Draw(
                            backgroundTexture,
                            new Rectangle((int)(position.X + x), (int)(position.Y + y), (int)scaledWidth, (int)scaledHeight),
                            Color.White
                        );
                    }
                }
            }

            // Draws the tiled layers
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

        public void DrawCollisionRectangles(SpriteBatch spriteBatch)
        {
            // Create a semi-transparent texture for the collision rectangles
            Texture2D rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.Red * 0.5f }); // Red color with 50% transparency

            foreach (var rect in CollisionRectangles)
            {
                spriteBatch.Draw(rectTexture, rect, Color.Red * 0.5f);
            }
        }
        public void SetScale(float newScale)
        {
            scale = newScale;

            CollisionRectangles.Clear();
            LoadCollisionRectanglesFromCsv("Content/JumpLand_Collisions.csv");
        }
    }
}
