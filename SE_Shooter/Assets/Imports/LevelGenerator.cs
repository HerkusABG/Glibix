using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject tileInstance;
    public GameObject enemyInstance;

    [SerializeField]
    Vector2 fieldDimensions;
    [SerializeField]
    Vector2 playerSpawnDimensions;
    [SerializeField]
    Vector2 enemySpawnDimensions;

    [SerializeField]
    PlayerMovement playerMovementAccess;

    [SerializeField]
    TurnManager turnManagerAccess;

    
    void Start()
    {
        GenerateLevel((int)fieldDimensions.x, (int)fieldDimensions.y);
        SpawnPlayer();
        SpawnEnemies();
    }

    private void SpawnPlayer()
    {
        playerMovementAccess.gameObject.transform.position = new Vector3(playerSpawnDimensions.x, 1, playerSpawnDimensions.y);
    }

    private void SpawnEnemies()
    {
        GameObject enemyClone = Instantiate(enemyInstance, new Vector3(enemySpawnDimensions.x, 1, enemySpawnDimensions.y), Quaternion.identity);
        enemyClone.SetActive(true);
        turnManagerAccess.enemyList.Add(enemyClone.GetComponent<EnemyScript>());
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
