using UnityEngine;
using System.Collections.Generic;
public class EnemyScript : MonoBehaviour
{

    public List<Vector2> movementDirections;
    public List<float> distances;

    public void MoveEnemy(Vector3 playerLocVector)
    {
        MeasureDistances(playerLocVector);
        //bool foundFreeTile = false;
        Vector3 possibleDelta;
        for (int i = 0; i < movementDirections.Count; i++)
        {
            possibleDelta = new Vector3(movementDirections[i].x, 0, movementDirections[i].y);
            if(TileDetector.instance.CanIMoveHere(transform.position, possibleDelta))
            {
                Interpolator.instance.InterpolateMovement(gameObject, transform.position, transform.position + possibleDelta, false);
            }
        }
            
        
    }

    private void Update()
    {

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
                if (distances[i] < distances[i + 1])
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
