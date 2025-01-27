using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheckScript : MonoBehaviour
{
    public List<Vector2> raycastPositions;

    public Transform groundRaycastLocator;

    public LayerMask areaCheckLayerMasks;


    Vector3 raycastDirection;


    Movement movementAccess;
    Rigidbody rigidbodyAccessACS;

    [SerializeField]
    float groundDrag;
    [SerializeField]
    float airDrag;

    private void Start()
    {
        raycastDirection = groundRaycastLocator.up;
        movementAccess = GetComponent<Movement>();
        rigidbodyAccessACS = GetComponent<Rigidbody>();
        ChangeRaycastSourceLocation(-1.5f);
        //areaCheckLayerMasks;// = movementAccess.manoLayerMask;
    }

    private void Update()
    {
        groundRaycastLocator.rotation = Quaternion.Euler(new Vector3(0,0,0));
        GroundCheckVoid();
    }

    public void ChangeRaycastSourceLocation(float newYpos)
    {
        groundRaycastLocator.localPosition = new Vector3(groundRaycastLocator.localPosition.x,  newYpos, groundRaycastLocator.localPosition.z);
    }
    
    public void GroundCheckVoid()
    {
        float jumpSum = 0;
        for (int i = 0; i < raycastPositions.Count; i++)
        {
            RaycastHit hitInfoP;
            Debug.DrawRay(groundRaycastLocator.position + new Vector3(raycastPositions[i].x, 0, raycastPositions[i].y), -raycastDirection * (transform.localScale.y * 0.5f + 0.2f), Color.cyan);
            if (Physics.Raycast(groundRaycastLocator.position + new Vector3(raycastPositions[i].x, 0, raycastPositions[i].y), -raycastDirection, out hitInfoP, transform.localScale.y * 0.5f + 0.4f, areaCheckLayerMasks))
            {
                jumpSum++;
            }
        }
        if (jumpSum >= 1)
        {
            movementAccess.isGrounded = true;
            movementAccess.coyoteBool = true;

            if(DeathScript.isAlive)
            {
                rigidbodyAccessACS.linearDamping = groundDrag;
            }
            
           
        }
        else
        {
            movementAccess.isGrounded = false;
            movementAccess.InvokeCoyoteBoolDisable();
            if (DeathScript.isAlive)
            {
                rigidbodyAccessACS.linearDamping = airDrag;
            }
            
            
            
        }
    }

    public bool CeilingCheck()
    {
        float ceilingJumpSum = 0;
        for (int i = 0; i < raycastPositions.Count; i++)
        {
            RaycastHit hitInfoP;
            Debug.DrawRay(groundRaycastLocator.position + new Vector3(raycastPositions[i].x, 0, raycastPositions[i].y), raycastDirection * (3.5f), Color.red);
            if (Physics.Raycast(groundRaycastLocator.position + new Vector3(raycastPositions[i].x, 0, raycastPositions[i].y), raycastDirection, out hitInfoP, 3.5f, areaCheckLayerMasks))
            {
                ceilingJumpSum++;
            }
        }
        if (ceilingJumpSum >= 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AreaCheckDeath()
    {
        rigidbodyAccessACS.linearDamping = 3f;
        ChangeRaycastSourceLocation(0);
    }

    public void AreaCheckReset()
    {
        rigidbodyAccessACS.linearDamping = groundDrag;
        ChangeRaycastSourceLocation(-1.5f);
    }
}
