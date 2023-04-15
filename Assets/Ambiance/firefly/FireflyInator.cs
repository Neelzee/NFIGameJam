using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class for controlling firefly specific behaviour.
/// </summary>
public class FireflyInator : MonoBehaviour
{
    private Animator _animator;

    // Variables for controlling when the fade animation should play
    private float _fadeTimer;
    private bool _doneFading = true;
    [SerializeField] private float maxFadeTime = 5f;
    [SerializeField] private float minFadeTime = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _fadeTimer = Random.Range(minFadeTime, maxFadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (_doneFading)
        {
            if (_fadeTimer <= 0)
            {
                _animator.Play("firefly_fade");
                _doneFading = false;
            }
            else
            {
                _fadeTimer -= Time.deltaTime;
            }
        }
    }
    
    public void DoneFading()
    {
        _doneFading = true;
        _fadeTimer = Random.Range(minFadeTime, maxFadeTime);
    }
}
