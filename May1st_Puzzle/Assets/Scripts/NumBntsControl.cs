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

    // 버튼 클릭 시 호출되는 함수
    public void OnButtonClick1()
    {
        //BackNumControl의 BackNum을 활성화
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 1;
    }

    public void OnButtonClick2()
    {
        //BackNumControl의 BackNum을 활성화
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 2;
    }

    public void OnButtonClick3()
    {
        //BackNumControl의 BackNum을 활성화
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 3;
    }

    public void OnButtonClick4()
    {
        //BackNumControl의 BackNum을 활성화
        backNumControl.BackNum.SetActive(true);
        buttonIndex = 4;
    }
}
