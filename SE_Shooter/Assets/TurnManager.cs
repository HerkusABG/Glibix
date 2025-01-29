using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TurnManager : MonoBehaviour
{
    public List<EnemyScript> enemyList;
    [SerializeField]
    PlayerMovement playerMovementAccess;

    public IEnumerator ExecuteEnemyTurns(Transform playerTransformInput)
    {
        foreach(EnemyScript enemy in enemyList)
        {
            if(enemy != null)
            {
                enemy.MoveEnemy(playerTransformInput);
            }       
        }
        yield return new WaitForSeconds(0.125f);
        playerMovementAccess.canMove = true;
    }
}
