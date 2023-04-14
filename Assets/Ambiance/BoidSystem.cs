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
    [SerializeField] private float coherenceFactor;
    [FormerlySerializedAs("maxSeparation")] [SerializeField] private float minDistance;
    [SerializeField] private float avoidanceFactor;
    [SerializeField] private float alignmentFactor;
    [SerializeField] private float visualRange;
    [SerializeField] private float speed;

    [Header("Other Settings")] 
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private float boidAmount;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 minMaxCoords;

    // Properties for behaviour settings
    public float CoherenceFactor => coherenceFactor;
    public float MinDistance => minDistance;
    public float AvoidanceFactor => avoidanceFactor;
    public float AlignmentFactor => alignmentFactor;
    public float VisualRange => visualRange;
    public float Speed => speed;
    
    // Properties for other settings
    public Vector3 MinMaxCoords => minMaxCoords;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boidAmount; i++)
        {
            var boid = Instantiate(boidPrefab, transform);
            boid.transform.position = spawnPosition;
        }
    }
}