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

[CreateAssetMenu(menuName = "Procedural Generation Method/FastNoise")]
public class FastNoise : ProceduralGenerationMethod
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Noise Parameters")]
    [SerializeField] private FastNoiseLite.NoiseType noiseType;
    [SerializeField, Range(0.01f, 0.1f)] private float _frequency = 0.01f;
    [SerializeField, Range(0.5f, 1.5f)] private float _amplitude = 1f;

    [Header("Fractal Parameters")]
    [SerializeField] private FastNoiseLite.FractalType fractalType;
    [SerializeField, Range(1,5)] private int _octaves = 3;
    [SerializeField, Range(1f,3f)] private float _lacunarity = 2f;
    [SerializeField, Range(0.5f, 1f)] private float _gain = 0.5f;

    [Header("Heights")]
    [SerializeField, Range(-1f,1f)] private float _waterHeight = -0.6f;
    [SerializeField, Range(-1f,1f)] private float _sandHeight = -0.3f;
    [SerializeField, Range(-1f,1f)] private float _grassHeight = 0.8f;
    [SerializeField, Range(-1f,1f)] private float _rockHeight = 1f;
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
        // Gather noise data
        float[,] noiseData = new float[128, 128];

        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Lenght; y++)
            {
                noiseData[x, y] = noise.GetNoise(x, y);
                if (!Grid.TryGetCellByCoordinates(x, y, out var chosenCell))
                {
                    Debug.LogError($"Unable to get cell on coordinates : ({x}, {y})");
                    continue;
                }
                if (noiseData[x,y] <= _waterHeight)
                {
                    GridGenerator.AddGridObjectToCell(chosenCell, waterTemplate, false);
                }
                if (noiseData[x,y] <= _sandHeight && noiseData[x,y] > _waterHeight)
                {
                    GridGenerator.AddGridObjectToCell(chosenCell, sandTemplate, false);
                }
                if (noiseData[x,y] <= _grassHeight && noiseData[x, y] > _sandHeight)
                {
                    GridGenerator.AddGridObjectToCell(chosenCell, grassTemplate, false);
                }
                if (noiseData[x,y] <= _rockHeight && noiseData[x, y] > _grassHeight)
                {
                    GridGenerator.AddGridObjectToCell(chosenCell, rockTemplate, false);
                }
                
                
            }
        }
    }
}
