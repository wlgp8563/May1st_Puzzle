using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class NumBntsControl : MonoBehaviour
{
    public static NumBntsControl numBntsControl;
    public BackNumControl backNumControl;

    public int buttonIndex;

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnButtonClick1()
    {
        //BackNumControl�� BackNum�� Ȱ��ȭ
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 1;
    }

    public void OnButtonClick2()
    {
        //BackNumControl�� BackNum�� Ȱ��ȭ
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 2;
    }

    public void OnButtonClick3()
    {
        //BackNumControl�� BackNum�� Ȱ��ȭ
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 3;
    }

    public void OnButtonClick4()
    {
        //BackNumControl�� BackNum�� Ȱ��ȭ
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 4;
    }
}
