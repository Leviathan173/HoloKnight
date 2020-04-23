using UnityEngine;
public interface IGameManager {
	ManagerStatus status {get;}

    float maxHealth { get; set; }
    float currentHealth { get; set; }
    float maxStamina { get; set; }
    float currentStamina { get; set; }
    float staminaIncreasement { get; set; }

	void Startup();
    void GetHit(float damage, Animator animator);
    void Death(Animator animator);
}
