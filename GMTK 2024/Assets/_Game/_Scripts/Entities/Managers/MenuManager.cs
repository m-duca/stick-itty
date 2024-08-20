using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Referências:")]
    [SerializeField] public List<GameObject> mainMenuElements;
    [SerializeField] public List<GameObject> stageSelectionElements;
    [SerializeField] public GameObject stage2ButtonContainer;
    [SerializeField] public GameObject stage3ButtonContainer;
    [SerializeField] public GameObject stage4ButtonContainer;
    [SerializeField] public GameObject stage5ButtonContainer;

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

        CheckUnlockedStages();
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

    public void CheckUnlockedStages()
    {
        int unlockedStages = PlayerPrefs.GetInt("UnlockedStages", 1);
        stage2ButtonContainer.SetActive(false);
        stage3ButtonContainer.SetActive(false);
        stage4ButtonContainer.SetActive(false);
        stage5ButtonContainer.SetActive(false);

        if (unlockedStages >= 2) stage2ButtonContainer.SetActive(true);
        if (unlockedStages >= 3) stage3ButtonContainer.SetActive(true);
        if (unlockedStages >= 4) stage4ButtonContainer.SetActive(true);
        if (unlockedStages >= 5) stage5ButtonContainer.SetActive(true);
    }
}
