using AStar;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private Vector3 origin;

    static PathFinder LocalPathFinder;
    public static System.Action<List<Node>> PathOfNodes = delegate (List<Node> nodes) {
        print("委托开始");
        if (nodes == null || nodes.Count == 0) {
            print("Node为空, 目标点不可达");
            return;
        }
        //if (StopCoroutine()) {

        //}
        //(nodes.All(tmp.Contains) && nodes.Count == tmp.Count)

        float cost = 0;
        foreach (var node in nodes) {
            if(node.prevNode!= null) {
                cost += LocalPathFinder.graphData.GetPathBetweenNode(node.prevNode, node).cost;
                if(cost > 2000) {
                    print("out of range");
                    break;
                }
            }
        }
        if (cost <= 500) {
            print("可以跟寻路线");
            // TODO 跟寻路线
            // 新建一个全局变量List，每次返回了一次路径的时候，把上次的路径放进List，判断这次的路径与上次的路径是否相同
            // 如果不相同：
            //              停止跟寻路径：停止跟寻路径的协程，并把这次的路径复制到List
            // 如果相同：
            //              继续跟寻，

        }
    };

    //void FixedUpdate() {
    //    if (currentStamina < maxStamina) {
    //        currentStamina += staminaIncreasement;
    //    }
    //}

    public void Startup() {
        
        _isFacingRight = true;
        _isGrounded = false;
        _isJumping = false;

        StartCoroutine(StaminaIncreaser());
        StartCoroutine(PathChecker());
        //StartCoroutine(Tester());

        origin = transform.position;

        //Attacks = GetComponentsInChildren<EnemyDetector>();
        health = GetComponentInChildren<HealthBarController>();
        LocalPathFinder = PathFinder.Instance;
        print("PathFinder is null?" + (LocalPathFinder == null ? "yes" : "no"));
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
    public void Death() {
        _animator.SetTrigger(EAParameters.DEAD);
    }

    public void Enemy1_Destroy() {
        Destroy(enemy);
    }

    private IEnumerator StaminaIncreaser() {
        int times = 0;
        int time = (int)Mathf.Abs(Time.time);
        while (true) {
            //print("times"+times+" currSp:" + currentStamina);
            if(currentStamina <= maxStamina - staminaIncreasement) {
                if(time == (int)Mathf.Abs(Time.time)) {
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
            if(LocalPathFinder != null) {
                if (Vector3.Distance(origin, Managers.Player.player.transform.position) < 30) {
                    LocalPathFinder.FindShortestPathOfNodes(LocalPathFinder.FindNearestNode(transform.position),
                        LocalPathFinder.FindNearestNode(Managers.Player.player.transform.position),
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
