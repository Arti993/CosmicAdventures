using UnityEngine;
using YG;

public class MainMenu : MonoBehaviour
{
    private const string NotFirstGameLaunch = "NotFirstGameLaunch";
    private Canvas _uiRoot;

    private void Awake()
    {
        _uiRoot = GetComponentInParent<Canvas>();
    }

    public void OnPlayButtonClick()
    {
        DIServicesContainer.Instance.GetService<IScreenFader>().FadeOutAndLoadScene((int)SceneIndex.Gameplay);
    }

    public void OnLevelsChooseButtonCLick()
    {
        DIServicesContainer.Instance.GetService<IScreenFader>().FadeOutAndLoadScene((int)SceneIndex.LevelChoose);
    }

    public void OnNewGameButtonClick()
    {
        if (PlayerPrefs.HasKey(NotFirstGameLaunch))
            DIServicesContainer.Instance.GetService<IUiStateMachine>().SetState<UiStateRestartGameQuestion>();
        else
        {
            OnTutorialButtonClick();

            PlayerPrefs.SetInt(NotFirstGameLaunch, 1);
            PlayerPrefs.Save();
        }
    }

    public void OnTutorialButtonClick()
    {
        DIServicesContainer.Instance.GetService<IScreenFader>().FadeOutAndLoadScene((int)SceneIndex.Tutorial);
    }

    public void OnLeaderBoardButtonClick()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if(YandexGame.auth)
            DIServicesContainer.Instance.GetService<IUiStateMachine>().SetState<UiStateLeaderboard>();
        else
            DIServicesContainer.Instance.GetService<IUiStateMachine>().SetState<UiStateAuthorizationQuestion>();
#endif 
    }

    public void OnSettingsButtonClick()
    {
        DIServicesContainer.Instance.GetService<IUiStateMachine>().SetState<UiStateSettings>();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}