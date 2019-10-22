

using UnityEngine;
using System.Collections;


public class CubeController : MonoBehaviour
{
	public Collider m_Cube;
	public GameObject celdaOcupada;
	public GameObject ObjetoCubo;

	public enum Comportamiento
	{
		Simple,
		SmallJumping,
		ZigZag,
		Titan
	}
	public Comportamiento m_Comportamiento = Comportamiento.Simple;

	public enum Estado
	{
		Iniciando,
		Parado,
		moviendo
	}

	public enum RodarHacia
	{
		delante,
		detras,
		izquierda,
		derecha
	}

	public Estado m_Estado = Estado.Iniciando;
	public RodarHacia ultimaDireccion; //el que hace zig zagg usa esta para repetir el patron
	public int contadorDeRepeticionDeDireccion = 0;
	public int repetirDireccion = 3;
	public float m_MoveSpeed = 0.15f;

	GameObject Player;
	float totalVal = 0;

	void Awake()
	{
		m_Estado = Estado.Iniciando;
		Player = GameObject.FindWithTag("Player");
	}

	void Start()
	{

		switch (m_Comportamiento)
		{
			case Comportamiento.Simple:
				{
					//orientamos el cuibo hacia el jugador
					Vector3 targetPosition = Player.transform.position;
					targetPosition.y = transform.position.y;
					transform.LookAt(targetPosition);
				}
				break;

			case Comportamiento.ZigZag:
				{
					// lo orientamos hacia el jugador y lo giramos 45 grados para que vaya haciendo zig zag
					Vector3 targetPosition = Player.transform.position;
					targetPosition.y = transform.position.y;
					transform.LookAt(targetPosition);
					Quaternion currentRotation = transform.rotation;
					transform.rotation = currentRotation * Quaternion.Euler(0, 45.0f, 0);
				}
				break;
		}

		//spam effect
		Vector3 EscalaOriginal = new Vector3(ObjetoCubo.transform.localScale.x, ObjetoCubo.transform.localScale.y, ObjetoCubo.transform.localScale.z);
		ObjetoCubo.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		LeanTween.scale(ObjetoCubo, EscalaOriginal, 0.4f).setEaseOutElastic().
			setOnComplete(()
		=>
		{
			m_Estado = Estado.Parado;
		});
	}

	bool ComprobarSiEsPosibleAvanzar(Transform cubeTransform, Vector3 dir)
	{
		//vamos a combrobar si donde queremos volcarnos no hay otro cubo o otro triger de otro cubo volcando
		Vector3 origin = cubeTransform.position;

		int layerId = LayerMask.NameToLayer("cubes");
		int layerMask = 1 << layerId;

		//vemos que hay donde queremos volcar
		Collider[] hitColliders = Physics.OverlapBox(dir, (cubeTransform.localScale * 0.9f) / 2, cubeTransform.rotation, layerMask);
		int i = 0;

		while (i < hitColliders.Length)
		{
			if (hitColliders[i].gameObject != celdaOcupada)
			{
				return false;// si no es nuestro trigger es que otro cubo se esta volcando aqui
			}
			i++;
		}
		return true;
	}

	public void ComandoAvanza()
	{
		switch (m_Comportamiento)
		{
			case Comportamiento.Simple:
				{
					EjecutaRodar(GetDireccionFromTarget(Player));
				}
				break;

			case Comportamiento.SmallJumping:
				{
					if ((Random.Range(0, 2) == 0))
					{
						EjecutaSaltar(GetDireccionFromTarget(Player));
					}
					else
					{
						EjecutaRodar(GetDireccionFromTarget(Player ));

					}
				}
				break;

			case Comportamiento.ZigZag:
				{
					// si estamos muy cerca vamos a ir a por el player si o si
					float tam = m_Cube.bounds.size.x * 4;
					float dist = Vector3.Distance(Player.transform.position, ObjetoCubo.transform.position);

					bool demasiadocerca = false;

					if (dist < tam)
					{
						//estamos demasiado cerca y vamos a dejar de hacer zigzag, vamos a por el player si o si 
						demasiadocerca = true;
					}

					if (contadorDeRepeticionDeDireccion == 0 || demasiadocerca)
					{
						ultimaDireccion = GetDireccionFromTarget(Player);
						contadorDeRepeticionDeDireccion = repetirDireccion;
					}
					else
					{
						contadorDeRepeticionDeDireccion--;
					}

					EjecutaRodar(ultimaDireccion);
				}
				break;
		}
		return;
	}
	private RodarHacia GetDireccionFromTarget(GameObject objetoHacia)
	{
		//esta funcion calcula la direccion respecto a la posicion que queremos ir... normalmente el player
		Vector3 DireccionHacia = objetoHacia.transform.position - m_Cube.transform.position;
		DireccionHacia = DireccionHacia.normalized;
		float dotHaciaDelanteOdetras = Vector3.Dot(DireccionHacia, transform.forward);
		float dotHaciaLosLados = Vector3.Dot(DireccionHacia, transform.right);

		if (Mathf.Abs(dotHaciaDelanteOdetras) > Mathf.Abs(dotHaciaLosLados))
		{
			if (dotHaciaDelanteOdetras > 0)
			{
				return (RodarHacia.delante);
			}
			else
			{
				return (RodarHacia.detras);
			}
		}
		else
		{
			if (dotHaciaLosLados > 0)
			{
				return (RodarHacia.izquierda);
			}
			else
			{
				return (RodarHacia.derecha);
			}
		}
	}

