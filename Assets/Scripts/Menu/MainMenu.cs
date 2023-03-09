using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
	private UIDocument doc;

	public void Awake()
	{
		doc = GetComponent<UIDocument>();

		var startBtn = doc.rootVisualElement.Q<Button>("PlayButton");
		var exitBtn = doc.rootVisualElement.Q<Button>("ExitButton");


		startBtn.clicked += StartGame;
		exitBtn.clicked += ExitGame;
	}

	public void StartGame()
    {
		SceneManager.LoadScene("MainScene");
    }

	public void ExitGame()
	{
		Application.Quit();
	}
}
