using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDevice : MonoBehaviour
{
    void Awake() {
    }
    void OnTriggerEnter2D(Collider2D collider) {
        //collider.SendMessage("OnLadderArea", SendMessageOptions.DontRequireReceiver);
        Managers.Player._hasLadder = true;
        //collider.SendMessage("SetLadderX",
        //    gameObject.transform.localPosition.x,
        //    SendMessageOptions.DontRequireReceiver);
        Managers.Player._ladderX = gameObject.transform.localPosition.x;
    }
    void OnTriggerExit2D(Collider2D collider) {
        //collider.SendMessage("ExitLadderArea", SendMessageOptions.DontRequireReceiver);
        Managers.Player._hasLadder = false;
    }

}
