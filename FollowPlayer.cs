using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public GameObject player;
    private Transform target;
    public float upOffset;

    public void Start()
    {
         target = player.transform;
    }
    
    public Vector3 offsetPosition;

    public Space offsetPositionSpace = Space.Self;

    public bool lookAt = true;

    private void LateUpdate()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = target.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = target.position + offsetPosition;
        }

        // compute rotation
        if (lookAt)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.up * upOffset);
        }
        else
        {
            transform.rotation = target.rotation;
        }
    }
}
