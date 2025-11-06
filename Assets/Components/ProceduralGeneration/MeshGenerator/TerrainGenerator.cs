using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int width = 256;
    public int height = 256;
    public int depth = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    TerrainData GenerateTerrain(TerrainData data)
    {
        data.heightmapResolution = width + 1;
        data.size = new Vector3(width, depth, height);
        data.SetHeights(0, 0, GenerateHeights());
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