	private void EjecutaRodar(RodarHacia hacia)
	{
		//calculamos el punto del que rotar y el eje y ejecutamos un tweener , no tiene mas misterio esta funcion
		Vector3 coordenadaParaPivote = Vector3.zero;
		Vector3 rotateArroundAxis = Vector3.zero;
		if (m_Estado == Estado.moviendo) return; //ey estamos ya en movimiento 

		m_Estado = Estado.moviendo;
		Vector3 CheckDir = Vector3.zero;

		switch (hacia)
		{
			case RodarHacia.delante:
				{
					coordenadaParaPivote = m_Cube.transform.position + transform.forward * (m_Cube.bounds.size.y / 2f);
					rotateArroundAxis = -transform.right;
					CheckDir = m_Cube.transform.position + transform.forward * (m_Cube.bounds.size.x);
				}
			break;

			case RodarHacia.detras:
				{
					coordenadaParaPivote = m_Cube.transform.position - transform.forward * (m_Cube.bounds.size.y / 2f);
					rotateArroundAxis = transform.right;
					CheckDir = m_Cube.transform.position - transform.forward * (m_Cube.bounds.size.x);
				}
			break;

			case RodarHacia.izquierda:
				{
					coordenadaParaPivote = m_Cube.transform.position + transform.right * (m_Cube.bounds.size.y / 2f);
					rotateArroundAxis = transform.forward;
					CheckDir = m_Cube.transform.position + transform.right * (m_Cube.bounds.size.x);
				}
			break;

			case RodarHacia.derecha:
				{
					coordenadaParaPivote = m_Cube.transform.position - transform.right * (m_Cube.bounds.size.y / 2f);
					rotateArroundAxis = -transform.forward;
					CheckDir = m_Cube.transform.position - transform.right * (m_Cube.bounds.size.x);
				}
			break;
		}

		if (ComprobarSiEsPosibleAvanzar(m_Cube.transform, CheckDir) == false)
		{
			m_Estado = Estado.Parado;
			return;
		}

		coordenadaParaPivote -= new Vector3(0, m_Cube.bounds.size.y / 2f, 0);

		if (celdaOcupada != null)
		{
			//este triger lo colocamos donde vamos a volcar para que nadie mas vuelque 
			celdaOcupada.transform.position = CheckDir;
			celdaOcupada.transform.rotation = (m_Cube.transform.rotation);
			celdaOcupada.SetActive(true);
		}

		//nos guardamos la altura pq el tweener tampco es perfecto y puede ir metiendo ruido
		float yvalue = m_Cube.gameObject.transform.position.y;

		LeanTween.value(m_Cube.gameObject, 0, -90, m_MoveSpeed).setEase(LeanTweenType.linear).setOnUpdate(
			(float val) =>
			{
				float arotar = val - totalVal; //el valor hasta alcanzar los 90 grados
				m_Cube.transform.RotateAround(coordenadaParaPivote, rotateArroundAxis, arotar);
				totalVal = val;
			}).setOnComplete(()=>
			{
				//le devolvemos la altura correcta
				m_Cube.gameObject.transform.position = new Vector3(m_Cube.gameObject.transform.position.x, yvalue,m_Cube.gameObject.transform.position.z);
				totalVal = 0;
				m_Estado = Estado.Parado;
			});
	}

	private void EjecutaSaltar(RodarHacia hacia)
	{
		if (m_Estado == Estado.moviendo) return;
		Vector3 direccionAvanceSalto = Vector3.zero;
		m_Estado = Estado.moviendo;
		Vector3 CheckDir = Vector3.zero;

		switch (hacia)
		{
			case RodarHacia.delante:
				{
					direccionAvanceSalto = transform.forward * (m_Cube.bounds.size.y / 2f);
					CheckDir = m_Cube.transform.position + transform.forward * (m_Cube.bounds.size.x);
				}
				break;

			case RodarHacia.detras:
				{
					direccionAvanceSalto = -transform.forward * (m_Cube.bounds.size.y / 2f);
					CheckDir = m_Cube.transform.position - transform.forward * (m_Cube.bounds.size.x);
				}
				break;
			case RodarHacia.izquierda:
				{
					direccionAvanceSalto = transform.right * (m_Cube.bounds.size.y / 2f);
					CheckDir = m_Cube.transform.position + transform.right * (m_Cube.bounds.size.x);
				}
				break;
			case RodarHacia.derecha:
				{
					direccionAvanceSalto = -transform.right * (m_Cube.bounds.size.y / 2f);
					CheckDir = m_Cube.transform.position - transform.right * (m_Cube.bounds.size.x);
				}
				break;
		}

		if (ComprobarSiEsPosibleAvanzar(m_Cube.transform, CheckDir) == false)
		{
			m_Estado = Estado.Parado;
			return;
		}

		if (celdaOcupada != null)
		{
			//ocupamos la posicion con nuetro trigger para que ningun cubo vuelque aqui
			celdaOcupada.transform.position = CheckDir;
			celdaOcupada.transform.rotation = (m_Cube.transform.rotation);
			celdaOcupada.SetActive(true);
		}

		float yvalue = m_Cube.gameObject.transform.position.y; // nos guardamos la altura de nuestro cubo para delvolversela
		Vector3 salto = new Vector3(0f, 10f, 0f) + m_Cube.gameObject.transform.position + (direccionAvanceSalto / 2);
		Vector3 caida = m_Cube.gameObject.transform.position + (direccionAvanceSalto);

		LeanTween.move(m_Cube.gameObject, salto, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()
		=>
			{
				LeanTween.move(m_Cube.gameObject, caida, 0.5f).setEase(LeanTweenType.easeOutBounce).setOnComplete(()
				=>
				{
					m_Estado = Estado.Parado;
				});
			});
	}

	void LateUpdate()
	{
		if (m_Estado == Estado.moviendo || m_Estado == Estado.Iniciando)
		{
			return;
		}

		ComandoAvanza();
	}
}
