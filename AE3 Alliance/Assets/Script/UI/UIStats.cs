using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStats : MonoBehaviour
{
    public GameObject PlayerStats;
    GameObject EnemyStats;
    GameObject EnemyTargetStats;

    public GameObject PlayerPanel;
    GameObject EnemyPanel;

    [System.Serializable]
    public struct Bars
    {
        public GameObject UI;
        public Image Char;
        public Slider Health;
        public Slider Mana;
    }
 
    public Bars Player;
    public Bars Enemy;
    public Bars EnemyTarget;

    public Slider Cast;

    public GameObject Buff;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #region UpdateBars
        EnemyStats = PlayerStats.GetComponent<Stats>().Target;
        EnemyTargetStats = EnemyStats?.GetComponent<Stats>().Target;


        Player.Char.sprite = PlayerStats.GetComponent<Stats>().Char;
        
        Player.Health.maxValue = PlayerStats.GetComponent<Stats>().MaxHealth;
        Player.Health.value = PlayerStats.GetComponent<Stats>().CurrentHealth;

        Player.Mana.maxValue = PlayerStats.GetComponent<Stats>().MaxMana;
        Player.Mana.value = PlayerStats.GetComponent<Stats>().CurrentMana;

        Enemy.UI.SetActive(PlayerStats.GetComponent<Stats>().Target);
        if (PlayerStats.GetComponent<Stats>().Target)
        {           

            Enemy.Char.sprite = EnemyStats.GetComponent<Stats>().Char;
            Enemy.Health.maxValue = EnemyStats.GetComponent<Stats>().MaxHealth;
            Enemy.Health.value = EnemyStats.GetComponent<Stats>().CurrentHealth;

            Enemy.Mana.maxValue = EnemyStats.GetComponent<Stats>().MaxMana;
            Enemy.Mana.value = EnemyStats.GetComponent<Stats>().CurrentMana;
        }

        EnemyTarget.UI.SetActive(Enemy.UI.activeSelf && EnemyStats?.GetComponent<Stats>().Target);
        if (EnemyStats?.GetComponent<Stats>().Target)
        {            

            EnemyTarget.Char.sprite = EnemyTargetStats.GetComponent<Stats>().Char;
            EnemyTarget.Health.maxValue = EnemyTargetStats.GetComponent<Stats>().MaxHealth;
            EnemyTarget.Health.value = EnemyTargetStats.GetComponent<Stats>().CurrentHealth;

            EnemyTarget.Mana.maxValue = EnemyTargetStats.GetComponent<Stats>().MaxMana;
            EnemyTarget.Mana.value = EnemyTargetStats.GetComponent<Stats>().CurrentMana;
        }
        #endregion

        #region CastBar
        Cast.gameObject.SetActive(PlayerStats.GetComponent<Paladin>().CastingActive);
        Cast.maxValue = PlayerStats.GetComponent<Paladin>().TotalCastingTime;
        Cast.value = PlayerStats.GetComponent<Paladin>().CurrentCastingTime;
        #endregion

        #region Effects
        


        for (int i = PlayerPanel.transform.childCount-1; i >= 0; i--)
        {
            Destroy(PlayerPanel.transform.GetChild(i).gameObject);

        }
        for (int i = 0; i < PlayerStats.GetComponent<Stats>().Buff.Count; i++)
        {
            GameObject Efect = Instantiate(Buff, PlayerPanel.transform);
            Efect.GetComponent<Image>().sprite = PlayerStats.GetComponent<Stats>().Buff[i].Buff.Icon;
            if (PlayerStats.GetComponent<Stats>().Buff[i].Buff.Cooldown > 0)
                Efect.transform.GetChild(0).gameObject.GetComponent<Text>().text = PlayerStats.GetComponent<Stats>().Buff[i].Buff.Cooldown.ToString("0.0");
            else
                Efect.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (Enemy.UI.activeSelf)
        {
            EnemyPanel = Enemy.UI.transform.Find("Panel").transform.GetChild(0).gameObject;
            

            for (int i = EnemyPanel.transform.childCount-1; i >= 0; i--)
            {
                Destroy(EnemyPanel.transform.GetChild(i).gameObject);                
            }
            for (int i = 0; i < EnemyStats.GetComponent<Stats>().Buff.Count; i++)
            {
                GameObject Efect = Instantiate(Buff, EnemyPanel.transform);
                Efect.GetComponent<Image>().sprite = EnemyStats.GetComponent<Stats>().Buff[i].Buff.Icon;
                if (EnemyStats.GetComponent<Stats>().Buff[i].Buff.Cooldown > 0)
                    Efect.transform.GetChild(0).gameObject.GetComponent<Text>().text = EnemyStats.GetComponent<Stats>().Buff[i].Buff.Cooldown.ToString("0.0");
                else
                    Efect.transform.GetChild(0).gameObject.SetActive(false);
            }
            
        }

        #endregion
    }

    public void UpdatePlayer(GameObject NewPlayer)
    {
        PlayerStats = NewPlayer;
    }

    public void UpdateEnemy(GameObject NewTarget)
    {
                
    }

}
