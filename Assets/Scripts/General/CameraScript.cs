using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] Vector3 targetOffset;
    [SerializeField] float cameraSmoothing;
    [SerializeField] Vector2 minbounds;
    [SerializeField] Vector2 maxbounds;
    Vector3 targetPosition;

    //cam box
    [SerializeField] private Camera cam;
    [SerializeField] private BoxCollider2D camBox;
    private float sizeX, sizeY, ratio;
    void Start()
    {
        //Cambox Compute based on Device screen
        sizeY = cam.orthographicSize * 2;
        ratio = (float)Screen.width / (float)Screen.height;
        sizeX = sizeY * ratio;
        camBox.size = new Vector2(sizeX, sizeY);
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = cameraTarget.position + targetOffset;
        Vector3 LerpedPos = Vector3.Lerp(transform.position, targetPosition, cameraSmoothing * Time.fixedDeltaTime);
        Vector3 boundedAndSmoothedPos = new Vector3(
            Mathf.Clamp(LerpedPos.x, minbounds.x, maxbounds.x),
            Mathf.Clamp(LerpedPos.y, minbounds.y, maxbounds.y),
            LerpedPos.z
        );
        transform.position = boundedAndSmoothedPos;
    }
}
