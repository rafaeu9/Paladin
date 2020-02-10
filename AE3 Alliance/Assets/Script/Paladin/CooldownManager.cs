using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{

    Stats PlayerStats;

    [System.Serializable]
    public struct Skill
    {
        public float Cooldown;
        public float ManaCost;
        internal float CurrentTime;

        internal bool Wait;

        internal GameObject Parent;
        internal GameObject Button;
        internal GameObject Time;       
    }

    public Skill Skill1;
    public Skill Skill2;
    public Skill Skill3;
    public Skill Skill4;

    void Start()
    {
        PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

        Skill1.Parent = gameObject.transform.GetChild(0).gameObject;
        Skill1.Button = Skill1.Parent.transform.GetChild(0).gameObject;
        Skill1.Time = Skill1.Parent.transform.GetChild(1).gameObject;
        Skill1.Wait = false;

        Skill2.Parent = gameObject.transform.GetChild(1).gameObject;
        Skill2.Button = Skill2.Parent.transform.GetChild(0).gameObject;
        Skill2.Time = Skill2.Parent.transform.GetChild(1).gameObject;
        Skill2.Wait = false;

        Skill3.Parent = gameObject.transform.GetChild(2).gameObject;
        Skill3.Button = Skill3.Parent.transform.GetChild(0).gameObject;
        Skill3.Time = Skill3.Parent.transform.GetChild(1).gameObject;
        Skill3.Wait = false;

        Skill4.Parent = gameObject.transform.GetChild(3).gameObject;
        Skill4.Button = Skill4.Parent.transform.GetChild(0).gameObject;
        Skill4.Time = Skill4.Parent.transform.GetChild(1).gameObject;
        Skill4.Wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        
            Skill1.Button.SetActive(Skill1.CurrentTime <= 0 && PlayerStats.CurrentMana >= PlayerStats.MaxMana * (Skill1.ManaCost / 100));
        
            Skill2.Button.SetActive(Skill2.CurrentTime <= 0 && PlayerStats.CurrentMana >= PlayerStats.MaxMana * (Skill2.ManaCost / 100));
        
            Skill3.Button.SetActive(Skill3.CurrentTime <= 0 && PlayerStats.CurrentMana >= PlayerStats.MaxMana * (Skill3.ManaCost / 100));
        
            Skill4.Button.SetActive(Skill4.CurrentTime <= 0 && PlayerStats.CurrentMana >= PlayerStats.MaxMana * (Skill4.ManaCost / 100));




        #region Timers

        if(Skill1.Wait)
        if (Skill1.CurrentTime <= 0)
        {
            Skill1.Time.SetActive(false);
            Skill1.Button.SetActive(true);
            Skill1.Wait = false;
        }
        else
        {
            Skill1.CurrentTime -= Time.deltaTime;
            Skill1.Time.GetComponent<Text>().text = Skill1.CurrentTime.ToString("0.0");
        }

        if(Skill2.Wait)
        if (Skill2.CurrentTime <= 0)
        {        
            Skill2.Time.SetActive(false);
            Skill2.Button.SetActive(true);
            Skill2.Wait = false;
        }        
        else
        {
            Skill2.CurrentTime -= Time.deltaTime;
            Skill2.Time.GetComponent<Text>().text = Skill2.CurrentTime.ToString("0.0");
        }

        if(Skill3.Wait)
        if (Skill3.CurrentTime <= 0)
        {
            Skill3.Time.SetActive(false);
            Skill3.Button.SetActive(true);
            Skill3.Wait = false;
        }
        else
        {
            Skill3.CurrentTime -= Time.deltaTime;
            Skill3.Time.GetComponent<Text>().text = Skill3.CurrentTime.ToString("0.0");
        }

        if (Skill4.Wait)
        if (Skill4.CurrentTime <= 0)
        {
            Skill4.Time.SetActive(false);
            Skill4.Button.SetActive(true);
            Skill4.Wait = false;
        }
        else
        {
            Skill4.CurrentTime -= Time.deltaTime;
            Skill4.Time.GetComponent<Text>().text = Skill4.CurrentTime.ToString("0.0");
        }
        #endregion

        

    }

    public void FSkill1()
    {
        PlayerStats.CurrentMana -= (int)(PlayerStats.MaxMana * (Skill1.ManaCost / 100));

        Skill1.CurrentTime = Skill1.Cooldown;
        Skill1.Button.SetActive(false);
        Skill1.Time.SetActive(true);
        Skill1.Wait = true;
    }
    public void FSkill2()
    {
        PlayerStats.CurrentMana -= (int)(PlayerStats.MaxMana * (Skill2.ManaCost / 100));

        Skill2.CurrentTime = Skill2.Cooldown;
        Skill2.Button.SetActive(false);
        Skill2.Time.SetActive(true);
        Skill2.Wait = true;
    }
    public void FSkill3()
    {
        PlayerStats.CurrentMana -= (int)(PlayerStats.MaxMana * (Skill3.ManaCost / 100));

        Skill3.CurrentTime = Skill3.Cooldown;
        Skill3.Button.SetActive(false);
        Skill3.Time.SetActive(true);
        Skill3.Wait = true;
    }
    public void FSkill4()
    {
        PlayerStats.CurrentMana -= (int)(PlayerStats.MaxMana * (Skill4.ManaCost / 100));

        Skill4.CurrentTime = Skill4.Cooldown;
        Skill4.Button.SetActive(false);
        Skill4.Time.SetActive(true);
        Skill4.Wait = true;
    }

}


