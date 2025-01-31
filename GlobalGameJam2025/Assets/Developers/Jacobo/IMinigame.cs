public interface IMinigameEventListener
{
    void OnMinigameInit() { }
    void OnMinigameStart() { }
    void OnMinigameEnd() { }
    void OnPlayerDeath(PlayerCore player) { }
}