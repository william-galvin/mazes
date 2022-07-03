using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintMaker : MonoBehaviour
{
    public GameObject footprints;
    private Vector3 previousPoint;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        previousPoint = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 newPoint = player.transform.position;
        float distance = Vector3.Distance(newPoint, previousPoint);
        if (distance > 3)
        {
            GameObject newFP = Instantiate(footprints, player.transform.position, player.transform.rotation);
            newFP.transform.Rotate(new Vector3(90, 0, 0));
            newFP.transform.position = new Vector3(newFP.transform.position.x, player.transform.localPosition.y + 1, newFP.transform.position.z);
            newFP.transform.Translate(Vector3.down *1.5f);
            newFP.transform.Translate(Vector3.forward * .7f);
            previousPoint = newPoint;
        }
    }
}
