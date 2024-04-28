using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSize : MonoBehaviour
{
    public float scalingFactor;
    private ParticleSystemRenderer psr;

    private void Start()
    {
        psr = GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = scalingFactor / Camera.main.orthographicSize * Vector3.one;
        // psr.minParticleSize = scalingFactor / Camera.main.orthographicSize;
    }
}
