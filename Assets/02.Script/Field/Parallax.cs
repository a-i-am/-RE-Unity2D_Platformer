using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform; // 카메라의 Transform 컴포넌트
    
    private Vector3 cameraStartPosition; // 게임 시작했을 때 카메라 시작 위치
    private float distance; // cameraStartPosition부터 현재 카메라까지의 x 이동거리

    private Material[] materials; // 배경 스크롤을 위한 Material 배열 변수
    private float[] layerMoveSpeed;

    [SerializeField][Range(0.01f, 1.0f)]
    private float parallaxSpeed; // layerMoveSpeed에 곱해서 사용하는 배경 스크롤 이동 속도

    void Awake()
    {
        // 게임 시작 때 카메라 위치 저장(이동 거리 계산용) 
        cameraStartPosition = cameraTransform.position;

        // 배경 개수를 구하고, 배경 정보를 저장할 GameObject 배열 선언
        int backgroundCount = transform.childCount;
        GameObject[] backgrounds = new GameObject[backgroundCount];

        // 각 배경의 material과 이동 속도를 저장할 배열 선언
        materials = new Material[backgroundCount];
        layerMoveSpeed = new float[backgroundCount];
    
        // GetChild() 메소드를 호출해 자식으로 있는 배경 정보들을 불러온다
        for(int i = 0; i < backgroundCount; ++i)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        // 레이어(카메라와의 z 거리 기준)별로 이동 속도 설정
        CalculateMoveSpeedByLayer(backgrounds, backgroundCount);
    
    }

    // 근경 레이어가 더 빠르게 재생될 필요 없으면 CalculateMoveSpeedByLayer() 는 호출하지 않아도 된다.
    // 그 대신 layerMoveSpeed를 직접 입력해서 속도를 조절해야한다.
    void CalculateMoveSpeedByLayer(GameObject[] backgrounds, int count)
    {
        float farthestBackDistance = 0;
        // 카메라로부터 가장 멀리 떨어진 배경 레이어와의 z 거리 계산
        for(int i = 0; i < count; ++ i)
        {
            if ((backgrounds[i].transform.position.z - cameraTransform.position.z) > farthestBackDistance)
            {
                farthestBackDistance = backgrounds[i].transform.position.z - cameraTransform.position.z;
            }
        }
        // 카메라와의 z 위치 거리가 다른 배경 레이어별로 이동 속도 설정
        for (int i = 0; i < count; ++ i)
        {
            // 가장 멀리 떨어진 배경 레이어의 이동 속도 = 0
            layerMoveSpeed[i] = 1 - (backgrounds[i].transform.position.z - cameraTransform.position.z) / farthestBackDistance;
            // 이동속도 확인용(테스트 후 삭제)
            //Debug.Log($"{layerMoveSpeed[i]}, 실제 이동속도 = {layerMoveSpeed[i] * parallaxSpeed}");
        }
    }

    void LateUpdate()
    {
        // 카메라가 이동한 거리 = 카메라의 현재 위치 - 시작 위치
        distance = cameraTransform.position.x - cameraStartPosition.x;
        // 배경의 x 위치를 현재 카메라의 x 위치로 설정
        transform.position = new Vector3(cameraTransform.position.x, transform.position.y, 0);

        // 레이어별로 현재 배경이 출력되는 offset 설정
        for(int i = 0; i < materials.Length; ++ i)
        {
            float speed = layerMoveSpeed[i] * parallaxSpeed;
            materials[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
