using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cell Controller
public class Cell : MonoBehaviour
{
    private CellModel _cellModel;
    
    [SerializeField]
    private CellView _cellView;

    private bool _canEditCell;

    void Start()
    {
        UpdateCellView();
    }
    
    public void Initialize(CellModel.InitialCellData initialCellData, CellManager _cellManager)
    {
        _cellModel = new CellModel(initialCellData, _cellManager);
    }

    public void InitializeNeighbors()
    {
        if (_cellModel != null)
        {
            _cellModel.InitializeNeighbors();
        }
    }

    public void EditCell(bool _canEditCell)
    {
        this._canEditCell = _canEditCell;
    }

    public void CheckCell()
    {
        _cellModel.CheckCellState();
    }

    public void UpdateCellLife()
    {
        _cellModel.SetCellLife(_cellModel.NextLifeState);
        UpdateCellView();
    }
    
    public void RandomizeCellLife()
    {
        _cellModel.RandomizeCellLife();
        UpdateCellView();
    }

    public void SetCellLife(bool lifeState)
    {
        _cellModel.SetCellLife(lifeState);
        _cellModel.NextLifeState = lifeState;
        UpdateCellView();
    }
    
    private void UpdateCellView()
    {
        _cellView.UpdateCellView(IsAlive());
    }

    public bool IsAlive()
    {
        return _cellModel.IsAlive;
    }
 
    public void CellSelected()
    {
        _cellView.CellSelected();
    }

    public void CellNeighborSelected()
    {
        _cellView.CellNeighborSelected();
    }
    
    public void CellBackState()
    {
        UpdateCellView();
    }

    #region Mouse Events

    private void OnMouseDown()
    {
        if(!_canEditCell)
            return;
        
        _cellModel.IsAlive = !_cellModel.IsAlive;
        UpdateCellView();
    }
    private void OnMouseEnter()
    {
        if(!_canEditCell)
            return;
        
        _cellView.CellSelected();
        
        _cellModel.CellAbove?.CellNeighborSelected();
        _cellModel.CellUpperRight?.CellNeighborSelected();
        _cellModel.CellRight?.CellNeighborSelected();
        _cellModel.CellLowerRight?.CellNeighborSelected();
        _cellModel.CellBelow?.CellNeighborSelected();
        _cellModel.CellLowerLeft?.CellNeighborSelected();
        _cellModel.CellLeft?.CellNeighborSelected(); 
        _cellModel.CellUpperLeft?.CellNeighborSelected();
    }

    private void OnMouseExit()
    {
        if(!_canEditCell)
            return;
        
        UpdateCellView();
        
        _cellModel.CellAbove?.CellBackState();
        _cellModel.CellUpperRight?.CellBackState();
        _cellModel.CellRight?.CellBackState();
        _cellModel.CellLowerRight?.CellBackState();
        _cellModel.CellBelow?.CellBackState();
        _cellModel.CellLowerLeft?.CellBackState();
        _cellModel.CellLeft?.CellBackState(); 
        _cellModel.CellUpperLeft?.CellBackState();
    }

    #endregion
   
}
