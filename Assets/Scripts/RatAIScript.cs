using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

using System;

public class RatAIScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public float randomMoveMax;

    public float decisionTimer;
    public float waitTime;

    public float eatingTimer;
    public float eatTime;

    public Vector3 destination;

    public LayerMask foodLayer;

    public GameObject foodTarget;

    public float smellRadius;
    public float eatDistance;
    public float tooFarDistance;

    public event Action<GameObject> OnMouseEating;
    public event Action<bool> OnMouseStopEating;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        OnMouseStopEating += OnMouseFinishEating;
    }

    private void OnDestroy()
    {
        OnMouseStopEating -= OnMouseFinishEating;

    }

    private void OnMouseFinishEating(bool value)
    {
        agent.isStopped = false;
    }

    private void Update()
    {
        RotateToMoveDirection();

        if (foodTarget != null)
        {
            TryGoToEatFood();
            return;
        }

        OnMouseStopEating?.Invoke(false);

        decisionTimer += Time.deltaTime;
        if (decisionTimer > waitTime)
        {
            DecideNextMove();
            decisionTimer = 0;
        }
    }

    public void DecideNextMove()
    {
        if (UnityEngine.Random.value <= 0.4)
        {
            MoveRandomly();
        }
        else
        {
            LookForFood(out foodTarget);
        } 
    }

    public bool LookForFood(out GameObject foodChosen)
    {
        List<Collider> foodsInRange = new List<Collider>(Physics.OverlapSphere(transform.position, smellRadius, foodLayer));

        float closestDistance = float.MaxValue;
        GameObject closestFood = null;

        if (foodsInRange.Count <= 0)
        {
            foodChosen = null;
            return false;
        }

        foreach (Collider food in foodsInRange)
        {
            if (food.gameObject.tag == "Platter")
            {
                continue;
            }

            float distanceToFood = Vector3.Distance(food.gameObject.transform.position, transform.position);

            if (distanceToFood < closestDistance)
            {
                closestDistance = distanceToFood;
                closestFood = food.gameObject;
            }

        }

        foodChosen = closestFood;
        return true;
    }

    public void MoveRandomly()
    {

        float randomX = UnityEngine.Random.Range(-randomMoveMax, randomMoveMax);
        float randomZ = UnityEngine.Random.Range(-randomMoveMax, randomMoveMax);

        
        Vector3 newDestination = transform.position + new Vector3 (randomX, 0, randomZ);

        if (!CheckIfValidPosition(newDestination))
        {
            return;
        }

        destination = newDestination;

        agent.SetDestination(destination);
    }

    public void TryGoToEatFood()
    {
        if (foodTarget == null)
        {
            agent.isStopped = false;

            eatingTimer = 0;
            OnMouseStopEating?.Invoke(false);
            print("stop eating");
            return;
        }

        float distanceToFood = Vector3.Distance(transform.position, foodTarget.transform.position);

        if (PlayerHandScript.instance.currentFoodHeldObj == foodTarget || distanceToFood >= tooFarDistance)
        {
            agent.isStopped = false;

            foodTarget = null;
            eatingTimer = 0;
            OnMouseStopEating?.Invoke(false);
            print("stop eating");

            return;
        }

        if (distanceToFood >= eatDistance)
        {
            agent.isStopped = false;


            eatingTimer = 0;
            OnMouseStopEating?.Invoke(false);

            agent.SetDestination(foodTarget.transform.position);
            print("stop eating");

            return;
        }

        OnMouseEating?.Invoke(foodTarget);
        agent.isStopped = true;

        eatingTimer += Time.deltaTime;
        if (eatingTimer >= eatTime)
        {
            OnMouseStopEating?.Invoke(true);

            Destroy(foodTarget);
            foodTarget = null;
            eatingTimer = 0;
        }

    }

    public bool CheckIfValidPosition(Vector3 point)
    {
        float tooClose = 0.5f;
        float distance = Vector3.Distance(point, transform.position);
        if (distance < tooClose)
        {
            return false;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RotateToMoveDirection()
    {
        if (agent.velocity.magnitude <= 0.01f)
        {
            return;
        }

        Vector3 angleVector = agent.velocity;
        angleVector.y = 0;

        // z because its 3d
        float angle = Mathf.Atan2(angleVector.x, angleVector.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle + 90, 0);
    }

    //public void SnapBodyToFood()
    //{
    //    //if (foodTarget == null)
    //    //{
    //    //    return;
    //    //}

    //    //Vector3 angleVector = (transform.position - foodTarget.transform.position).normalized;
    //    //float angle = Mathf.Atan2(angleVector.x, angleVector.z) * Mathf.Rad2Deg;

    //    //transform.rotation = Quaternion.Euler(0, angle, 0);

    //}
}
