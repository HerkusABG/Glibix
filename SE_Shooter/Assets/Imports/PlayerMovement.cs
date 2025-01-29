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

    private void Start()
    {
        canMove = true;
        playerCombatAccess = GetComponent<PlayerCombat>();
    }
    private void Update()
    {
        if(AreInputsDetected() && canMove)
        {
            int movementStatus = TileDetector.instance.CanIMoveHere(transform.position, GetInputsAsVector(), true);
            if (movementStatus == 0)
            {
                playerCombatAccess.Attack();
            }
            else if(movementStatus == 1)
            {
                StartMovePlayer();
            }
        }    
    }

    public void StartMovePlayer()
    {
        canMove = false;
        MovePlayer();
    }

    private bool AreInputsDetected()
    {
        horizontalInputFloat = Input.GetAxisRaw("Horizontal");
        verticalInputFloat = Input.GetAxisRaw("Vertical");
        if(horizontalInputFloat != 0 || verticalInputFloat != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private Vector3 GetInputsAsVector()
    {
        transformDelta = new Vector3(horizontalInputFloat, 0, verticalInputFloat);
        return transformDelta;
    }

    /*private bool CanIMoveHere(Vector3 inputDelta)
    {
        Vector3 rayStartPos = transform.position + inputDelta;
        Debug.DrawRay(rayStartPos, Vector3.down * 3, Color.magenta, 5);
        if(Physics.Raycast(rayStartPos, Vector3.down, 3, groundLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    } */
    private void MovePlayer()
    {
        //Vector3 transformDelta = GetInputsAsVector();
        Vector3 endPosOutput = transform.position + transformDelta;
        Interpolator.instance.InterpolateMovement(gameObject, transform.position, endPosOutput, true);
    }

    public void BeginTurnTransfer()
    {
        canMove = true;
        turnManagerAccess.ExecuteEnemyTurns(transform.position);
    }
}
