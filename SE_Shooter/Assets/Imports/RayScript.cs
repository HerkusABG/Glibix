using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScript : MonoBehaviour
{
    Transform rayOriginTransform;
    public float x, y, z;
    [SerializeField] LayerMask rayLayerMasks;
    [SerializeField] GameObject trailObject;
    private void Start()
    {
        rayOriginTransform = GetComponentInChildren<CameraScript>().transform;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpellShootVoid();
        }
    }

    private void SpellShootVoid()
    {
        for(int i = 0; i < 6; i++)
        {
            Vector3 rayDirectionRandomized = Random.insideUnitSphere * 0.1f;
            Debug.DrawRay(rayOriginTransform.position, (rayOriginTransform.forward + rayDirectionRandomized) * 30, Color.magenta, 3);
            RaycastHit rayHitInfo;
            if(Physics.Raycast(rayOriginTransform.position, (rayOriginTransform.forward + rayDirectionRandomized), out rayHitInfo, 30, rayLayerMasks))
            {
                Debug.Log(rayHitInfo.transform.name);
                if (rayHitInfo.transform.GetComponent<EnemyScript>() != null)
                {
                    rayHitInfo.transform.GetComponent<EnemyScript>().EnemyDeath();
                    Debug.Log("Enemy Dead");
                }
                else
                {
                    Debug.Log("false");
                }
            }
        }
    }
}
