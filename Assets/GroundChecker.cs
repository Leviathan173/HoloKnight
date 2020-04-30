using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private Collider2D collider;
    ContactFilter2D contactFilter2D = new ContactFilter2D();
    Collider2D[] res = new Collider2D[10];
    bool hited = false;

    void Awake() {
        collider = GetComponent<Collider2D>();
        contactFilter2D.useTriggers = true;
    }

    void FixedUpdate() {
        collider.OverlapCollider(contactFilter2D, res);
        if (res != null) {
            foreach (var hit in res) {
                if (hit.tag.Equals("Ground")) {
                    Managers.Player._isGrounded = true;
                    Managers.Player.animator.SetBool(PAParameters.GROUND, true);
                    hited = true;
                    break;
                }
            }
            if (!hited) {
                Managers.Player._isGrounded = false;
                Managers.Player.animator.SetBool(PAParameters.GROUND, false);
            }
        }
        
        //print("IsTouchingLayers?" + collider.IsTouchingLayers(LayerMask.NameToLayer("Ground")));
        //if (collider.IsTouchingLayers(LayerMask.NameToLayer("Ground"))) {
        //    Managers.Player._isGrounded = true;
        //    Managers.Player.animator.SetBool(PAParameters.GROUND, true);
        //} else {
        //    Managers.Player._isGrounded = false;
        //    Managers.Player.animator.SetBool(PAParameters.GROUND, false);
        //}
    }

    //void OnTriggerEnter2D(Collider2D collider) {
        
    //    if (collider.gameObject.tag.Equals("Ground")) {
    //        Managers.Player._isGrounded = true;
    //        Managers.Player.animator.SetBool(PAParameters.GROUND, true);
    //    }
    //}

    //void OnTriggerExit2D(Collider2D collider) {
    //    Managers.Player._isGrounded = false;
    //    Managers.Player.animator.SetBool(PAParameters.GROUND, false);
    //}
}
