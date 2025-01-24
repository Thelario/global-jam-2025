using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerSelection : MonoBehaviour
{
    [HideInInspector] public int playerIndex;
    [HideInInspector] public SelectionMultiplayerManager selectionMultiplayerManager;
    [HideInInspector] public CharacterData characterSelected;

    // Referencias
    [Header("References")]
    [SerializeField] MeshRenderer gfxBody;
    [SerializeField] TMP_Text readyText;
    [SerializeField] TMP_Text pressForSelect;
    [SerializeField] Transform selectionArrows;

    bool isReady;

    // Cuando los valores ya estan asignados, 
    private void Start()
    {
        UpdateCharacterGfx(characterSelected);
        SetIsReady(false);
    }

    void UpdateCharacterGfx(CharacterData characterData)
    {
        if (characterData != null)
            gfxBody.material = characterData.characterMaterial;
    }

    void UpdateReadyGfx(bool ready)
    {
        readyText.gameObject.SetActive(ready);
        pressForSelect.gameObject.SetActive(!ready);
        selectionArrows.gameObject.SetActive(!ready);
    }

    #region Input

    float preInputX;

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        if (isReady)
            return;

        float inputX = context.ReadValue<Vector2>().x;
        //input_ver = context.ReadValue<Vector2>().y;

        if (preInputX == 0)
        {
            if (inputX > 0)
            {
                characterSelected = selectionMultiplayerManager.SelectLeftCharacterData(characterSelected.character);
                UpdateCharacterGfx(characterSelected);
            }
            else if (inputX < 0)
            {
                characterSelected = selectionMultiplayerManager.SelectRightCharacterData(characterSelected.character);
                UpdateCharacterGfx(characterSelected);
            }
        }

        preInputX = inputX;
    }

    public virtual void SelectInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SetIsReady(!isReady);
        }
    }

    #endregion

    public bool GetIsReady() { return isReady; }
    void SetIsReady(bool _isReady)
    {
        isReady = _isReady;
        UpdateReadyGfx(isReady);
        selectionMultiplayerManager.PlayersReadyRefresh();
    }
}
