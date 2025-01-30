using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelGenerator : MonoBehaviour
{
    public GameObject tileInstance;
    public GameObject enemyInstance;
    public GameObject obstacleInstance;

    [SerializeField]
    Vector2 fieldDimensions;
    [SerializeField]
    Vector2 playerSpawnDimensions;
    [SerializeField]
    Vector2 obstacleSpawnDimensions;
    [SerializeField]
    int howManyObstacles;

    [SerializeField]
    PlayerMovement playerMovementAccess;

    [SerializeField]
    TurnManager turnManagerAccess;

    public List<GameObject> floorObjects;
    public List<GameObject> obstacleObjects;

    public bool enoughConnectedTiles;
    public int connectedTileCount;

    Vector2[] directions = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
    //directions[0] = new Vector2(0,0);

    
    void Start()
    {
        enoughConnectedTiles = false;

        StartCoroutine(GenerationCoroutine());
    }

    private IEnumerator GenerationCoroutine()
    {
        GenerateNewLevel();
        while(!enoughConnectedTiles)
        {
            yield return new WaitForEndOfFrame();
            //Invoke("GenerateNewLevel", 3);
            GenerateNewLevel();
        }
        /*for (int i = 0; i < 30; i++)
        {
            Debug.Log("loop " + i);
            //yield return new WaitForSeconds(0.125f);
            yield return new WaitForEndOfFrame();
            //Invoke("GenerateNewLevel", 3);
            GenerateNewLevel();
        }*/
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            enoughConnectedTiles = false;
            StartCoroutine(GenerationCoroutine());
        }
    }

    private void GenerateLevelInitial()
    {

    }

    public void GenerateNewLevel()
    {

            connectedTileCount = 0;
            DeleteOldAssets();

            floorObjects.Clear();
            obstacleObjects.Clear();

            GenerateFloor((int)fieldDimensions.x, (int)fieldDimensions.y);
            SpawnObstacles(howManyObstacles);
            SpawnPlayer();
            CheckConnectedTileCount();

        /*if (!enoughConnectedTiles)
        {
            connectedTileCount = 0;
            DeleteOldAssets();
            floorObjects.Clear();
            obstacleObjects.Clear();
            //GenerateFloor((int)fieldDimensions.x, (int)fieldDimensions.y);
            //SpawnObstacles(howManyObstacles);
            //SpawnPlayer();
            //enoughConnectedTiles = true;
            //CheckConnectedTileCount();
        }  */


        //enoughConnectedTiles = false;

        PurgeClosedOffTiles();
        //SpawnEnemies(7);
    }

    private void SpawnPlayer()
    {
        Vector3 playerSpawnLocation = FindFreeSpot();
        //playerMovementAccess.gameObject.transform.position = new Vector3(playerSpawnDimensions.x, 1, playerSpawnDimensions.y);
        playerMovementAccess.gameObject.transform.position = playerSpawnLocation;
        RaycastHit hit;
        if (Physics.Raycast(playerMovementAccess.transform.position, -transform.up, out hit, 2))
        {
            if(hit.transform.GetComponent<TileScript>())
            {
                hit.transform.GetComponent<TileScript>().FindConnectedTiles(directions);
            }
        }
    }

    private Vector3 FindFreeSpot()
    {
        Vector3 freeSpotLocation = new Vector3((int)Random.Range(0, (int)fieldDimensions.x), 1, (int)Random.Range(0, (int)fieldDimensions.y) + 1);
        int outcomeId = TileDetector.instance.CanIMoveHere(freeSpotLocation, new Vector3(0, 0, 0), false);
        while (outcomeId != 3)
        {
            freeSpotLocation = new Vector3((int)Random.Range(0, (int)fieldDimensions.x), 1, (int)Random.Range(0, (int)fieldDimensions.y) + 1);
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
            turnManagerAccess.enemyList.Add(enemyClone.GetComponent<EnemyScript>());
        }
    }

    private void SpawnObstacles(int howManyObstacles)
    {
        for (int i = 0; i < howManyObstacles; i++)
        {
            Vector3 obstacleSpawnLocation = FindFreeSpot();

            GameObject obstacleClone = Instantiate(obstacleInstance, obstacleSpawnLocation, Quaternion.identity);
            Debug.Log("Spawned");
            obstacleClone.SetActive(true);
            obstacleObjects.Add(obstacleClone);
        }
    }
    private void GenerateFloor(int inputX, int inputY)
    {
        GameObject parent = new GameObject("floorParent");
        floorObjects.Add(parent);
        for (int x = 0; x < inputX; x++)
        {
            for (int y = 0; y < inputY; y++)
            {
               GameObject tileClone =  Instantiate(tileInstance, new Vector3(x, 0, y), Quaternion.identity);
                tileClone.SetActive(true);
                floorObjects.Add(tileClone);
                tileClone.transform.SetParent(parent.transform);
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

    private void CheckConnectedTileCount()
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
            Debug.Log("enough");
        }
        else
        {
            enoughConnectedTiles = false;

            Debug.Log("not enough");
        }
        
    }
}
