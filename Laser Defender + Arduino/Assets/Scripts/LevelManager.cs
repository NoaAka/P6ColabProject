﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadLevel (string name){

		Debug.Log("Level load requested for "+name);
		//Application.LoadLevel(name );
		SceneManager.LoadScene(name);
		
	}
	public void LoadNextLevel(){
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}

	public void QuitRequest(){
	
		Debug.Log("Quit requested");
		Application.Quit();
	}




}
