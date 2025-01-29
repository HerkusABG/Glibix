using UnityEngine;
using System.Collections.Generic;


public class TurnManager : MonoBehaviour
{
    public List<EnemyScript> enemyList;
    

    public void ExecuteEnemyTurns(Transform playerTransformInput)
    {
        foreach(EnemyScript enemy in enemyList)
        {
            if(enemy != null)
            {
                enemy.MoveEnemy(playerTransformInput);
            }       
        }
    }
}
