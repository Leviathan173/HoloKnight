using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public GameObject enemy;
    public Rigidbody2D _body;
    public Animator _animator;
    public GameObject Forward;
    public HealthBarController health;
    public EnemyDetector[] Attacks;

    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float maxStamina { get; set; }
    public float currentStamina { get; set; }
    public float staminaIncreasement { get; set; }

    public bool _isFacingRight { get; set; }
    public bool _isGrounded { get; set; }
    public bool _isJumping { get; set; }
    public bool _usingShield { get; set; }
    public float jumpForce = 12.0f;
    public float _width;

    void FixedUpdate() {
        if (currentStamina < maxStamina) {
            currentStamina += staminaIncreasement;
        }
    }

    public void Startup() {
        
        _isFacingRight = true;
        _isGrounded = false;
        _isJumping = false;

        //Attacks = GetComponentsInChildren<EnemyDetector>();
        health = GetComponentInChildren<HealthBarController>();

        status = ManagerStatus.Started;
    }
    // 初始化组件
    public void InitComponents(GameObject enemy, Rigidbody2D _body, Animator _animator, GameObject Forward, EnemyDetector[] Attacks, float _width) {
        this.enemy = enemy;
        this._body = _body;
        this._animator = _animator;
        this.Forward = Forward;
        this.Attacks = Attacks;
        this._width = _width;
    }
    // 初始化血量等数值
    public void InitStats(float maxHealth, float maxStamina, float staminaIncreasement) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.maxStamina = maxStamina;
        currentStamina = maxStamina;
        this.staminaIncreasement = staminaIncreasement;
    }

    //攻击
    public bool IsAttacking() {
        return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_A) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_B));
    }
    // 添加向前的力
    public void AddFrontForce(float force = 0) {
        if (force == 0) {
            _body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * 9, _body.velocity.y);
        } else {
            _body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * force, _body.velocity.y);
        }
    }


    // 控制朝向
    public void Turn(float deltaX) {
        if (!IsAttacking()) {
            enemy.transform.localScale = new Vector3(Mathf.Sign(deltaX) * enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
            if (_isFacingRight && Mathf.Sign(deltaX) < 0) {
                //do turn left
                _isFacingRight = !_isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x - _width, enemy.transform.position.y, enemy.transform.position.z);
            } else if (!_isFacingRight && Mathf.Sign(deltaX) > 0) {
                //do turn right
                _isFacingRight = !_isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x + _width, enemy.transform.position.y, enemy.transform.position.z);
            }
        }
    }
    // 移动
    // 他需要持续的调用
    // 每次调用执行一次
    public void Move(float deltaX) {
        _animator.SetFloat(EAParameters.SPEED, 1.0f);
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        if (movement != Vector2.zero && !_isJumping && !IsAttacking()
             && _isGrounded) {
            _body.velocity = movement;
        }
    }
    // 跳跃
    public void Jump() {
        if (_isGrounded && !IsAttacking()) {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AddFrontForce(24);
            _isGrounded = false;
        }
    }
    // 攻击A
    public void AttackAEnter() {
        if (_isGrounded && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_A);
        }
    }
    // TODO 更加精准的判定
    // 或许修改一下碰撞体，让他和挥剑的动作一样移动
    // 或许就仅仅只是对每个攻击进行单独的延时触发判定
    
    // 攻击A判定
    public void AttackACheck() {
        if (Attacks[0].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    // 攻击A取消
    public void AttackAExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_A);
    }
    // 攻击B
    public void AttackBEnter() {
        if (_isGrounded && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_B);
        }
    }
    // 攻击B判定
    public void AttackBCheck() {
        if (Attacks[1].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    // 攻击B取消
    public void AttackBExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // slime攻击B
    public void AttackBEnterSlime() {
        if (_isGrounded && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_B);
            _body.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
            AddFrontForce(18);
        }
    }
    public void AttackBCheckSlime() {
        if (Attacks[1].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    public void AttackBExitSlime() {
        _animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // 攻击C
    public void AttackCEnter() {
        _animator.SetTrigger(EAParameters.ATTACK_C);
    }
    // 攻击C判定
    public void AttackCCheck() {
        if (Attacks[2].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    // 攻击C取消
    public void AttackCExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_C);
    }

    // 举盾
    public void UseShield() {
        _animator.SetBool(EAParameters.SHIELD, true);
    }
    public void UnuseShield() {
        _animator.SetBool(EAParameters.SHIELD, false);
    }

    // 受击
    public void GetHit(float damage) {
        _animator.SetTrigger(EAParameters.HIT);
        if(currentHealth < damage) {
            currentHealth = 0;
        } else {
            currentHealth -= damage;
        }
        health.SetHealth(damage);
    }

    // 死亡
    // TODO 消除敌人并给予玩家经验
    public void Death() {
        _animator.SetTrigger(EAParameters.DEAD);
    }

    public void Enemy1_Destroy() {
        Destroy(enemy);
    }

    public IEnumerator Follow(List<Node> finalPath) {
        // TODO 根据寻路算法获取的路线行走
        // 判断X坐标就行，Y轴给一个大概判定值，毕竟也不能飞
        // 巡逻的行动
        // 遇到了玩家之后的行动
        // 这个协程不会一直存在，当玩家的位置发生变化之后要结束这个协程，然后重新判断路径，重新寻路
        // 需要一个上限值来限制COST的上限，超过上限就可以不用进行移动
        while (true) {
            for (int i = 0; i < finalPath.Count; i++) {
                if (finalPath[i + 1] != null) {
                    if(finalPath[i].Position.x > enemy.transform.position.x) {
                        if (_isFacingRight) {
                            Move(3);
                            yield return new WaitForSeconds(1);
                        } else {
                            Turn(-1);
                            Move(3);
                            yield return new WaitForSeconds(1);
                        }
                    }
                }
            }
        }
    }

}
