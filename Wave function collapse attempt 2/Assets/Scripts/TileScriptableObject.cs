using UnityEngine;

[CreateAssetMenu(fileName = "TileScriptableObject", menuName = "ScriptableObjects/Tile")]
public class TileScriptableObject : ScriptableObject
{
    [SerializeField] GameObject tileMesh;
    [SerializeField] float rotationIndex;
    [SerializeField] string positiveZ;
    [SerializeField] string negativeZ;
    [SerializeField] string positiveX;
    [SerializeField] string negativeX;

    public GameObject TileMesh => tileMesh;
    public float RotationIndex => rotationIndex;
    public string PositiveZ => positiveZ;
    public string NegativeZ => negativeZ;
    public string PositiveX => positiveX;
    public string NegativeX => negativeX;
}
