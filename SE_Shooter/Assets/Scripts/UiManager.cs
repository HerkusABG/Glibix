using UnityEngine;
using System.Collections;

public class UiManager : MonoBehaviour
{
    public GameObject loserText;

    [SerializeField]
    GameManager gameManagerAccess;
    private void Start()
    {
        gameManagerAccess.playerDeathEvent += UiGameLost;
        gameManagerAccess.restartEvent += ResetUi;
        loserText.SetActive(false);
    }
    public void UiGameLost()
    {
        loserText.SetActive(true);
    }
    public void ResetUi()
    {
        loserText.SetActive(false);
    }
    private IEnumerator MovementInterpolationCoroutine(Transform objectToMove, Vector3 startPos, Vector3 endPos, bool isPlayer)
    {
        float time = 0;
        float duration = 0.125f;
        float fraction;
        while (time < duration)
        {
            time += Time.deltaTime;
            fraction = time / duration;
            objectToMove.position = Vector3.Lerp(startPos, endPos, fraction);
            yield return new WaitForEndOfFrame();
        }
        objectToMove.position = Vector3.Lerp(startPos, endPos, 1);
        if (isPlayer)
        {
            //objectToMove.GetComponent<PlayerMovement>().canMove = true;
            objectToMove.GetComponent<PlayerMovement>().BeginTurnTransfer();
        }
    }
}
