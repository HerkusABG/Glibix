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
        if(TileDetector.instance.targetEnemy.GetComponent<EnemyScript>().TakeDamageAndCheckIfDead())
        {
            Destroy(TileDetector.instance.targetEnemy);
            playerMovementAccess.StartMovePlayer();
        }
    }
}
