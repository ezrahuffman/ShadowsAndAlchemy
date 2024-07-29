using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLight : MonoBehaviour
{
    [SerializeField] SphereCollider sphereCollider;

    private void OnTriggerEnter(Collider other)
    {
        MageController mageController = other.GetComponent<MageController>();
        if (mageController != null)
        {
            mageController.AddLight(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MageController mageController = other.GetComponent<MageController>();
        if (mageController != null)
        {
            mageController.RemoveLight(this);
        }
    }
}
