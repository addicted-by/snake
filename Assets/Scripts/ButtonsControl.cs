using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClick_NewGame()
    {
        SceneManager.LoadScene(0);
    }
    public void OnClick_Quit()
    {
        Application.Quit();

    }
    public void OnClick_Train()
    {
        SceneManager.LoadScene(1);
    }

}
