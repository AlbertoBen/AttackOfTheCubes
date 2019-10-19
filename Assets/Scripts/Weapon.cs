using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
	public AudioClip SonidoDisparar;
	public float latenciaContador = 0.0f;
	public GameObject Proyectil;
	public List<GameObject> Proyectiles = new List<GameObject>();
	public List<WeaponParameters> WeaponParameters = new List<WeaponParameters>();
	public int mCurrentWeapon = 0;

	void Update()
	{
		latenciaContador -= Time.deltaTime;

		if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
		{
			mCurrentWeapon++;
			if (mCurrentWeapon == WeaponParameters.Count) mCurrentWeapon = 0;
			InterfaceManager.SetWeaponText(WeaponParameters[mCurrentWeapon].mName);

		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
		{
			mCurrentWeapon--;
			if (mCurrentWeapon < 0) mCurrentWeapon = WeaponParameters.Count - 1;
			InterfaceManager.SetWeaponText(WeaponParameters[mCurrentWeapon].mName);
		}

		if (latenciaContador > 0) return;

		if (Time.timeScale == 0)
		{
			return;
		}

		if (Input.GetMouseButton(0))
		{
			Dispara();
			latenciaContador = WeaponParameters[mCurrentWeapon].mCadence;
		}
	}

	void Start()
	{
		mCurrentWeapon = 0;
		latenciaContador = WeaponParameters[mCurrentWeapon].mCadence;
		InterfaceManager.SetWeaponText(WeaponParameters[mCurrentWeapon].mName);
	}

	public bool Dispara()
	{
		if (SonidoDisparar)
		{
			AudioSource.PlayClipAtPoint(SonidoDisparar, transform.position, 0.30f);
		}

		for (int a = 0; a < WeaponParameters[mCurrentWeapon].mProyectiles; a++)
		{
			Quaternion rotation = Quaternion.LookRotation(RandomInsideCone(WeaponParameters[mCurrentWeapon].mSpreadAngle), Vector3.up);
			GameObject instantiatedObject = Object.Instantiate(Proyectil, transform.position, transform.rotation * rotation) as GameObject;
			Transform weapon = instantiatedObject.transform;
			instantiatedObject.GetComponent<ProyectilPistola>().VelocidadDeAvance = WeaponParameters[mCurrentWeapon].mProyectileSpeed;
			instantiatedObject.GetComponent<ProyectilPistola>().mDamagePerProyectile = WeaponParameters[mCurrentWeapon].mDamagePerProyectile;
			Proyectiles.Add(instantiatedObject);
		}

		return true;
	}

	public static Vector3 RandomInsideCone(float radius)
	{
		//funcion copiada de internet
		float radradius = radius * Mathf.PI / 360;
		float z = Random.Range(Mathf.Cos(radradius), 1);
		float t = Random.Range(0, Mathf.PI * 2);
		return new Vector3(Mathf.Sqrt(1 - z * z) * Mathf.Cos(t), Mathf.Sqrt(1 - z * z) * Mathf.Sin(t), z);
	}

}
