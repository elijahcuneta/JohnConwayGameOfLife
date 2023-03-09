using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameModel _gameModel;

    [SerializeField]
    private GameView _gameView;

    [Range(0.5f, 1.5f)]
    [SerializeField] 
    private float cameraZoomValue = 1;

    public static event Action OnGridAddStep;
    public static event Action<bool> OnGridPausePlay;
    public static event Action OnGridRandomize;
    public static event Action OnGridClear;
    public static event Action OnGridDestroy;

    private CellManager _cellManager;
    private float previousZoom;

    private bool runGridAtStart;

    private bool _canEditCell;

    private bool controlPanelOpen = true;
    private bool gameStatInfoPanelOpen = true;
    
    private void Start()
    {
        _gameModel.Initialize();
        UseScreenResolution(true); //default
        RunGridAtStartToggle(true); //default
        _gameView.UpdateScreenResolutionInfoText(_gameModel.GridModelData.SCREEN_WIDTH.ToString() + "x" +
                                                 _gameModel.GridModelData.SCREEN_HEIGHT);
        
        previousZoom = cameraZoomValue;
    }

    private void Update()
    {
        if (_cellManager != null)
        {
            _cellManager.Update();
            _gameView.DynamicDataUpdate(_gameModel);
            
            if (previousZoom != cameraZoomValue)
            {
                transform.localScale = new Vector3(cameraZoomValue, cameraZoomValue, cameraZoomValue); 
                previousZoom = cameraZoomValue;
            }
        }
    }

    #region Title Screen Input/Data

    public void GridWidthUserInput(string gridWidthInput)
    {
        if (int.TryParse(gridWidthInput, out int width))
        {
            _gameModel.inputWidth = int.Parse(gridWidthInput);
        }
    }
    
    public void GridHeightUserInput(string gridHeightInput)
    {
        if (int.TryParse(gridHeightInput, out int height))
        {
            _gameModel.inputHeight = int.Parse(gridHeightInput);
        }
    }

    public void UseScreenResolution(bool useScreenResolution)
    {
        _gameModel.fillScreenCell = useScreenResolution;
        
        _gameView.ChangeWidthInputFieldInteractableState(!useScreenResolution);
        _gameView.ChangeHeightInputFieldInteractableState(!useScreenResolution);
            
        _gameView.ChangeWidthPlaceHolderText(useScreenResolution ? 
            Mathf.FloorToInt(_gameModel.GridModelData.SCREEN_WIDTH / _gameModel.GridModelData.SPRITE_DIMENSION).ToString() : "",
            true);
        _gameView.ChangeHeightPlaceHolderText(useScreenResolution ? 
            Mathf.FloorToInt(_gameModel.GridModelData.SCREEN_HEIGHT / _gameModel.GridModelData.SPRITE_DIMENSION).ToString() : "",
            true);
    }

    public void RunGridAtStartToggle(bool runGridAtStart)
    {
        this.runGridAtStart = runGridAtStart;
    }

    public void StartGame()
    {
        _cellManager = new CellManager(_gameModel, InstantiateCell, DestroyObject, transform);
        if (_cellManager != null)
        {
            _cellManager.GenerateGrid();
            if(runGridAtStart)
                _cellManager.RunGrid();
        }
        
        _gameView.StaticDataUpdate(_gameModel);
        _gameView.UpdatePausePlayButton(_cellManager.IsGridRunning ? "Pause" : "Play");
    }

    #endregion

    #region Game Input/Data

    public void PausePlayGrid()
    {
        OnGridPausePlay?.Invoke(!_cellManager.IsGridRunning);
        _gameView.UpdatePausePlayButton(_cellManager.IsGridRunning ? "Pause" : "Play");
    }
     
    public void AddStepGrid()
    {
        if (_gameModel.CellCount <= 0)
        {
            return;
        }
        
        OnGridPausePlay?.Invoke(false); //pause grid when using this feature
        OnGridAddStep?.Invoke();
    }
         
    public void RandomizeGrid()
    {
        OnGridRandomize?.Invoke();
    }
     
    public void ClearGrid()
    {
        OnGridClear?.Invoke();
    }
     
    public void OnSpeedChange(float newValue)
    {
        float newSpeed = newValue + 3; //for visual purposes (show 1-5)
        if (newValue == 0)
        {
            _gameModel.speed = 0.1f; //default speed
        }
        else
        {
            _gameModel.speed = (2 / newSpeed) - 0.35f;
        }
             
        _gameView.UpdateSpeedText((int)newSpeed);
    }
         
    public void OnCameraZoomChange(float newValue)
    {
        cameraZoomValue = 1 + (newValue - 0.5f);
        _gameView.UpdateCameraZoomText(cameraZoomValue.ToString());
    }

    public void EditCell()
    {
        _canEditCell = !_canEditCell;
        if (_cellManager != null)
        {
            _cellManager.CanEditCell(_canEditCell);
            _gameView.UpdateEditCellText(_canEditCell ? "On" : "Off");
        }
    }

    public void BackToMainMenu()
    {
        OnGridDestroy?.Invoke();
        if (_cellManager != null)
        {
            _cellManager = null;
        }
    }

    public void ToggleControlPanel()
    {
        controlPanelOpen = !controlPanelOpen;
        _gameView.OpenControlPanel(controlPanelOpen);
    }

    public void ToggleDataPanel()
    {
        gameStatInfoPanelOpen = !gameStatInfoPanelOpen;
        _gameView.OpenDataPanel(gameStatInfoPanelOpen);
    }

    #endregion
  
    
    public Cell InstantiateCell(GameObject cellObject, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(cellObject, position, quaternion).GetComponent<Cell>();
    }

    public void DestroyObject(GameObject destroyObject)
    {
        Destroy(destroyObject);
    }
}
