using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGeneratorScript : MonoBehaviour
{
    [SerializeField] GameObject tileStateHolder;
    [SerializeField] int mapWidth;
    [SerializeField] int mapHeight;
    [SerializeField] float tileSize;
    [SerializeField] TileScriptableObject test;

    GameObject[] tileStateHolderArray;

    void Start()
    {
        tileStateHolderArray = new GameObject[mapWidth * mapHeight];
        int uncollapsedTileCount = mapWidth * mapHeight;

        SpawnTileStateHolders();

        StartCoroutine(GenerateMap(uncollapsedTileCount));
    }

    void SpawnTileStateHolders()
    {
        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                tileStateHolderArray[i * mapWidth + j] =
                    Instantiate(tileStateHolder, new Vector3(i * tileSize, 0f, j * tileSize), Quaternion.identity);
            }
        }
    }

    IEnumerator GenerateMap(int uncollapsedTileCount)
    {
        while (uncollapsedTileCount > 0)
        {
            int lowestEntropyIndex = FindLowestEntropy();
            tileStateHolderArray[lowestEntropyIndex].GetComponent<TileStateHolderScript>().CollapseTile();

            Propagate(lowestEntropyIndex);

            uncollapsedTileCount--;
            yield return new WaitForSeconds(0.1f);
        }
    }

    int FindLowestEntropy()
    {
        int minEntropy = int.MaxValue;
        int minEntropyIndex = -1;

        for(int i = 0; i < mapWidth * mapHeight; i++)
        {
            if (!tileStateHolderArray[i].GetComponent<TileStateHolderScript>().Collapsed)
            {
                int currentTileEntropy = tileStateHolderArray[i].GetComponent<TileStateHolderScript>().Entropy;

                if (currentTileEntropy < minEntropy)
                {
                    minEntropy = currentTileEntropy;
                    minEntropyIndex = i;
                }
            }
        }
        return minEntropyIndex;
    }

    void Propagate(int lowestEntropyIndex)
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(lowestEntropyIndex);

        while (stack.Count > 0)
        {
            int currentIndex = stack.Pop();
            List<TileScriptableObject> currentListOfTiles =
                tileStateHolderArray[currentIndex].GetComponent<TileStateHolderScript>().ListOfTiles;

        }
    }
    void UpdatePositiveZ(int currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        int otherIndex = currentIndex + mapWidth;
        if (otherIndex >= mapWidth * mapHeight)
        {
            return;
        }
        List<TileScriptableObject> otherListOfTiles = 
            tileStateHolderArray[otherIndex].GetComponent<TileStateHolderScript>().ListOfTiles;
        int otherListCount = otherListOfTiles.Count;

        int i = 0;
        while (i < otherListOfTiles.Count)
        {
            for(int j = 0; j < currentListOfTiles.Count; j++)
            {

            }
        }
    }
}
