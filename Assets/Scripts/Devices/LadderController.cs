using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    private BoxCollider2D box;

    void Awake() {
        box = GetComponent<BoxCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D collider) {
        collider.SendMessage("OnLadder", SendMessageOptions.DontRequireReceiver);
        collider.SendMessage("SetLadderX",
            gameObject.transform.localPosition.x,
            SendMessageOptions.DontRequireReceiver);
    }
    void OnTriggerExit2D(Collider2D collider) {
        collider.SendMessage("ExitLadder", SendMessageOptions.DontRequireReceiver);
    }

}
