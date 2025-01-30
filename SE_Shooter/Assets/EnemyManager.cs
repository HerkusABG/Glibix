using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<Vector3> pickedPositionList;
    public List<EnemyScript> enemyList;

    public int enemyCount;

    [SerializeField]
    LevelGenerator levelGeneratorAccess;

    private void Awake()
    {
        enemyCount = 0;
    }

    public bool AreAllEnemiesDead()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            Debug.Log("Spawn new wave!");
            //levelGeneratorAccess.GenerateNewLevel();
            StartCoroutine(levelGeneratorAccess.GenerationCoroutine());
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteAllEnemies()
    {
        foreach(EnemyScript enemy in enemyList)
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject);
            }
            
        }
    }

}
