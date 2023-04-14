using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Simulates a single boid for the boid algorithm. Uses settings from a BoidParent class with settings for
/// Coherence, Separation, Alignment, and Visual Range
/// </summary>
public class Boid : MonoBehaviour
{
    private BoidSystem _boidSystem;

    private SphereCollider _trigger;

    private Rigidbody _rb;
    private Vector3 _direction;

    private List<Transform> _neighbours = new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        _boidSystem = GetComponentInParent<BoidSystem>();

        // Set the trigger to the visual range of the boid.
        // TODO: Boids now detect other boids when their visual range touch another boids visual range.
        // This is not the intended behaviour? The visual range should maybe be a sphere around the boid?
        _trigger = GetComponent<SphereCollider>();
        _trigger.radius = _boidSystem.VisualRange / transform.localScale.x / 2;
        
        _rb = GetComponent<Rigidbody>();
        _direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        _direction = _direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Coherence();
        Separation();
        Alignment();
        StayInBounds();
        _trigger.radius = _boidSystem.VisualRange / transform.localScale.x / 2;
        _direction = _direction.normalized;
        _rb.velocity = _direction * _boidSystem.Speed;
        transform.forward = _direction;
    }

    /// <summary>
    /// Calculates the direction to the center of mass of the boids neighbours and adds it to the direction vector
    /// proportionally to the CoherenceFactor.
    /// </summary>
    private void Coherence()
    {
        var center = Vector3.zero;
        foreach (var boid in _neighbours)
        {
            center += boid.transform.position;
        }
        center /= _neighbours.Count;
        _direction += (center - transform.position) * _boidSystem.CoherenceFactor;
    }

    /// <summary>
    /// Calculates the direction away from the boids neighbours and adds it to the direction vector proportionally
    /// to the AvoidanceFactor.
    /// </summary>
    private void Separation()
    {
        var separation = Vector3.zero;
        foreach (var boid in _neighbours)
        {
            var distance = Vector3.Distance(transform.position, boid.transform.position);
            if (distance < _boidSystem.MinDistance)
            {
                separation += transform.position - boid.transform.position;
            }
        }
        _direction += separation * _boidSystem.AvoidanceFactor;
    }

    /// <summary>
    /// Calculates the direction of the average direction of the boids neighbours and adds it to the direction
    /// proportionally to the AlignmentFactor.
    /// </summary>
    private void Alignment()
    {
        var alignment = Vector3.zero;
        foreach (var boid in _neighbours)
        {
            alignment += boid.transform.forward;
        }
        alignment /= _neighbours.Count;
        _direction += alignment * _boidSystem.AlignmentFactor;
    }

    /// <summary>
    /// Makes sure the boid stays within the bounds of the BoidParent. If the boid is outside the bounds, it will
    /// turn around and head back in. Mainly used for debugging.
    /// </summary>
    private void StayInBounds()
    {
        if (transform.position.x < -_boidSystem.MinMaxCoords.x)
        {
            _direction.x = 1;
        }
        else if (transform.position.x > _boidSystem.MinMaxCoords.x)
        {
            _direction.x = -1;
        }
        
        if (transform.position.y < -_boidSystem.MinMaxCoords.y)
        {
            _direction.y = 1;
        }
        else if (transform.position.y > _boidSystem.MinMaxCoords.y)
        {
            _direction.y = -1;
        }
        
        if (transform.position.z < -_boidSystem.MinMaxCoords.z)
        {
            _direction.z = 1;
        }
        else if (transform.position.z > _boidSystem.MinMaxCoords.z)
        {
            _direction.z = -1;
        }
    }

    // Adds and removes boids from the neighbours list when they enter or leave the trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == transform.parent)
        {
            _neighbours.Add(other.transform);
        }
    }
    private  void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform.parent)
        {
            _neighbours.Remove(other.transform);
        }
    }
}
