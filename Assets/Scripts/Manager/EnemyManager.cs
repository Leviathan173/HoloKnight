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
    public float attackCost { get; set; }
    public float defence { get; set; }

    public bool isFacingRight { get; set; }
    public bool isGrounded { get; set; }
    public bool isJumping { get; set; }
    public bool usingShield { get; set; }
    public float jumpForce = 12.0f;
    public float width;

    private Vector3 origin;

    public PathFollower follower;
    public ActionController action;
    public PathFinderData PFData;
    //public List<Node> currNodes = new List<Node>();
    public PathFinder finder;
    public ActionController.ActionMode mode;
    public bool hasShield;
    /// <summary>
    /// 寻路委托
    /// </summary>
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
        //StartCoroutine(PathChecker());
        //StartCoroutine(Tester());

        origin = transform.position;
        follower = GetComponent<PathFollower>();
        finder = GetComponent<PathFinder>();
        action = GetComponent<ActionController>();

        health = GetComponentInChildren<HealthBarController>();
        PFData = PathFinderData.Instance;

        status = ManagerStatus.Started;
    }
    /// <summary>
    /// 初始化组件
    /// </summary>
    /// <param name="enemy">敌人自己</param>
    /// <param name="body">敌人Rigidbody</param>
    /// <param name="animator">敌人Animator</param>
    /// <param name="Forward">敌人面向方向的物体指示器</param>
    /// <param name="Attacks">检测攻击范围的检测器</param>
    /// <param name="width">控制敌人转向的宽度，可以通过裁剪精灵图废弃</param>
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
    /// <summary>
    /// 初始化血量等数值
    /// </summary>
    /// <param name="maxHealth">血量上限</param>
    /// <param name="maxStamina">精力上限</param>
    /// <param name="staminaIncreasement">精力回复速度</param>
    /// <param name="attackCost">攻击消耗精力</param>
    /// <param name="mode">进攻模式</param>
    /// <param name="hasShield">是否有盾</param>
    public void InitStats(float maxHealth, float maxStamina, float staminaIncreasement, float attackCost, ActionController.ActionMode mode, bool hasShield, float defence) {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.maxStamina = maxStamina;
        currentStamina = maxStamina;
        this.staminaIncreasement = staminaIncreasement;
        this.attackCost = attackCost;
        this.mode = mode;
        this.hasShield = hasShield;
        this.defence = defence;
    }

    /// <summary>
    /// 判断是否在攻击中
    /// </summary>
    /// <returns></returns>
    public bool IsAttacking() {
        return (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_A) ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(EAStat.ENEMY_ATTACK_B));
    }
    /// <summary>
    /// 添加向前的力
    /// </summary>
    /// <param name="force">力的大小</param>
    public void AddFrontForce(float force = 0) {
        if (force == 0) {
            body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * 9, body.velocity.y);
        } else {
            body.velocity = new Vector2((Forward.transform.position.x - enemy.transform.position.x) * force, body.velocity.y);
        }
    }


    /// <summary>
    /// 控制朝向
    /// </summary>
    public void Turn() {
        if (!IsAttacking()) {
            enemy.transform.localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
            if (gameObject.name.Contains("Slime")) {
                isFacingRight = !isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
                health.Turn();
                return;
            }
            if (isFacingRight) {
                //do turn left
                isFacingRight = !isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x - width, enemy.transform.position.y, enemy.transform.position.z);
                health.Turn();
            } else if (!isFacingRight) {
                //do turn right
                isFacingRight = !isFacingRight;
                enemy.transform.position = new Vector3(enemy.transform.position.x + width, enemy.transform.position.y, enemy.transform.position.z);
                health.Turn();
            }
        }
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="deltaX">移动方向</param>
    public void Move(float deltaX = 0) {
        if (deltaX < 0) {
            if (isFacingRight) {
                deltaX = -enemy.Speed;
            } else {
                deltaX = enemy.Speed;
            }
        } else {
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
    /// <summary>
    /// 跳跃，暂时废弃
    /// </summary>
    public void Jump() {
        if (isGrounded && !IsAttacking()) {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AddFrontForce(24);
            isGrounded = false;
        }
    }
    /// <summary>
    /// 攻击A
    /// </summary>
    public void AttackAEnter() {
        if (isGrounded && !isJumping) {
            animator.SetTrigger(EAParameters.ATTACK_A);
            currentStamina -= attackCost;
        }
    }
    // TODO 更加精准的判定
    // 或许修改一下碰撞体，让他和挥剑的动作一样移动
    // 或许就仅仅只是对每个攻击进行单独的延时触发判定

    /// <summary>
    /// 攻击A判定
    /// </summary>
    public void AttackACheck() {
        if (Attacks[0].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    /// <summary>
    /// 退出攻击A的动画
    /// </summary>
    public void AttackAExit() {
        animator.ResetTrigger(EAParameters.ATTACK_A);
    }
    /// <summary>
    /// 攻击B
    /// </summary>
    public void AttackBEnter() {
        if (isGrounded && !isJumping) {
            animator.SetTrigger(EAParameters.ATTACK_B);
            currentStamina -= attackCost;
        }
    }
    /// <summary>
    /// 攻击B判定
    /// </summary>
    public void AttackBCheck() {
        if (Attacks[1].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    /// <summary>
    /// 退出攻击B的动画
    /// </summary>
    public void AttackBExit() {
        animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // TODO 或许可以通过继承来避免这样的实现
    // 暂时废弃
    /// <summary>
    /// slime的攻击B
    /// </summary>
    public void AttackBEnterSlime() {
        if (isGrounded && !isJumping) {
            animator.SetTrigger(EAParameters.ATTACK_B);
            body.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
            AddFrontForce(18);
        }
    }
    /// <summary>
    /// slime攻击B判定
    /// </summary>
    public void AttackBCheckSlime() {
        if (Attacks[1].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    /// <summary>
    /// slime攻击B动画退出
    /// </summary>
    public void AttackBExitSlime() {
        animator.ResetTrigger(EAParameters.ATTACK_B);
    }

    // 攻击C只用于slime
    /// <summary>
    /// 攻击C
    /// </summary>
    public void AttackCEnter() {
        if (isGrounded && !isJumping) {
            animator.SetTrigger(EAParameters.ATTACK_C);
            currentStamina -= attackCost;
        }

    }
    /// <summary>
    /// 攻击C判定
    /// </summary>
    public void AttackCCheck() {
        if (Attacks[2].hasPlayer) {
            Managers.Player.GetHit(10);
        }
    }
    /// <summary>
    /// 退出攻击C动画
    /// </summary>
    public void AttackCExit() {
        animator.ResetTrigger(EAParameters.ATTACK_C);
    }

    /// <summary>
    /// 举盾
    /// </summary>
    public void UseShield() {
        animator.SetBool(EAParameters.SHIELD, true);
        usingShield = true;
    }
    /// <summary>
    /// Un举盾
    /// </summary>
    public void UnuseShield() {
        animator.SetBool(EAParameters.SHIELD, false);
        usingShield = false;
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage">税前伤害</param>
    public void GetHit(float damage) {
        damage = damage / (defence / 100);
        if (usingShield) {
            damage = damage * 0.2f;
            currentStamina -= damage * 2;
            if (currentStamina <= 0) {
                UnuseShield();
                damage = damage / 0.2f;
                animator.SetTrigger(EAParameters.HIT);
                health.SetHealth(damage);
                if (currentHealth < damage) {
                    currentHealth = 0;
                    Death();
                    return;
                } else {
                    currentHealth -= damage;
                }
            } else {
                health.SetHealth(damage);
                if (currentHealth < damage) {
                    currentHealth = 0;
                    Death();
                    return;
                } else {
                    currentHealth -= damage;
                }
            }
            return;
        }
        health.SetHealth(damage);
        if (currentHealth <= damage) {
            currentHealth = 0;
            Death();
            return;
        } else {
            currentHealth -= damage;
        }
        animator.SetTrigger(EAParameters.HIT);
    }

    /// <summary>
    /// 播放死亡动画
    /// </summary>
    public void Death() {
        animator.SetTrigger(EAParameters.DEAD);
    }
    /// <summary>
    /// 摧毁敌人物体
    /// </summary>
    public void Enemy_Destroy() {
        Managers.Player.gold += 50;
        float ran = Random.Range(0, 5);
        if(ran == 0) {
            GameObject heal = (GameObject)Resources.Load("Prefabs/Item/Red Potion");
            heal = Instantiate(heal);
            heal.transform.position = enemy.transform.position;
        }
        Destroy(enemy.gameObject);
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
                    if (usingShield) {
                        currentStamina += staminaIncreasement * 0.25f;
                    } else {
                        currentStamina += staminaIncreasement;
                    }
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
    // TODO 或许可以通过修改值来控制寻路的频率
    /// <summary>
    /// 寻路协程
    /// </summary>
    /// <returns></returns>
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
