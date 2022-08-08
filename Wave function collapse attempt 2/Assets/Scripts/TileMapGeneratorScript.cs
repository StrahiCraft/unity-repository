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

    GameObject[,] tileStateHolderMatrix;

    void Start()
    {
        tileStateHolderMatrix = new GameObject[mapWidth, mapHeight];
        int uncollapsedTileCount = mapWidth * mapHeight;

        SpawnTileStateHolders();

        StartCoroutine(GenerateMap(uncollapsedTileCount));
    }

    void SpawnTileStateHolders()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                tileStateHolderMatrix[i, j] =
                    Instantiate(tileStateHolder, new Vector3(i * tileSize, 0f, j * tileSize), Quaternion.identity);
            }
        }
    }

    IEnumerator GenerateMap(int uncollapsedTileCount)
    {
        while (uncollapsedTileCount > 0)
        {
            Vector2 lowestEntropyPos = FindLowestEntropy();
            tileStateHolderMatrix[(int)lowestEntropyPos.x, (int)lowestEntropyPos.y].GetComponent<TileStateHolderScript>().CollapseTile();

            Propagate(lowestEntropyPos);

            uncollapsedTileCount--;
            yield return new WaitForSeconds(0.1f);
        }
    }

    Vector2 FindLowestEntropy()
    {
        int minEntropy = int.MaxValue;
        List<Vector2> possibeIndexes = new List<Vector2>();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (!tileStateHolderMatrix[x, y].GetComponent<TileStateHolderScript>().Collapsed)
                {
                    int currentTileEntropy = tileStateHolderMatrix[x, y].GetComponent<TileStateHolderScript>().Entropy;

                    if (currentTileEntropy == minEntropy)
                    {
                        possibeIndexes.Add(new Vector2(x, y));
                    }
                    if (currentTileEntropy < minEntropy)
                    {
                        possibeIndexes.Clear();
                        minEntropy = currentTileEntropy;
                        possibeIndexes.Add(new Vector2(x, y));
                    }
                }
            }
        }
        return possibeIndexes[Random.Range(0, possibeIndexes.Count)];
    }

    Stack<Vector2> stack = new Stack<Vector2>();
    void Propagate(Vector2 lowestEntropyPos)
    {
        stack.Push(lowestEntropyPos);

        while (stack.Count > 0)
        {
            Vector2 currentPos = stack.Pop();
            List<TileScriptableObject> currentListOfTiles =
                tileStateHolderMatrix[(int)currentPos.x, (int)currentPos.y].GetComponent<TileStateHolderScript>().ListOfTiles;
            UpdatePositiveZ(currentPos, currentListOfTiles);
            UpdateNegativeZ(currentPos, currentListOfTiles);
            UpdatePositiveX(currentPos, currentListOfTiles);
            UpdateNegativeX(currentPos, currentListOfTiles);
        }
    }
    void UpdatePositiveZ(Vector2 currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        Vector2 otherIndex = currentIndex;
        otherIndex.y += 1;
        if (otherIndex.y >= mapHeight)
        {
            return;
        }
        List<TileScriptableObject> otherListOfTiles =
            tileStateHolderMatrix[(int)otherIndex.x, (int)otherIndex.y].GetComponent<TileStateHolderScript>().ListOfTiles;
        int otherListCount = otherListOfTiles.Count;

        int i = 0;
        while (i < otherListOfTiles.Count)
        {
            bool validNeighbour = false;
            for (int j = 0; j < currentListOfTiles.Count; j++)
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
    void UpdateNegativeZ(Vector2 currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        Vector2 otherIndex = currentIndex;
        otherIndex.y -= 1;
        if (otherIndex.y < 0)
        {
            return;
        }
        List<TileScriptableObject> otherListOfTiles =
            tileStateHolderMatrix[(int)otherIndex.x, (int)otherIndex.y].GetComponent<TileStateHolderScript>().ListOfTiles;
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
    void UpdatePositiveX(Vector2 currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        Vector2 otherIndex = currentIndex;
        otherIndex.x += 1;
        if (otherIndex.x >= mapWidth)
        {
            return;
        }
        List<TileScriptableObject> otherListOfTiles =
            tileStateHolderMatrix[(int)otherIndex.x, (int)otherIndex.y].GetComponent<TileStateHolderScript>().ListOfTiles;
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
    void UpdateNegativeX(Vector2 currentIndex, List<TileScriptableObject> currentListOfTiles)
    {
        Vector2 otherIndex = currentIndex;
        otherIndex.x -= 1;
        if (otherIndex.x < 0)
        {
            return;
        }
        List<TileScriptableObject> otherListOfTiles =
            tileStateHolderMatrix[(int)otherIndex.x, (int)otherIndex.y].GetComponent<TileStateHolderScript>().ListOfTiles;
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