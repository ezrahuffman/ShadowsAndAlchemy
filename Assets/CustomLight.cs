using UnityEngine;

public class CustomLight : MonoBehaviour
{
    [SerializeField] bool excludeBehind;
    [SerializeField] bool debug;
    [SerializeField] Transform lightTrans;

    Transform playerTrans;
    bool added;

    private void OnTriggerEnter(Collider other)
    {

        MageController mageController = other.GetComponent<MageController>();
        if (mageController != null)
        {
            playerTrans = other.transform;
            if (excludeBehind)
            {
                return;
            }
            mageController.AddLight(this);
            added = true;
        }
    }

    private void Update()
    {
        if (playerTrans != null && excludeBehind)
        {
            Vector3 fromLightToPlayer = playerTrans.transform.position - lightTrans.transform.position;
            float dot = Vector3.Dot(fromLightToPlayer, transform.forward);
            int opt = 1;
            bool prevAdded = added;
            if (dot >= 0 && !prevAdded)
            {
                playerTrans.GetComponent<MageController>().AddLight(this);
                added = true;
                opt = 2;
            }
            else if (dot < 0 && prevAdded) { }
            {
                playerTrans.GetComponent<MageController>().RemoveLight(this);
                added = false;
                opt = 3;
            }
            if (debug)
            {
                Debug.Log($"Dot: {dot} ({dot >= 0}), prevAdded: {prevAdded}, {dot >= 0 && !prevAdded}, option: {opt} (1 nothing, 2 add, 3 remove)");
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        MageController mageController = other.GetComponent<MageController>();
        if (mageController != null)
        {
            playerTrans = null;
            mageController.RemoveLight(this);
            added = false; 
        }
    }
}
