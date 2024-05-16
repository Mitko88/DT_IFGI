using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    public void OnButtonLoadData()
    {
        DbDataReadPolygon[] polygons = FindObjectsOfType<DbDataReadPolygon>();

        foreach (var polygon in polygons)
        {
            polygon.LoadDataFromDb();
        }

        DbDataReadPoint[] points = FindObjectsOfType<DbDataReadPoint>();

        foreach (var point in points)
        {
            point.LoadDataFromDb();
        }
    }
}
