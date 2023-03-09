using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellManager
{
    private GameModel _gameModel;
    private GameModel.GridData _gridData;

    private System.Random _random;
    private Transform _cellParent;
    private Camera _cam;

    private Func<GameObject, Vector3, Quaternion, Cell> _cellInstantiator;
    private Action<GameObject> _cellDestroyer;
    
    private Cell[,] _cellsArray;
    private Vector2 _startingPosition;

    private float _startingXPosition;
    private float _timer;

    private int _gridWidth;
    private int _gridHeight;
    
    private int _cellCount;
    private int _cellSpawned;
    private int _cellDied;
    private int _steps;

    private bool _runGrid;
    public bool IsGridRunning
    {
        get { return _runGrid; }
    }

    public CellManager(GameModel _gameModel, Func<GameObject, Vector3, Quaternion, Cell> _cellInstantiator, Action<GameObject> _cellDestroyer,Transform _cellParent = null)
    {
        this._gameModel = _gameModel;
        this._cellParent = _cellParent;
        this._cellInstantiator = _cellInstantiator;
        this._cellDestroyer = _cellDestroyer;

        _gridData = _gameModel.GridModelData;
        
        _gameModel.ScreenWidth = (int)_gridData.SCREEN_WIDTH;
        _gameModel.ScreenHeight = (int)_gridData.SCREEN_HEIGHT;

        _cam = Camera.main;
        _random = new System.Random();

        GameController.OnGridAddStep += CheckAllCells;
        GameController.OnGridPausePlay += RunGrid;
        GameController.OnGridRandomize += RandomizeGridCellLife;
        GameController.OnGridClear += ClearGridCell;
        GameController.OnGridDestroy += DeleteEntireGrid;
    }
    
    public void GenerateGrid()
    {
        _startingPosition = _cam.ViewportToWorldPoint(new Vector2(0, 1)); //upper left of the screen

       float SPRITE_DIMENSION = _gridData.SPRITE_DIMENSION;
        
        if (_gameModel.fillScreenCell)
        {
            _gridWidth = Mathf.FloorToInt(_gridData.SCREEN_WIDTH / SPRITE_DIMENSION);
            _gridHeight = Mathf.FloorToInt(_gridData.SCREEN_HEIGHT / SPRITE_DIMENSION);
        }
        else
        {
            _gridWidth = _gameModel.inputWidth;
            _gridHeight = _gameModel.inputHeight;
        }

        _gameModel.GridWidth = _gridWidth;
        _gameModel.GridHeight = _gridHeight;
        
        float cellWidth = (_gridData.SCREEN_WIDTH / _gridWidth) / SPRITE_DIMENSION;
        float cellHeight = (_gridData.SCREEN_HEIGHT / _gridHeight) / SPRITE_DIMENSION;
        
        _cellsArray = new Cell[_gridHeight, _gridWidth];
        
        _startingPosition = new Vector2(_startingPosition.x + ((cellWidth * (SPRITE_DIMENSION / 100)) / 2), _startingPosition.y - ((cellHeight * (SPRITE_DIMENSION / 100)) / 2));
        _startingXPosition = _startingPosition.x;
        
        for (int col = 0; col < _gridHeight; col++)
        {
            for (int row = 0; row < _gridWidth; row++)
            {
                Cell newCell = _cellInstantiator(_gridData.cell, Vector3.zero, Quaternion.identity);
                // newCell.gameObject.name = "Cell " + col + " " + row;
                newCell.transform.parent = _cellParent;
                newCell.transform.position = _startingPosition;
                newCell.transform.localScale = new Vector3(cellWidth, cellHeight, 1);
                
                _startingPosition += (Vector2.right * (cellWidth * (SPRITE_DIMENSION / 100)));
                newCell.Initialize(new CellModel.InitialCellData
                {
                    _column = col,
                    _row = row,
                    _isAlive = _random.Next(0, 100) <= 50 ? false : true
                }, this);
                
                _cellsArray[col, row] = newCell;
            }
            _startingPosition = new Vector2(_startingXPosition, _startingPosition.y - (cellHeight * (SPRITE_DIMENSION / 100)));
        }
        
        for (int col = 0; col < _gridHeight; col++)
        {
            for (int row = 0; row < _gridWidth; row++)
            {
                _cellsArray[col, row].InitializeNeighbors();
            }
        }
    }

    private void RandomizeGridCellLife()
    {
        if (_cellsArray != null)
        {
            foreach (Cell cell in _cellsArray)
            {
                cell.RandomizeCellLife();
            }
        }
    }

    private void ClearGridCell()
    {
        if (_cellsArray != null)
        {
            foreach (Cell cell in _cellsArray)
            {
                cell.SetCellLife(false);
            }
        }

        ClearGameInformation();
    }

    private void DeleteEntireGrid()
    {
        RunGrid(false);

        for (int col = 0; col < _gridHeight; col++)
        {
            for (int row = 0; row < _gridWidth; row++)
            {
                if (_cellsArray[col, row] != null)
                {
                    _cellDestroyer?.Invoke(_cellsArray[col, row].gameObject);
                }
            }
        }
    }

    public void RunGrid(bool _runGrid = true)
    {
        this._runGrid = _runGrid;
    }

    public Cell GetCell(int column, int row)
    {
        if (_cellsArray == null)
        {
            return null;
        }

        if ((row < 0 || row >= _gridWidth) || (column < 0 || column >= _gridHeight))
        {
            return null;
        }
        
        return _cellsArray[column, row];
    }

    public void CanEditCell(bool canEditCell)
    {
        foreach (Cell cell in _cellsArray)
        {
            cell.EditCell(canEditCell);
        }
    }
    
    private void CheckAllCells()
    {
        _cellCount = 0;
        foreach (Cell cell in _cellsArray)
        {
            cell.CheckCell();
        }

        foreach (Cell cell in _cellsArray)
        {
            bool previouslyAlive = cell.IsAlive();
            cell.UpdateCellLife();
            if (cell.IsAlive())
            {
                if(!previouslyAlive)
                {
                    _cellSpawned++;
                }
                _cellCount++;
            }
            else
            {
                if (previouslyAlive)
                {
                    _cellDied++;
                }
            }
        }
        
        _steps++;
        UpdateGameInformation();
    }

    private void UpdateGameInformation()
    {
        _gameModel.CellCount = _cellCount;
        _gameModel.CellSpawned = _cellSpawned;
        _gameModel.CellDied = _cellDied;
        _gameModel.Steps = _steps;
    }

    private void ClearGameInformation()
    {
        _gameModel.CellCount = 0;
        _gameModel.CellSpawned = 0;
        _gameModel.CellDied = 0;
        _gameModel.Steps = 0;

        _cellCount = 0;
        _cellSpawned = 0;
        _cellDied = 0;
        _steps = 0;
    }
    
    public void Update()
    {
        if (_runGrid)
        {
            _timer += Time.deltaTime;
            if (_timer >= _gameModel.speed)
            {
                _timer = 0;
                CheckAllCells();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P)) //pause
        {
            _runGrid = !_runGrid;
        }

        if (!_runGrid)
        {
            if (Input.GetKeyDown(KeyCode.Space)) //check next step
            {
                CheckAllCells();
            }
            if (Input.GetKeyDown(KeyCode.R)) //Randomize
            {
                RandomizeGridCellLife();
            }
            if (Input.GetKeyDown(KeyCode.C)) //clear
            {
                ClearGridCell();
            }
        }
    }

    //Destructor
    ~CellManager()
    {
        GameController.OnGridAddStep -= CheckAllCells;
        GameController.OnGridPausePlay -= RunGrid;
        GameController.OnGridRandomize -= RandomizeGridCellLife;
        GameController.OnGridClear -= ClearGridCell;
        GameController.OnGridDestroy -= DeleteEntireGrid;
    }

}
