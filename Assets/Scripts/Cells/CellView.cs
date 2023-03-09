using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
    [SerializeField] 
    private Material _deadMaterial;

    [SerializeField] 
    private Material _aliveMaterial;

    [SerializeField] 
    private Material _selectedMaterial;
    
    [SerializeField] 
    private Material _neighborSelectedMaterial;

    [SerializeField]
    private Renderer _cellMeshRenderer;

    public void UpdateCellView(bool _isAlive)
    {
        _cellMeshRenderer.material = _isAlive ? _aliveMaterial : _deadMaterial;
    }

    public void CellSelected()
    {
        _cellMeshRenderer.material = _selectedMaterial;
    }

    public void CellNeighborSelected()
    {
        _cellMeshRenderer.material = _neighborSelectedMaterial;
    }
}
