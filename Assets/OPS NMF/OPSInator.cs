using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages all OPSInatrons
///
/// <c>Author: Nils Michael Fitjar</c>
/// </summary>
public class OPSInator : MonoBehaviour
{
    private static OPSInator _instance;

    public static OPSInator Instance => _instance;

    private List<OPSInatron> _inatrons = new();

    public void Add(OPSInatron inatron)
    {
        _inatrons.Add(inatron);
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (_inatrons.All(inatron => inatron.IsValid()) && Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Valid!");
        }
        
    }
}
