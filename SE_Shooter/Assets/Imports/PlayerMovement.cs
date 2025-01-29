using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float horizontalInputFloat, verticalInputFloat;
    public bool canMove;
    public LayerMask groundLayerMask;

    Vector3 transformDelta;


    private void Start()
    {
        canMove = true;
    }
    private void Update()
    {
        if(AreInputsDetected() && canMove)
        { 
            if(CanIMoveHere(GetInputsAsVector()))
            {
                canMove = false;
                InitiateMovement();
            }
            
        }
        
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

    private bool CanIMoveHere(Vector3 inputDelta)
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
    }
    private void InitiateMovement()
    {
        //Vector3 transformDelta = GetInputsAsVector();
        Vector3 endPosOutput = transform.position + transformDelta;
        Interpolator.instance.InterpolateMovement(gameObject, transform.position, endPosOutput, true);
    }
}
