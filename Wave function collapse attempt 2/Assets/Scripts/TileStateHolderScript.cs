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
        int chosenTileIndex = Random.Range(0, listOfTiles.Count - 1);
        TileScriptableObject chosenTile = listOfTiles[chosenTileIndex];

        Quaternion rotation = Quaternion.Euler(0, chosenTile.RotationIndex * 90, 0);

        Instantiate(chosenTile.TileMesh, transform.position, rotation, transform);

        listOfTiles.Clear();
        listOfTiles.Add(chosenTile);
        Collapsed = true;
    }
}
