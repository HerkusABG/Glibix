using UnityEngine;

public class TileDetector : MonoBehaviour
{
    public static TileDetector instance;
    public LayerMask groundLayerMask;
    void Awake()
    {
        instance = this;
    }

    public bool CanIMoveHere(Vector3 originalPosition ,Vector3 inputDelta)
    {
        Vector3 rayStartPos = originalPosition + inputDelta;
        Debug.DrawRay(rayStartPos, Vector3.down * 3, Color.magenta, 5);
        if (Physics.Raycast(rayStartPos, Vector3.down, 3, groundLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
