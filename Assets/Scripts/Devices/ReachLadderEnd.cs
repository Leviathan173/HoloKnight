using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachLadderEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        if (gameObject.name.Equals("Top")) {
            collider.SendMessage("SetLadderTopPos", GetRealPos(gameObject.transform,Vector2.zero), SendMessageOptions.DontRequireReceiver);
            collider.SendMessage("ReachLadderEnd", "Top", SendMessageOptions.DontRequireReceiver);
        } else {
            collider.SendMessage("SetLadderBottomPos", GetRealPos(gameObject.transform, Vector2.zero), SendMessageOptions.DontRequireReceiver);
            collider.SendMessage("ReachLadderEnd", "Bottom", SendMessageOptions.DontRequireReceiver);
        }
        
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (gameObject.name.Equals("Top")) {
            collider.SendMessageUpwards("LeaveLadderEnd", "Top", SendMessageOptions.DontRequireReceiver);
        } else {
            collider.SendMessageUpwards("LeaveLadderEnd", "Bottom", SendMessageOptions.DontRequireReceiver);
        }
    }
    
    private Vector2 GetRealPos(Transform obj, Vector2 sum) {
        //print(obj.parent);
        //print(obj.parent.transform.parent);
        //print(obj.parent.transform.parent.transform.parent);
        if (obj.parent != null) {
            sum += new Vector2(obj.transform.position.x, obj.transform.position.y);
            GetRealPos(obj.transform.parent, sum);
        }
        sum += new Vector2(obj.transform.position.x, obj.transform.position.y);
        return sum;
    }
}
