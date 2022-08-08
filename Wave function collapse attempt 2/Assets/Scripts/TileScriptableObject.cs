using UnityEngine;

[CreateAssetMenu(fileName = "TileScriptableObject", menuName = "ScriptableObjects/Tile")]
public class TileScriptableObject : ScriptableObject
{
    [SerializeField] GameObject tileMesh;
    [SerializeField] float rotationIndex;
    [Range(0.0f,100.0f)]
    [SerializeField] float tileWeight;

    [SerializeField] string positiveZ;
    [SerializeField] string negativeZ;
    
    [SerializeField] string positiveX;
    [SerializeField] string negativeX;

    [SerializeField] string positiveY;
    [SerializeField] string negativeY;

    public GameObject TileMesh => tileMesh;
    public float RotationIndex => rotationIndex;
    public float TileWeight => tileWeight;
    public string PositiveZ => positiveZ;
    public string NegativeZ => negativeZ;
    public string PositiveX => positiveX;
    public string NegativeX => negativeX;
    public string PositiveY => positiveY;
    public string NegativeY => negativeY;
}
