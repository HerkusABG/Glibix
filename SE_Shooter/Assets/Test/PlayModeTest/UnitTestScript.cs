using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTestScript
{
    
    [UnityTest]
    public IEnumerator TileGenerationTest()
    {
        GameObject tileObject = new GameObject();
        TileScript tileScript = tileObject.AddComponent<TileScript>();
        tileObject.AddComponent<BoxCollider>();
        tileObject.transform.localScale = new Vector3(0.9f, 1, 0.9f);
        tileObject.layer = 6;
        GameObject tileDetectorObject = new GameObject();
        tileDetectorObject.AddComponent<TileDetector>();


        GameObject levelGeneratorObject = new GameObject();
        LevelGenerator levelGenerator = levelGeneratorObject.AddComponent<LevelGenerator>();
        levelGenerator.fieldDimensions.x = 5;
        levelGenerator.fieldDimensions.y = 5;
        levelGenerator.howManyObstacles = 5;
        levelGenerator.tileInstance = tileObject;

        
        //GameObject enemyManagerObject = new GameObject();
        //EnemyManager enemyManager = enemyManagerObject.AddComponent<EnemyManager>();

        //GameObject enemyObject = new GameObject();
        //enemyObject.AddComponent<EnemyScript>();

        GameObject playerObject = new GameObject();
        levelGenerator.playerMovementAccess = playerObject.AddComponent<PlayerMovement>();
        
        levelGenerator.GenerateFloor((int)levelGenerator.fieldDimensions.x, (int)levelGenerator.fieldDimensions.y);
        Assert.AreEqual((int)levelGenerator.fieldDimensions.x * (int)levelGenerator.fieldDimensions.y + 1, levelGenerator.floorObjects.Count);

        yield return null;
    }
     [UnityTest]
     public IEnumerator ConnectionTest()
     {
         GameObject tileObject = new GameObject();
         TileScript tileScript = tileObject.AddComponent<TileScript>();
         tileObject.AddComponent<BoxCollider>();
         tileObject.transform.localScale = new Vector3(0.9f, 1, 0.9f);
         tileObject.layer = 6;
         GameObject tileDetectorObject = new GameObject();
         tileDetectorObject.AddComponent<TileDetector>();


         GameObject levelGeneratorObject = new GameObject();
         LevelGenerator levelGenerator = levelGeneratorObject.AddComponent<LevelGenerator>();
         levelGenerator.fieldDimensions.x = 5;
         levelGenerator.fieldDimensions.y = 5;
         levelGenerator.howManyObstacles = 5;
         levelGenerator.tileInstance = tileObject;

         GameObject playerObject = new GameObject();
         levelGenerator.playerMovementAccess = playerObject.AddComponent<PlayerMovement>();

         levelGenerator.GenerateFloor((int)levelGenerator.fieldDimensions.x, (int)levelGenerator.fieldDimensions.y);

        levelGenerator.playerMovementAccess.transform.position = new Vector3(2, 1, 0);
        levelGenerator.InitiateTileCheck();
        yield return new WaitForSeconds(1);
        for (int i = 2; i < levelGenerator.floorObjects.Count; i++)
        {
            if (levelGenerator.floorObjects[i].GetComponent<TileScript>() != null)
            {
                Assert.AreEqual(1, levelGenerator.floorObjects[i].GetComponent<TileScript>().connectionStatus);
            }

        }
        yield return null;
     }

    [UnityTest]
    public IEnumerator MovementTest()
    {
        GameObject tileObject = new GameObject();
        TileScript tileScript = tileObject.AddComponent<TileScript>();
        tileObject.AddComponent<BoxCollider>();
        tileObject.transform.localScale = new Vector3(0.9f, 1, 0.9f);
        tileObject.layer = 6;
        GameObject tileDetectorObject = new GameObject();
        tileDetectorObject.AddComponent<TileDetector>();


        GameObject levelGeneratorObject = new GameObject();
        LevelGenerator levelGenerator = levelGeneratorObject.AddComponent<LevelGenerator>();
        levelGenerator.fieldDimensions.x = 5;
        levelGenerator.fieldDimensions.y = 5;
        levelGenerator.howManyObstacles = 5;
        levelGenerator.tileInstance = tileObject;

        GameObject playerObject = new GameObject();
        levelGenerator.playerMovementAccess = playerObject.AddComponent<PlayerMovement>();

        GameObject interpolatorObject = new GameObject();
        interpolatorObject.AddComponent<Interpolator>();

        levelGenerator.GenerateFloor((int)levelGenerator.fieldDimensions.x, (int)levelGenerator.fieldDimensions.y);

        levelGenerator.playerMovementAccess.transform.position = new Vector3(2, 1, 0);
        Vector3 oldPlayerPosition = levelGenerator.playerMovementAccess.transform.position;
        levelGenerator.playerMovementAccess.horizontalInputFloat = 1;
        Vector3 delta = levelGenerator.playerMovementAccess.GetInputsAsVector();
        Interpolator.instance.InterpolateMovement(levelGenerator.playerMovementAccess.gameObject, oldPlayerPosition, oldPlayerPosition + delta, false);
        

        yield return new WaitForSeconds(2);
        Assert.AreEqual(oldPlayerPosition + delta, levelGenerator.playerMovementAccess.transform.position);

        yield return null;
    }
}
