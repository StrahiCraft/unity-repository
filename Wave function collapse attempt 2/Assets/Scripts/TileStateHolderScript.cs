using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStateHolderScript : MonoBehaviour
{
    [SerializeField] List<TileScriptableObject> listOfTiles;
    public bool Collapsed
    {
        get;
        private set;
    } = false;

    public int Entropy => listOfTiles.Count;
    public List<TileScriptableObject> ListOfTiles => listOfTiles;

    public void CollapseTile()
    {
        float[] tileWeights = new float[Entropy];
        int chosenTileIndex = 0;
        float maxWeight = 0;

        for(int i = 0; i < tileWeights.Length; i++)
        {
            float currentTileWeight = Random.Range(0, listOfTiles[i].TileWeight);
            if (currentTileWeight > maxWeight)
            {
                maxWeight = currentTileWeight;
                chosenTileIndex = i;
            }
        }
        TileScriptableObject chosenTile = listOfTiles[chosenTileIndex];

        Quaternion rotation = Quaternion.Euler(0, chosenTile.RotationIndex * 90f, 0);

        Instantiate(chosenTile.TileMesh, transform.position, rotation, transform);

        listOfTiles.Clear();
        listOfTiles.Add(chosenTile);
        Collapsed = true;
    }
}
