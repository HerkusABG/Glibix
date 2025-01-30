using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField]
    GameObject followObject;
    
    void Update()
    {
        transform.position = followObject.transform.position;
    }
}
