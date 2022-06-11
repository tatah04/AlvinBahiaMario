using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
  
    [SerializeField] float spriteLength;
    [SerializeField] float startPosition;
    [SerializeField] float parallaxSpeed;
    [SerializeField] GameObject target;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform targetTransforms;
    //cam box

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetTransforms = GetComponent<Transform>();
        startPosition = targetTransforms.position.x;
        spriteLength = spriteRenderer.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {


        var distance = target.transform.position.x * parallaxSpeed;
        var displacement = target.transform.position.x * (1 - parallaxSpeed);

        targetTransforms.position = new Vector3(startPosition + distance, targetTransforms.position.y);

        if (displacement > (startPosition + spriteLength))
        {
            startPosition += spriteLength;
        }
        else if (displacement < (startPosition - spriteLength))
        {
            startPosition -= spriteLength;
        }


    }
}
