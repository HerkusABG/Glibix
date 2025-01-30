using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TurnManager : MonoBehaviour
{
    [SerializeField]
    PlayerMovement playerMovementAccess;
    [SerializeField]
    EnemyManager enemyManagerAccess;

    public IEnumerator ExecuteEnemyTurns(Transform playerTransformInput)
    {
        Debug.Log("enemy turns commence");
        enemyManagerAccess.pickedPositionList.Clear();
        /* foreach (EnemyScript enemy in enemyList)
         {
             if(enemy != null)
             { 
                 if(!enemy.justAwoken)
                 {
                     enemy.MoveEnemy(playerTransformInput);
                 }
                 else
                 {
                    // enemy.justAwoken = false;
                 }

             }       
         } */
        int cycleCount = enemyManagerAccess.enemyList.Count;
        for(int i = 0; i < cycleCount; i++)
        {
            if(enemyManagerAccess.enemyList[i] != null)
            {
                enemyManagerAccess.enemyList[i].MoveEnemy(playerTransformInput);

            }
        } 
        yield return new WaitForSeconds(0.125f);
        playerMovementAccess.canMove = true;
    }
}
