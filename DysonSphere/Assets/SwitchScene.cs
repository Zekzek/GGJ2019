using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void SceneSwitch()
    {
        SceneManager.LoadScene(sceneName);
    }
	private void Awake()
	{
		Time.timeScale = 1;
	}
}
