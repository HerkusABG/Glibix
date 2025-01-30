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
        if(objectToMove != null)
        {
            Debug.Log("activated");
            float time = 0;
            float duration = 0.125f;
            float fraction;
            objectToMove.position = Vector3.Lerp(startPos, endPos, 0);
            while (time < duration)
            {
                time += Time.deltaTime;
                //time += 0.004f;
                fraction = time / duration;
                objectToMove.position = Vector3.Lerp(startPos, endPos, fraction);
                yield return new WaitForEndOfFrame();
                //yield return new WaitForSeconds(0.004f);
            }
            yield return new WaitForEndOfFrame();
            objectToMove.position = Vector3.Lerp(startPos, endPos, 1);
            if (isPlayer)
            {
                //objectToMove.GetComponent<PlayerMovement>().canMove = true;
                objectToMove.GetComponent<PlayerMovement>().BeginTurnTransfer();
            }
        }
        
    }
}
