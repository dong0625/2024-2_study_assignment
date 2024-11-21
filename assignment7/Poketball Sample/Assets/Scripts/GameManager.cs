using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        PlayerBall = GameObject.Find("PlayerBall");
        CamObj = GameObject.Find("Main Camera");
        MyUIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ---------- 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ShootBallTo(hit.point);
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ----------
        const float a = BallRadius + RowSpacing, r3a = a * 1.732050807f;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                GameObject newball = Instantiate(BallPrefab, StartPosition, StartRotation);
                newball.transform.position += new Vector3(a * (2 * j - i), 0, r3a * (-i));
                newball.GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/ball_{i * (i + 1) / 2 + j + 1}");
                newball.name = $"{i * (i + 1) / 2 + j + 1}";
            }
        }
        // -------------------- 
    }
    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        if (PlayerBall != null)
        {
            Vector3 dir = (PlayerBall.transform.position - CamObj.transform.position).normalized;
            dir.y = 0;
            CamObj.transform.position += dir * CamSpeed * Time.deltaTime;
        }
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ----------
        Vector3 dir = targetPos - PlayerBall.transform.position;
        dir.y = 0;
        Vector3 power = dir.normalized * CalcPower(targetPos);
        PlayerBall.GetComponent<Rigidbody>().AddForce(power, ForceMode.Impulse);
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        MyUIManager.DisplayText($"{ballName} falls", 1);
        // -------------------- 
    }
}
