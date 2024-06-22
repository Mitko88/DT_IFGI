using System.Collections.Generic;
using UnityEngine;

public static class DominantColorExtractor
{
    public static List<string> ExtractDominantColors(List<Renderer> renderers)
    {
        var renderersDominantColor = new List<string>();
        foreach (var renderer in renderers)
        {
            List<Color> dominantColors = new List<Color>();
            List<int> pixelCounts = new List<int>();

            foreach (Material mat in renderer.materials)
            {
                // Check for texture properties
                foreach (string property in mat.GetTexturePropertyNames())
                {
                    Texture texture = mat.GetTexture(property);
                    if (texture != null && texture is Texture2D texture2D)
                    {
                        Color dominantColor = GetDominantColor(texture2D);
                        dominantColors.Add(dominantColor);
                        pixelCounts.Add(texture2D.width * texture2D.height);                     
                    }
                }

                // Check for common color properties
                string[] colorProperties = { "_Color", "_BaseColor", "_EmissionColor" };
                foreach (string property in colorProperties)
                {
                    if (mat.HasProperty(property))
                    {
                        Color color = mat.GetColor(property);
                        dominantColors.Add(color);
                        pixelCounts.Add(1); // Count as 1 pixel for simplicity
                    }
                }
            }

            renderersDominantColor.Add(CalculateWeightedAverageColor(dominantColors, pixelCounts));
        }
        return renderersDominantColor;
    }

    static Color GetDominantColor(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        Dictionary<Color, int> colorCounts = new Dictionary<Color, int>();

        foreach (Color pixel in pixels)
        {
            if (colorCounts.ContainsKey(pixel))
            {
                colorCounts[pixel]++;
            }
            else
            {
                colorCounts[pixel] = 1;
            }
        }

        Color dominantColor = Color.black;
        int maxCount = 0;

        foreach (KeyValuePair<Color, int> kvp in colorCounts)
        {
            if (kvp.Value > maxCount)
            {
                dominantColor = kvp.Key;
                maxCount = kvp.Value;
            }
        }

        return dominantColor;
    }

    static string CalculateWeightedAverageColor(List<Color> colors, List<int> counts)
    {
        if (colors.Count == 0 || colors.Count != counts.Count)
        {
            return ColorToHex(Color.white);
        }

        float totalR = 0f;
        float totalG = 0f;
        float totalB = 0f;
        int totalPixels = 0;

        for (int i = 0; i < colors.Count; i++)
        {
            totalR += colors[i].r * counts[i];
            totalG += colors[i].g * counts[i];
            totalB += colors[i].b * counts[i];
            totalPixels += counts[i];
        }

        return ColorToHex(new Color(totalR / totalPixels, totalG / totalPixels, totalB / totalPixels));
    }

    static string ColorToHex(Color color)
    {
        int r = Mathf.Clamp((int)(color.r * 255), 0, 255);
        int g = Mathf.Clamp((int)(color.g * 255), 0, 255);
        int b = Mathf.Clamp((int)(color.b * 255), 0, 255);
        int a = Mathf.Clamp((int)(color.a * 255), 0, 255);

        return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }
}
