using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    LevelGenerator levelGeneratorAccess;

    private void Start()
    {
        StartCoroutine(levelGeneratorAccess.GenerationCoroutine());
        
    }
}
