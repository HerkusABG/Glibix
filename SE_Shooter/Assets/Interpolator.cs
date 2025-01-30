using UnityEngine;
using System.Collections;

public class Interpolator : MonoBehaviour
{
    public static Interpolator instance;


    private void Awake()
    {
        instance = this;
    }

   
    public void InterpolateMovement(GameObject objectToMove, Vector3 startPos, Vector3 endPos, bool isPlayer)
    {
        StartCoroutine(MovementInterpolationCoroutine(objectToMove.transform, startPos, endPos, isPlayer));
    }

    private IEnumerator MovementInterpolationCoroutine(Transform objectToMove, Vector3 startPos, Vector3 endPos, bool isPlayer)
    {
        float time = 0;
        float duration = 0.125f;
        float fraction;
        while(time < duration)
        {
            time += Time.deltaTime;
            fraction = time / duration;
            objectToMove.position = Vector3.Lerp(startPos, endPos, fraction);
            yield return new WaitForEndOfFrame();
        }
        objectToMove.position = Vector3.Lerp(startPos, endPos, 1);
        if(isPlayer)
        {
            //objectToMove.GetComponent<PlayerMovement>().canMove = true;
            objectToMove.GetComponent<PlayerMovement>().BeginTurnTransfer();
        }
    }
}
