using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BackNumControl : MonoBehaviour
{
    public static BackNumControl backNumControl;
    public NumBntsControl numBntsControl;

    public List<GameObject> allNumbersList = new List<GameObject>();  // 처음 시작 시 뒷배경 숫자 다 받아오기
    public GameObject baseNumber;
    public GameObject BackNum;

    public int baseIndex;
    int newButtonUINumber;

    public int buttonUINumber1 = 0;
    public int buttonUINumber2 = 0;
    public int buttonUINumber3 = 0;
    public int buttonUINumber4 = 0;

    public TMP_Text buttonText1;
    public TMP_Text buttonText2;
    public TMP_Text buttonText3;
    public TMP_Text buttonText4;
    private void Start()
    {
        // Grid로 나열된 14개의 이미지를 리스트로 받아 초기에 9번째가 저장됨
        for (int i = 0; i < 14; i++)
        {
            allNumbersList.Add(GameObject.Find("PMNum").transform.GetChild(i).gameObject);
        }
        ResetBaseIndex();

        BackNum = GameObject.Find("Canvas").transform.Find("PMNum").gameObject;
        BackNum.SetActive(false);

        buttonText1.text = buttonUINumber1.ToString();
        buttonText2.text = buttonUINumber2.ToString();
        buttonText3.text = buttonUINumber3.ToString();
        buttonText4.text = buttonUINumber4.ToString();
    }
     public void ResetBaseIndex()
    {
        baseNumber = allNumbersList[9];
        baseIndex = allNumbersList.IndexOf(baseNumber);
    }

    public void IncreaseButtonNumber(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 1:
                if (buttonUINumber1 < 9)
                    buttonUINumber1++;
                else if (buttonUINumber1 == 9)
                    buttonUINumber1 = 9;
                buttonText1.text = buttonUINumber1.ToString();
                break;
            case 2:
                if (buttonUINumber2 < 9)
                    buttonUINumber2++;
                else if (buttonUINumber2 == 9)
                    buttonUINumber2 = 9;
                buttonText2.text = buttonUINumber2.ToString();
                break;
            case 3:
                if (buttonUINumber3 < 9)
                    buttonUINumber3++;
                else if (buttonUINumber3 == 9)
                    buttonUINumber3 = 9;
                buttonText3.text = buttonUINumber3.ToString();
                break;
            case 4:
                if (buttonUINumber4 < 9)
                    buttonUINumber4++;
                else if (buttonUINumber4 == 9)
                    buttonUINumber4 = 9;
                buttonText4.text = buttonUINumber4.ToString();
                break;
            default:
                break;
        }
    }

    public void DecreaseButtonNumber(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 1:
                if (buttonUINumber1 > 0)
                    buttonUINumber1--;
                else if (buttonUINumber1 == 0)
                    buttonUINumber1 = 0;
                buttonText1.text = buttonUINumber1.ToString();
                break;
            case 2:
                if (buttonUINumber2 > 0)
                    buttonUINumber2--;
                else if (buttonUINumber2 == 0)
                    buttonUINumber2 = 0;
                buttonText2.text = buttonUINumber2.ToString();
                break;
            case 3:
                if (buttonUINumber3 > 0)
                    buttonUINumber3--;
                else if (buttonUINumber3 == 0)
                    buttonUINumber3 = 0;
                buttonText3.text = buttonUINumber3.ToString();
                break;
            case 4:
                if (buttonUINumber4 > 0)
                    buttonUINumber4--;
                else if (buttonUINumber4 == 0)
                    buttonUINumber4 = 0;
                buttonText4.text = buttonUINumber4.ToString();
                break;
            default:
                break;
        }
    }

    public void CpmpareBackNumIndex(NumCount nextnumber)
    {
        int newIndex = allNumbersList.IndexOf(nextnumber.gameObject);
        baseIndex = allNumbersList.IndexOf(baseNumber);

        if (newIndex < baseIndex)
        {
            switch (numBntsControl.buttonIndex)
            {
                case 1:
                    baseNumber = nextnumber.gameObject;
                    IncreaseButtonNumber(1);
                    break;
                case 2:
                    baseNumber = nextnumber.gameObject;
                    IncreaseButtonNumber(2);
                    break;
                case 3:
                    baseNumber = nextnumber.gameObject;
                    IncreaseButtonNumber(3);
                    break;
                case 4:
                    baseNumber = nextnumber.gameObject;
                    IncreaseButtonNumber(4);
                    break;
                default:
                    break;
            }
        }
        else
        {
            baseIndex = newIndex;
            switch (numBntsControl.buttonIndex)
            {
                case 1:
                    baseNumber = nextnumber.gameObject;
                    DecreaseButtonNumber(1);
                    break;
                case 2:
                    baseNumber = nextnumber.gameObject;
                    DecreaseButtonNumber(2);
                    break;
                case 3:
                    baseNumber = nextnumber.gameObject;
                    DecreaseButtonNumber(3);
                    break;
                case 4:
                    baseNumber = nextnumber.gameObject;
                    DecreaseButtonNumber(4);
                    break;
                default:
                    break;
            }
        }
    }
}
