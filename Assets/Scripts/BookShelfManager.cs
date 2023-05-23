using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelfManager : MonoBehaviour
{
    [SerializeField] GameObject shelf;
    [SerializeField] int maxCovers;
    [SerializeField] float offset;

    void Start()
    {
        for(int i = 0; i < maxCovers;i++)
        {
            GameObject go = Instantiate(shelf, transform);
            go.transform.localPosition = new Vector3(shelf.transform.localPosition.x - (offset * (i+1)), shelf.transform.localPosition.y, shelf.transform.localPosition.z);

            FilesManager.Instance.columns.Add(go.transform);
        }
        FilesManager.Instance.columns.Reverse();
    }

}
