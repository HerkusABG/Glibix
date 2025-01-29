using UnityEngine;
using System.Collections.Generic;


public class TurnManager : MonoBehaviour
{
    public List<EnemyScript> enemyList;
    

    public void ExecuteEnemyTurns(Vector3 playerPositionInput)
    {
        foreach(EnemyScript enemy in enemyList)
        {
            if(enemy != null)
            {
                enemy.MoveEnemy(playerPositionInput);
            }
            
        }
    }
}
