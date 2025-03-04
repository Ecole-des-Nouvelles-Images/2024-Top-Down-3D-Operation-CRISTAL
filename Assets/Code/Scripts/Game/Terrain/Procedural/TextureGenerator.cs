using UnityEngine;

namespace Game.Terrain.Procedural
{
    public static class TextureGenerator
    {
        public static Texture2D TextureFromColourMap(Color[] colourMap, FilterMode mode, int width, int height) {
            Texture2D texture = new(width, height)
            {
                filterMode = mode,
                wrapMode = TextureWrapMode.Clamp
            };
            texture.SetPixels (colourMap);
            texture.Apply ();
            return texture;
        }
        
        public static Texture2D TextureFromHeightMap(float[,] heightMap, FilterMode filterMode) {
            int width = heightMap.GetLength (0);
            int height = heightMap.GetLength (1);

            Color[] colourMap = new Color[width * height];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]);
                }
            }

            return TextureFromColourMap (colourMap, filterMode, width, height);
        }

    }
}
