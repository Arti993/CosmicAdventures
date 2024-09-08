
public interface IGameProgress : IService
{
    public void SaveLevelProgress(int points);

    public int GetCurrentLevelNumber();

    public int GetLevelResult(int levelNumber);

    public int GetTotalScore();

    public void ClearSaves();

    public void SelectLevel(int levelNumber);
    public void ClearSelectedLevel();

}
