using TMPro;
using UnityEngine;

public class Guard : Enemy
{
    GameController gameController;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] float lookAngle;
    [SerializeField] float lookRange;
    [SerializeField] bool  debug;
    [SerializeField] GameObject visionIndicator;
    //[SerializeField] TMP_Text debugText;
    [SerializeField] LayerMask layerMask;
    MageController mageController;
    bool playerNearby;
   
    protected override void OverridableStart()
    {
        base.OverridableStart();

        gameController = GameController.instance;

        if (sphereCollider == null)
        {
            Debug.LogError("No attached sphere collider");
        }
        else
        {
            sphereCollider.radius = lookRange;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        MageController testController = other.gameObject.GetComponentInParent<MageController>();
        if (testController != null)
        {
            if (debug)
                Debug.Log("player entered trigger");
            playerNearby = true;
            mageController = testController;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    MageController testController = other.gameObject.GetComponentInParent<MageController>();
    //    if (testController != null)
    //    {
    //        if (debug)
    //            Debug.Log("player exited trigger");
    //        playerNearby = false;
    //        mageController = null;
    //    }
    //}

    private void Update()
    {
        //debugText.text = "";
        if (canSeePlayer())
        {
            CatchPlayer();
        }

        if (gameController.playerCaught)
        {
            return;
        }

        if (canHearPlayer())
        {
            // move toward the last heard spot
        }

        drawLineOfSight();
    }

    bool canSeePlayer()
    {
        
        if (!playerNearby)
        {
            //debugText.text = "not nearby";
            return false;
        }

        float distance = Vector3.Distance(transform.position, mageController.GetPlayerPosition());
        if (distance > lookRange)
        {
            return false;
        }
        
        if (mageController != null && mageController.IsVisable())
        {
            //if (debug)
            //{
            //    Debug.Log("guard check arc");
            //}
            Vector3 fromGuardToPlayer = mageController.GetPlayerPosition() - transform.position;
            fromGuardToPlayer = new Vector3(fromGuardToPlayer.x, 0, fromGuardToPlayer.z);
            Quaternion fromTo = Quaternion.FromToRotation(transform.forward, fromGuardToPlayer);
            float fromToAngleDeg = fromTo.eulerAngles.y;

            float dot = Vector3.Dot(fromGuardToPlayer.normalized, transform.forward);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;


            //if (debug)
            //    //debugText.text = angle.ToString();

            // Test the amount of rotation needed to see if it is within the line of sight of the player
            float half = (lookAngle / 2);
            if (angle < half || angle > 360 - half)
            {
                Debug.Log($"{gameObject.name}| fromToAngleDeg: {angle}; < {half} or > {360-half}  HALF = {half}");

                // Test that we can actually see the player
                RaycastHit hitInfo;
                Physics.Linecast(transform.position, mageController.GetPlayerPosition(), out hitInfo, layerMask);
                Debug.Log("guard raycast hit: " + hitInfo.collider.gameObject.name);
                //Debug.Break();
                return hitInfo.collider.gameObject.GetComponent<MageController>() != null;
                   
            }

        }
       

        // Not looking at the object, or not the player
        return false;
    }

    bool canHearPlayer ()
    {
        return false;
    }

    void drawLineOfSight()
    {
        if (canSeePlayer()) { return; }

        float halfAng = (lookAngle / 2);
        Vector3 forwardShift = (transform.forward * Mathf.Cos(halfAng * Mathf.Deg2Rad)) * lookRange;
        Vector3 rightShift = (transform.right * Mathf.Sin(halfAng * Mathf.Deg2Rad)) * lookRange;
        Vector3 leftShit = -rightShift;

        Vector3 rightEnd = forwardShift + rightShift + transform.position;
        Vector3 leftEnd = forwardShift + leftShit + transform.position;

        Debug.DrawLine(transform.position, rightEnd, Color.red);
        Debug.DrawLine(transform.position, leftEnd, Color.red);
    }

    void CatchPlayer()
    {
        Debug.Log($"Caught by {gameObject.name}");
        gameController.OnPlayerCaught();
    }

    protected override void OnDie()
    {
        base.OnDie();

        visionIndicator?.SetActive(false);
        GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;
    }
}
