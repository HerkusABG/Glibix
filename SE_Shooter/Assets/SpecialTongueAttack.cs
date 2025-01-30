using UnityEngine;

public class SpecialTongueAttack : MonoBehaviour
{
    EnemyScript enemyScriptAccess;

    [SerializeField]
    LevelGenerator levelGeneratorAccess;

    private void Start()
    {
        enemyScriptAccess = GetComponent<EnemyScript>();
        enemyScriptAccess.specialAttackEvent += TongueAttack;
    }
    private void TongueAttack()
    {
        Debug.Log("hfhf");
        //levelGeneratorAccess.SpawnEnemies(1);
    }
}
