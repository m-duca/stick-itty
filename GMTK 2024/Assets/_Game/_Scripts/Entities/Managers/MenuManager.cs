using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Transição:")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float loadTime;

    [Header("Referências:")]
    public List<GameObject> mainMenuElements;
    public List<GameObject> stageSelectionElements;
    public BoxCollider2D stage2ButtonContainer;
    public BoxCollider2D stage3ButtonContainer;
    [SerializeField] public BoxCollider2D stage4ButtonContainer;
    [SerializeField] public BoxCollider2D stage5ButtonContainer;
    public GameObject stage2ButtonChains;
    public GameObject stage3ButtonChains;
    [SerializeField] public GameObject stage4ButtonChains;
    [SerializeField] public GameObject stage5ButtonChains;

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
        TransitionManager.Instance().Transition(stage, transitionSettings, loadTime);
    }

    public void CheckUnlockedStages()
    {
        int unlockedStages = PlayerPrefs.GetInt("UnlockedStages", 1);
        stage2ButtonContainer.enabled = false;
        stage2ButtonChains.SetActive(true);
        stage3ButtonContainer.enabled = false;
        stage3ButtonChains.SetActive(true);
        stage4ButtonContainer.enabled = false;
        stage4ButtonChains.SetActive(true);
        stage5ButtonContainer.enabled = false;
        stage5ButtonChains.SetActive(true);

        if (unlockedStages >= 2) 
        {
            stage2ButtonContainer.enabled = true;
            stage2ButtonChains.SetActive(false);
        }
        if (unlockedStages >= 3)
        {
            stage3ButtonContainer.enabled = true;
            stage3ButtonChains.SetActive(false);
        }
        if (unlockedStages >= 4)
        {
            stage4ButtonContainer.enabled = true;
            stage4ButtonChains.SetActive(false);
        }
        if (unlockedStages >= 5)
        {
            stage5ButtonContainer.enabled = true;
            stage5ButtonChains.SetActive(false);
        }
    }
}
