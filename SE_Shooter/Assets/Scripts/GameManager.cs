using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    LevelGenerator levelGeneratorAccess;

    public bool isGameLost;


    public delegate void PlayerDeathAction();
    public event PlayerDeathAction playerDeathEvent;

    public delegate void RestartAction();
    public event RestartAction restartEvent;
    private void Start()
    {
        StartCoroutine(levelGeneratorAccess.GenerationCoroutine());
        
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(isGameLost)
            {
                isGameLost = false;
                CallRestartEvent();
                StartCoroutine(levelGeneratorAccess.GenerationCoroutine());
            }
            
        }
    }
}
