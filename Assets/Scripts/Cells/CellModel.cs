using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellModel
{
    public enum CellState
    {
        Alive,
        Dead,
        Selected,
        SelectedFromNeighbor
    }
    
    public struct InitialCellData
    {
        public int _row;
        public int _column;
        public bool _isAlive;
    }
    
    private Cell[] _cellNeighbors;
    private CellManager _cellManager;

    private CellState _cellState;
    public CellState CellCurrentState
    {
        get { return _cellState; }
        set { _cellState = value; }
    }
    
    private int _cellNeighborCount;
    public int CellNeighborCount
    {
        get { return _cellNeighborCount; }
    }
    
    private bool _isAlive;
    public bool IsAlive
    {
        set { _isAlive = value; }
        get { return _isAlive; }
    }
    
    public bool NextLifeState;

    private int _row;
    private int _column;

    public CellModel(InitialCellData initialCellData, CellManager _cellManager)
    {
        this._column = initialCellData._column;
        this._row = initialCellData._row;
        this._isAlive = initialCellData._isAlive;

        this._cellManager = _cellManager;
    }

    public void InitializeNeighbors()
    {
        if (_cellNeighbors == null)
        {
            _cellNeighbors = new Cell[8]
            {
                CellAbove,      //↑
                CellUpperRight, //↗
                CellRight,      //→
                CellLowerRight, //↘
                CellBelow,      //↓
                CellLowerLeft,  //↙
                CellLeft,       //←
                CellUpperLeft   //↖
            };
        }
    }

    #region Adjacent Cells
    public Cell CellAbove //↑
    {
        get { return _cellManager.GetCell(_column - 1, _row); }
    }
    
    public Cell CellUpperRight //↗
    {
        get { return _cellManager.GetCell(_column - 1, _row + 1); }
    }
    
    public Cell CellRight //→
    {
        get { return _cellManager.GetCell(_column, _row + 1); }
    }
    
    public Cell CellLowerRight //↘
    {
        get { return _cellManager.GetCell(_column + 1, _row + 1); }
    }

    public Cell CellBelow //↓
    {
        get { return _cellManager.GetCell(_column + 1, _row); }
    }
    
    public Cell CellLowerLeft //↙
    {
        get { return _cellManager.GetCell(_column + 1, _row - 1); }
    }

    public Cell CellLeft //←
    {
        get { return _cellManager.GetCell(_column, _row - 1); }
    }
    
    public Cell CellUpperLeft //↖
    {
        get { return _cellManager.GetCell(_column - 1, _row - 1); }
    }
    #endregion
    
    public void CheckCellState()
    {
       CheckNeighborCount();
       CheckCellRules();
    }
    private void CheckNeighborCount()
    {
        _cellNeighborCount = 0;
        for (int i = 0; i < _cellNeighbors.Length; i++)
        {
            if (_cellNeighbors[i] != null)
            {
                if (_cellNeighbors[i].IsAlive())
                {
                    _cellNeighborCount++;
                }
            }
        }
    }
    private void CheckCellRules()
    {
        if (IsAlive) {
            if (CellNeighborCount < 2) { //rule 1
                NextLifeState = false;
            }
            if (CellNeighborCount == 2 || CellNeighborCount == 3) { //rule 2
                NextLifeState = true;
            }
            if (CellNeighborCount > 3) { //rule 3
                NextLifeState = false;
            }    
        } else {
            if (CellNeighborCount == 3) { //rule 4
                NextLifeState = true;
            }
        }
    }

    public void RandomizeCellLife()
    {
        IsAlive = Random.Range(0, 100) > 50 ? true : false;
    }

    public void SetCellLife(bool currentLifeState)
    {
        IsAlive = currentLifeState;
    }
    
}
