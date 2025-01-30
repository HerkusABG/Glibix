using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float horizontalInputFloat, verticalInputFloat;
    public bool canMove;
    //public LayerMask groundLayerMask;
    Vector3 transformDelta;
    [SerializeField]
    TurnManager turnManagerAccess;

    PlayerCombat playerCombatAccess;

    [SerializeField]
    GameManager gameManagerAccess;

    private void Start()
    {
        gameManagerAccess.playerDeathEvent += PlayerDeath;
        gameManagerAccess.restartEvent += ResetPlayer;
        canMove = true;
        playerCombatAccess = GetComponent<PlayerCombat>();
    }

    private void ResetPlayer()
    {
        canMove = true;
    }
    private void Update()
    {
        if(AreInputsDetected() && canMove)
        {
            int outcomeId = TileDetector.instance.CanIMoveHere(transform.position, GetInputsAsVector(), true);
            if (outcomeId == 0)
            {
                canMove = false;

                playerCombatAccess.Attack();
                //Invoke("BeginTurnTransfer", 0.5f);
            }
            else if(outcomeId == 3)
            {
                canMove = false;

                StartMovePlayer();
            }
        }    
    }

    private void PlayerDeath()
    {
        gameObject.SetActive(false);
    }
    public void StartMovePlayer()
    {      
        MovePlayer();
    }

    private bool AreInputsDetected()
    {
        horizontalInputFloat = Input.GetAxisRaw("Horizontal");
        verticalInputFloat = Input.GetAxisRaw("Vertical");
        if(horizontalInputFloat != 0 ^ verticalInputFloat != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Vector3 GetInputsAsVector()
    {
        transformDelta = new Vector3(horizontalInputFloat, 0, verticalInputFloat);
        return transformDelta;
    }

    private void MovePlayer()
    {
        Vector3 endPosOutput = transform.position + transformDelta;
        Interpolator.instance.InterpolateMovement(gameObject, transform.position, endPosOutput, true);
    }

    public void BeginTurnTransfer()
    {
        //canMove = true;
        StartCoroutine(turnManagerAccess.ExecuteEnemyTurns(transform));
        //turnManagerAccess.ExecuteEnemyTurns(transform);
    }
}
