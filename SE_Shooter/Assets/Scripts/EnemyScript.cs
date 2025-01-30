using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
public class EnemyScript : MonoBehaviour
{
    public List<Vector2> movementDirections;
    public List<float> distances;
    public int enemyHealth;
    public int moveCycleInterval, moveCycleCurrent;
    public int specialCycleInterval, specialCycleCurrent;
    public bool hasSpecialAbility;

    public delegate void SpecialAttackAction();
    public event SpecialAttackAction specialAttackEvent;


    [SerializeField]
    GameManager gameManagerAccess;
    
    public bool isDead;

    [SerializeField]
    EnemyManager enemyManagerAccess;

    public int myChoice;
    
    private void Start()
    {
        enemyHealth = 1;
        moveCycleInterval = 2;
        moveCycleCurrent = 0;
        specialCycleInterval = 3;
        specialCycleCurrent = 0;
    }

    private void Awake()
    {
        isDead = false;
    }

    public void EnemySetUp()
    {
        enemyManagerAccess.enemyCount++;
    }
    public void MoveEnemy(Transform playerTransform)
    {
        
        if(!isDead)
        {
            if (hasSpecialAbility)
            {
                specialCycleCurrent++;
                if (specialCycleCurrent >= specialCycleInterval)
                {
                    CallSpecialAttackEvent();
                    specialCycleCurrent = 0;
                }
            }
            moveCycleCurrent++;
            if (moveCycleCurrent >= moveCycleInterval)
            {
                Debug.Log("moving");
                MeasureDistances(playerTransform.position);
                Vector3 possibleDelta;
                int outcomeId;
                bool isNotSteppingOnOthers;
                for (int i = 0; i < movementDirections.Count; i++)
                {
                    isNotSteppingOnOthers = true;
                    possibleDelta = new Vector3(movementDirections[i].x, 0, movementDirections[i].y);
                    foreach(Vector3 pickedPosition in enemyManagerAccess.pickedPositionList)
                    {
                        if((transform.position + possibleDelta) == pickedPosition)
                        {
                            isNotSteppingOnOthers = false;
                        }
                    }
                    if(isNotSteppingOnOthers)
                    {
                        outcomeId = TileDetector.instance.CanIMoveHere(transform.position, possibleDelta, false);
                        myChoice = i;
                        if (outcomeId == 1)
                        {
                            //Destroy(playerTransform.gameObject);
                            //playerTransform.gameObject.SetActive(false);
                            gameManagerAccess.CallDeathEvent();
                            Vector3 enemyEndPosition = transform.position + possibleDelta;
                            Interpolator.instance.InterpolateMovement(gameObject, transform.position, enemyEndPosition, false);
                            enemyManagerAccess.pickedPositionList.Add(enemyEndPosition);
                            break;
                        }
                        else if (outcomeId == 3)
                        {
                            Vector3 enemyEndPosition = transform.position + possibleDelta;
                            Interpolator.instance.InterpolateMovement(gameObject, transform.position, enemyEndPosition, false);
                            enemyManagerAccess.pickedPositionList.Add(enemyEndPosition);
                            break;
                        }
                    }
                   
                }
                moveCycleCurrent = 0;
            }     
        } 
    }
    public void CallSpecialAttackEvent()
    {
        if (specialAttackEvent != null)
        {
            specialAttackEvent();
        }
    }

    public int TakeDamageAndCheckIfDead()
    {
        Debug.Log("took damage");
        enemyHealth--;
        if(enemyHealth <= 0)
        {
            if(enemyManagerAccess.AreAllEnemiesDead())
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 2;
        }
    }
    private void MeasureDistances(Vector3 playerLocVector)
    {
        //float[] distances = new float[movementDirections.Count];
        float distance;
        
        //int optimalMoveIndex = 0;
        distances.Clear();
        for(int i = 0; i < movementDirections.Count; i++)
        {
            //distances[i] =
            distance = Vector3.Distance((transform.position + new Vector3(movementDirections[i].x, 0, movementDirections[i].y)) , playerLocVector);
            distances.Add(distance);
        }
        SortDistances();
    }

    private void SortDistances()
    {
        float tempFloat;
        Vector2 tempVector2;
        for (int j = 0; j < distances.Count; j++)
        {
            for (int i = 0; i < distances.Count - 1; i++)
            {
                if (distances[i] > distances[i + 1])
                {
                    tempFloat = distances[i];
                    distances[i] = distances[i + 1];
                    distances[i + 1] = tempFloat;
                    tempVector2 = movementDirections[i];
                    movementDirections[i] = movementDirections[i + 1];
                    movementDirections[i + 1] = tempVector2;
                }
            }
        }
    }
}
