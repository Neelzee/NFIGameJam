using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Simulates a single boid for the boid algorithm. Uses settings from a BoidParent class with settings for
/// Coherence, Separation, Alignment, and Visual Range
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour
{
    private BoidSystem _boidSystem;

    private SphereCollider _trigger;

    private Rigidbody _rb;
    private Vector3 _direction;

    private List<Rigidbody> _neighbours = new List<Rigidbody>();
    
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
        LimitSpeed();
        StayInBounds();
        _trigger.radius = _boidSystem.VisualRange / transform.localScale.x / 2;
        _rb.velocity = _direction;
    }

    /// <summary>
    /// Calculates the direction to the center of mass of the boids neighbours and adds it to the direction vector
    /// proportionally to the CoherenceFactor.
    /// </summary>
    private void Coherence()
    {
        if (_neighbours.Count == 0) return;
        
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
            if (distance < _boidSystem.MinDistance && distance > 0)
            {
                separation += (transform.position - boid.transform.position).normalized / distance;
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
        if (_neighbours.Count == 0) return;
        
        var alignment = Vector3.zero;
        
        foreach (var boid in _neighbours)
        {
            // TODO: Slow
            alignment += boid.velocity;
        }
        alignment /= _neighbours.Count;
        _direction += alignment * _boidSystem.AlignmentFactor;
    }

    /// <summary>
    /// Makes sure the boid stays within the bounds of the BoidParent. If the boid is outside the bounds, it will
    /// turn around and head back in.
    /// </summary>
    private void StayInBounds()
    {
        var bounds = _boidSystem.Bounds;
        var center = _boidSystem.Center;
        var position = transform.position;
        var newDirection = Vector3.zero;
        
        if (position.x < center.x - bounds.x / 2)
        {
            newDirection.x += Mathf.Abs(_direction.x);
        }
        else if (position.x > center.x + bounds.x / 2)
        {
            newDirection.x += -Mathf.Abs(_direction.x);
        }
        if (position.y < center.y - bounds.y / 2)
        {
            newDirection.y += Mathf.Abs(_direction.y);
        }
        else if (position.y > center.y + bounds.y / 2)
        {
            newDirection.y += -Mathf.Abs(_direction.y);
        }
        if (position.z < center.z - bounds.z / 2)
        {
            newDirection.z += Mathf.Abs(_direction.z);
        }
        else if (position.z > center.z + bounds.z / 2)
        {
            newDirection.z += -Mathf.Abs(_direction.z);
        }
        _direction += newDirection * _boidSystem.StayInBoundsFactor;
    }

    private void LimitSpeed()
    {
        var speed = _direction.magnitude;
        if (speed > _boidSystem.MaxSpeed)
        {
            _direction = _direction.normalized * _boidSystem.MaxSpeed;
        }
    }

    // Adds and removes boids from the neighbours list when they enter or leave the trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == transform.parent)
        {
            _neighbours.Add(other.GetComponent<Rigidbody>());
        }
    }
    private  void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform.parent)
        {
            _neighbours.Remove(other.GetComponent<Rigidbody>());
        }
    }
}
