using UnityEngine;

public class SeatScript : MonoBehaviour
{
    public Transform sitTarget;
    public bool IsReserved = false;
    public bool IsOccupied = false;

    // Search for a free seat and return true if the seat is free, otherwise return false
    public bool IsFree()
    {
        if (IsReserved == false && IsOccupied == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Try to reserve the seat and return true if the seat is reserved, otherwise return false
    public bool TryReserve()
    {
        if (IsFree() == false)
        {
            return false;
        }

        IsReserved = true;
        return true;
    }

    // Occupy the seat and set IsReserved to false and IsOccupied to true
    // Later add the customer order function to or from other scripts
    public void Occupy()
    {
        IsReserved = false;
        IsOccupied = true;
    }

    // Release the seat and set IsReserved to false and IsOccupied to false
    // Later add the customer leaving the seats function to or from other scripts
    public void Release()
    {
        IsReserved = false;
        IsOccupied = false;
    }
}
