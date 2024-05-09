using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlowScr : MonoBehaviour
{
    public Transform target;
    public float speed = 3;
    public BoxCollider2D bound;
    //public Vector2 offset; // 현재 카메라 위치를 수정
    //public float limitMinX, limitMaxX, limitMinY, limitMaxY;
    private Vector3 targetPos;
    private Vector3 minBound; // Map의 왼쪽 아래
    private Vector3 maxBound; // Map의 오른쪽 아래
    private Camera theCamera;
    // 맵의 최소, 최대 값을 받아 영역 지정
    float halfWidth, halfHeight;
    
    

    // Start is called before the first frame update
    void Start()
    {
        if(theCamera == null)
            theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        
        // 카메라 이동 영역 지정 1. 카메라 x축, y축 길이 지정
        /* Camera.main.aspect
        : 해상도 width/height를 계산한 비율을 나타내는 속성 */
        halfWidth = Camera.main.aspect * Camera.main.orthographicSize ;
        /* Camera.main.orthographicSize : 카메라의 Size */
        halfHeight = Camera.main.orthographicSize;
    }

    void Update() {
        if (target.gameObject != null) {
            targetPos.Set(target.transform.position.x, 
                          target.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position,
                targetPos, speed * Time.deltaTime);
            float clampX = Mathf.Clamp(transform.position.x,
                minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampY = Mathf.Clamp(transform.position.y,
                minBound.y + halfHeight, maxBound.y - halfHeight);
            transform.position = new Vector3(clampX, clampY,
                                         transform.position.z);
        }
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        //Vector3 desiredPosition = new Vector3(
        //    /* Mathf.Clamp(값, 최솟값, 최댓값)
        //     : 최솟값, 최댓값을 넘지 않게 지정 */
        //    Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth), // X
        //    Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
        //    -10);

        //transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);// Z
    
    
    }
}
