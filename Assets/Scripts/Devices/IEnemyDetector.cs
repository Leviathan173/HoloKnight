using System.Collections.Generic;
public interface IEnemyDetector {
    List<string> EnemyList { get; set; }
    bool hasPlayer { get; set; }
}