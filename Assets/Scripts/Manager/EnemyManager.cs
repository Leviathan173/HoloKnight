using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float maxStamina { get; set; }
    public float currentStamina { get; set; }
    public float staminaIncreasement { get; set; }

    public bool _isFacingRight { get; set; }
    public bool _isGrounded { get; set; }
    public bool _isJumping { get; set; }
    public bool _usingShield { get; set; }
    public float _ladderX { get; set; }
    public float jumpForce = 12.0f;

    void FixedUpdate() {
        if (currentStamina < maxStamina) {
            currentStamina += staminaIncreasement;
        }
    }

    public void Startup() {
        
        _isFacingRight = true;
        _ladderX = 0;
        _isGrounded = false;
        _isJumping = false;


        status = ManagerStatus.Started;
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
    public bool IsAttacking(Animator _animator) {
        return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_A) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_B));
    }
    // 添加向前的力
    public void AddFrontForce(Rigidbody2D _body, GameObject enemy, GameObject Forward, float force = 0) {
        if (force == 0) {
            _body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * 9, _body.velocity.y);
        } else {
            _body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * force, _body.velocity.y);
        }
    }


    // 控制朝向
    public void Turn(float deltaX, float _width, GameObject enemy, Animator _animator) {
        if (!IsAttacking(_animator)) {
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
    public void Move(float deltaX, Animator _animator, Rigidbody2D _body) {
        _animator.SetFloat(EAParameters.SPEED, 1.0f);
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        if (movement != Vector2.zero && !_isJumping && !IsAttacking(_animator)
             && _isGrounded) {
            _body.velocity = movement;
        }
    }
    // 跳跃
    public void Jump(Rigidbody2D _body, GameObject enemy, GameObject Forward, Animator _animator) {
        if (_isGrounded && !IsAttacking(_animator)) {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AddFrontForce(_body, enemy, Forward, 24);
            _isGrounded = false;
        }
    }
    // 攻击A
    public void AttackAEnter(Animator _animator) {
        if (_isGrounded && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_A);
            // TODO 攻击判定
        }
    }
    // 攻击A判定
    public void AttackACheck(IEnemyDetector attack, Animator _animator) {
        foreach (string name in attack.EnemyList) {
            print(name);
            Managers.managers.GetManager(name).GetHit(10, _animator);
        }
    }
    // 攻击A取消
    public void AttackAExit(Animator _animator) {
        _animator.ResetTrigger(EAParameters.ATTACK_A);
    }
    // 攻击B
    public void AttackBEnter(Animator _animator) {
        if (_isGrounded && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_B);
            // TODO 攻击判定

        }
    }
    // 攻击B判定
    public void AttackBCheck(IEnemyDetector attack, Animator _animator) {
        foreach (string name in attack.EnemyList) {
            print(name);
            Managers.managers.GetManager(name).GetHit(10, _animator);
        }
    }
    // 攻击B取消
    public void AttackBExit(Animator _animator) {
        _animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // slime攻击B
    public void AttackBEnterSlime(Rigidbody2D _body,GameObject enemy, GameObject Forward, Animator _animator) {
        if (_isGrounded && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_B);
            _body.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
            AddFrontForce(_body, enemy, Forward, 18);
        }
    }
    public void AttackBCheckSlime(IEnemyDetector attack, Animator _animator) {
        foreach (string name in attack.EnemyList) {
            print(name);
            Managers.managers.GetManager(name).GetHit(10, _animator);
        }
    }
    public void AttackBExitSlime(Animator _animator) {
        _animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // 攻击C
    public void AttackCEnter(Animator _animator) {
        _animator.SetTrigger(EAParameters.ATTACK_C);
    }
    // 攻击C判定
    public void AttackCCheck(IEnemyDetector attack, Animator _animator) {
        foreach (string name in attack.EnemyList) {
            print(name);
            Managers.managers.GetManager(name).GetHit(10, _animator);
        }
    }
    // 攻击C取消
    public void AttackCExit(Animator _animator) {
        _animator.ResetTrigger(EAParameters.ATTACK_C);
    }

    // 举盾
    public void UseShield(Animator _animator) {
        _animator.SetBool(EAParameters.SHIELD, true);
    }
    public void UnuseShield(Animator _animator) {
        _animator.SetBool(EAParameters.SHIELD, false);
    }

    // 受击
    // TODO 把传入的damage进行判断
    public void GetHit(float damage, Animator _animator) {
        _animator.SetTrigger(EAParameters.HIT);
    }

    // 死亡
    // TODO 消除敌人并给予玩家经验
    public void Death(Animator _animator) {
        _animator.SetTrigger(EAParameters.DEAD);
    }

    public void Enemy1_Destroy(GameObject enemy) {
        Destroy(enemy);
    }
}
