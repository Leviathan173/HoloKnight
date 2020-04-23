﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    [SerializeField] private King king;
    [SerializeField] private GameObject Back;
    [SerializeField] private GameObject Forward;

    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    private float JumpAttackAirBounce;


    public Vector2 _ladderTopPos { get; set; }
    public Vector2 _ladderBottomPos { get; set; }
    public bool _isReachTopLadder { get; set; }
    public bool _isReachBottomLadder { get; set; }
    public bool _isOnLadder { get; set; }
    public bool _hasLadder { get; set; }
    public bool _isFacingRight { get; set; }
    public bool _isGrounded { get; set; }
    public bool _isJumping { get; set; }
    public float _ladderX { get; set; }
    public float _width { get; set; }
    public float jumpForce = 0.001f;

    public void Startup() {



        _body = king.GetComponent<Rigidbody2D>();
        _boxCollider = king.GetComponent<BoxCollider2D>();
        _animator = king.GetComponent<Animator>();
        _width = king.GetComponent<SpriteRenderer>().bounds.size.x / 3;

        JumpAttackAirBounce = 12.0f;
        _isReachTopLadder = false;
        _isReachBottomLadder = false;
        _isFacingRight = true;
        _isOnLadder = false;
        _hasLadder = false;
        _ladderX = 0;
        _isGrounded = false;
        _isJumping = false;


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
            _body.velocity = new Vector2((Forward.transform.position.x - king.transform.position.x) * 9, _body.velocity.y);
        } else {
            _body.velocity = new Vector2((Forward.transform.position.x - king.transform.position.x) * force, _body.velocity.y);
        }

    }

    /*梯子
    public void FallDownLadder() {
        _body.gravityScale = 3;
        _boxCollider.isTrigger = false;
        print("robot!!!");
        _body.velocity = new Vector2((Back.transform.position.x - skleton.transform.position.x) * 7, _body.velocity.y);
    }
    public void LadderMoveUp() {
        if (_isReachTopLadder) {
            _animator.SetInteger(PAParameters.CLIMB_STAT, -1);
            _animator.SetTrigger(PAParameters.LADDER_TOP);
            //_animator.ResetTrigger(PAParameters.LADDER_TOP);
            _body.gravityScale = 3;
            _boxCollider.isTrigger = false;
            skleton.transform.position = new Vector3(_ladderTopPos.x, _ladderTopPos.y, skleton.transform.position.z);
        } else {
            skleton.transform.position = new Vector3(skleton.transform.position.x, skleton.transform.position.y + 0.05f, skleton.transform.position.z);
        }
    }
    public void LadderMoveDown() {
        if (_isReachBottomLadder) {
            _animator.SetInteger(PAParameters.CLIMB_STAT, -1);
            _animator.SetTrigger(PAParameters.LADDER_BOTTOM);
            //_animator.ResetTrigger(PAParameters.LADDER_BOTTOM);
            _body.gravityScale = 3;
            _boxCollider.isTrigger = false;
            //gameObject.transform.position = new Vector3(_ladderBottomPos.x, _ladderBottomPos.y, gameObject.transform.position.z);
            _body.velocity = new Vector2(_ladderBottomPos.x / 3, _ladderBottomPos.y);
        } else {
            skleton.transform.position = new Vector3(skleton.transform.position.x, skleton.transform.position.y - 0.05f, skleton.transform.position.z);
        }
    }
    爬梯
    public void ClimbStart() {
        if (_hasLadder && !_isOnLadder) {
            _animator.SetInteger(PAParameters.CLIMB_STAT, 0);
            _body.gravityScale = 0;
            _body.velocity = Vector2.zero;
            _boxCollider.isTrigger = true;
        }
    }*/

    // 控制朝向
    public void Turn(float deltaX) {
        if (!IsAttacking()
            && !_isOnLadder) {
            king.transform.localScale = new Vector3(Mathf.Sign(deltaX) * king.transform.localScale.x, king.transform.localScale.y, king.transform.localScale.z);
            if (_isFacingRight && Mathf.Sign(deltaX) < 0) {
                //do turn left
                _isFacingRight = !_isFacingRight;
                king.transform.position = new Vector3(king.transform.position.x - _width, king.transform.position.y, king.transform.position.z);
            } else if (!_isFacingRight && Mathf.Sign(deltaX) > 0) {
                //do turn right
                _isFacingRight = !_isFacingRight;
                king.transform.position = new Vector3(king.transform.position.x + _width, king.transform.position.y, king.transform.position.z);
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
             && _isGrounded && !_isOnLadder) {
            _body.velocity = movement;
        }
    }
    // 跳跃
    public void Jump() {
        if (_isGrounded && !IsAttacking()  /*&& !_isOnLadder*/) {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //_body.AddForce(new Vector2(Forward.transform.position.x - skleton.transform.position.x, gameObject.transform.position.y) * jumpForce, ForceMode2D.Impulse);
            AddFrontForce(24);
            _isGrounded = false;
        }
    }
    // 攻击A
    public void AttackAEnter() {
        if (_isGrounded && !_isOnLadder && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_A);
            // TODO 攻击判定
        }
    }
    // 攻击A取消
    public void AttackAExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_A);
    }
    // 攻击B
    public void AttackBEnter() {
        if (_isGrounded && !_isOnLadder && !_isJumping) {
            _animator.SetTrigger(EAParameters.ATTACK_B);
            // TODO 攻击判定

        }
    }
    // 攻击B取消
    public void AttackBExit() {
        _animator.ResetTrigger(EAParameters.ATTACK_B);
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
        king.Enemy1_Death();
    }
}
