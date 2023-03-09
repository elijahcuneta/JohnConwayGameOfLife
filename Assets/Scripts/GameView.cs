using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [Header("Ingame UI's")]

    [SerializeField] 
    private Text _cellCountText;
    [SerializeField] 
    private Text _cellSpawnedText;
    [SerializeField] 
    private Text _cellDiedText;
    [SerializeField] 
    private Text _gridDimensionText;
    [SerializeField] 
    private Text _screenResolutionText;
    [SerializeField] 
    private Text _stepsText;

    [SerializeField] 
    private Text _speedText;
    [SerializeField] 
    private Text _cameraZoomText;

    [SerializeField] 
    private Text _pausePlayText;

    [SerializeField] 
    private Animator _controlPanelAnimator;
    [SerializeField] 
    private Animator _dataPanelAnimator;

    [SerializeField] 
    private Text _editCellText;

    [Header("Title Screen UI's")] 
    [Space(25)]
    
    [SerializeField]
    private Text _screenResolutionInfoText;
    
    [SerializeField] 
    private InputField _gridWidthInputField;
    [SerializeField] 
    private InputField _gridHeightInputField;
    
    [SerializeField] 
    private Text _placeHolderWidth;
    [SerializeField] 
    private Text _placeHolderHeight;

    public void StaticDataUpdate(GameModel gameStatData)
    {
        _gridDimensionText.text = "Grid Dimension: " + gameStatData.GridWidth.ToString() + "x" + gameStatData.GridHeight.ToString();
        _screenResolutionText.text = "Screen Res: " + gameStatData.ScreenWidth.ToString() + "x" + gameStatData.ScreenHeight.ToString();
    }

    public void DynamicDataUpdate(GameModel gameStatData)
    {
        _cellCountText.text = "Cell Count: " + gameStatData.CellCount.ToString();
        _cellSpawnedText.text = "Cell Spawned: " + gameStatData.CellSpawned.ToString();
        _cellDiedText.text = "Cell Died: " + gameStatData.CellDied.ToString();
        _stepsText.text = "Steps: " + gameStatData.Steps;
    }

    public void OpenControlPanel(bool isOpen)
    {
        _controlPanelAnimator.SetBool("isOpen", isOpen);
    }
    
    public void OpenDataPanel(bool isOpen)
    {
        _dataPanelAnimator.SetBool("isOpen", isOpen);
    }

    public void UpdateScreenResolutionInfoText(string screenResolution)
    {
        _screenResolutionInfoText.text = "Screen Resolution\n" + screenResolution;
    }

    public void UpdateEditCellText(string editCellText)
    {
        _editCellText.text = "Edit Cell: " + editCellText;
    }

    public void UpdatePausePlayButton(string buttonTitle)
    {
        _pausePlayText.text = buttonTitle;
    }

    public void UpdateSpeedText(float speedValue)
    {
        _speedText.text = speedValue.ToString();
    }
    
    public void UpdateCameraZoomText(string cameraZoomValue)
    {
        _cameraZoomText.text = cameraZoomValue;
    }

    public void ChangeWidthInputFieldInteractableState(bool isInteractable)
    {
        _gridWidthInputField.interactable = isInteractable;
    }
    
    public void ChangeHeightInputFieldInteractableState(bool isInteractable)
    {
        _gridHeightInputField.interactable = isInteractable;
    }
    
    public void ChangeWidthPlaceHolderText(string widthPlaceHolder, bool showPlaceHolder = false)
    {
        _placeHolderWidth.text = widthPlaceHolder;
        if (showPlaceHolder)
        {
            _gridWidthInputField.text = "";
        }
    }
    
    public void ChangeHeightPlaceHolderText(string heightPlaceHolder, bool showPlaceHolder = false)
    {
        _placeHolderHeight.text = heightPlaceHolder;
        if (showPlaceHolder)
        {
            _gridHeightInputField.text = "";
        }
    }
}
