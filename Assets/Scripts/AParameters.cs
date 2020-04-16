public class AParameters {
    //初始化状态, trigger
    public const string IDLE = "Idle";
    /*攻击种类，int
     * 0 = 攻击A
     * 1 = 攻击B
     * 2 = 攻击C
     * 3 = 攻击D
     */
    public const string ATTACKSTAT = "AttackStat";
    // 当前移动速度，float
    public const string SPEED = "Speed";
    // 是否着地，bool
    public const string GROUND = "Grounded";

    //public const string CLIMB = "Climb";

    // 开始翻滚，trigger
    public const string ROLL = "Roll";
    // 踢击，trigger
    public const string KICK = "Kick";
    // 受伤，trigger
    public const string HIT = "Hit";
    // 死亡，trigger
    public const string DEATH = "Death";
    // 举盾，trigger
    public const string SHIELD = "Shield";
    /* 攀爬状态，int
     * 0 = 上梯子的途中
     * 1 = 在梯子待机
     * 2 = 爬梯子
     * 
     */
    public const string CLIMB_STAT = "ClimbStat";
    // 从梯子落下，trigger
    public const string FALL_DOWN_LADDER = "FallDownLadder";
    // 控制动画播放速度的delta值，负数就倒放，int
    public const string ANIME_PLAY_DELTA = "AnimePlayDelta";
    // 从梯子上方离开，trigger
    public const string LADDER_TOP = "LadderTop";
    // 从梯子底部离开，trigger
    public const string LADDER_BOTTOM = "LadderBottom";
}