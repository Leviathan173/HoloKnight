public interface IGameManager {
	ManagerStatus status {get;}
    //string name { get; set; }

	void Startup();
    void GetHit(float damage);
    void Death();
}
