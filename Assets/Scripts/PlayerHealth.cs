using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
	public int mStartingHealth = 100;
	public int mCurrentHealth;

	public delegate void OnHealthChange(float newValue);
	public event OnHealthChange onHealthChange;

	void Awake()
	{
		mCurrentHealth = mStartingHealth;
	}

	void Start()
	{
		if (onHealthChange != null)
			onHealthChange(mCurrentHealth);
	}

	public void TakeDamage(int amount)
	{
		
		mCurrentHealth -= amount;

		if (onHealthChange != null)
			onHealthChange(mCurrentHealth);

		if (mCurrentHealth <= 0)
		{
			CubeGameManager.Restart();
		}
	}
}
