using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager {
    [SerializeField] public Player player;
    [SerializeField] PlayerBarController bar;
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
    public int gold = 0;
    public float AttackCost = 10;
    public Vector3 lastSpawnPos;
    //public float speed = 3.0f;

    public ManagerStatus status { get; private set; }

    public void Startup() {
        print("starting PlayerManager...");

        JumpAttackAirBounce = 12.0f;
        jumpStat = -1;

        maxHealth = 100;
        currentHealth = maxHealth;
        maxStamina = 50;
        currentStamina = maxStamina;
        staminaIncreasement = 0.75f;
        StartCoroutine(StaminaIncreaser());
        lastSpawnPos = player.transform.position;
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
    /// <summary>
    /// 初始化组件
    /// </summary>
    /// <param name="body">玩家Rigidbody</param>
    /// <param name="animator">玩家Animator</param>
    /// <param name="Forward">指示玩家前方的指示器</param>
    /// <param name="Attacks">攻击检测器</param>
    /// <param name="width">控制玩家转向的宽度值</param>
    public void InitComponents(Rigidbody2D body, Animator animator, GameObject Forward, EnemyDetector[] Attacks, float width) {
        this.body = body;
        this.animator = animator;
        this.Forward = Forward;
        this.Attacks = Attacks;
        this.width = width;
    }
    /// <summary>
    /// 是否在攻击中
    /// </summary>
    /// <returns></returns>
    public bool IsAttacking() {
        return (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_A) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_B) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_C) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ATTACK_D));
    }
    /// <summary>
    /// 添加向前的力
    /// </summary>
    /// <param name="force">力的大小</param>
    public void AddFrontForce(float force = 0) {
        if(force == 0) {
            body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * 9, body.velocity.y);
        } else {
            body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * force, body.velocity.y);
        }
            
    }
    /// <summary>
    /// 添加向上的力
    /// </summary>
    /// <param name="force">力的大小</param>
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
    /// <summary>
    /// 是否在翻滚中
    /// </summary>
    /// <returns></returns>
    public bool IsRolling() {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(PAStat.ROLL);
    }
    /// <summary>
    /// 翻滚中每次动画状态更新调用，添加一个朝向翻滚方向的加速度
    /// </summary>
    public void OnRollGoing() {
        body.velocity = new Vector2((Forward.transform.position.x - player.transform.position.x) * 14, body.velocity.y);
    }
    /// <summary>
    /// 退出翻滚动画
    /// </summary>
    public void OnRollExit() {
        _isRolling = false;
        animator.ResetTrigger(PAParameters.ROLL);
    }
    /// <summary>
    /// 控制朝向
    /// </summary>
    /// <param name="deltaX">转向的方向，负数为向左，正数反之</param>
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
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="deltaX">移动的速度</param>
    public void Move(float deltaX) {
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        if (movement != Vector2.zero && !_isRunning && !_isJumping && !IsAttacking() && !IsRolling()
            && !_isRolling && _isGrounded /*&& !_isOnLadder*/) {
            body.velocity = movement;
        }
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    public void Jump() {
        if (_isGrounded && !IsAttacking() && !IsRolling() /*&& !_isOnLadder*/) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }
    /// <summary>
    /// 跑
    /// </summary>
    /// <param name="deltaX">速度</param>
    public void Run(float deltaX) {
        if (new Vector2(deltaX, body.velocity.y) != Vector2.zero
            && !_isJumping && !IsAttacking() && !IsRolling() && _isGrounded /*&& !_isOnLadder*/) {
            _isRunning = true;
            body.velocity = new Vector2(new Vector2(deltaX, body.velocity.y).x * 2, body.velocity.y);
            animator.SetFloat(PAParameters.SPEED, Mathf.Abs(deltaX * 2));
        }
    }
    /// <summary>
    /// 结束跑步动画
    /// </summary>
    public void RunExit() {
        _isRunning = false;
    }
    /// <summary>
    /// 攻击A
    /// </summary>
    public void AttackAEnter() {
        if (_isGrounded && !IsRolling() && currentStamina >= AttackCost && !_isJumping && !IsAttacking()) {
            currentStamina -= AttackCost;
            bar.UpdateSp();
            AddFrontForce();
            animator.SetInteger(PAParameters.ATTACKSTAT, 0);
        }
    }
    /// <summary>
    /// 攻击A动画退出
    /// </summary>
    public void AttackAExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    /// <summary>
    /// 攻击A判定
    /// </summary>
    public void AttackACheck() {
        foreach(string name in Attacks[0].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    /// <summary>
    /// 攻击B
    /// </summary>
    public void AttackBEnter() {
        if (_isGrounded && !IsRolling() && currentStamina >= AttackCost && !_isJumping) {
            currentStamina -= AttackCost;
            bar.UpdateSp();
            AddFrontForce();
            animator.SetInteger(PAParameters.ATTACKSTAT, 1);
        }
    }
    /// <summary>
    /// 攻击B判定
    /// </summary>
    public void AttackBCheck() {
        foreach (string name in Attacks[1].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    /// <summary>
    /// 攻击B动画退出
    /// </summary>
    public void AttackBExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    /// <summary>
    /// 攻击C
    /// </summary>
    public void AttackCEnter() {
        if (_isGrounded && !IsRolling() && currentStamina >= AttackCost && !_isJumping) {
            currentStamina -= AttackCost;
            bar.UpdateSp();
            AddFrontForce();
            animator.SetInteger(PAParameters.ATTACKSTAT, 2);
        }
    }
    /// <summary>
    /// 攻击C判定
    /// </summary>
    public void AttackCCheck() {
        foreach (string name in Attacks[2].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    /// <summary>
    /// 攻击C动画退出
    /// </summary>
    public void AttackCExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    /// <summary>
    /// 攻击D
    /// </summary>
    public void AttackDEnter() {
        if (_isGrounded && !IsRolling() && currentStamina >= AttackCost && !_isJumping) {
            currentStamina -= AttackCost;
            bar.UpdateSp();
            animator.SetInteger(PAParameters.ATTACKSTAT, 3);
            AddFrontForce();
        }
    }
    /// <summary>
    /// 攻击D判定
    /// </summary>
    public void AttackDCheck() {
        foreach (string name in Attacks[3].EnemyList) {
            EnemyManager manager = (EnemyManager)Managers.managers.GetManager(name);
            manager.GetHit(10);
        }
    }
    /// <summary>
    /// 攻击D动画退出
    /// </summary>
    public void AttackDExit() {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }
    /// <summary>
    /// 跳跃攻击
    /// </summary>
    public void JumpAttack() { 
        if (!_isGrounded && _isJumping && currentStamina >= AttackCost) {
            currentStamina -= AttackCost;
            bar.UpdateSp();
            animator.SetInteger(PAParameters.JUMP_ATTACK_STAT, 0);
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }
    /// <summary>
    /// 翻滚
    /// </summary>
    public void Roll() {
        if (_isGrounded && !IsAttacking() && currentStamina >= AttackCost) {
            _isRolling = true;
            animator.SetTrigger(PAParameters.ROLL);
        }
    }
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage">伤害</param>
    public void GetHit(float damage) {
        // TODO 玩家受伤
        print("hit player");
        if (_isRolling) return;
        currentHealth -= damage;
        bar.UpdateHealth();
        if (currentHealth <= 0) {
            Death();
            return;
        }
        animator.SetTrigger(PAParameters.HIT);
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public void Death() {
        print("death");
        animator.SetTrigger(PAParameters.DEATH);
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }
    public void Destroy() {
        animator.ResetTrigger(PAParameters.DEATH);
        player.transform.position = lastSpawnPos;
        gold -= 100;
        
    }
    /// <summary>
    /// 精力增长控制协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator StaminaIncreaser() {
        int times = 0;
        int time = (int)Mathf.Abs(Time.time);
        while (true) {
            //print("times"+times+" currSp:" + currentStamina);
            if (currentStamina <= maxStamina - staminaIncreasement) {
                if (time == (int)Mathf.Abs(Time.time)) {
                    currentStamina += staminaIncreasement;
                    times++;
                    bar.UpdateSp();
                } else {
                    times = 0;
                    time = (int)Mathf.Abs(Time.time);
                }
                yield return new WaitForSeconds(0.001f);
            } else {
                yield return new WaitForSeconds(0.017f);
            }
        }
    }
}
