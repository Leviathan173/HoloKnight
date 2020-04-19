class EAParameters {
    // 用来标记行走方向的速度，不参与移速运算，float
    public const string SPEED = "Speed";
    // 是否接地，bool
    public const string GROUNDED = "Grounded";
    // 受伤，trigger
    public const string HIT = "Hit";
    // 死亡，trigger
    public const string DEAD = "Dead";
    // 攻击A，trigger
    public const string ATTACK_A = "AttackA";
    // 攻击B，trigger
    public const string ATTACK_B = "AttackB";
    // TODO 缓慢行走，float
    // Walk动画播放速度的系数
    public const string WALK_SPEED = "WalkSpeed";
    // 盾牌使用，bool
    public const string SHIELD = "Shield";
}
