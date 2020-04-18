using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager {
    [SerializeField] private Player player;
    [SerializeField] private GameObject Back;
    [SerializeField] private GameObject Forward;

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    private float JumpAttackAirBounce;


    public Vector2 _ladderTopPos { get; set; }
    public Vector2 _ladderBottomPos { get; set; }
    public int jumpStat {get;set;}
    public bool _reachTopLadder { get; set; }
    public bool _reachBottomLadder { get; set; }
    public bool _onLadder { get; set; }
    public bool _hasLadder { get; set; }
    public bool _rolling { get; set; }
    public bool _facing_right { get; set; }
    public bool _running { get; set; }
    public bool _grounded { get; set; }
    public bool _jumping { get; set; }
    public float _ladderX { get; set; }
    public float _width { get; set; }
    public float jumpForce = 12.0f;
    

    public ManagerStatus status { get; private set; }

    public void Startup() {
        print("starting PlayerManager...");
        _body = player.GetComponent<Rigidbody2D>();
        _boxCollider = player.GetComponent<BoxCollider2D>();
        _animator = player.GetComponent<Animator>();
        _width = player.GetComponent<SpriteRenderer>().bounds.size.x / 3;
        JumpAttackAirBounce = 12.0f;
        jumpStat = -1;
        _reachTopLadder = false;
        _reachBottomLadder = false;
        _facing_right = true;
        _onLadder = false;
        _hasLadder = false;
        _ladderX = 0;
        _rolling = false;
        _running = false;
        _grounded = false;
        _jumping = false;


        status = ManagerStatus.Started;
    }
    //攻击
    public bool IsAttacking() {
        return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_A) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_B) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_C) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ATTACK_D));
    }
    //跳跃攻击
    public void AddForce(float force = 0) {
        //print("add force" + force);
        if(Mathf.Approximately(force,0)) {
            if(_body.velocity.y >= 0) {
                _body.AddForce(Vector2.up * JumpAttackAirBounce, ForceMode2D.Impulse);
            } else {
                _body.AddForce(Vector2.up * (JumpAttackAirBounce + (-_body.velocity.y)), ForceMode2D.Impulse);
            }
            
        } else {
            if (_body.velocity.y >= 0) {
                _body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            } else {
                _body.AddForce(Vector2.up * (force + (-_body.velocity.y)), ForceMode2D.Impulse);
            }
        }
    }
    // 梯子
    public void FallDownLadder() {
        _body.gravityScale = 3;
        _boxCollider.isTrigger = false;
        print("robot!!!");
        _body.velocity = new Vector2((Back.transform.position.x - player.transform.position.x) * 7, _body.velocity.y);
    }
    public void LadderMoveUp() {
        if (_reachTopLadder) {
            _animator.SetInteger(AParameters.CLIMB_STAT, -1);
            _animator.SetTrigger(AParameters.LADDER_TOP);
            //_animator.ResetTrigger(AParameters.LADDER_TOP);
            _body.gravityScale = 3;
            _boxCollider.isTrigger = false;
            player.transform.position = new Vector3(_ladderTopPos.x, _ladderTopPos.y, player.transform.position.z);
        } else {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.05f, player.transform.position.z);
        }
    }
    public void LadderMoveDown() {
        if (_reachBottomLadder) {
            print("leave ladder due to reach bottom and reaceve a down cmd");
            _animator.SetInteger(AParameters.CLIMB_STAT, -1);
            _animator.SetTrigger(AParameters.LADDER_BOTTOM);
            //_animator.ResetTrigger(AParameters.LADDER_BOTTOM);
            _body.gravityScale = 3;
            _boxCollider.isTrigger = false;
            //gameObject.transform.position = new Vector3(_ladderBottomPos.x, _ladderBottomPos.y, gameObject.transform.position.z);
            _body.velocity = new Vector2(_ladderBottomPos.x / 3, _ladderBottomPos.y);
        } else {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.05f, player.transform.position.z);
        }
    }
    // 爬梯
    public void ClimbStart() {
        if (_hasLadder && !_onLadder) {
            /*
             * 获取梯子的xy，改变玩家xy，切换动画，关闭重力
             * 
             */
            _animator.SetInteger(AParameters.CLIMB_STAT, 0);
            _body.gravityScale = 0;
            _body.velocity = Vector2.zero;
            _boxCollider.isTrigger = true;
        }
    }
    // 翻滚
    public bool IsRolling() {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(Astat.ROLL);
    }
    public void OnRollGoing() {
        //_rolling = true;
        //_animator.SetTrigger(AParameters.ROLL);
        _body.velocity = new Vector2((Forward.transform.position.x - gameObject.transform.position.x) * 7, _body.velocity.y);
    }
    public void OnRollExit() {
        _rolling = false;
        _animator.ResetTrigger(AParameters.ROLL);
    }
    // 控制朝向
    public void Turn(float deltaX) {
        print("turn,deltaX:" + deltaX);
        if (!IsRolling()) {
            player.transform.localScale = new Vector3(Mathf.Sign(deltaX) * 3, 3, 3);
            if (_facing_right && Mathf.Sign(deltaX) < 0) {
                //do turn left
                _facing_right = !_facing_right;
                player.transform.position = new Vector3(player.transform.position.x - _width, player.transform.position.y, player.transform.position.z);
            } else if (!_facing_right && Mathf.Sign(deltaX) > 0) {
                //do turn right
                _facing_right = !_facing_right;
                player.transform.position = new Vector3(player.transform.position.x + _width, player.transform.position.y, player.transform.position.z);
            }
        }
    }
    //移动
    public void Move(float deltaX) {
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        if (movement != Vector2.zero && !_running && !_jumping && !IsAttacking() && !IsRolling()
            && !_rolling && _grounded && !_onLadder) {
            _body.velocity = movement;
        }
    }
    // 跳跃
    public void Jump() {
        if (_grounded && !IsAttacking() && !IsRolling() && !_onLadder) {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _grounded = false;
        }
    }
    // 跑动
    public void Run(float deltaX) {
        if (new Vector2(deltaX, _body.velocity.y) != Vector2.zero
            && !_jumping && !IsAttacking() && !IsRolling() && _grounded && !_onLadder) {
            _running = true;
            _body.velocity = new Vector2(new Vector2(deltaX, _body.velocity.y).x * 2, _body.velocity.y);
            _animator.SetFloat(AParameters.SPEED, Mathf.Abs(deltaX * 2));
        }
    }
    //结束跑动
    public void RunExit() {
        _running = false;
    }
    // 攻击A
    public void AttackAEnter() {
        if (_grounded && !IsRolling() && !_onLadder) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 0);
            // TODO 攻击判定
        }
    }
    // 攻击A取消
    public void AttackAExit() {
        _animator.SetInteger(AParameters.ATTACKSTAT, -1);
    }
    // 攻击B
    public void AttackBEnter() {
        if (_grounded && !IsRolling() && !_onLadder) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 1);
            // TODO 攻击判定

        }
    }
    // 攻击B取消
    public void AttackBExit() {
        _animator.SetInteger(AParameters.ATTACKSTAT, -1);
    }
    // 攻击C
    public void AttackCEnter() {
        if (_grounded && !IsRolling() && !_onLadder) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 2);
            // TODO 攻击判定

        }
    }
    // 攻击C取消
    public void AttackCExit() {
        _animator.SetInteger(AParameters.ATTACKSTAT, -1);
    }
    // 攻击D
    public void AttackDEnter() {
        if (_grounded && !IsRolling() && !_onLadder) {
            _animator.SetInteger(AParameters.ATTACKSTAT, 3);
            // TODO 攻击判定

        }
    }
    // 攻击D取消
    public void AttackDExit() {
        _animator.SetInteger(AParameters.ATTACKSTAT, -1);
    }
    // 跳跃攻击
    public void JumpAttack() {
        if (!_grounded) {
            _animator.SetInteger(AParameters.JUMP_ATTACK_STAT, 0);
        }
    }
    // 翻滚
    public void Roll() {
        if (_grounded && !IsAttacking() && !_onLadder) {
            _rolling = true;
            _animator.SetTrigger(AParameters.ROLL);
        }
    }
    
}
