using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public static SeatManager Instance;
    public SeatScript[] seats;

    // Singleton pattern to ensure only one instance of SeatManager exists
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Get the nearest free seat to the customer position and set it as reserved
    // Use this function to set the AI movement script
    public SeatScript GetNearestFreeSeat(Vector3 customerPosition)
    {
        SeatScript nearestSeat = null;
        float nearestDistance = 5000f;

        for (int i = 0; i < seats.Length; i++)
        {
            SeatScript currentSeat = seats[i];

            if (currentSeat.IsFree() == true)
            {
                float distance = Vector3.Distance(customerPosition, currentSeat.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestSeat = currentSeat;
                }
            }
        }

        if (nearestSeat != null)
        {
            bool seatReserved = nearestSeat.TryReserve();

            if (seatReserved == true)
            {
                return nearestSeat;
            }
        }

        return null;
    }
}
