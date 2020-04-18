using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    //[SerializeField] private GameObject Forward;
    //[SerializeField] private GameObject Back;

    public float speed = 3.0f;
    public float jumpForce = 12.0f;

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    //private Vector2 _ladderTopPos;
    //private Vector2 _ladderBottomPos;
    //private float _width;
    //private bool _facing_right;
    //private bool _running;
    //private bool _jumping;
    //private bool _grounded;
    //private bool _rolling;
    //private bool _hasLadder;
    //private bool _reachTopLadder;
    //private bool _onLadder;
    //private bool _reachBottomLadder;
    //private int _facingRight;// 1 = true, -1 = false
    //private float _ladderX;
    private float _deltaX;
    private float _deltaY;
    private string _currentStat;//无用代码

    // Start is called before the first frame update
    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        //_width = GetComponent<SpriteRenderer>().bounds.size.x/3;
        _boxCollider = GetComponent<BoxCollider2D>();
        //_facing_right = true;
        //_running = false;
        //_jumping = false; 
        //_rolling = false;
        //_hasLadder = false;
        //_onLadder = false;
        //_reachTopLadder = false;
        //_reachBottomLadder = false;
        _currentStat = AParameters.IDLE;//无用代码
    }

    // Update is called once per frame
    void Update() {
        //if(Forward.transform.position.x > Back.transform.position.x) {

        //}

        _deltaX = Input.GetAxis("Horizontal") * speed;
        _animator.SetFloat(AParameters.SPEED, Mathf.Abs(_deltaX));
        // 当获取不为零的横向输入时，判断转向与移动
        if (!Mathf.Approximately(_deltaX, 0)) {
            Managers.Player.Turn(_deltaX);
            Managers.Player.Move(_deltaX);
        }

        // 跳跃条件
        Vector3 max = _boxCollider.bounds.max;
        Vector3 min = _boxCollider.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .2f);
        Vector2 corner2 = new Vector2(min.x, min.y - .3f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        //_grounded = false; // 老代码
        if (hit != null && !hit.isTrigger) {
            Managers.Player._isGrounded = true;
            _animator.SetBool(AParameters.GROUND, true);
            Managers.Player._isJumping = false;
            
        } else {
            Managers.Player._isGrounded = false;
            Managers.Player._isJumping = true;
            _animator.SetBool(AParameters.GROUND, false);
        }
        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space)) {
            Managers.Player.Jump();
        }

        // 控制碰撞
            _boxCollider.offset = new Vector2(-0.18f, 0.0f);
        _boxCollider.size = new Vector2(0.33f, 0.63f);

        // 跑动
        //  Warning！！！这段代码大概率不会正常工作
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
        _deltaY = Input.GetAxis("Vertical");
        if (!Mathf.Approximately(_deltaY,0)) {
            Managers.Player.ClimbStart();
        }

        

    }

    private void ChangeStat(string changeStat) {
        _animator.ResetTrigger(_currentStat);
        _animator.SetTrigger(changeStat);
        _currentStat = changeStat;
    }

    //private bool IsAttacking() {
    //    return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_A) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_B) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_C) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_D));
    //}
    /*
     * Roll Part Start
     * 
     * 
     * 
     */
    

    //private bool IsRolling() {
    //    return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ROLL);
    //}

    //private void OnRollGoing() {
    //    //_rolling = true;
    //    //_animator.SetTrigger(AParameters.ROLL);
    //    _body.velocity = new Vector2((Forward.transform.position.x - gameObject.transform.position.x) * 7, _body.velocity.y);
    //}

    //private void OnRollExit() {
    //    _rolling = false;
    //    _animator.ResetTrigger(AParameters.ROLL);
    //}
    /*
     * Ladder Part Start
     * 
     * 
     * 
     */
    //private void OnLadderArea() {
    //    _hasLadder = true;
    //    print("Enter Ladder area");
    //}
    //private void ExitLadderArea() {
    //    _hasLadder = false;
    //    print("Exit Ladder area");
    //}
    //private void SetLadderX(float value) {
    //    _ladderX = value;
    //    //print(value);
    //}
    //private void FallDownLadder() {
    //    _body.gravityScale = 3;
    //    _boxCollider.isTrigger = false;
    //    print("robot!!!");
    //    _body.velocity = new Vector2((Back.transform.position.x - gameObject.transform.position.x) * 7, _body.velocity.y);
    //}
    //private void LadderMoveUp() {
    //    if (_reachTopLadder) {
    //        _animator.SetInteger(AParameters.CLIMB_STAT, -1);
    //        _animator.SetTrigger(AParameters.LADDER_TOP);
    //        //_animator.ResetTrigger(AParameters.LADDER_TOP);
    //        _body.gravityScale = 3;
    //        _boxCollider.isTrigger = false;
    //        gameObject.transform.position = new Vector3(_ladderTopPos.x, _ladderTopPos.y, gameObject.transform.position.z);
    //    } else {
    //        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f, gameObject.transform.position.z);
    //    }
    //}
    //private void LadderMoveDown() {
    //    if (_reachBottomLadder) {
    //        print("leave ladder due to reach bottom and reaceve a down cmd");
    //        _animator.SetInteger(AParameters.CLIMB_STAT, -1);
    //        _animator.SetTrigger(AParameters.LADDER_BOTTOM);
    //        //_animator.ResetTrigger(AParameters.LADDER_BOTTOM);
    //        _body.gravityScale = 3;
    //        _boxCollider.isTrigger = false;
    //        //gameObject.transform.position = new Vector3(_ladderBottomPos.x, _ladderBottomPos.y, gameObject.transform.position.z);
    //        _body.velocity = new Vector2(_ladderBottomPos.x/3, _ladderBottomPos.y);
    //    } else {
    //        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.05f, gameObject.transform.position.z);
    //    }
    //}
    //private void ReachLadderEnd(string end) {
    //    print("Enter"+end);
    //    if (end.Equals("Top")) {
    //        _reachTopLadder = true;
    //    } else {
    //        _reachBottomLadder = true;
    //    }
    //}
    //private void LeaveLadderEnd(string end) {
    //    print("leave:"+end);
    //    if (end.Equals("Top")) {
    //        _reachTopLadder = false;
    //    } else {
    //        _reachBottomLadder = false;
    //    }
    //}
    //private void SetLadderTopPos(Vector2 pos) {
    //    _ladderTopPos = pos;
    //}
    //private void SetLadderBottomPos(Vector2 pos) {
    //    _ladderBottomPos = pos;
    //}
    // 废弃方法
    //private bool IsOnLadder() {
    //    bool res = (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.CLIMB_LADDER_START) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.STAY_IN_LADDER) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.MOVE_IN_LADDER) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.FALLDOWN) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.CLIMB_TO_LADDER_TOP_END) ||
    //        _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.LADDER_BOTTOM));
    //    print(res);
    //    return res;
    //}
    //private void OnLadder() {
    //    _onLadder = true;
    //}
    //private void ExitLadder() {
    //    _onLadder = false;
    //}
    /*
     * Jump Attack Start
     * 
     * 
     * 
     */
}
