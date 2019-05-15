using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMoveScript : MonoBehaviour
{
    float speed = .5f;

    private void Start()
    {
        speed = speed * PlayerScript.myBulletDirection;
    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Translate(Vector2.right * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
