using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager {
    [SerializeField] private Player player;

    public Rigidbody2D body;
    public Animator animator;
    public GameObject Forward;
    public EnemyDetector[] Attacks;

    private float JumpAttackAirBounce;

    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float maxStamina { get; set; }
    public float currentStamina { get; set; }
    public float staminaIncreasement { get; set; }

    //public Vector2 _ladderTopPos { get; set; }
    //public Vector2 _ladderBottomPos { get; set; }
    //public float _ladderX { get; set; }
    //public bool _isReachTopLadder { get; set; }
    //public bool _isReachBottomLadder { get; set; }
    //public bool _isOnLadder { get; set; }
    //public bool _hasLadder { get; set; }
    public int jumpStat { get; set; }
    public bool _isRolling { get; set; }
    public bool _isFacingRight { get; set; }
    public bool _isRunning { get; set; }
    public bool _isGrounded { get; set; }
    public bool _isJumping { get; set; }

    public float width;
    public float jumpForce = 12.0f;
    //public float speed = 3.0f;

    public ManagerStatus status { get; private set; }

    public void Startup() {
        print("starting PlayerManager...");

        JumpAttackAirBounce = 12.0f;
        jumpStat = -1;
        //_isReachTopLadder = false;
        //_isReachBottomLadder = false;
        
        //_isOnLadder = false;
        //_hasLadder = false;
        //_ladderX = 0;
        _isFacingRight = true;
        _isRolling = false;
        _isRunning = false;
        _isGrounded = false;
        _isJumping = false;


        status = ManagerStatus.Started;
    }
    // 初始化
    public void InitComponents(Rigidbody2D body, Animator animator, GameObject Forward, EnemyDetector[] Attacks, float width) {
        this.body = body;
        this.animator = animator;
        this.Forward = Forward;
        this.Attacks = Attacks;
        this.width = width;
    }
    // 是否在攻击中
    public bool IsAttacking() {
        return (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_A) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_B) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_C) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_D));
    }
    // 添加向前的力
    public void AddFrontForce(float force = 0) {
        if(force == 0) {
            body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * 9, body.velocity.y);
        } else {
            body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * force, body.velocity.y);
        }
            
    }
    // 跳跃攻击
    public void AddUpForce(float force = 0) {
        if(Mathf.Approximately(force,0)) {
            if(body.velocity.y >= 0) {
                body.AddForce(Vector2.up * JumpAttackAirBounce, ForceMode2D.Impulse);
            } else {
                body.AddForce(Vector2.up * (JumpAttackAirBounce + (-body.velocity.y)), ForceMode2D.Impulse);
            }
            
        } else {
            if (body.velocity.y >= 0) {
                body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            } else {
                body.AddForce(Vector2.up * (force + (-body.velocity.y)), ForceMode2D.Impulse);
            }
        }
    }
    // 梯子
    //public void FallDownLadder() {
    //    _body.gravityScale = 3;
    //    _boxCollider.isTrigger = false;
    //    print("robot!!!");
    //    _body.velocity = new Vector2((Back.transform.position.x - player.transform.position.x) * 7, _body.velocity.y);
    //}
    //public void LadderMoveUp() {
    //    if (_isReachTopLadder) {
    //        _animator.SetInteger(PAParameters.CLIMB_STAT, -1);
    //        _animator.SetTrigger(PAParameters.LADDER_TOP);
    //        //_animator.ResetTrigger(PAParameters.LADDER_TOP);
    //        _body.gravityScale = 3;
    //        _boxCollider.isTrigger = false;
    //        player.transform.position = new Vector3(_ladderTopPos.x, _ladderTopPos.y, player.transform.position.z);
    //    } else {
    //        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.05f, player.transform.position.z);
    //    }
    //}
    //public void LadderMoveDown() {
    //    if (_isReachBottomLadder) {
    //        _animator.SetInteger(PAParameters.CLIMB_STAT, -1);
    //        _animator.SetTrigger(PAParameters.LADDER_BOTTOM);
    //        //_animator.ResetTrigger(PAParameters.LADDER_BOTTOM);
    //        _body.gravityScale = 3;
    //        _boxCollider.isTrigger = false;
    //        //gameObject.transform.position = new Vector3(_ladderBottomPos.x, _ladderBottomPos.y, gameObject.transform.position.z);
    //        _body.velocity = new Vector2(_ladderBottomPos.x / 3, _ladderBottomPos.y);
    //    } else {
    //        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.05f, player.transform.position.z);
    //    }
    //}
    // 爬梯
    //public void ClimbStart() {
    //    if (_hasLadder && !_isOnLadder) {
    //        /*
    //         * 获取梯子的xy，改变玩家xy，切换动画，关闭重力
    //         * 
    //         */
    //        _animator.SetInteger(PAParameters.CLIMB_STAT, 0);
    //        _body.gravityScale = 0;
    //        _body.velocity = Vector2.zero;
    //        _boxCollider.isTrigger = true;
    //    }
    //}
    // 翻滚
    // 是否在翻滚中，用于转向判断
    public bool IsRolling() {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ROLL);
    }
    // 翻滚中每次动画状态更新调用
    // 添加一个朝向翻滚方向的加速度
    public void OnRollGoing() {
        body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * 14, body.velocity.y);
    }
    // 退出翻滚状态
    public void OnRollExit() {
        _isRolling = false;
        animator.ResetTrigger(PAParameters.ROLL);
    }
    // 控制朝向
    public void Turn(float deltaX) {
        if (!IsRolling() // 翻滚、攻击、爬梯子时不能转向
            && !IsAttacking()
            /*&& !_isOnLadder*/) { 
            player.transform.localScale = new Vector3(Mathf.Sign(deltaX) * 3, 3, 3);
            if (_isFacingRight && Mathf.Sign(deltaX) < 0) {
                //do turn left
                _isFacingRight = !_isFacingRight;
                player.transform.position = new Vector3(player.transform.position.x - width, player.transform.position.y, player.transform.position.z);
            } else if (!_isFacingRight && Mathf.Sign(deltaX) > 0) {
                //do turn right
                _isFacingRight = !_isFacingRight;
                player.transform.position = new Vector3(player.transform.position.x + width, player.transform.position.y, player.transform.position.z);
            }
        }
    }
    //public void Turn(float deltaX, Vector3 elevator) {
    //    if (!IsRolling() // 翻滚、攻击、爬梯子时不能转向
    //        && !IsAttacking()
    //        /*&& !_isOnLadder*/) {
    //        player.transform.localScale = new Vector3(Mathf.Sign(deltaX) * 3 / elevator.x, 3 / elevator.y, 3);
    //        if (_isFacingRight && Mathf.Sign(deltaX) < 0) {
    //            //do turn left
    //            _isFacingRight = !_isFacingRight;
    //            player.transform.position = new Vector3(player.transform.position.x - width, player.transform.position.y, player.transform.position.z);
    //        } else if (!_isFacingRight && Mathf.Sign(deltaX) > 0) {
    //            //do turn right
    //            _isFacingRight = !_isFacingRight;
    //            player.transform.position = new Vector3(player.transform.position.x + width, player.transform.position.y, player.transform.position.z);
    //        }
    //    }
    //}
    //移动
    public void Move(float deltaX) {
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        if (movement != Vector2.zero && !_isRunning && !_isJumping && !IsAttacking() && !IsRolling()
            && !_isRolling && _isGrounded /*&& !_isOnLadder*/) {
            body.velocity = movement;
        }
    }
    // 跳跃
    public void Jump() {
        if (_isGrounded && !IsAttacking() && !IsRolling() /*&& !_isOnLadder*/) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }
    // 跑动
    public void Run(float deltaX) {
        if (new Vector2(deltaX, body.velocity.y) != Vector2.zero
            && !_isJumping && !IsAttacking() && !IsRolling() && _isGrounded /*&& !_isOnLadder*/) {
            _isRunning = true;
            body.velocity = new Vector2(new Vector2(deltaX, body.velocity.y).x * 2, body.velocity.y);
            animator.SetFloat(PAParameters.SPEED, Mathf.Abs(deltaX * 2));
        }
    }
    //结束跑动
    public void RunExit() {
        _isRunning = false;
    }
    // 攻击A
    public void AttackAEnter() {
        if (_isGrounded && !IsRolling() /*&& !_isOnLadder*/ && !_isJumping && !IsAttacking()) {
            AddFrontForce();
            animator.SetInteger(PAParameters.ATTACKSTAT, 0);
        }
    }
    // 攻击A取消
    public void AttackAExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 攻击A判定
    public void AttackACheck() {
        foreach(string name in Attacks[0].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    // 攻击B
    public void AttackBEnter() {
        if (_isGrounded && !IsRolling() /*&& !_isOnLadder*/ && !_isJumping) {
            AddFrontForce();
            animator.SetInteger(PAParameters.ATTACKSTAT, 1);
        }
    }
    // 攻击B判定
    public void AttackBCheck() {
        foreach (string name in Attacks[1].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    // 攻击B取消
    public void AttackBExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 攻击C
    public void AttackCEnter() {
        if (_isGrounded && !IsRolling() /*&& !_isOnLadder*/ && !_isJumping) {
            AddFrontForce();
            animator.SetInteger(PAParameters.ATTACKSTAT, 2);
        }
    }
    // 攻击C判定
    public void AttackCCheck() {
        foreach (string name in Attacks[2].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    // 攻击C取消
    public void AttackCExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 攻击D
    public void AttackDEnter() {
        if (_isGrounded && !IsRolling() /*&& !_isOnLadder*/ && !_isJumping) {
            animator.SetInteger(PAParameters.ATTACKSTAT, 3);
            AddFrontForce();
        }
    }
    // 攻击D判定
    public void AttackDCheck() {
        foreach (string name in Attacks[3].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    // 攻击D取消
    public void AttackDExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    // 跳跃攻击
    public void JumpAttack() {
        if (!_isGrounded && _isJumping) {
            animator.SetInteger(PAParameters.JUMP_ATTACK_STAT, 0);
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }
    // 翻滚
    public void Roll() {
        if (_isGrounded && !IsAttacking() /*&& !_isOnLadder*/) {
            _isRolling = true;
            animator.SetTrigger(PAParameters.ROLL);
        }
    }
    // 受伤
    public void GetHit(float damage) {
        // TODO 玩家受伤
        print("hit player");
        animator.SetTrigger(PAParameters.HIT);
        //animator.ResetTrigger(PAParameters.HIT);
    }
    // 死亡
    public void Death() {
        // TODO 玩家死亡
    }
}
