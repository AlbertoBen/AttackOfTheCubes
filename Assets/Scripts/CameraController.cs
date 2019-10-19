using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	//esto esta al 50% copiado de ejemplos del propio foro de unity y modificado para mis neceisdades

	RaycastHit currentHit;
	public Vector3 camAim = new Vector3(0f, 0f, 0f);
	float rayRange = 500;
	public float XMinRotation;
	public float XMaxRotation;
	[Range(1.0f, 10.0f)]
	public float Xsensitivity;
	[Range(1.0f, 10.0f)]
	public float Ysensitivity;
	public Camera cam;
	private float rotAroundX, rotAroundY;
	private bool camMoved = false;
	public GameObject Player;
	public GameObject Cam;
	public GameObject Arma;
	public bool firstPerson = true;
	public Transform FirstPersonPos;
	public Transform ThirdPersonPos;

	void Start()
	{
		rotAroundX = transform.eulerAngles.x;
		rotAroundY = transform.eulerAngles.y;
	}

	private void Update()
	{
		//si estamos en pasuda nos salimos
		if (Time.timeScale == 0) return;

		//cambio de primera a tercera persona usando un tweener
		if (Input.GetMouseButtonDown(1))
		{
			if (firstPerson)
			{
				firstPerson = false;
				LeanTween.moveLocal(Cam, ThirdPersonPos.localPosition, 1.5f).setEase(LeanTweenType.easeInOutQuad);
			}
			else
			{
				firstPerson = true;
				LeanTween.moveLocal(Cam, FirstPersonPos.localPosition, 1.5f).setEase(LeanTweenType.easeInOutQuad);
			}
		}

		rotAroundX += Input.GetAxis("Mouse Y") * Xsensitivity;
		rotAroundY += Input.GetAxis("Mouse X") * Ysensitivity;
		rotAroundX = Mathf.Clamp(rotAroundX, XMinRotation, XMaxRotation);

		//la rotacion de la camara mueve en horizontal al personaje 
		// y en vertical al rig de la camara
		Cam.transform.localRotation = Quaternion.Euler(-rotAroundX, 0, 0);
		Player.transform.rotation = Quaternion.Euler(0, rotAroundY, 0); 

		//si la mirilla esta en un cubo se dispara a ese punto , si no se dispara en la misma direccion
		//lo que corresponde al horizonte a las malas
		if (ShootRay())
		{
			Arma.transform.LookAt(currentHit.point);
		}
		else
		{
			Arma.transform.localRotation = Quaternion.Euler(-rotAroundX, 0, 0);
		}
	}


	public bool ShootRay()
	{
		//lo dicho si tenemos un rayo dando a un cubo nuestra arma apuntara al punto de impacto
		//esta funcion esta copiada de intenrnet...
		camAim = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
		LayerMask layerMask = (1 << LayerMask.NameToLayer("cubes")); 

		if (Physics.Raycast(camAim, cam.transform.forward, out currentHit, rayRange, layerMask))
		{
			return true;
		}
		return false;
	}
}
