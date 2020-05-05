using UnityEngine;
public interface IGameManager {
	ManagerStatus status {get;}

    //float maxHealth { get; set; }
    //float currentHealth { get; set; }
    //float maxStamina { get; set; }
    //float currentStamina { get; set; }
    //float staminaIncreasement { get; set; }

	void Startup();
    //void GetHit(float damage);
    //void Death();
    //void InitComponents(GameObject enemy, Rigidbody2D _body, Animator _animator, GameObject Forward, EnemyDetector[] Attacks, float _width);
}
