/// <summary>
/// 敌人动画机的参数
/// </summary>
class EAParameters {
    /// <summary>
    /// 用来标记行走方向的速度，不参与移速运算，float
    /// </summary>
    public const string SPEED = "Speed";
    /// <summary>
    /// 是否接地，bool
    /// </summary>
    public const string GROUNDED = "Grounded";
    /// <summary>
    /// 受伤，trigger
    /// </summary>
    public const string HIT = "Hit";
    /// <summary>
    /// 死亡，trigger
    /// </summary>
    public const string DEAD = "Dead";
    /// <summary>
    /// 攻击A，trigger
    /// </summary>
    public const string ATTACK_A = "AttackA";
    /// <summary>
    /// 攻击B，trigger
    /// </summary>
    public const string ATTACK_B = "AttackB";
    /// <summary>
    /// 攻击C，trigger
    /// </summary>
    public const string ATTACK_C = "AttackC";
    // TODO 缓慢行走
    /// <summary>
    /// Walk动画播放速度的系数，float
    /// </summary>
    public const string WALK_SPEED = "WalkSpeed";
    /// <summary>
    /// 盾牌使用，bool
    /// </summary>
    public const string SHIELD = "Shield";
}
