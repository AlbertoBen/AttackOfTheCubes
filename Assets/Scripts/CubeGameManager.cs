using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGameManager : MonoBehaviour
{
	public InterfaceManager interfaceManager;
	static public CubeGameManager instance;

	void Start()
	{
		instance = this;
	}

	static public void Restart()
	{
		instance.interfaceManager.GameOver();
	}
}
