using System;
using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameObject Forward;
    [SerializeField] private EnemyDetector[] Attacks;
    //[SerializeField] private GameObject Back;

    public float speed = 3.0f;

    public Vector2 collSize { get; private set; }
    public Vector2 collOffset { get; private set; }

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private float _deltaX;
    private float _deltaY;
    private float _width;
    ElevatorController elevator = null;

    [SerializeField] public int Vigor = 10;
    [SerializeField] public int Strength = 10;
    [SerializeField] public int Dexterity = 10;
    [SerializeField] public int Defense = 90;
    [SerializeField] public float Absorption;


    // Start is called before the first frame update
    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();

        Absorption = Defense / 30;

        _width = GetComponent<SpriteRenderer>().bounds.size.x / 3;

        collSize = _boxCollider.size;
        collOffset = _boxCollider.offset;
        Managers.Player.InitComponents(_body, _animator, Forward, Attacks, _width);
    }


    void FixedUpdate() {
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        _body.GetContacts(contacts);
        if (contacts != null) {
            bool hited = false;
            foreach(var contact in contacts) {
                if(contact.collider != null) {
                    elevator = contact.collider.GetComponent<ElevatorController>();
                    if(elevator != null) {
                        transform.parent = elevator.transform;

                        Managers.Player._isGrounded = true;
                        _animator.SetBool(PAParameters.GROUND, true);
                        Managers.Player._isJumping = false;
                        hited = true;
                        break;
                    } else {
                        transform.parent = null;
                    }

                    if (contact.collider.name.Contains("Ground")||
                        contact.collider.name.Contains("Plat")) {
                        if(contact.point.y <= _boxCollider.bounds.min.y+0.2) {
                            Managers.Player._isGrounded = true;
                            _animator.SetBool(PAParameters.GROUND, true);
                            Managers.Player._isJumping = false;
                            hited = true;
                            break;
                        }
                    }
                }
                
            }
            if (!hited) {
                Managers.Player._isGrounded = false;
                Managers.Player._isJumping = true;
                _animator.SetBool(PAParameters.GROUND, false);
            }
        }
    }


    // Update is called once per frame
    void Update() {



        _deltaX = Input.GetAxis("Horizontal") * speed;
        _animator.SetFloat(PAParameters.SPEED, Mathf.Abs(_deltaX));
        // 当获取不为零的横向输入时，判断转向与移动
        if (!Mathf.Approximately(_deltaX, 0)) {
            Managers.Player.Turn(_deltaX);
            Managers.Player.Move(_deltaX);
        }

        // TODO 不在倾斜的平台上滑动
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("Idle")) {
            //_body.gravityScale = (Managers.Player._isGrounded && Mathf.Approximately(_deltaX, 0)) ? 0 : 3;
            //_body.velocity = Vector2.zero;
        }
        
        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space)) {
            Managers.Player.Jump();
        }

        // 控制碰撞
        _boxCollider.offset = collOffset;
        _boxCollider.size = collSize;

        // 跑动
        if (Input.GetKey(KeyCode.LeftShift)){
            Managers.Player.Run(_deltaX);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Managers.Player.RunExit();
        }
        

        // 攻击A
        if (Input.GetKeyDown(KeyCode.J)) {
            print("key down");
            Managers.Player.AttackAEnter();
        }
        // 攻击A取消
        if (Input.GetKeyUp(KeyCode.J)) {
            Managers.Player.AttackAExit();
        }

        // 攻击B
        if (Input.GetKeyDown(KeyCode.K)) {
            Managers.Player.AttackBEnter();
        }
        // 攻击B取消
        if (Input.GetKeyUp(KeyCode.K)) {
            Managers.Player.AttackBExit();
        }

        // 攻击C
        if (Input.GetKeyDown(KeyCode.U)) {
            Managers.Player.AttackCEnter();
        }
        // 攻击C取消
        if (Input.GetKeyUp(KeyCode.U)) {
            Managers.Player.AttackCExit();
        }

        // 攻击D
        if (Input.GetKeyDown(KeyCode.I)) {
            Managers.Player.AttackDEnter();
        }
        // 攻击D取消
        if (Input.GetKeyUp(KeyCode.I)) {
            Managers.Player.AttackDExit();
        }

        // 跳跃攻击
        if(Input.GetKeyDown(KeyCode.J)) {
            Managers.Player.JumpAttack();
        }

        // 翻滚
        if (Input.GetKeyDown(KeyCode.L)) {
            Managers.Player.Roll();
        }

        // 爬梯
        //_deltaY = Input.GetAxis("Vertical");
        //if (!Mathf.Approximately(_deltaY,0)) {
        //    Managers.Player.ClimbStart();
        //}


        if (Input.GetKeyDown(KeyCode.M)) {
            Managers.UI.OnPlayStatusOpen();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Managers.UI.OnMenuOpen();
        }

    }

    // TODO 不让玩家能够推动敌人
    //void OnCollisionEnter2D(Collision2D collision) {
    //    if (collision.gameObject.name.Contains("E_")) {
    //        _body.velocity = Vector2.zero;
    //        speed = 0.003f;
    //    }
        
    //}

    //void OnCollisionExit2D(Collision2D collision) {
    //    if (collision.gameObject.name.Contains("E_")) {
    //        speed = 3.0f;
    //    }
    //}
}
