using UnityEngine;
using UnityEngine.UI;

public class ConfigPanel : UIPanel
{
    [SerializeField] private Button backButton;

    private void OnEnable()
    {
        if (backButton) {
            backButton.onClick.AddListener(GoBack);
        }
    }

    private void OnDisable()
    {
        if (backButton) {
            backButton.onClick.RemoveListener(GoBack);
        }
    }

    private void GoBack()
    {
        GetPanel(typeof(RosterPanel)).Show();
    }
}
