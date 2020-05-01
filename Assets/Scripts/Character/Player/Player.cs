﻿using System;
using System.Collections;
using System.Collections.Generic;
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
    private string _currentStat;//无用代码 

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
        _currentStat = PAParameters.IDLE;//无用代码

        Absorption = Defense / 30;

        _width = GetComponent<SpriteRenderer>().bounds.size.x / 3;

        collSize = _boxCollider.size;
        collOffset = _boxCollider.offset;
        Managers.Player.InitComponents(_body, _animator, Forward, Attacks, _width);
    }


    void FixedUpdate() {
        //print("grounded?" + Managers.Player._isGrounded);
        // 跳跃条件
        //Vector3 max = _boxCollider.bounds.max;// 右上
        //Vector3 min = _boxCollider.bounds.min;// 左下
        //Vector2 corner1 = new Vector2(max.x-0.2f, min.y - .2f);
        //Vector2 corner2 = new Vector2(min.x+0.2f, min.y - .3f);

        //Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        //if (hit != null) {
        //    if (!hit.isTrigger) {
        //        if (hit.tag.Equals("Ground")) {
        //            print("Ground");
        //        }
        //        Managers.Player._isGrounded = true;
        //        _animator.SetBool(PAParameters.GROUND, true);
        //        Managers.Player._isJumping = false;
        //    }
        //} else {
        //    Managers.Player._isGrounded = false;
        //    Managers.Player._isJumping = true;
        //    _animator.SetBool(PAParameters.GROUND, false);
        //}
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        _body.GetContacts(contacts);
        if (contacts != null) {
            bool hited = false;
            foreach(var contact in contacts) {
                if(contact.collider != null) {
                    if (contact.collider.name.Contains("Ground")) {
                        Managers.Player._isGrounded = true;
                        _animator.SetBool(PAParameters.GROUND, true);
                        Managers.Player._isJumping = false;
                        hited = true;
                        break;
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

        // 不在倾斜的平台上滑动
        // TODO
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
        

    }

    // TODO 不让玩家能够推动敌人
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name.Contains("E_")) {
            _body.velocity = Vector2.zero;
            speed = 0.003f;
        }
        
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.name.Contains("E_")) {
            speed = 3.0f;
        }
    }
    private void ChangeStat(string changeStat) {
        _animator.ResetTrigger(_currentStat);
        _animator.SetTrigger(changeStat);
        _currentStat = changeStat;
    }
}
