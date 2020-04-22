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
    public int jumpStat { get; set; }
    public bool _isReachTopLadder { get; set; }
    public bool _isReachBottomLadder { get; set; }
    public bool _isOnLadder { get; set; }
    public bool _hasLadder { get; set; }
    public bool _isRolling { get; set; }
    public bool _isFacingRight { get; set; }
    public bool _isRunning { get; set; }
    public bool _isGrounded { get; set; }
    public bool _isJumping { get; set; }
    public float _ladderX { get; set; }
    public float _width { get; set; }
    public float jumpForce = 12.0f;
    //public float speed = 3.0f;

    public ManagerStatus status { get; private set; }

    public void Startup() {
        print("starting PlayerManager...");
        

        _body = player.GetComponent<Rigidbody2D>();
        _boxCollider = player.GetComponent<BoxCollider2D>();
        _animator = player.GetComponent<Animator>();
        _width = player.GetComponent<SpriteRenderer>().bounds.size.x / 3;

        JumpAttackAirBounce = 12.0f;
        jumpStat = -1;
        _isReachTopLadder = false;
        _isReachBottomLadder = false;
        _isFacingRight = true;
        _isOnLadder = false;
        _hasLadder = false;
        _ladderX = 0;
        _isRolling = false;
        _isRunning = false;
        _isGrounded = false;
        _isJumping = false;


        status = ManagerStatus.Started;
    }
    //攻击
    public bool IsAttacking() {
        return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_A) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_B) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_C) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_D));
    }
    // 添加向前的力
    public void AddFrontForce(float force = 0) {
        if(force == 0) {
            _body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * 9, _body.velocity.y);
        } else {
            _body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * force, _body.velocity.y);
        }
            
    }
    // 跳跃攻击
    public void AddUpForce(float force = 0) {
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
        if (_isReachTopLadder) {
            _animator.SetInteger(PAParameters.CLIMB_STAT, -1);
            _animator.SetTrigger(PAParameters.LADDER_TOP);
            //_animator.ResetTrigger(PAParameters.LADDER_TOP);
            _body.gravityScale = 3;
            _boxCollider.isTrigger = false;
            player.transform.position = new Vector3(_ladderTopPos.x, _ladderTopPos.y, player.transform.position.z);
        } else {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.05f, player.transform.position.z);
        }
    }
    public void LadderMoveDown() {
        if (_isReachBottomLadder) {
            print("leave ladder due to reach bottom and reaceve a down cmd");
            _animator.SetInteger(PAParameters.CLIMB_STAT, -1);
            _animator.SetTrigger(PAParameters.LADDER_BOTTOM);
            //_animator.ResetTrigger(PAParameters.LADDER_BOTTOM);
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
        if (_hasLadder && !_isOnLadder) {
            /*
             * 获取梯子的xy，改变玩家xy，切换动画，关闭重力
             * 
             */
            _animator.SetInteger(PAParameters.CLIMB_STAT, 0);
            _body.gravityScale = 0;
            _body.velocity = Vector2.zero;
            _boxCollider.isTrigger = true;
        }
    }
    // 翻滚
    // 是否在翻滚中，用于转向判断
    public bool IsRolling() {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ROLL);
    }
    // 翻滚中每次动画状态更新调用
    // 添加一个朝向翻滚方向的加速度
    public void OnRollGoing() {
        //_rolling = true;
        //_animator.SetTrigger(PAParameters.ROLL);
        _body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * 14, _body.velocity.y);
    }
    // 退出翻滚状态
    public void OnRollExit() {
        _isRolling = false;
        _animator.ResetTrigger(PAParameters.ROLL);
    }
    // 控制朝向
    public void Turn(float deltaX) {
        //print("turn,deltaX:" + deltaX);
        if (!IsRolling() // 翻滚、攻击、爬梯子时不能转向
            && !IsAttacking()
            && !_isOnLadder) { 
            player.transform.localScale = new Vector3(Mathf.Sign(deltaX) * 3, 3, 3);
            if (_isFacingRight && Mathf.Sign(deltaX) < 0) {
                //do turn left
                _isFacingRight = !_isFacingRight;
                player.transform.position = new Vector3(player.transform.position.x - _width, player.transform.position.y, player.transform.position.z);
            } else if (!_isFacingRight && Mathf.Sign(deltaX) > 0) {
                //do turn right
                _isFacingRight = !_isFacingRight;
                player.transform.position = new Vector3(player.transform.position.x + _width, player.transform.position.y, player.transform.position.z);
            }
        }
    }
    //移动
    public void Move(float deltaX) {
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        if (movement != Vector2.zero && !_isRunning && !_isJumping && !IsAttacking() && !IsRolling()
            && !_isRolling && _isGrounded && !_isOnLadder) {
            _body.velocity = movement;
        }
    }
    // 跳跃
    public void Jump() {
        print("grounded:" + _isGrounded);
        print("!IsAttacking():" + IsAttacking());
        print("IsRolling:" + IsRolling());
        print("_isOnLadder:" + _isOnLadder);
        if (_isGrounded && !IsAttacking() && !IsRolling() && !_isOnLadder) {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }
    // 跑动
    public void Run(float deltaX) {
        if (new Vector2(deltaX, _body.velocity.y) != Vector2.zero
            && !_isJumping && !IsAttacking() && !IsRolling() && _isGrounded && !_isOnLadder) {
            _isRunning = true;
            _body.velocity = new Vector2(new Vector2(deltaX, _body.velocity.y).x * 2, _body.velocity.y);
            _animator.SetFloat(PAParameters.SPEED, Mathf.Abs(deltaX * 2));
        }
    }
    //结束跑动
    public void RunExit() {
        _isRunning = false;
    }
    // 攻击A
    public void AttackAEnter() {
        if (_isGrounded && !IsRolling() && !_isOnLadder && !_isJumping) {
            _animator.SetInteger(PAParameters.ATTACKSTAT, 0);
            // TODO 攻击判定
        }
    }
    // 攻击A取消
    public void AttackAExit() {
        _animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 攻击B
    public void AttackBEnter() {
        if (_isGrounded && !IsRolling() && !_isOnLadder && !_isJumping) {
            _animator.SetInteger(PAParameters.ATTACKSTAT, 1);
            // TODO 攻击判定

        }
    }
    // 攻击B取消
    public void AttackBExit() {
        _animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 攻击C
    public void AttackCEnter() {
        if (_isGrounded && !IsRolling() && !_isOnLadder && !_isJumping) {
            _animator.SetInteger(PAParameters.ATTACKSTAT, 2);
            // TODO 攻击判定

        }
    }
    // 攻击C取消
    public void AttackCExit() {
        _animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 攻击D
    public void AttackDEnter() {
        if (_isGrounded && !IsRolling() && !_isOnLadder && !_isJumping) {
            _animator.SetInteger(PAParameters.ATTACKSTAT, 3);
            // TODO 攻击判定

        }
    }
    // 攻击D取消
    public void AttackDExit() {
        _animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 跳跃攻击
    public void JumpAttack() {
        if (!_isGrounded && _isJumping) {
            _animator.SetInteger(PAParameters.JUMP_ATTACK_STAT, 0);
            _body.velocity = new Vector2(0, _body.velocity.y);
        }
    }
    // 翻滚
    public void Roll() {
        if (_isGrounded && !IsAttacking() && !_isOnLadder) {
            _isRolling = true;
            _animator.SetTrigger(PAParameters.ROLL);
        }
    }
    
}
