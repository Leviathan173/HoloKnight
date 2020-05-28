/// <summary>
/// 管理器的基类
/// </summary>
public interface IGameManager {
    /// <summary>
    /// 管理器运行状态
    /// </summary>
	ManagerStatus status {get;}

    /// <summary>
    /// 管理器启动
    /// </summary>
	void Startup();
}
