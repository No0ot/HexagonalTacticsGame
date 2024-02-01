using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLocation : MonoBehaviour
{
    public float radius;


    public void PlaceUnit(Unit unit)
    {
        unit.transform.position = ((Vector2)transform.position + Random.insideUnitCircle) * radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
