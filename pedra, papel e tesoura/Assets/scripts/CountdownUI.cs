using System.Collections;
using UnityEngine;
using TMPro;


public class CountdownUI : MonoBehaviour
{
    public TMP_Text countdownText;


    private void Start()
    {
        countdownText.gameObject.SetActive(false);
    }


    public IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);


        countdownText.text = "3";
        yield return new WaitForSeconds(1);


        countdownText.text = "2";
        yield return new WaitForSeconds(1);


        countdownText.text = "1";
        yield return new WaitForSeconds(1);


        countdownText.text = "JÁ!";
        yield return new WaitForSeconds(1);


        countdownText.gameObject.SetActive(false);
    }
}
