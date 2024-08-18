using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Referências:")]
    [SerializeField] public GameObject playButtonContainer;
    [SerializeField] public GameObject playButtonText;
    [SerializeField] public GameObject quitButtonContainer;
    [SerializeField] public GameObject quitButtonText;

    public void OpenStageSelection()
    {
        Debug.Log("Selecionando fases...");
        playButtonContainer.SetActive(false);
        playButtonText.SetActive(false);

        quitButtonContainer.SetActive(false);
        quitButtonText.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitou");
        Application.Quit();
    }

    public void ReturnToMenu()
    {

    }

    public void OpenStage(string stage)
    {

    }
}
