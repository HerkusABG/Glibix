using UnityEngine;
using System.Collections.Generic;
public class EnemyScript : MonoBehaviour
{

    public List<Vector2> movementDirections;
    public List<float> distances;
    public int enemyHealth;
    public int moveCycle, moveCycleCurrent;

    private void Start()
    {
        enemyHealth = 4;
        moveCycle = 2;
        moveCycleCurrent = 0;
    }
    public void MoveEnemy(Transform playerTransform)
    {
        moveCycleCurrent++;
        if(moveCycleCurrent >= moveCycle)
        {
            MeasureDistances(playerTransform.position);
            Vector3 possibleDelta;
            int outcomeId;
            for (int i = 0; i < movementDirections.Count; i++)
            {
                possibleDelta = new Vector3(movementDirections[i].x, 0, movementDirections[i].y);
                outcomeId = TileDetector.instance.CanIMoveHere(transform.position, possibleDelta, false);
                if (outcomeId == 1)
                {
                    Destroy(playerTransform.gameObject);
                    Interpolator.instance.InterpolateMovement(gameObject, transform.position, transform.position + possibleDelta, false);
                    break;
                }
                else if (outcomeId == 3)
                {
                    Interpolator.instance.InterpolateMovement(gameObject, transform.position, transform.position + possibleDelta, false);
                    break;
                }
            }
            moveCycleCurrent = 0;
        }
    }
    

    public bool TakeDamageAndCheckIfDead()
    {
        Debug.Log("took damage");
        enemyHealth--;
        if(enemyHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
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
