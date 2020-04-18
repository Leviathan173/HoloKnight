using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachLadderEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        //print("v2 + v2:" + (new Vector2(12, 3) + new Vector2(12, 3)));
        if (gameObject.name.Equals("Top")) {
            //collider.SendMessage("SetLadderTopPos", GetRealPos(gameObject.transform), SendMessageOptions.DontRequireReceiver);
            Managers.Player._ladderTopPos = GetRealPos(gameObject.transform);
            //collider.SendMessage("ReachLadderEnd", "Top", SendMessageOptions.DontRequireReceiver);
            Managers.Player._reachTopLadder = true;
        } else {
            //collider.SendMessage("SetLadderBottomPos", GetRealPos(gameObject.transform), SendMessageOptions.DontRequireReceiver);
            Managers.Player._ladderBottomPos = GetRealPos(gameObject.transform);
            //collider.SendMessage("ReachLadderEnd", "Bottom", SendMessageOptions.DontRequireReceiver);
            Managers.Player._reachBottomLadder = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (gameObject.name.Equals("Top")) {
            //collider.SendMessage("LeaveLadderEnd", "Top", SendMessageOptions.DontRequireReceiver);
            Managers.Player._reachTopLadder = false;
        } else {
            //collider.SendMessage("LeaveLadderEnd", "Bottom", SendMessageOptions.DontRequireReceiver);
            Managers.Player._reachBottomLadder = false;
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
