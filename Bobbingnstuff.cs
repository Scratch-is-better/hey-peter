using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbingnstuff : MonoBehaviour
{
    public float amplitude = 0.5f, speed = 1f, phase, spinSpeed = 90f;
    Vector3 start;
    public 
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin((Time.time * speed) + phase) * amplitude;
        transform.position = start + new Vector3(0, y, 0);
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}
