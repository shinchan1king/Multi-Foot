using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class menumanager : MonoBehaviour
{
    public static menumanager Instance;
    [SerializeField] menu[] menus;
    void Awake()
    {
        Instance=this;
    }
    public void OpenMenu(string menuName)
    {
        for(int i=0;i<menus.Length;i++)
        {
            if(menus[i].menuName==menuName)
            {
                menus[i].Open();
            }
            else if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }
    public void OpenMenu(menu menua)
    {
        for(int i=0;i<menus.Length;i++)
        {
           if(menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menua.Open();
    }
    public void CloseMenu(menu menua)
    {
        menua.Close();
    }
}
