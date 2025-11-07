using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using VTools.Grid;
using VTools.RandomService;
using VTools.ScriptableObjectDatabase;

[CreateAssetMenu(menuName = "Procedural Generation Method/Terrain")]
public class TerrainGenerator : ProceduralGenerationMethod
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Terrain terrain;
    public int width = 256;
    public int height = 256;
    public int depth = 10;
    [Header("Noise Parameters")]
    [SerializeField] private FastNoiseLite.NoiseType noiseType;
    [SerializeField, Range(0.01f, 0.1f)] private float _frequency = 0.01f;
    [SerializeField, Range(0.5f, 1.5f)] private float _amplitude = 1f;

    [Header("Fractal Parameters")]
    [SerializeField] private FastNoiseLite.FractalType fractalType;
    [SerializeField, Range(1, 5)] private int _octaves = 3;
    [SerializeField, Range(1f, 3f)] private float _lacunarity = 2f;
    [SerializeField, Range(0.5f, 1f)] private float _gain = 0.5f;

    [Header("Heights")]
    [SerializeField, Range(-1f, 1f)] private float _waterHeight = -0.6f;
    [SerializeField, Range(-1f, 1f)] private float _sandHeight = -0.3f;
    [SerializeField, Range(-1f, 1f)] private float _grassHeight = 0.8f;
    [SerializeField, Range(-1f, 1f)] private float _rockHeight = 1f;
    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        var grassTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
        var waterTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Water");
        var rockTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Rock");
        var sandTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Sand");
        FastNoiseLite noise = new FastNoiseLite();
        noise.SetNoiseType(noiseType);
        noise.SetSeed(GridGenerator._seed);
        noise.SetFrequency(_frequency);
        noise.SetDomainWarpAmp(_amplitude);
        noise.SetFractalType(fractalType);
        noise.SetFractalOctaves(_octaves);
        noise.SetFractalLacunarity(_lacunarity);
        noise.SetFractalGain(_gain);

        terrain = GridGenerator.gameObject.GetComponent<Terrain>();

        terrain.terrainData = GenerateTerrain(terrain.terrainData, noise);
      
    }
    TerrainData GenerateTerrain(TerrainData data, FastNoiseLite noise)
    {
        data.heightmapResolution = width + 1;
        data.size = new Vector3(width, depth, height);
        data.SetHeights(0, 0, GenerateHeights(noise));
        return data;
    }
    float[,] GenerateHeights(FastNoiseLite noise)
    {
        float[,] heights = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                heights[i,j] = noise.GetNoise(i, j);
            }
        }
        return heights;
    }
}
