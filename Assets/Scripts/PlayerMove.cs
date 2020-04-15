using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    public float speed = 3.0f;

    private Rigidbody2D _body;
    private Animator _animator;
    private float _width;
    private bool _facing_right;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _width = GetComponent<SpriteRenderer>().bounds.size.x;//gameObject.renderer.bounds.size.x;
        _facing_right = true;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        // 控制朝向
        if (!Mathf.Approximately(deltaX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(deltaX)*3, 3, 3);
            if (_facing_right && Mathf.Sign(deltaX)>0) {
                //do nothing
            }else if(_facing_right && Mathf.Sign(deltaX) < 0) {
                //do turn left
                _facing_right = !_facing_right;
                transform.position = new Vector3(transform.position.x - _width, transform.position.y, transform.position.z);
            }else if(!_facing_right && Mathf.Sign(deltaX) < 0) {
                //do nothing
            } else {
                //do turn right
                _facing_right = !_facing_right;
                transform.position = new Vector3(transform.position.x + _width, transform.position.y, transform.position.z);
            }
            
        }
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        if(movement != Vector2.zero) {
            _animator.ResetTrigger("Idle");
            _animator.SetTrigger("Walk");
            _body.velocity = movement;
        } else {
            _animator.ResetTrigger("Walk");
            _animator.SetTrigger("Idle");
        }
    }
}
