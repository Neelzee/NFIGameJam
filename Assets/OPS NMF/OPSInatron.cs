using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class OPSInatron : MonoBehaviour
{
    [Header("Differences")]
    [Tooltip("What Color this object should be.")]
    [SerializeField] private Color color;
    [SerializeField] private bool checkColor;
    [Tooltip("The position this object should be at.")]
    [SerializeField] private Vector3 position;
    [SerializeField] private bool checkPosition;
    [Tooltip("How close the object needs too be to the position.")]
    [SerializeField] private float accuracy;

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = Color.red;
        OPSInator.Instance.Add(this);
    }

    public bool IsValid()
    {
        bool pos = true;

        bool col = true;
        
        if (checkPosition)
        {
            pos = Vector3.Distance(transform.position, position) <= accuracy;
        }

        if (checkColor)
        {
            col = color.Equals(_meshRenderer.material.color);
        }

        return col && pos;
    }
}
