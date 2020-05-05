using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject Forward;
    [SerializeField] private EnemyDetector[] Attacks;

    [SerializeField] public float Speed = 2.0f;
    [SerializeField] public float SlowSpeed = 1.0f;
    [SerializeField] public float MaxHp = 100.0f;
    [SerializeField] public float MaxStamina = 100.0f;
    [SerializeField] public float StaminaIncreasement = 0.25f; // 每秒增长60次 0.25*60 = 15

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Vector2 collSize;
    private Vector2 collOffset;
    private ManagerRegister register;
    private static EnemyManager manager;

    private float _width;


    void Start() {
        register = GetComponent<ManagerRegister>();
        register.Register();
        manager = (EnemyManager)Managers.managers.GetManager(gameObject.name);

        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _width = GetComponent<SpriteRenderer>().bounds.size.x / 3;

        collSize = _boxCollider.size;
        collOffset = _boxCollider.offset;

        manager.InitComponents(gameObject, _body, _animator, Forward, Attacks, _width);
        manager.InitStats(MaxHp, MaxStamina, StaminaIncreasement);

    }

    void FixedUpdate() {
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        _body.GetContacts(contacts);
        if (contacts != null) {
            bool hited = false;
            foreach (var contact in contacts) {
                if (contact.collider != null) {
                    if (contact.collider.name.Contains("Ground") ||
                        contact.collider.name.Contains("Plat")) {
                        manager._isGrounded = true;
                        _animator.SetBool(EAParameters.GROUNDED, true);
                        manager._isJumping = false;
                        hited = true;
                        break;
                    }
                }

            }
            if (!hited) {
                manager._isGrounded = false;
                manager._isJumping = true;
                _animator.SetBool(EAParameters.GROUNDED, false);
            }
        }
    }

    void Update() {

        // 控制碰撞
        _boxCollider.offset = collOffset;
        _boxCollider.size = collSize;

        // 移动
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            manager.Turn(Speed);
            manager.Move(Speed);
        }

        // 跳跃
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            manager.Jump();
        }

        // 攻击A
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            manager.AttackAEnter();
        }
        // 攻击B
        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            manager.AttackBEnter();
        }

        // 受击
        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            manager.GetHit(0);
        }

        // 死亡
        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            manager.Death();
        }
    }

    public void Enemy1_Death() {
        Destroy(gameObject, 0);
    }
}
