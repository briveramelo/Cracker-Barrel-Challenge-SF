using UnityEngine;
using System.Collections;

public class GameEscaper : MonoBehaviour {

	
	void Update () {
        if (Input.GetButtonDown("ExitGame")) {
            #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
            #else
             Application.Quit();
            #endif
        }
	}
}
