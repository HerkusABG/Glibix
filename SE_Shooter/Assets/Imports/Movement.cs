using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    [HideInInspector]
    public bool coyoteBool;
    [HideInInspector]
    public float savedSpeedLimit;


    [SerializeField]
    float playerSpeed;
    [SerializeField]
    float playerJumpStrength;
    [SerializeField]
    float playerSpeedLimit;
    [SerializeField]
    float gravity;
    [SerializeField]
    float gravityDelayValue;


    [SerializeField]
    Vector3 debugVector;
    [SerializeField]
    float debugFloat;

    
    


    public bool isGrounded;
    public ReferenceData referenceDataAccess;


    public float horizontalInputFloat, verticalInputFloat;
    float fallTimer;
    float horizontalMovementValue;


    bool canJump;

    Vector3 moveDirection;
    public Transform cameraTransform;
    Rigidbody manoRigidbody;
    CrouchScript crouchScriptAccess;
    //AreaCheckScript areaCheckScriptAccess;

    private void Start()
    {
        savedSpeedLimit = playerSpeedLimit;
        fallTimer = 0;
        //cameraTransform = referenceDataAccess.cameraReference;
        canJump = true;
        //DeathScript.isAlive = true;
        manoRigidbody = GetComponent<Rigidbody>();
        crouchScriptAccess = GetComponent<CrouchScript>();
    }

    private void GetInputs()
    {
        horizontalInputFloat = Input.GetAxisRaw("Horizontal");
        verticalInputFloat = Input.GetAxisRaw("Vertical");
    }

    private void Update()
    {
        Vector3 debugVelocity = new Vector3(manoRigidbody.linearVelocity.x, 0, manoRigidbody.linearVelocity.z);
        debugFloat = debugVelocity.magnitude;
        debugVector = manoRigidbody.linearVelocity;
        if (Input.GetKey(KeyCode.Space))
        {
            
            if (DeathScript.isAlive && !crouchScriptAccess.areWeCrouching)
            {
                if (canJump && coyoteBool)
                {
                    Jump(transform.up);
                }            
            }
        }
        else
        {
            playerSpeedLimit = savedSpeedLimit;
        }
        //areaCheckScriptAccess.GroundCheckVoid();
        GetInputs();
    }
   
    void Jump(Vector3 direction)
    {
        canJump = false;
        Invoke("EnableJumping", 0.2f);  
        manoRigidbody.linearVelocity = new Vector3(manoRigidbody.linearVelocity.x, 0, manoRigidbody.linearVelocity.z);
        manoRigidbody.AddForce((direction) * 10000 * playerJumpStrength, ForceMode.Force);
    }
    private void FixedUpdate()
    {
        MovingVoid();   
        SpeedClamping();
        if(!isGrounded)
        {
            fallTimer = fallTimer + Time.fixedDeltaTime;
            float newGravity = -Mathf.Pow(gravity, fallTimer - gravityDelayValue);
            manoRigidbody.linearVelocity += new Vector3(0, newGravity, 0);
            manoRigidbody.linearVelocity = new Vector3(manoRigidbody.linearVelocity.x, Mathf.Clamp(manoRigidbody.linearVelocity.y, -180, 1000), manoRigidbody.linearVelocity.z);
        }
        else
        {
            fallTimer = 0;
        }
    }
    private void MovingVoid()
    {
        moveDirection = (transform.forward * verticalInputFloat + transform.right * horizontalInputFloat).normalized;
        horizontalMovementValue = moveDirection.magnitude;
        //moveDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection;
        manoRigidbody.AddForce(moveDirection * playerSpeed * 1000, ForceMode.Force);
        //manoRigidbody.AddForce(moveDirection * playerSpeed * 1000, ForceMode.Acceleration);
    }
    private void SpeedClamping()
    {
        Vector3 manoVelocity = new Vector3(manoRigidbody.linearVelocity.x, 0, manoRigidbody.linearVelocity.z);
        if (manoVelocity.magnitude > playerSpeedLimit)
        {
            Vector3 clampedVelocity = manoVelocity.normalized * playerSpeedLimit;
            manoRigidbody.linearVelocity = new Vector3(clampedVelocity.x, manoRigidbody.linearVelocity.y, clampedVelocity.z);
           // Debug.Log("clamping 1");

        }
    }
    
    public void InvokeCoyoteBoolDisable()
    {
        Invoke("CoyoteBoolDisable", 0.18f);
    }
    private void CoyoteBoolDisable()
    {
        coyoteBool = false;
    }

    private void DisableJumping()
    {
        canJump = false;
    }
    private void EnableJumping()
    {
        canJump = true;
    }

    public void ChangeSpeedLimit(float newSpeedLimit)
    {
        playerSpeedLimit = newSpeedLimit;
    }
}
