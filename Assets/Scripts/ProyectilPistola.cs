using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ProyectilPistola : MonoBehaviour
{
	public float VelocidadDeAvance = 10.0f;
	public int mDamagePerProyectile = 10;
	public float avance = 0;
	public float distanciaArrecorrer = 5000;

	void Awake()
	{
		avance = 0;
	}

	void Update()
	{
		Avance();
	}

	void Avance()
	{
		float delta = Time.deltaTime * VelocidadDeAvance;
		avance += delta;
		transform.position += transform.forward * delta;

		if (avance > distanciaArrecorrer)
		{
			Destroy(this.gameObject);
		}
	}
}
