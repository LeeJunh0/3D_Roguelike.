using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public GameObject Shop_UI;
    public GameObject Title_UI;
    private void Awake()
    {
        Shop_Close();
    }
    public void StartButton()
    {
        SceneManager.LoadScene("3DGame");
    }
    public void ESC()
    {
        DataController.instance.SaveData();
        Application.Quit();
    }
    public void Shop_Open()
    {
        Shop_UI.SetActive(true);
        Title_UI.SetActive(false);
    }
    public void Shop_Close()
    {
        Shop_UI.SetActive(false);
        Title_UI.SetActive(true);
    }
}
