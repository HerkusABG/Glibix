using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerMovement playerMovementAccess;

    private void Start()
    {
        playerMovementAccess = GetComponent<PlayerMovement>();
    }
    public void Attack()
    {
        int outcomeId = TileDetector.instance.targetEnemy.GetComponent<EnemyScript>().TakeDamageAndCheckIfDead();
        if (outcomeId == 0)
        {
            TileDetector.instance.targetEnemy.GetComponent<EnemyScript>().isDead = true;
            Destroy(TileDetector.instance.targetEnemy);
            playerMovementAccess.canMove = true;


        }
        else if(outcomeId == 1)
        {
            TileDetector.instance.targetEnemy.GetComponent<EnemyScript>().isDead = true;
            Destroy(TileDetector.instance.targetEnemy);
            playerMovementAccess.StartMovePlayer();
        }
    }
}
