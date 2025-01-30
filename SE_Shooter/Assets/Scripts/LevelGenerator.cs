using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelGenerator : MonoBehaviour
{
    public GameObject tileInstance;
    public GameObject enemyInstance;
    public GameObject obstacleInstance;

    public Vector2 fieldDimensions;
    [SerializeField]
    Vector2 playerSpawnDimensions;
    [SerializeField]
    Vector2 obstacleSpawnDimensions;
  
    public int howManyObstacles;
    public float deprecationFactor;

    
    public PlayerMovement playerMovementAccess;

    [SerializeField]
    TurnManager turnManagerAccess;

    [SerializeField]
    public EnemyManager enemyManagerAccess;

    public List<GameObject> floorObjects = new List<GameObject>();
    public List<GameObject> obstacleObjects = new List<GameObject>();

    public bool enoughConnectedTiles;
    public int connectedTileCount;

    Vector2[] directions = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    [HideInInspector]
    public GameObject initialCubeInstance;

    bool playerGeneratedOnTile;

    
    void Start()
    {
        if(deprecationFactor == 0)
        {
            deprecationFactor = 1;
        }
        enoughConnectedTiles = false;

       // StartCoroutine(GenerationCoroutine());
    }

    public IEnumerator GenerationCoroutine()
    {
        enoughConnectedTiles = false;

        GenerateNewLevel();

        while(!enoughConnectedTiles)
        {
            yield return new WaitForEndOfFrame();
            GenerateNewLevel();
        }
        yield return new WaitForEndOfFrame();
        SpawnPlayer();
        SpawnEnemies(7);
    }

    

    public void GenerateNewLevel()
    {
        connectedTileCount = 0;
        DeleteOldAssets();
        floorObjects.Clear();
        obstacleObjects.Clear();

        GenerateFloor((int)fieldDimensions.x, (int)fieldDimensions.y);
        SpawnObstacles(howManyObstacles);
        
        InitiateTileCheck();
        CheckConnectedTileCount();
        PurgeClosedOffTiles();
        
        //SpawnEnemies(7);
    }
    public void InitiateTileCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3((int)(fieldDimensions.x * 0.5f), 1, (int)(fieldDimensions.x * 0.5f)), -transform.up, out hit, 2))
        {
            if (hit.transform.GetComponent<TileScript>())
            {
                hit.transform.GetComponent<TileScript>().FindConnectedTiles(directions);
            }
        }
    }

    public void SpawnPlayer()
    {
        Vector3 playerSpawnLocation = FindFreeSpot();
        playerMovementAccess.gameObject.transform.position = playerSpawnLocation;
        playerMovementAccess.gameObject.SetActive(true);


    }

    private Vector3 FindFreeSpot()
    {
        Vector3 freeSpotLocation = new Vector3((int)Random.Range(0, (int)fieldDimensions.x), 1, (int)Random.Range(0, (int)fieldDimensions.y));
        //Debug.Log(freeSpotLocation);
        int outcomeId = TileDetector.instance.CanIMoveHere(freeSpotLocation, new Vector3(0, 0, 0), false);
        while (outcomeId != 3)
        {
            freeSpotLocation = new Vector3((int)Random.Range(0, (int)fieldDimensions.x), 1, (int)Random.Range(0, (int)fieldDimensions.y));
            outcomeId = TileDetector.instance.CanIMoveHere(freeSpotLocation, new Vector3(0, 0, 0), false);
            
        }
        return freeSpotLocation;
    }

    public void SpawnEnemies(int howManyEnemies)
    {
        for(int i = 0; i < howManyEnemies; i++)
        {
            Vector3 enemySpawnLocation = FindFreeSpot();

            GameObject enemyClone = Instantiate(enemyInstance, enemySpawnLocation, Quaternion.identity);
            enemyClone.SetActive(true);
            enemyClone.GetComponent<EnemyScript>().EnemySetUp();
            enemyManagerAccess.enemyList.Add(enemyClone.GetComponent<EnemyScript>());
        }
    }

    private void SpawnObstacles(int howManyObstacles)
    {
        for (int i = 0; i < howManyObstacles; i++)
        {
            Vector3 obstacleSpawnLocation = FindFreeSpot();

            GameObject obstacleClone = Instantiate(obstacleInstance, obstacleSpawnLocation, Quaternion.identity);
            //Debug.Log("Spawned");
            obstacleClone.SetActive(true);
            obstacleObjects.Add(obstacleClone);
        }
    }
    public void GenerateFloor(int inputX, int inputY)
    {
        GameObject parent = new GameObject("floor_parent");
        floorObjects.Add(parent);

        for (int x = 0; x < inputX; x++)
        {
            for (int y = 0; y < inputY; y++)
            {
                if(Random.Range(1, 11) < deprecationFactor * 10)
                {
                    GameObject tileClone = Instantiate(tileInstance, new Vector3(x, 0, y), Quaternion.identity);
                    tileClone.SetActive(true);
                    floorObjects.Add(tileClone);
                    tileClone.transform.SetParent(parent.transform);
                }
                
            }
        } 
    }
    private void DeleteOldAssets()
    {
        /*foreach(GameObject floorObject in floorObjects)
        {
            if(floorObject != null)
            {
                Destroy(floorObject);
            }
        }*/
        if(floorObjects.Count > 0)
        {
            Destroy(floorObjects[0]);
        }
        if(obstacleObjects.Count > 0)
        {
            Destroy(obstacleObjects[0]);
        }
        enemyManagerAccess.DeleteAllEnemies();
        /*foreach (GameObject obstacle in obstacleObjects)
        {
            if (obstacle != null)
            {
                Destroy(obstacle);
            }
        } */

    }
    private void PurgeClosedOffTiles()
    {
        foreach (GameObject floorObject in floorObjects)
        {
            if(floorObject.GetComponent<TileScript>())
            {
                if (floorObject.GetComponent<TileScript>().connectionStatus == 0)
                {
                    floorObject.GetComponent<TileScript>().IsWallTile();

                    Destroy(floorObject);
                    //floorObject.SetActive(false);
                }
            }
           
        }
    }

    public void CheckConnectedTileCount()
    {
        foreach (GameObject floorObject in floorObjects)
        {
            if (floorObject.GetComponent<TileScript>())
            {
                if (floorObject.GetComponent<TileScript>().connectionStatus == 1)
                {
                    connectedTileCount++;
                }
            }
        }
        if(connectedTileCount > ((int)fieldDimensions.x * (int)fieldDimensions.y - obstacleObjects.Count) * 0.5f)
        {
            enoughConnectedTiles = true;
            //SpawnEnemies(7);

            //Debug.Log("enough");
        }
        else
        {
            enoughConnectedTiles = false;

           // Debug.Log("not enough");
        }
        
    }
}
