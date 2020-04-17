using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachLadderEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        //print("v2 + v2:" + (new Vector2(12, 3) + new Vector2(12, 3)));
        if (gameObject.name.Equals("Top")) {
            collider.SendMessage("SetLadderTopPos", GetRealPos(gameObject.transform), SendMessageOptions.DontRequireReceiver);
            collider.SendMessage("ReachLadderEnd", "Top", SendMessageOptions.DontRequireReceiver);
        } else {
            collider.SendMessage("SetLadderBottomPos", GetRealPos(gameObject.transform), SendMessageOptions.DontRequireReceiver);
            collider.SendMessage("ReachLadderEnd", "Bottom", SendMessageOptions.DontRequireReceiver);
        }
        
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (gameObject.name.Equals("Top")) {
            collider.SendMessage("LeaveLadderEnd", "Top", SendMessageOptions.DontRequireReceiver);
        } else {
            collider.SendMessage("LeaveLadderEnd", "Bottom", SendMessageOptions.DontRequireReceiver);
        }
    }
    
    private Vector2 GetRealPos(Transform obj) {
        //print(obj.parent);
        //print(obj.parent.transform.parent);
        //print(obj.parent.transform.parent.transform.parent);
        //if (obj.parent != null) {
        //    sum += new Vector2(obj.transform.position.x,obj.transform.position.y );
        //    print("pos = "+sum);
        //    GetRealPos(obj.parent.transform, sum);
        //}
        //sum += new Vector2(obj.transform.position.x, obj.transform.position.y);

        //return sum;
        Vector2 v2 = new Vector2(obj.transform.position.x, obj.transform.position.y);
        //print("v2:"+v2);
        return v2;
    }
}
