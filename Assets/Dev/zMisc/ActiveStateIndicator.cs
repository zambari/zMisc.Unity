using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ActiveStateIndicator : MonoBehaviour {
    //Θ ø•◈  

    const char activeChar = '☀';
    const char inactiveChar = '☼';
    // Use this for initialization

    private void OnEnable()
    {
        print("gameObject.name[0]"+ gameObject.name[0]);
        if (gameObject.name[0]!=activeChar)
        {
            print("not active char");
            if (gameObject.name[0] == inactiveChar)
            {
                print("was inactive, changing");
                gameObject.name = activeChar + gameObject.name.Substring(1);
            }
            else
                gameObject.name = activeChar + gameObject.name;
        }
    }


    private void OnDisable()
    {
        if (!gameObject.activeInHierarchy)
        {
            print("gameObject.name[0]" + gameObject.name[0]);
            //print("gameObject.name[0]");
            if (gameObject.name[0] != inactiveChar)
            {
                print("not active char");
                if (gameObject.name[0] == activeChar)
                {
                    print("was active, changing");
                    gameObject.name = inactiveChar + gameObject.name.Substring(1);
                }
                else
                { 
                   gameObject.name = inactiveChar + gameObject.name;
             }
            }
        }
    }



}
