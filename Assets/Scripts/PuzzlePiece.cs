using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{
    private TMP_Text number;
    private Image imgBG;
    private bool isLockedIn = false; //
    // Start is called before the first frame update
    void Start()
    {
        number = transform.GetChild(0).GetComponent<TMP_Text>();
        imgBG = GetComponent<Image>();
    }

    public void AddButton()
    {

        if(!isLockedIn)
        {
            ChangeAppearance(Color.black);
            int num = int.Parse(number.text);
            num++;
            if (num > 9) num = 0;
            number.text = num.ToString();
        }
        
    }

    public void SubtractButton()
    {
        if (!isLockedIn)
        {
            ChangeAppearance(Color.black);
            int num = int.Parse(number.text);
            num--;
            if (num < 0) num = 9;
            number.text = num.ToString();
        }
    }

    public void LockInChoice()
    {
        imgBG.color = new Color(0, 0, 0, 0);
        number.color = Color.green;
        isLockedIn = true;
        Destroy(transform.GetChild(1).gameObject);
        Destroy(transform.GetChild(2).gameObject);
    }

    public int GetDigit()
    {
        return int.Parse(number.text);
    }
    public void ChangeAppearance(Color newColor)
    {
        number.color = newColor;
    }
}
