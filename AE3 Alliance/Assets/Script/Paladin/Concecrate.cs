using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concecrate : MonoBehaviour
{
    internal List<GameObject> Enemys = new List<GameObject>();
    float CurrentTime;
    int Dmg;
    Stats Player;


    // Start is called before the first frame update
    void Start()
    {
        Enemys.Clear();
        CurrentTime = 2;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

        Dmg = (int)(Player.WeaponBaseDamage * 3.8);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (CurrentTime <= 0)
        {
            CurrentTime = 2;

            if (Enemys.Count > 0)
            {
                for (int i = 0; i < Enemys.Count; i++)
                {
                    Enemys[i].GetComponent<Stats>().Damage(Dmg, true, gameObject);
                }
            }
        }
        else
            CurrentTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Enemy"))
        Enemys.Add(collision.gameObject);

        if (collision.gameObject.tag.Equals("Player"))
            Player.GetComponent<Paladin>().OnConcecrationLand = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
            Enemys.Remove(collision.gameObject);

        if (collision.gameObject.tag.Equals("Player"))
            Player.GetComponent<Paladin>().OnConcecrationLand = false;
    }


}
