using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColourSettings biomeColourSettings;
    public Gradient oceanColour;

    [System.Serializable]
    public class BiomeColourSettings
    {
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;

        [Range(0, 1)]
        public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;

            [Range(0f, 1f)]
            public float startHeight;

            [Range(0f, 1f)]
            public float tintPercent;
        }
    }
}
