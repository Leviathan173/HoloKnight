using AStar;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public Enemy enemy;
    public Rigidbody2D body;
    public Animator animator;
    public GameObject Forward;
    public HealthBarController health;
    public EnemyDetector[] Attacks;


    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float maxStamina { get; set; }
    public float currentStamina { get; set; }
    public float staminaIncreasement { get; set; }

    public bool isFacingRight { get; set; }
    public bool isGrounded { get; set; }
    public bool isJumping { get; set; }
    public bool usingShield { get; set; }
    public float jumpForce = 12.0f;
    public float width;

    private Vector3 origin;

    [SerializeField] public PathFollower follower;
    public PathFinderData PFData;
    //public List<Node> currNodes = new List<Node>();
    [SerializeField] public PathFinder finder;
    public System.Action<List<Node>, EnemyManager> PathOfNodes = delegate (List<Node> nodes, EnemyManager manager) {
        print("委托开始");
        if (nodes == null || nodes.Count == 0) {
            print("Node为空, 目标点不可达");
            return;
        }
        float cost = 0;
        foreach (var node in nodes) {
            if (node.prevNode != null) {
                cost += manager.PFData.graphData.GetPathBetweenNode(node.prevNode, node).cost;
                if (cost > 2000) {
                    print("out of range");
                    return;
                }
            }
        }
        print("可以跟寻路线");
        // UNDONE 跟寻路线
        // 新建一个全局变量List，每次返回了一次路径的时候，把上次的路径放进List，判断这次的路径与上次的路径是否相同
        // 如果不相同：
        //              停止跟寻路径：停止跟寻路径的协程，并把这次的路径复制到List
        // 如果相同：
        //              继续跟寻，
        if (manager.follower.hasCoroutine) {
            print("has coroutine ...");
            manager.follower.StopFollow();
            manager.follower.FollowPath(nodes, manager);
        } else {
            print("has no coroutine");
            manager.follower.FollowPath(nodes, manager);
        }

    };

    public void Startup() {

        isFacingRight = true;
        isGrounded = false;
        isJumping = false;

        StartCoroutine(StaminaIncreaser());
        StartCoroutine(PathChecker());
        //StartCoroutine(Tester());

        origin = transform.position;
        follower = GetComponent<PathFollower>();
        finder = GetComponent<PathFinder>();

        health = GetComponentInChildren<HealthBarController>();
        PFData = PathFinderData.Instance;

        status = ManagerStatus.Started;
    }
    // 初始化组件
    public void InitComponents(Enemy enemy, Rigidbody2D body, Animator animator, GameObject Forward, EnemyDetector[] Attacks, float width) {
        this.enemy = enemy;
        this.body = body;
        this.animator = animator;
        this.Forward = Forward;
        this.Attacks = Attacks;
        this.width = width;
        if (enemy == null || body == null || animator == null || Forward == null || Attacks == null || width == 0) {
            Debug.LogError("has null Componets");
        }
    }
    // 初始化血量等数值
    public void InitStats(float maxHealth, float maxStamina, float staminaIncreasement) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.maxStamina = maxStamina;
        currentStamina = maxStamina;
        this.staminaIncreasement = staminaIncreasement;
    }

    // 是否在攻击中
    public bool IsAttacking() {
        return (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_A) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_B));
    }
    // 添加向前的力
    public void AddFrontForce(float force = 0) {
        if (force == 0) {
            body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * 9, body.velocity.y);
        } else {
            body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * force, body.velocity.y);
        }
    }


    // 控制朝向
    public void Turn() {
        if (!IsAttacking()) {
            enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
            if (gameObject.name.Contains("Slime")) {
                isFacingRight = !isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
                return;
            }
            if (isFacingRight) {
                //do turn left
                isFacingRight = !isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x - width, enemy.transform.position.y, enemy.transform.position.z);
            } else if (!isFacingRight) {
                //do turn right
                isFacingRight = !isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x + width, enemy.transform.position.y, enemy.transform.position.z);
            }
        }
    }
    // 移动
    // 他需要持续的调用
    // 每次调用执行一次
    public void Move(float deltaX = 0) {
        if (deltaX == 0) {
            if (isFacingRight) {
                deltaX = enemy.Speed;
            } else {
                deltaX = -enemy.Speed;
            }

        }
        animator.SetFloat(EAParameters.SPEED, 1.0f);
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        print(gameObject.name + " move  movement:" + movement + " isJumping:" + isJumping + " IsAttacking:" + IsAttacking() + " isGrounded:" + isGrounded);
        if (movement != Vector2.zero && !isJumping && !IsAttacking()
             && isGrounded) {
            body.velocity = movement;
        }
    }
    // 跳跃
    public void Jump() {
        if (isGrounded && !IsAttacking()) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AddFrontForce(24);
            isGrounded = false;
        }
    }
    // 攻击A
    public void AttackAEnter() {
        if (isGrounded && !isJumping) {
            animator.SetTrigger(EAParameters.ATTACK_A);
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
        animator.ResetTrigger(EAParameters.ATTACK_A);
    }
    // 攻击B
    public void AttackBEnter() {
        if (isGrounded && !isJumping) {
            animator.SetTrigger(EAParameters.ATTACK_B);
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
        animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // slime攻击B
    public void AttackBEnterSlime() {
        if (isGrounded && !isJumping) {
            animator.SetTrigger(EAParameters.ATTACK_B);
            body.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
            AddFrontForce(18);
        }
    }
    public void AttackBCheckSlime() {
        if (Attacks[1].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    public void AttackBExitSlime() {
        animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // 攻击C
    public void AttackCEnter() {
        animator.SetTrigger(EAParameters.ATTACK_C);
    }
    // 攻击C判定
    public void AttackCCheck() {
        if (Attacks[2].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    // 攻击C取消
    public void AttackCExit() {
        animator.ResetTrigger(EAParameters.ATTACK_C);
    }

    // 举盾
    public void UseShield() {
        animator.SetBool(EAParameters.SHIELD, true);
    }
    public void UnuseShield() {
        animator.SetBool(EAParameters.SHIELD, false);
    }

    // 受击
    public void GetHit(float damage) {
        animator.SetTrigger(EAParameters.HIT);
        if (currentHealth < damage) {
            currentHealth = 0;
        } else {
            currentHealth -= damage;
        }
        health.SetHealth(damage);
    }

    // 死亡
    public void Death() {
        animator.SetTrigger(EAParameters.DEAD);
    }

    public void Enemy1_Destroy() {
        Destroy(enemy);
    }

    private IEnumerator StaminaIncreaser() {
        int times = 0;
        int time = (int)Mathf.Abs(Time.time);
        while (true) {
            //print("times"+times+" currSp:" + currentStamina);
            if (currentStamina <= maxStamina - staminaIncreasement) {
                if (time == (int)Mathf.Abs(Time.time)) {
                    currentStamina += staminaIncreasement;
                    times++;
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

    private IEnumerator Tester() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0, 20));
            currentStamina -= 50;
        }
    }

    private IEnumerator PathChecker() {
        while (true) {
            if (PFData != null) {
                if (Vector3.Distance(origin, Managers.Player.player.transform.position) < 30) {
                    finder.FindShortestPathOfNodes(PFData.FindNearestNode(transform.position),
                        PFData.FindNearestNode(Managers.Player.player.transform.position),
                        this,
                        PathOfNodes);
                    yield return new WaitForSeconds(1);
                }
                yield return new WaitForSeconds(1);
            } else {
                print("PathFinder instance is null!");
            }
            yield return new WaitForSeconds(1);
        }
    }

}
