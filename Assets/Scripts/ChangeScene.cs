using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Button button;
    public string sceneName;

    // Start is called before the first frame update
    public void OnClickSceneChangeButton()
    {   
        try {
            SceneManager.LoadScene(sceneName);
        } catch (Exception e) {
            Debug.Log(e);
        }
    }
}
