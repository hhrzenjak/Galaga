using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript2 : MonoBehaviour
{
    public Vector2 StartPosition;
    
    private LineRenderer _lineRenderer;
    private int _layerMask;
    public Vector3 _startPosition;
    public Vector2 _endPosition;

    public float counter;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _layerMask = LayerMask.GetMask("Player");
        counter = 0;

    }

    private void Start()
    {
        _lineRenderer.SetPosition(0, StartPosition);
        _startPosition = StartPosition; // converting to Vector3 with z = 0
        counter = 0;
    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(_startPosition  + transform.position, _endPosition*counter, 2f, _layerMask);
        Debug.DrawRay(transform.position, _endPosition*counter, Color.green, 3f);

        if (hit && hit.transform.tag.Equals("Player"))
        {
            _lineRenderer.SetPosition(1, transform.InverseTransformPoint(hit.point));
            if (hit.transform.tag.Equals("Player"))
            {
                LifeManager lifeManager = hit.transform.GetComponent<LifeManager>();
                if (lifeManager)
                    lifeManager.Hit();
            }
        }
        else
        {
            if(counter <= 1)
                counter += 0.01f;
            
            _lineRenderer.SetPosition(1, _endPosition*counter);
        }
    }

    private void OnBecameInvisible()
    {
        counter = 0;
        _lineRenderer.SetPosition(1,  StartPosition);
    }

}