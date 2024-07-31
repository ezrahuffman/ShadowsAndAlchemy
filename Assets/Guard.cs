using UnityEngine;

public class Guard : Enemy
{
    GameController gameController;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] float lookAngle;
    [SerializeField] float lookRange;
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
        mageController = other.gameObject.GetComponentInParent<MageController>();
        if (mageController != null)
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        mageController = other.gameObject.GetComponentInParent<MageController>();
        if (mageController != null)
        {
            playerNearby = false;
            mageController = null;
        }
    }

    private void Update()
    {
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
            return false;
        }

        if (mageController != null && mageController.IsVisable())
        {
            Vector3 fromGuardToPlayer = mageController.GetPlayerPosition() - transform.position;
            Quaternion fromTo = Quaternion.FromToRotation(transform.forward, fromGuardToPlayer);
            float fromToAngleDeg = fromTo.eulerAngles.y;


            // Test the amount of rotation needed to see if it is within the line of sight of the player
            float half = (lookAngle / 2);
            if (fromToAngleDeg < half || fromToAngleDeg > 360 - half)
            {
                //Debug.Log($"fromToAngleDeg: {fromToAngleDeg}; < {half} or > {360-half}  HALF = {half}");
                return true;
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

        //float halfAng = (lookAngle / 2);
        //Vector3 forwardShift = (transform.forward * Mathf.Cos(halfAng * Mathf.Deg2Rad)) * lookRange;
        //Vector3 rightShift = (transform.right * Mathf.Sin(halfAng * Mathf.Deg2Rad)) * lookRange;
        //Vector3 leftShit = -rightShift;

        //Vector3 rightEnd = forwardShift + rightShift + transform.position;
        //Vector3 leftEnd = forwardShift + leftShit + transform.position;

        //Debug.DrawLine(transform.position, rightEnd, Color.red);
        //Debug.DrawLine(transform.position, leftEnd, Color.red);
    }

    void CatchPlayer()
    {
        gameController.OnPlayerCaught();
    }
}
