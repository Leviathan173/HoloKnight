using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager {
    [SerializeField] private Player player;

    private Rigidbody2D _body;
    private float jumpForce;

    public int jumpStat {
        get;
        set;
    }

    public ManagerStatus status { get; private set; }

    public void Startup() {
        print("starting PlayerManager...");
        _body = player.GetComponent<Rigidbody2D>();
        jumpForce = 12.0f;
        jumpStat = -1;

        status = ManagerStatus.Started;
    }

    public void AddForce(float force = 0) {
        //print("add force" + force);
        if(Mathf.Approximately(force,0)) {
            if(_body.velocity.y >= 0) {
                _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            } else {
                _body.AddForce(Vector2.up * (jumpForce + (-_body.velocity.y)), ForceMode2D.Impulse);
            }
            
        } else {
            if (_body.velocity.y >= 0) {
                _body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            } else {
                _body.AddForce(Vector2.up * (force + (-_body.velocity.y)), ForceMode2D.Impulse);
            }
        }
    }


}
