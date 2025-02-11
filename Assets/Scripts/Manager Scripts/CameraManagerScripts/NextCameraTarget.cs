using UnityEngine;

public class NextCameraTargetTrigger : NewCameraTargetTrigger
{
    protected override void Start()
    {
        // No need to handle deadZone in this class, so we skip that
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // We can reuse the base logic but ignore deadZone activation
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            _virtualCamera.Follow = newtarget;
            // deadZone is not used here, as it's obsolete for this class
        }
    }
}