using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleFollowingPoints : MonoBehaviour
{
    [SerializeField]
    private float angle = 30f;
    [SerializeField]
    private float distance = 5f;
    private int startCount = 5;

    private int pointCount = 0;


    private bool isMovingInCircle = false;

    [HideInInspector]
    public bool returnedToPosition = true;
    
    private float movingInter;
    private float movingStartTime;
    private float movingSpeed;

    [SerializeField]
    private float circleMovingSpeed = 10f;
    [SerializeField]
    private float circleMovingAccel = 30f;


    private List<Transform> followingPoints;
    private Queue<Transform> disabledPoints;

    private void Awake()
    {
        followingPoints = new List<Transform>();
        disabledPoints = new Queue<Transform>();

        for(int i = 0; i < startCount; i++)
        {
            Transform newPoint = CreatePoint();
            newPoint.gameObject.SetActive(false);
            disabledPoints.Enqueue(newPoint);
        }
    }

    private void FixedUpdate()
    {
        if (isMovingInCircle)
        {
            float time = Time.time - movingStartTime;
            if(time > movingInter)
            {
                isMovingInCircle = false;
            }

            movingSpeed = Mathf.MoveTowards(movingSpeed, circleMovingSpeed, circleMovingAccel * Time.fixedDeltaTime);
            float startAngle = (followingPoints.Count - 1) * angle / 2;
            for (int i = 0; i < followingPoints.Count; i++) 
            {
                float angle = startAngle - 360 / followingPoints.Count * i;
                angle += circleMovingSpeed * time;
                UpdatePosition(followingPoints[i], angle, movingSpeed * (6f + (i + 1) * 0.4f));
            }

        }else if (!returnedToPosition)
        {
            bool isInPosition = true;
            float startAngle = (followingPoints.Count - 1) * angle / 2;
            for (int i = 0; i < followingPoints.Count; i++)
            {
                float leftAngle = Mathf.Abs(Vector2.SignedAngle(Vector2.left, followingPoints[i].position - transform.position) - startAngle);
                if (leftAngle < 1)
                {
                    SetPointPostion(followingPoints[i], startAngle);
                }
                else
                {
                    UpdatePosition(followingPoints[i], startAngle, Mathf.Min(1, leftAngle / 10 + 0.2f) * movingSpeed * 3);
                    isInPosition = false;
                }
                startAngle -= angle;
            }
            returnedToPosition = isInPosition;
        }
        else
        {
            UpdatePointPosition();
        }
    }

    private void UpdatePosition(Transform point, float targetAngle, float speed)
    {
        Vector2 direction = point.position - transform.position;
        float currentAngle = Vector2.SignedAngle(Vector2.left, direction);
        while (targetAngle > currentAngle)
        {
            targetAngle -= 360f;
        }
        float nextAngle = Mathf.MoveTowards(currentAngle, targetAngle, speed * Time.fixedDeltaTime);
        SetPointPostion(point, nextAngle);
    }

    public Transform AddPoint()
    {
        Transform newPoint;
        if(disabledPoints.Count != 0)
        {
            newPoint = disabledPoints.Dequeue();
            newPoint.gameObject.SetActive(true);
        }
        else
        {
            newPoint = CreatePoint();
        }
        
        followingPoints.Add(newPoint);

        UpdatePointPosition();

        return newPoint;
    }

    public void DeletePoint(Transform point)
    {
        followingPoints.Remove(point);
        UpdatePointPosition();
        point.gameObject.SetActive(false);
        disabledPoints.Enqueue(point);
    }

    private void UpdatePointPosition()
    {
        float startAngle = (followingPoints.Count - 1) * angle / 2;
        for (int i = 0; i < followingPoints.Count; i++)
        {
            SetPointPostion(followingPoints[i], startAngle);
            startAngle -= angle;
        }
    }

    private void SetPointPostion(Transform point, float angle)
    {
        //Debug.Log($"{point.name} set to angle {angle}");
        float radius = angle * Mathf.Deg2Rad;
        Vector2 vec = Vector2.left;
        float x = (vec.x * Mathf.Cos(radius) - vec.y * Mathf.Sin(radius)) * distance;
        float y = (vec.x * Mathf.Sin(radius) + vec.y * Mathf.Cos(radius)) * distance;
        point.localPosition = new Vector3(x, y, 0);
    }

    private Transform CreatePoint()
    {
        pointCount++;
        GameObject obj = new GameObject();
        obj.transform.parent = transform;
        obj.name = $"{pointCount}";
        return obj.transform;
    }

    public void MoveInCircle(float time)
    {
        movingStartTime = Time.time;
        movingInter = time;
        movingSpeed = 0;
        isMovingInCircle = true;
        returnedToPosition = false;
    }
}
