using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InterfaceManager : MonoBehaviour
{

	public Canvas mCanvas;
	public bool mPaused = true;
	public bool mGameOver = false;

	public GameObject mPausePanel;
	public GameObject mStartPanel;
	public GameObject mGameOverPanel;
	public Slider mHealth;
	public Text mWeaponText;
	public Text mCubesText;
	static InterfaceManager instance; 

	void Awake()
	{
		instance = this;
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().onHealthChange += updateHealth;
	}

	void Start()
	{
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		mStartPanel.SetActive(true);
		mPausePanel.SetActive(false);
		mGameOverPanel.SetActive(false);
	}

	void Update()
	{
		if (!mGameOver && Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}
	public void GameOver()
	{
		mGameOver = true;
		mCanvas.enabled = true;
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		mStartPanel.SetActive(false);
		mPausePanel.SetActive(false);
		mGameOverPanel.SetActive(true);

	}

	public void RestartGame()
	{
		Application.LoadLevel(0);
		EnemyManager.EnemigosSpameadosTotal = 0;
		EnemyManager.EnemyToKill = 151;
	}
	public void Pause()
	{
		mPaused = true;
		mCanvas.enabled = true;
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		mPausePanel.SetActive(true);
	}

	public void Resume()
	{
		Time.timeScale = 1;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		mStartPanel.SetActive(false);
		mPausePanel.SetActive(false);
		mGameOverPanel.SetActive(false);
	}

	public void StartGame()
	{
		Time.timeScale = 1;
		mCanvas.enabled = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		mStartPanel.SetActive(false);
		mPausePanel.SetActive(false);
		mGameOverPanel.SetActive(false);
	}
	public void updateHealth(float value)
	{
		mHealth.value = value;
	}

	static public void SetWeaponText(string Name)
	{
		instance.mWeaponText.text = Name;
	}
	static public void SetCubesText(string Name)
	{
		instance.mCubesText.text = Name;
	}

	public void Quit()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
