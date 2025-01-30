using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    public Transform playerTransform;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 2, -10);

    private GameObject GoMinBound;
    private GameObject GoMaxBound;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void Start()
    {
        AttachToPlayer();
    }

    void LateUpdate()
    {
        if (playerTransform == null)
        {
            AttachToPlayer();
        }
        else
        {
            FollowPlayer();
        }
    }

    public void AttachToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;

            GoMinBound = GameObject.FindGameObjectWithTag("MinBound");
            GoMaxBound = GameObject.FindGameObjectWithTag("MaxBound");

            if (GoMinBound != null && GoMaxBound != null)
            {
                minBounds = GoMinBound.transform.position;
                maxBounds = GoMaxBound.transform.position;

                cameraHalfHeight = Camera.main.orthographicSize;
                cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

                minBounds.x += cameraHalfWidth;
                minBounds.y += cameraHalfHeight;
                maxBounds.x -= cameraHalfWidth;
                maxBounds.y -= cameraHalfHeight;
            }
        }
        else
        {
            playerTransform = null;
            transform.position = new Vector3(0, 0, -10);
        }
    }

    public void FollowPlayer()
    {
        Vector3 desiredPosition = playerTransform.position + offset;

        if (GoMinBound != null && GoMaxBound != null)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}