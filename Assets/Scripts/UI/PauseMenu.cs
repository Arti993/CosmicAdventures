using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenu : GameplayUIWindow
{
    [SerializeField] private CanvasGroup _backgroundPanel;
    
    private bool _isGamePaused;

    private void Awake()
    {
        PauseGame();
    }
    
    public void ResumeGame()
    {
        if (_isGamePaused == false)
            throw new InvalidOperationException();

        PausePanelOutro();

        Time.timeScale = 1f;
        
        GameObject uiRoot = AllServicesContainer.Instance.GetService<IUiWindowFactory>().GetUIRoot();

        AllServicesContainer.Instance.GetService<IUiWindowFactory>().GetPauseButton(uiRoot);
    }

    public void RestartLevel()
    {
        _isGamePaused = false;
        
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        AllServicesContainer.Instance.GetService<IScreenFader>().FadeOutAndLoadScene(sceneIndex);
    }

    public void ChangeGameSettings()
    {
        GameObject uiRoot = AllServicesContainer.Instance.GetService<IUiWindowFactory>().GetUIRoot();
        
        AllServicesContainer.Instance.GetService<IUiWindowFactory>().GetLanguageChangerWindow(uiRoot.gameObject);
    }

    private void PausePanelIntro()
    {
        _backgroundPanel.DOFade(1, PanelAnimationDuration).SetUpdate(true);
        
        PanelIntro();
    }

    private void PausePanelOutro()
    {
        _backgroundPanel.DOFade(0, PanelAnimationDuration).SetUpdate(true);
        
        PanelOutro();
    }

    private void PauseGame()
    {
        if (_isGamePaused)
            throw new InvalidOperationException();

        _isGamePaused = true;

        DestroyPauseButton();
        
        PausePanelIntro();

        Time.timeScale = 0f;
    }
}