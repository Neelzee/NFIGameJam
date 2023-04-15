using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Parent class for a boid system. The boid system contains the settings for a boid system shared between
/// all boids in the system. 
/// </summary>
public class BoidSystem : MonoBehaviour
{
    [Header("Behaviour Settings")]
    [SerializeField][Range(0f, 2f)] private float coherenceFactor;
    [SerializeField] [Range(0f, 10f)] private float minDistance;
    [SerializeField] [Range(0f, 2f)] private float avoidanceFactor;
    [SerializeField] [Range(0f, 2f)] private float alignmentFactor;
    [SerializeField] [Range(0f, 10f)] private float visualRange;
    [SerializeField] [Range(0f, 10f)] private float maxSpeed;

    [Header("Other Settings")] 
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private float boidAmount;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 bounds;

    // Properties for behaviour settings
    public float CoherenceFactor => coherenceFactor;
    public float MinDistance => minDistance;
    public float AvoidanceFactor => avoidanceFactor;
    public float AlignmentFactor => alignmentFactor;
    public float VisualRange => visualRange;
    public float MaxSpeed => maxSpeed;
    
    // Properties for other settings
    public Vector3 Bounds => bounds;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boidAmount; i++)
        {
            var boid = Instantiate(boidPrefab, transform);
            boid.transform.position = spawnPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}