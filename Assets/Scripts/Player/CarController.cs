using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10f;    
    public float laneWidth = 3.4f;    
    private int currentLane = -1;     // (-1 = left lane, 1 = right lane on road)

    public float smoothTime;   
    private Vector3 velocity = Vector3.zero; 

    private float targetYRotation = 0f; 
    private float currentYRotation = 0f; 
    private bool isChangingLane = false; 
    GameController gameController;
    private void Start()
    {
        
        transform.position = new Vector3(-laneWidth, transform.position.y, transform.position.z);
        targetYRotation = 0; 
        gameController = gameObject.GetComponent<GameController>();
    }

    private void Update()
    {
        MoveCar();
        HandleSwipe();
        float targetXPosition = currentLane * laneWidth;
        Vector3 targetPosition = new Vector3(targetXPosition, transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        UpdateRotation();
        //Debug.Log(currentLane);
    }

    private void MoveCar()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void HandleSwipe()
    {
        if (Input.touchCount > 0 && !gameController.GameEnded)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.x > 0 && currentLane < 1)
                {
                    ChangeLane(1);
                    AudioManager.Instance.PlaySFX("LaneChange");
                }
                else if (touch.deltaPosition.x < 0 && currentLane > -1)
                {
                    ChangeLane(-1);
                    AudioManager.Instance.PlaySFX("LaneChange");
                }
            }
        }
    }

    private void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;

        if (targetLane <= 1 && targetLane >= -1)
        {
            currentLane = direction;
            isChangingLane = true; 
            targetYRotation = direction == 1 ? 15f : -15f; // Cars rotation between lanes
        }
    }

    private void UpdateRotation()
    {
        if (isChangingLane)
        {
            currentYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, Time.deltaTime / .2f);
            transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

            if (Mathf.Abs(transform.position.x - (currentLane * laneWidth)) < 1f)
            {
                isChangingLane = false; 
                targetYRotation = 0f; 
            }
        }
        else
        {
            currentYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, Time.deltaTime / .05f);
            transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
        }
    }
}