using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlampFollow : MonoBehaviour
{

    public Transform player;
    public float rotateDown;
    public float moveForward;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.Translate(Vector3.up * 1.8f);
        transform.Translate(Vector3.forward * moveForward);
        transform.Rotate(Vector3.down * rotateDown);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.Translate(Vector3.up * 1.8f);
        transform.Translate(Vector3.forward * moveForward);
        transform.Rotate(Vector3.left * rotateDown);
    }
}
