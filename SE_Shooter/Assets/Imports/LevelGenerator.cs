using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject tileInstance;

    [SerializeField]
    Vector2 fieldDimensions;
    [SerializeField]
    Vector2 playerSpawnDimensions;

    [SerializeField]
    PlayerMovement playerMovementAccess;
    void Start()
    {
        GenerateLevel((int)fieldDimensions.x, (int)fieldDimensions.y);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        playerMovementAccess.gameObject.transform.position = new Vector3(playerSpawnDimensions.x, 1, playerSpawnDimensions.y);
    }
    private void GenerateLevel(int inputX, int inputY)
    {
        for(int x = 0; x < inputX; x++)
        {
            for (int y = 0; y < inputY; y++)
            {
               GameObject tileClone =  Instantiate(tileInstance, new Vector3(x, 0, y), Quaternion.identity);
                tileClone.SetActive(true);
            }
        }
    }
   
}
