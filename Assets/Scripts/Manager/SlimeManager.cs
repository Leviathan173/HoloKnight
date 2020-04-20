using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private Slime slime;
    [SerializeField] private GameObject Back;
    [SerializeField] private GameObject Forward;

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    private float AttackBBounce;


    public bool _isFacingRight { get; set; }
    public bool _isGrounded { get; set; }
    public bool _isJumping { get; set; }
    public float _width { get; set; }

    public void Startup() {
        //print("starting Skeleton manager...");



        _body = slime.GetComponent<Rigidbody2D>();
        _boxCollider = slime.GetComponent<BoxCollider2D>();
        _animator = slime.GetComponent<Animator>();
        _width = slime.GetComponent<SpriteRenderer>().bounds.size.x / 3;

        _isFacingRight = true;
        _isGrounded = false;
        _isJumping = false;

        AttackBBounce = 3.0f;


        status = ManagerStatus.Started;
    }

    //攻击
    public bool IsAttacking() {
        return (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_A) ||
            _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_B));
    }
    // 添加向前的力
    public void AddFrontForce(float force = 0) {
        if (force == 0) {
            _body.velocity = new Vector2((Forward.transform.position.x - slime.transform.position.x) * 9, _body.velocity.y);
        } else {
            _body.velocity = new Vector2((Forward.transform.position.x - slime.transform.position.x) * force, _body.velocity.y);
        }

    }
    // 控制朝向
    public void Turn(float deltaX) {
        print("turn,deltaX:" + deltaX);
        if (!IsAttacking()) {
            slime.transform.localScale = new Vector3(Mathf.Sign(deltaX) * slime.transform.localScale.x, slime.transform.localScale.y, slime.transform.localScale.z);
            if (_isFacingRight && Mathf.Sign(deltaX) < 0) {
                //do turn left
                _isFacingRight = !_isFacingRight;
                slime.transform.position = new Vector3(slime.transform.position.x - _width, slime.transform.position.y, slime.transform.position.z);
            } else if (!_isFacingRight && Mathf.Sign(deltaX) > 0) {
                //do turn right
                _isFacingRight = !_isFacingRight;
                slime.transform.position = new Vector3(slime.transform.position.x + _width, slime.transform.position.y, slime.transform.position.z);
            }
        }
    }
    // 移动
    // 他需要持续的调用
    // 每次调用执行一次
    public void Move(float deltaX) {
        _animator.SetFloat(EAParameters.SPEED, 1.0f);
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        if (movement != Vector2.zero && !IsAttacking()) {
            _body.velocity = movement;
        }
    }
    // 攻击A
    public void AttackAEnter() {
        _animator.SetTrigger(EAParameters.ATTACK_A);
        // TODO 攻击判定
    }
    // 攻击A取消
    public void AttackAExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_A);
    }
    // 攻击B
    public void AttackBEnter() {
        _animator.SetTrigger(EAParameters.ATTACK_B);
        _body.AddForce(Vector2.up * AttackBBounce, ForceMode2D.Impulse);
        AddFrontForce(18);
        // TODO 攻击判定
    }
    // 攻击B取消
    public void AttackBExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_B);
    }
    // 攻击C
    public void AttackCEnter() {
        _animator.SetTrigger(EAParameters.ATTACK_C);
    }
    // 攻击C取消
    public void AttackCExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_C);
    }

    // 受击
    // TODO 把传入的damage进行判断
    public void GetHit(float damage) {
        _animator.SetTrigger(EAParameters.HIT);
    }

    // 死亡
    // TODO 消除敌人并给予玩家经验
    public void Death() {
        _animator.SetTrigger(EAParameters.DEAD);
    }

    public void Enemy1_Destroy() {
        slime.Enemy1_Death();
    }
}
