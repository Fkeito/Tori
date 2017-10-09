using UnityEngine;
using System.Collections;

public class BirdDirection:MonoBehaviour{

	public static GameObject OnDirection(GameObject direction,Vector3 startPos,Vector3 endPos)
    {
        GameObject dire = Instantiate(direction, 0.5f * (endPos + startPos), Quaternion.identity) as GameObject;
        var lr = dire.GetComponent<LineRenderer>();
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        return dire;
    }
    public static GameObject OnDirection(GameObject direction,Vector3 position,GameObject player)
    {
        GameObject dire = Instantiate(direction, position, Quaternion.identity) as GameObject;
        dire.transform.LookAt(player.transform);
        return dire;
    }
}
