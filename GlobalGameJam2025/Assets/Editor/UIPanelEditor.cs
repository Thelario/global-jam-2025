using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIPanel), true)]
public class UIPanelEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Add a custom button to the Inspector
        if (GUILayout.Button("Hide All Other Panels"))
        {
            HideAllUIPanels();
        }
    }
    public static List<UIPanel> GetAllPanelsEditor()
    {
        return new List<UIPanel>(FindObjectsByType<UIPanel>(FindObjectsSortMode.None));
    }
    private void HideAllUIPanels()
    {
        foreach (var panel in GetAllPanelsEditor())
        {
            if (panel != null)
            {
                CanvasGroup cg = panel.GetComponent<CanvasGroup>();
                if (cg != null) cg.alpha = 0.0f;
            }
        }
        CanvasGroup cgg = ((UIPanel)target).GetComponent<CanvasGroup>();
        if (cgg != null)
        {
            cgg.alpha = 1.0f;
        }
    }
}