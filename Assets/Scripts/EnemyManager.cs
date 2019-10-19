using UnityEngine;


public class EnemyManager : MonoBehaviour
{
	public GameObject enemy;
	public float spawnTime = 3f;
	public Transform spawnPoints;
	public int EnemigosMininimosSpameados = 0;
	public int EnemigosASpamearAqui = 20;
	public int EnemigosSpameadosAqui = 0;
	public static int EnemigosSpameadosTotal = 0;
	public static int EnemyToKill = 151;

	void Start()
	{
		InvokeRepeating("Spawn", spawnTime, spawnTime);
		InterfaceManager.SetCubesText("Cubes to kill:" + EnemyManager.EnemyToKill.ToString());
	}

	void Spawn()
	{
		//enemigos espameados en total minimos para empezar en este enemy manager
		if (EnemigosSpameadosTotal < EnemigosMininimosSpameados)
		{
			return;
		}

		//limmite de enemigos a espamear en este enemy manager
		if (EnemigosSpameadosAqui >= EnemigosASpamearAqui)
		{
			return;
		}
		EnemigosSpameadosAqui++;
		EnemigosSpameadosTotal++;

		if (spawnPoints.childCount > 0)
		{
			int spawnPointIndex = Random.Range(0, spawnPoints.childCount - 1);
			Instantiate(enemy, spawnPoints.transform.GetChild(spawnPointIndex).position, spawnPoints.transform.GetChild(spawnPointIndex).rotation);
		}

		if (EnemigosSpameadosAqui > EnemigosASpamearAqui)
		{
			this.enabled = false;
		}
	}
}
