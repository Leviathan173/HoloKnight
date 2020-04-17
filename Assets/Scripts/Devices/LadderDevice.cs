using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDevice : MonoBehaviour
{
    void Awake() {
    }
    void OnTriggerEnter2D(Collider2D collider) {
        collider.SendMessage("OnLadderArea", SendMessageOptions.DontRequireReceiver);
        collider.SendMessage("SetLadderX",
            gameObject.transform.localPosition.x,
            SendMessageOptions.DontRequireReceiver);
    }
    void OnTriggerExit2D(Collider2D collider) {
        collider.SendMessage("ExitLadderArea", SendMessageOptions.DontRequireReceiver);
    }

}
