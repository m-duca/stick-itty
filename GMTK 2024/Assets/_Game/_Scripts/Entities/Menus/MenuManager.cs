using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Referências:")]
    [SerializeField] public List<GameObject> mainMenuElements;
    [SerializeField] public List<GameObject> stageSelectionElements;

    public void OpenStageSelection()
    {
        Debug.Log("Selecionando fases...");
        foreach (GameObject element in mainMenuElements)
        {
            if (element != null)
            {
                element.SetActive(false);
            }
        }

        foreach (GameObject element in stageSelectionElements)
        {
            if (element != null)
            {
                element.SetActive(true);
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitou");
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        Debug.Log("Voltou ao menu");
        foreach (GameObject element in stageSelectionElements)
        {
            if (element != null)
            {
                element.SetActive(false);
            }
        }

        foreach (GameObject element in mainMenuElements)
        {
            if (element != null)
            {
                element.SetActive(true);
            }
        }
    }

    public void OpenStage(string stage)
    {
        Debug.Log("Fase " +  stage + " Aberta");
        SceneManager.LoadScene(stage);
    }
}
