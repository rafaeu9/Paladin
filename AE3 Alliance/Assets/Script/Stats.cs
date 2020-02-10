using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Effects
{
    public Sprite Icon;
    public float Cooldown;
}


public class Buffs
{

    public Effects Buff;
    public int ID;

    public Buffs(Effects newEffect, int newID)
    {
        Buff = newEffect;
        ID = newID;
    }
}

public class Stats : MonoBehaviour
{

    public int MaxHealth = 0;
    public int CurrentHealth = 0;
    public int MaxMana = 0;
    public int CurrentMana = 0;
    protected float PhysicalDmgReducion = 1;
    protected float DmgReducion = 1;
    public int MinBaseDmg = 0;
    public int MaxBaseDmg = 0;
    internal int WeaponBaseDamage = 0;
    public float AttackSpeed = 1;
    protected float CurrentAttackTime = 0;
    protected float ChanceHit = 0;
    internal float ChanceCrit = 0;
    protected float CritMultiplier = 1;
    protected float SpellChanceCrit = 0;
    protected float SpellCritMultiplier = 1;
    protected int ManaRegenBase = 0;
    protected int ManaRegenWhileCombat = 0;
    protected int ManaRegenWhileCasting = 0;
    protected bool Casting = false;
    internal bool Combat = false;
    public int Range = 0;
    public float MoveSpeed = 0;
    public float MoveSlow = 1;

    public GameObject FloatNumber;

    internal bool TargetJudgementOfWisdom = false;
    internal bool TargetJudgementOfRighteousness = false;

    internal float StunTime = 0;
    public bool Stun;

    public Sprite Char;

    public GameObject Target;



    internal void StunMe(float Time)
    {
        Stun = true;
        if (Time > StunTime)
            StunTime = Time;
    }

    internal int EffectCount;
    internal List<Buffs> Buff = new List<Buffs>();

    public virtual void Damage(int dmg, bool Physical, GameObject DmgDealer)
    {

        if (TargetJudgementOfWisdom && Physical)
            if (Random.Range(0, 100) <= 30)
                DmgDealer.GetComponent<Stats>().CurrentMana += (int)(dmg * 0.02);

        if (TargetJudgementOfWisdom && Physical)
            if (Random.Range(0, 100) <= 30)
                DmgDealer.GetComponent<Stats>().CurrentHealth += (int)(dmg * 0.02);


        dmg -= (int)(dmg - dmg * DmgReducion);

        Instantiate(FloatNumber, gameObject.transform).GetComponent<TextMesh>().text = dmg.ToString();


        if (Physical)
        {
            CurrentHealth -= (int)(dmg - dmg * PhysicalDmgReducion);            
        }
        else
            CurrentHealth -= dmg;
    }

    internal virtual void DealPhysicalDamage(int Dmg)
    {

        if (Random.Range(0, 100) <= ChanceCrit)
        {
            Target.GetComponent<Stats>().Damage((int)(Dmg * CritMultiplier), true, gameObject);
        }
        else
            Target.GetComponent<Stats>().Damage(Dmg, true, gameObject);

    }
    internal virtual void DealSpellDamage(int Dmg)
    {

        if (Random.Range(0, 100) <= ChanceCrit)
        {
            Target.GetComponent<Stats>().Damage((int)(Dmg * SpellCritMultiplier), false, gameObject);
        }
        else
            Target.GetComponent<Stats>().Damage(Dmg, false, gameObject);
    }

    internal virtual void Slow(float inpSlow)
    {
        MoveSlow -= inpSlow;

        if (MoveSlow < 0)
            MoveSlow = 0;

        if (MoveSlow > 1)
            MoveSlow = 1;
    }



    public virtual void Heal(int inpHeal)
    {
        CurrentHealth += inpHeal;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }


}
