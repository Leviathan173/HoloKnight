using AStar;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject Forward;
    [SerializeField] private EnemyDetector[] Attacks;
    [SerializeField] private EnemyManager manager;
    [SerializeField] private PathFinder finder;

    [SerializeField] public float Speed = 2.0f;
    [SerializeField] public float SlowSpeed = 1.0f;
    [SerializeField] public float MaxHp = 100.0f;
    [SerializeField] public float MaxStamina = 100.0f;
    [SerializeField] public float StaminaIncreasement = 0.25f; // 每秒增长60次 0.25*60 = 15

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private Vector2 collSize;
    private Vector2 collOffset;
    private ManagerRegister register;
    

    private float width;


    void Start() {
        // 初始化值
        register = GetComponent<ManagerRegister>();
        register.Register();
        manager = (EnemyManager)Managers.managers.GetManager(gameObject.name);
        //manager = GetComponent<EnemyManager>();
        print("start manager name :" + manager.name);
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        finder = GetComponent<PathFinder>();

        width = GetComponent<SpriteRenderer>().bounds.size.x / 3;

        collSize = boxCollider.size;
        collOffset = boxCollider.offset;

        manager.InitComponents(this, body, animator, Forward, Attacks, width);
        manager.InitStats(MaxHp, MaxStamina, StaminaIncreasement);

    }

    void FixedUpdate() {
        // 落地检测
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        body.GetContacts(contacts);
        if (contacts != null) {
            bool hited = false;
            foreach (var contact in contacts) {
                if (contact.collider != null) {
                    if (contact.collider.name.Contains("Ground") ||
                        contact.collider.name.Contains("Plat")) {
                        manager.isGrounded = true;
                        animator.SetBool(EAParameters.GROUNDED, true);
                        manager.isJumping = false;
                        hited = true;
                        break;
                    }
                }

            }
            if (!hited) {
                manager.isGrounded = false;
                manager.isJumping = true;
                animator.SetBool(EAParameters.GROUNDED, false);
            }
        }
    }
    // 测试用
    void Update() {

        if (Input.GetKeyDown(KeyCode.F3)) {
            finder.FindShortestPathOfNodes(PathFinderData.Instance.FindNearestNode(transform.position),
                        PathFinderData.Instance.FindNearestNode(Managers.Player.player.transform.position),
                        manager,
                        manager.PathOfNodes);
        }

        // 控制碰撞
        boxCollider.offset = collOffset;
        boxCollider.size = collSize;

        // 转向
        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            manager.Turn();
        }


        // 移动
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
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
    /// <summary>
    /// 敌人死亡
    /// </summary>
    public void Enemy_Death() {
        Destroy(gameObject, 0);
    }
}
