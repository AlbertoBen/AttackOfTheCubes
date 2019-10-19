using System.Collections;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
	public Color ColorVivo = Color.blue;
	public Color ColorMuerto = Color.red;
	public MeshRenderer geometria;
	public bool ReiniciaJuegoAlMorir = false;
	public AudioClip soundImpactProyectile;
	public int startingHealth = 100;
	public int currentHealth;
	AudioSource enemyAudio;
	CapsuleCollider capsuleCollider;
	public bool isDead = false;
	public GameObject CubeBase;
	void Awake()
	{
		geometria = GetComponent<MeshRenderer>();
		currentHealth = startingHealth;
	}

	void OnTriggerEnter(Collider other)
	{
		if (isDead) return;
		if (other.gameObject.tag == "proyectil")
		{
			int damage = other.gameObject.GetComponent<ProyectilPistola>().mDamagePerProyectile;
			if (soundImpactProyectile)
			{
				AudioSource.PlayClipAtPoint(soundImpactProyectile, transform.position, 2.30f);
			}

			Debug.Log("proyectil");
			TakeDamage(damage, new Vector3(0, 0, 0));
			Destroy(other.gameObject);
		}
	}

	public void TakeDamage(int amount, Vector3 hitPoint)
	{

		Debug.Log("TakeDamage");
		currentHealth -= amount;
		if (currentHealth < 0)
		{
			currentHealth = 0;
		}

		if (currentHealth == 0) isDead = true;

		float interpolacion = (float)currentHealth / (float)startingHealth;
		geometria.material.color = Color.Lerp(ColorMuerto, ColorVivo, interpolacion);

		if (currentHealth <= 0)
		{
			Debug.Log("Destroy");
			LeanTween.scale(gameObject, new Vector3(0.0f, 0.0f, 0.0f), 0.2f);
			Destroy(CubeBase, 1f);
			EnemyManager.EnemyToKill--;
			InterfaceManager.SetCubesText("Cubes to kill:" + EnemyManager.EnemyToKill.ToString());

			if (EnemyManager.EnemyToKill == 0)
			{
				StartCoroutine("ReiniciaJuego", 0.5f);
			}
		}
	}

	IEnumerator ReiniciaJuego()
	{
		yield return new WaitForSeconds(0.9f);
		CubeGameManager.Restart();
	}

}
