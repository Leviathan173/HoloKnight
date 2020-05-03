using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMoveChecker : MonoBehaviour
{
    public ElevatorController elevator;

    void Awake() {
        elevator = GetComponentInParent<ElevatorController>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.name.Equals("Player"))
            elevator.Move();
    }
}
