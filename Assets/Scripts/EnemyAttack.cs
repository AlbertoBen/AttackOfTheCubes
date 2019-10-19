using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
	public int attackDamage = 10;
	public GameObject enemy;
	GameObject player;
	PlayerHealth playerHealth;
	EnemyHealth enemyHealth;
	public MeshRenderer geometria;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		geometria = GetComponent<MeshRenderer>();
		enemyHealth = GetComponent<EnemyHealth>();
	}

	void OnCollisionEnter(Collision col)
	{
		if (enemyHealth.isDead) return;
		if (col.gameObject == player)
		{
			Attack();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (enemyHealth.isDead) return;
		if (other.gameObject == player)
		{
			Attack();
		}
	}

	void Attack()
	{
		if (playerHealth.mCurrentHealth > 0)
		{
			playerHealth.TakeDamage(attackDamage);
			EnemyManager.EnemyToKill--;
			InterfaceManager.SetCubesText("Cubes to kill:" + EnemyManager.EnemyToKill.ToString());
			enemyHealth.isDead = true;

			//efectillo de destruccion de enemigo
			geometria.material.color = Color.red;
			LeanTween.scale(gameObject, new Vector3(0.0f, 0.0f, 0.0f), 0.3f).setEaseInBack();
			Destroy(enemy, 0.3f);
		}
	}
}
