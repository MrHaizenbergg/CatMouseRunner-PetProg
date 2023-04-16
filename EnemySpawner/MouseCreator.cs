using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCreator : Singleton<MouseCreator>
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    private MouseAbstractFactory _mouseFactory;


    public MouseController CreateMouseStandard()
    {
        _mouseFactory = new MouseStandartFactory(point1);
        GameObject mouseGo= _mouseFactory.CreateMouseStandard();
        var mouseGoo= mouseGo.GetComponentInChildren<MouseController>();
        Debug.Log(mouseGoo);
        return mouseGoo;
    }

    public void CreateMouseModify()
    {
        _mouseFactory = new MouseStandartFactory(point2);
        _mouseFactory.CreateMouseModify();
    }
}