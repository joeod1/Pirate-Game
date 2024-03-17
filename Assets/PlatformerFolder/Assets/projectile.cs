using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ 
    private CircleCollider2D circleCollider;

    [SerializeField] private float speed;
    private float direction;
    private bool hit;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        circleCollider.enabled = false;
        gameObject.SetActive(false);
    }
    public void SetSpeedAndDirection(float _speed, float _direction)
    {
        speed += _speed;
        direction = _direction;
    }

}
