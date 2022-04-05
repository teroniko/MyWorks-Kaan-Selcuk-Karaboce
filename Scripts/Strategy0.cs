using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Strategy0 : MonoBehaviour
{
    //üste pick your strategy yaz
    //life min value 0.01 yap
    //başlarken attackable ları saveden get et
    public Slider AttackPowerSlider;
    public Slider AttackSpeedSlider;
    public Slider LifeSlider;
    public Slider CountSlider;
    public Slider ManaSlider;

    public GameObject Warrior;
    public GameObject Archer;
    public GameObject Gaint;

    public static Attackable[] attackables;
    public static int[] counts;
    public static int unitNumber = 0;
    public GameObject think;
    public static float mana;
    public static int level = 1;
    public GameObject ReadyButton;
    int time = 0;
    private void Awake()
    {
        attackables = new Attackable[3];
        counts = new int[3];
        PlayerPrefs.DeleteAll();
        
    }
    void SetMaxMana(float mana)
    {
        Strategy0.mana = mana;
        ManaSlider.maxValue = mana;
        ManaSlider.value = mana;
        ManaSlider.transform.Find("Value").GetComponent<Text>().text = mana + "";
    }
    void Start()
    {
        for (int i = 0; i < counts.Length; i++)
        {
            //attackables[i] = new Attackable();
            //attackables[i].attackPower = PlayerPrefs.GetFloat("AttackPower" + i);
            //attackables[i].attackSpeed = PlayerPrefs.GetFloat("AttackSpeed" + i);
            //attackables[i].life = PlayerPrefs.GetFloat("Life" + i);
            //counts[i] = PlayerPrefs.GetInt("Count" + i);
            //if (PlayerPrefs.HasKey("Mana"))
            //{
            //    mana = PlayerPrefs.GetFloat("Mana");
            //}
            attackables[i] = new Attackable();
            attackables[i].attackPower = 0.1f;
            attackables[i].attackSpeed = 0.1f;
            attackables[i].life = 0.1f;
            counts[i] = 0;
            switch (level)
            {
                case 1:
                    SetMaxMana(30);
                    break;
                case 2:
                    SetMaxMana(45);
                    break;
                case 3:
                    SetMaxMana(55);
                    break;
            }
        }
    }


    void SetSliderasSaved()
    {
        Slider s = AttackPowerSlider;
        s.value = PlayerPrefs.GetFloat("AttackPower" + unitNumber);
        s.transform.Find("Value").GetComponent<Text>().text = PlayerPrefs.GetFloat("AttackPower" + unitNumber) + "";
        //attackables[unitNumber].attackPower = s.value;

        s = AttackSpeedSlider;
        s.value = PlayerPrefs.GetFloat("AttackSpeed" + unitNumber);
        s.transform.Find("Value").GetComponent<Text>().text = PlayerPrefs.GetFloat("AttackSpeed" + unitNumber) + "";
        //attackables[unitNumber].attackSpeed = s.value;


        s = LifeSlider;
        s.value = PlayerPrefs.GetFloat("Life" + unitNumber);
        s.transform.Find("Value").GetComponent<Text>().text = PlayerPrefs.GetFloat("Life" + unitNumber) + "";
        //attackables[unitNumber].life = s.value;

        s = CountSlider;
        s.value = PlayerPrefs.GetInt("Count" + unitNumber);
        s.transform.Find("Value").GetComponent<Text>().text = PlayerPrefs.GetInt("Count" + unitNumber) + "";
        //counts[unitNumber] = (int)s.value;

        s = ManaSlider;
        s.value = PlayerPrefs.GetFloat("Mana");
        s.transform.Find("Value").GetComponent<Text>().text = PlayerPrefs.GetFloat("Mana") + "";

    }
    public void Next()
    {



        DeactivateAll();
        Save();
        unitNumber++;
        Pass();
        //if (counts[unitNumber] == 0)
        //{
        //    SetZero(AttackPowerSlider);
        //    SetZero(AttackSpeedSlider);
        //    SetZero(LifeSlider);
        //    SetZero(CountSlider);
        //}

        SetSliderasSaved();

    }

    public void Previous()
    {
        DeactivateAll();
        Save();
        unitNumber--;
        Pass();
        SetSliderasSaved();
    }
    void Pass()
    {
        if (unitNumber > 2)
        {
            ReadyButton.SetActive(true);
            think.SetActive(false);
            unitNumber = 0;

        }
        //else if (unitNumber == 2)
        //{
        //    ReadyButton.SetActive(true);
        //    think.SetActive(false);
        //}
        else if (unitNumber < 0)
        {
            unitNumber = 2;
        }
        switch (unitNumber)
        {
            case 0: Warrior.SetActive(true); break;
            case 1: Archer.SetActive(true); break;
            case 2: Gaint.SetActive(true); break;
        }

    }
    void DeactivateAll()
    {
        Warrior.SetActive(false);
        Archer.SetActive(false);
        Gaint.SetActive(false);
    }

    void Save()//Ready0
    {
        Main_Camera.JustGame = false;

        PlayerPrefs.SetInt("UnitNumber", unitNumber);
        //attackables[i] = new Attackable();
        PlayerPrefs.SetFloat("AttackPower" + unitNumber, attackables[unitNumber].attackPower);
        PlayerPrefs.SetFloat("AttackSpeed" + unitNumber, attackables[unitNumber].attackSpeed);
        PlayerPrefs.SetFloat("Life" + unitNumber, attackables[unitNumber].life);
        PlayerPrefs.SetInt("Count" + unitNumber, counts[unitNumber]);
        PlayerPrefs.SetFloat("Mana", mana);
        PlayerPrefs.Save();
    }
    public void Ready()
    {
        if (mana >= 0)
        {
            StartCoroutine(Scene());
            ReadyButton.SetActive(false);
        }

    }
    IEnumerator Scene()
    {
        yield return new WaitForSeconds(3);
        //SceneManager.UnloadSceneAsync("Strategy");
        SceneManager.LoadScene("GamePlay");
    }
}
