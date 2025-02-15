using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : UIPanel
{
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;

    private void OnEnable()
    {
        if (optionsButton)
        {
            optionsButton.onClick.AddListener(GoToOptions);
        }

        if (creditsButton)
        {
            creditsButton.onClick.AddListener(GoToCredits);
        }
    }

    private void OnDisable()
    {
        if (optionsButton)
        {
            optionsButton.onClick.RemoveListener(GoToOptions);
        }

        if (creditsButton)
        {
            creditsButton.onClick.RemoveListener(GoToCredits);
        }
    }

    private void GoToOptions()
    {
        GetPanel(typeof(OptionsPanel)).Show();
    }

    private void GoToCredits()
    {
        GetPanel(typeof(CreditsPanel)).Show();
    }
}
