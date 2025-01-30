using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    LevelGenerator levelGeneratorAccess;

    public bool isGameLost;

    [SerializeField]
    UiManager uiManagerAccess;

    public delegate void PlayerDeathAction();
    public event PlayerDeathAction playerDeathEvent;

    public delegate void RestartAction();
    public event RestartAction restartEvent;

    public delegate void PreRestartAction();
    public event PreRestartAction preRestartEvent;
    private void Start()
    {
        RestartGameLogic();
        // restartEvent += RestartGameLogic;
        //CallPreRestartEvent();
    }
    public void CallRestartEvent()
    {
        if (restartEvent != null)
        {
            restartEvent();
        }
    }
    public void CallDeathEvent()
    {
        if (playerDeathEvent != null)
        {
            isGameLost = true;
            playerDeathEvent();
        }
    }
    public void CallPreRestartEvent()
    {
        if (preRestartEvent != null)
        {
            preRestartEvent();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(isGameLost)
            {
               
                //CallRestartEvent();
                CallPreRestartEvent();
                //StartCoroutine(levelGeneratorAccess.GenerationCoroutine());
            }
            
        }
    }

    public void RestartGameLogic()
    {
        isGameLost = false;
        StartCoroutine(levelGeneratorAccess.GenerationCoroutine());
    }
}
