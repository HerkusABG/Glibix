using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<Vector3> pickedPositionList;
    public List<EnemyScript> enemyList;

    public int enemyCount;



    [SerializeField]
    GameManager gameManagerAccess;

    private void Awake()
    {
        enemyCount = 0;
        gameManagerAccess.preRestartEvent += ResetEnemyManager;
    }


    private void ResetEnemyManager()
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
            //StartCoroutine(levelGeneratorAccess.GenerationCoroutine());
            gameManagerAccess.CallPreRestartEvent();
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
