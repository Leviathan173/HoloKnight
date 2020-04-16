using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private GameObject Forward;
    [SerializeField] private GameObject Back;

    public float speed = 3.0f;
    public float jumpForce = 12.0f;
    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private float _width;
    private bool _facing_right;
    private bool _running;
    private bool _jumping;
    private bool _grounded;
    private bool _rolling;
    private bool _hasLadder;
    private int _facingRight;// 1 = true, -1 = false
    private float _ladderX;
    private float _deltaX;
    private float _deltaY;
    private string _currentStat;

    // Start is called before the first frame update
    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _width = GetComponent<SpriteRenderer>().bounds.size.x/3;//gameObject.renderer.bounds.size.x;
        _boxCollider = GetComponent<BoxCollider2D>();
        _facing_right = true;
        _running = false;
        _jumping = false;
        _rolling = false;
        _hasLadder = false;
        _currentStat = AParameters.IDLE;
    }

    // Update is called once per frame
    void Update() {
        if(Forward.transform.position.x > Back.transform.position.x) {

        }

        _deltaX = Input.GetAxis("Horizontal") * speed;
        _animator.SetFloat(AParameters.SPEED, Mathf.Abs(_deltaX));
        // 控制朝向
        if (!Mathf.Approximately(_deltaX, 0) && !IsRolling()) {
            transform.localScale = new Vector3(Mathf.Sign(_deltaX) * 3, 3, 3);
            if (_facing_right && Mathf.Sign(_deltaX) < 0) {
                //do turn left
                _facing_right = !_facing_right;
                transform.position = new Vector3(transform.position.x - _width, transform.position.y, transform.position.z);
            } else if (!_facing_right && Mathf.Sign(_deltaX) > 0) {
                //do turn right
                _facing_right = !_facing_right;
                transform.position = new Vector3(transform.position.x + _width, transform.position.y, transform.position.z);
            }
        }

        //移动
        Vector2 movement = new Vector2(_deltaX, _body.velocity.y);
        if (movement != Vector2.zero && !_running && !_jumping && !IsAttacking() && !IsRolling() && !_rolling && _grounded) {
            _body.velocity = movement;
        }

        // 跳跃条件
        Vector3 max = _boxCollider.bounds.max;
        Vector3 min = _boxCollider.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .2f);
        Vector2 corner2 = new Vector2(min.x, min.y - .3f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        _grounded = false;
        if(hit != null && !hit.isTrigger) {
            _grounded = true;
            _animator.SetBool(AParameters.GROUND, true);
            _jumping = false;
            
        } else {
            _jumping = true;
            _animator.SetBool(AParameters.GROUND, false);
        }
        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space) && _grounded && !IsAttacking() && !IsRolling() && !IsOnLadder()) {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _grounded = false;
            //_animator.SetBool("Grounded", false);
        }

        // 控制碰撞
        _boxCollider.offset = new Vector2(-0.18f, 0.0f);
        _boxCollider.size = new Vector2(0.33f, 0.63f);

        // 跑动
        if (Input.GetKey(KeyCode.LeftShift) && movement != Vector2.zero && !_jumping && !IsAttacking() && !IsRolling()) {
            _running = true;
            _body.velocity = new Vector2(movement.x*2,_body.velocity.y);
            _animator.SetFloat(AParameters.SPEED, Mathf.Abs(_deltaX * 2));
            //print(_animator.GetFloat(AParameters.SPEED));
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            _running = false;
        }

        // 攻击A
        if (Input.GetKeyDown(KeyCode.J) && _grounded && !IsRolling()) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 0);
            // TODO 攻击判定

        }
        // 攻击A取消
        if (Input.GetKeyUp(KeyCode.J)) {
            _animator.SetInteger(AParameters.ATTACKSTAT, -1);
        }

        // 攻击B
        if (Input.GetKeyDown(KeyCode.K) && _grounded && !IsRolling()) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 1);
            // TODO 攻击判定

        }
        // 攻击B取消
        if (Input.GetKeyUp(KeyCode.K)) {
            _animator.SetInteger(AParameters.ATTACKSTAT, -1);
        }

        // 攻击C
        if (Input.GetKeyDown(KeyCode.U) && _grounded && !IsRolling()) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 2);
            // TODO 攻击判定

        }
        // 攻击C取消
        if (Input.GetKeyUp(KeyCode.U)) {
            _animator.SetInteger(AParameters.ATTACKSTAT, -1);
        }

        // 攻击D
        if (Input.GetKeyDown(KeyCode.I) && _grounded && !IsRolling()) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 3);
            // TODO 攻击判定

        }
        // 攻击D取消
        if (Input.GetKeyUp(KeyCode.I)) {
            _animator.SetInteger(AParameters.ATTACKSTAT, -1);
        }

        // 翻滚
        if (Input.GetKeyDown(KeyCode.L) && _grounded && !IsAttacking()) {
            _rolling = true;
            _animator.SetTrigger(AParameters.ROLL);
        }

        // TODO 爬梯
        _deltaY = Input.GetAxis("Vertical");
        if (_deltaY != 0 && _hasLadder && !IsOnLadder()) {
            /*
             * 获取梯子的xy，改变玩家xy，切换动画，关闭重力
             * 
             */
            _animator.SetInteger(AParameters.CLIMB_STAT,0);
            _body.gravityScale = 0;
        }

    }

    private void ChangeStat(string changeStat) {
        _animator.ResetTrigger(_currentStat);
        _animator.SetTrigger(changeStat);
        _currentStat = changeStat;
    }

    private bool IsAttacking() {
        return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_A) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_B) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_C) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_D));
    }

    private bool IsOnLadder() {
        return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.CLIMB_LADDER_START) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.STAY_IN_LADDER) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.MOVE_IN_LADDER) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.FALLDOWN) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.CLIMB_TO_LADDER_TOP_END) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.LADDER_BOTTOM));
    }

    private bool IsRolling() {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ROLL);
    }

    private void OnRollGoing() {
        //_rolling = true;
        //_animator.SetTrigger(AParameters.ROLL);
        _body.velocity = new Vector2((Forward.transform.position.x - gameObject.transform.position.x) * 7, _body.velocity.y);
    }

    private void OnRollExit() {
        _rolling = false;
        _animator.ResetTrigger(AParameters.ROLL);
    }

    private void OnLadder() {
        _hasLadder = true;
        print("Enter Ladder area");
    }
    private void ExitLadder() {
        _hasLadder = false;
        print("Exit Ladder area");
    }
    private void SetLadderX(float value) {
        _ladderX = value;
        //print(value);
    }
    private void Say() {
        _body.gravityScale = 3;
        print("robot!!!");
        _body.velocity = new Vector2((Back.transform.position.x - gameObject.transform.position.x) * 7, _body.velocity.y);
    }
}
