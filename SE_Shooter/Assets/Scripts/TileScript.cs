using UnityEngine;

public class TileScript : MonoBehaviour
{
    //public bool isConnected;
    public int connectionStatus;
    public string stringy;
    //GameObject myWall;

    private void Awake()
    {
        //connectionStatus = 0;
        //isConnected = false;
    }

    public void FindConnectedTiles(Vector2[] rayDirections)
    {
        RaycastHit hit;
        connectionStatus = 1;
        for (int i = 0; i < 4; i++)
        {
            if(Physics.Raycast(transform.position, new Vector3(rayDirections[i].x, 0 , rayDirections[i].y), out hit, 2))
            {
                Debug.DrawRay(transform.position, new Vector3(rayDirections[i].x, 0, rayDirections[i].y), Color.red, 4);
                if (hit.transform.GetComponent<TileScript>())
                {
                    TileScript referenceTileScript = hit.transform.GetComponent<TileScript>();
                    if(referenceTileScript.connectionStatus != 2)
                    {
                        if (referenceTileScript.IsWallTile())
                        {
                            referenceTileScript.connectionStatus = 2;
                        }
                        else if (referenceTileScript.connectionStatus == 0)
                        {
                            //Debug.DrawRay(transform.position, transform.up * 2, Color.red, 4);
                            referenceTileScript.FindConnectedTiles(rayDirections);
                            //hit.transform.GetComponent<TileScript>().connectionStatus = 1;
                        }
                    }
                    
                }
                
            }
        }
    }
    
    public bool IsWallTile()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.up, out hit, 2))
        {
            
            if(hit.transform.gameObject.layer == 9)
            {
                //Debug.DrawRay(transform.position, transform.up * 2, Color.green, 4);
                hit.transform.SetParent(gameObject.transform);
                return true;
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.up * 2, Color.red, 4);
                return false;
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.up * 2, Color.white, 4);
            return false;
        }
    }
}
