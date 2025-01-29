using UnityEngine;

public class TileDetector : MonoBehaviour
{
    public static TileDetector instance;
    public LayerMask hittableLayerMasks;
    public GameObject targetEnemy;
    void Awake()
    {
        instance = this;
    }

    public int CanIMoveHere(Vector3 originalPosition ,Vector3 inputDelta, bool isPlayer)
    {
        Vector3 rayStartPos = originalPosition + inputDelta + new Vector3(0, 4, 0);
        //Debug.DrawRay(rayStartPos, Vector3.down * 3, Color.magenta, 5);
        RaycastHit hitInfo;
        if (Physics.Raycast(rayStartPos, Vector3.down, out hitInfo, 7, hittableLayerMasks))
        {
            if(hitInfo.transform.GetComponent<EnemyScript>() != null)
            {
                if(isPlayer)
                {
                    targetEnemy = hitInfo.transform.gameObject;
                }               
                return 0;
            }
            else if(hitInfo.transform.GetComponent<PlayerMovement>() != null)
            {
                return 1;
            }
            else if(hitInfo.transform.gameObject.layer == 9)
            {
                Debug.Log("detected obstacle");
                return 2;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            return 3;
        }
    }
}
