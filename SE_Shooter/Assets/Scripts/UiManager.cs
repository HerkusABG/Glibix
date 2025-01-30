using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiManager : MonoBehaviour
{
    public GameObject loserText;
    public GameObject panel;

    Vector4 fullColor = new Vector4(0, 0, 0, 1);
    Vector4 blankColor = new Vector4(0, 0, 0, 0);

    [SerializeField]
    GameManager gameManagerAccess;
    private void Start()
    {
        gameManagerAccess.playerDeathEvent += UiGameLost;
        gameManagerAccess.restartEvent += ResetUi;
        gameManagerAccess.preRestartEvent += BeginResetUi;
        loserText.SetActive(false);
        //panel.GetComponent<Image>().color = blankColor;
    }
    public void UiGameLost()
    {
        loserText.SetActive(true);
    }
    private void BeginResetUi()
    {
        StartCoroutine(CanvasInterpolation(blankColor, fullColor, true));
    }
    public void ResetUi()
    {
        loserText.SetActive(false);
        StartCoroutine(CanvasInterpolation(fullColor, blankColor, false));
    }

    private IEnumerator CanvasInterpolation(Vector4 startVec, Vector4 endVec, bool shouldCallEvent)
    {
        float time = 0;
        float duration = 0.8f;
        float fraction;
        while (time < duration)
        {
            time += Time.deltaTime;
            fraction = time / duration;
            panel.GetComponent<Image>().color = Vector4.Lerp(startVec, endVec, fraction);
            yield return new WaitForEndOfFrame();
        }
        panel.GetComponent<Image>().color = Vector4.Lerp(startVec, endVec, 1);
        if(shouldCallEvent)
        {
            gameManagerAccess.RestartGameLogic();
        }
    }
}
