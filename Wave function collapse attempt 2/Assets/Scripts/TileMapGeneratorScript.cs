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
        List<int> possibeIndexes = new List<int>();

        for(int i = 0; i < mapWidth * mapHeight; i++)
        {
            if (!tileStateHolderArray[i].GetComponent<TileStateHolderScript>().Collapsed)
            {
                int currentTileEntropy = tileStateHolderArray[i].GetComponent<TileStateHolderScript>().Entropy;

                if(currentTileEntropy == minEntropy)
                {
                    possibeIndexes.Add(i);
                }
                if (currentTileEntropy < minEntropy)
                {
                    possibeIndexes.Clear();
                    minEntropy = currentTileEntropy;
                    possibeIndexes.Add(i);
                }
            }
        }
        return possibeIndexes[Random.Range(0, possibeIndexes.Count)];
    }

    Stack<int> stack = new Stack<int>();
    void Propagate(int lowestEntropyIndex)
    {
        stack.Push(lowestEntropyIndex);

        while (stack.Count > 0)
        {
            int currentIndex = stack.Pop();
            List<TileScriptableObject> currentListOfTiles =
                tileStateHolderArray[currentIndex].GetComponent<TileStateHolderScript>().ListOfTiles;
            UpdatePositiveZ(currentIndex, currentListOfTiles);
            UpdateNegativeZ(currentIndex, currentListOfTiles);
            UpdatePositiveX(currentIndex, currentListOfTiles);
            UpdateNegativeX(currentIndex, currentListOfTiles);
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
            bool validNeighbour = false;
            for(int j = 0; j < currentListOfTiles.Count; j++)
            {
                if (otherListOfTiles[i].NegativeZ == currentListOfTiles[j].PositiveZ)
                {
                    validNeighbour = true;
                    break;
                }
            }
            if (validNeighbour)
            {
                i++;
            }
            else
            {
                otherListOfTiles.RemoveAt(i);
            }
        }
        if (otherListOfTiles.Count != otherListCount)
        {
            stack.Push(otherIndex);
        }
    }
    void UpdateNegativeZ(int currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        int otherIndex = currentIndex - mapWidth;
        if (otherIndex < 0)
        {
            return;
        }
        List<TileScriptableObject> otherListOfTiles =
            tileStateHolderArray[otherIndex].GetComponent<TileStateHolderScript>().ListOfTiles;
        int otherListCount = otherListOfTiles.Count;

        int i = 0;
        while (i < otherListOfTiles.Count)
        {
            bool validNeighbour = false;
            for (int j = 0; j < currentListOfTiles.Count; j++)
            {
                if (otherListOfTiles[i].PositiveZ == currentListOfTiles[j].NegativeZ)
                {
                    validNeighbour = true;
                    break;
                }
            }
            if (validNeighbour)
            {
                i++;
            }
            else
            {
                otherListOfTiles.RemoveAt(i);
            }
        }
        if (otherListOfTiles.Count != otherListCount && !stack.Contains(otherIndex))
        {
            stack.Push(otherIndex);
        }
    }
    void UpdatePositiveX(int currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        int otherIndex = currentIndex + mapHeight;
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
            bool validNeighbour = false;
            for (int j = 0; j < currentListOfTiles.Count; j++)
            {
                if (otherListOfTiles[i].NegativeX == currentListOfTiles[j].PositiveX)
                {
                    validNeighbour = true;
                    break;
                }
            }
            if (validNeighbour)
            {
                i++;
            }
            else
            {
                otherListOfTiles.RemoveAt(i);
            }
        }
        if (otherListOfTiles.Count != otherListCount && !stack.Contains(otherIndex))
        {
            stack.Push(otherIndex);
        }
    }
    void UpdateNegativeX(int currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        int otherIndex = currentIndex - mapHeight;
        
        if (otherIndex < 0)
        {
            return;
        }
        List<TileScriptableObject> otherListOfTiles =
            tileStateHolderArray[otherIndex].GetComponent<TileStateHolderScript>().ListOfTiles;
        int otherListCount = otherListOfTiles.Count;

        int i = 0;
        while (i < otherListOfTiles.Count)
        {
            bool validNeighbour = false;
            for (int j = 0; j < currentListOfTiles.Count; j++)
            {
                if (otherListOfTiles[i].PositiveX == currentListOfTiles[j].NegativeX)
                {
                    validNeighbour = true;
                    break;
                }
            }
            if (validNeighbour)
            {
                i++;
            }
            else
            {
                otherListOfTiles.RemoveAt(i);
            }
        }
        if (otherListOfTiles.Count != otherListCount && !stack.Contains(otherIndex))
        {
            stack.Push(otherIndex);
        }
    }
}
