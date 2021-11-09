using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private List<GameObject> _list;

    private void Update()
    {
        if (_list.Count == 0) return;
        print(_list[0].name);
    }
}
