/// <summary>
/// 玩家动画机的参数
/// </summary>
public class PAParameters {  
    /// <summary>
    /// 待机状态, trigger
    /// </summary>
    public const string IDLE = "Idle";
     /// <summary>
     /// 攻击种类，int;
     /// 0 = 攻击A;
     /// 1 = 攻击B;
     /// 2 = 攻击C;
     /// 3 = 攻击D;
    /// </summary>
    public const string ATTACKSTAT = "AttackStat";  
    /// <summary>
    /// 当前移动速度，float
    /// </summary>
    public const string SPEED = "Speed";
    /// <summary>
    /// 是否着地，bool
    /// </summary>
    public const string GROUND = "Grounded";
    /// <summary>
    /// 翻滚，trigger
    /// </summary>
    public const string ROLL = "Roll";
    /// <summary>
    /// 踢击，trigger
    /// </summary>
    public const string KICK = "Kick"; 
    /// <summary>
    /// 受伤，trigger
    /// </summary>
    public const string HIT = "Hit";   
    /// <summary>
    /// 死亡，trigger
    /// </summary>
    public const string DEATH = "Death";
    /// <summary>
    /// 举盾，trigger
    /// </summary>
    public const string SHIELD = "Shield";
    /* 攀爬状态，int
     * 0 = 上梯子的途中
     * 1 = 在梯子待机
     * 2 = 爬梯子
     * 
     */
    /*
   public const string CLIMB_STAT = "ClimbStat";
   //      从梯子落下，trigger
   public const string FALL_DOWN_LADDER = "FallDownLadder";
   //      控制动画播放速度的delta值，负数就倒放，int
   public const string ANIME_PLAY_DELTA = "AnimePlayDelta";
   //      从梯子上方离开，trigger
   public const string LADDER_TOP = "LadderTop";
   //      从梯子底部离开，trigger
   public const string LADDER_BOTTOM = "LadderBottom";*/
    /// <summary>
    /// 跳跃攻击状态
    /// 0 = 跳跃攻击一式
    /// 1 = 跳跃攻击二式
    /// </summary>
    public const string JUMP_ATTACK_STAT = "JumpAttackStat";

}