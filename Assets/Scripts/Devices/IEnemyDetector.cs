using System.Collections.Generic;
/// <summary>
/// 废弃接口
/// </summary>
public interface IEnemyDetector {
    List<string> EnemyList { get; set; }
    bool hasPlayer { get; set; }
}