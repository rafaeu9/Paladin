using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBlackfathomDeeps : Stats
{

    bool UseAbility = false;
    int ChanceAbillity = 40;
    int CurrentChanceAbillity = 40;
    int AddChanceAbillity = 20;


    float AttackCurrentTime = 0;

    bool Chase = false;

    float VisionRange = 5;

    int DoubleDmg = 1;
    float ActiveVengefulStance = 1;

    Vector3 spawnPos;
    Vector2 Velocity;

    enum skills { BlackfathomHamstring, Bash, MyrmidonSlash, VengefulStance, END}

    skills LastSkill;
    skills NewSkill;

    GameObject Player;

    public GameObject Text;

    #region IconsEffects
    public Sprite IconBlackfathomHamstring;
    public Sprite IconBash;
    public Sprite IconVengefulStance;
    public Sprite IconNagaSpirit;
    #endregion

    internal bool VengefulStanceActive = false; 

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ///Stats
        MaxHealth = 600;
        CurrentHealth = 0;
        WeaponBaseDamage = 51;
        AttackSpeed = 2;
        PhysicalDmgReducion = 0.35f;
        MinBaseDmg = 37;
        MaxBaseDmg = 51;
        ChanceHit = 85;
        ChanceCrit = 10;
        CritMultiplier = 1.5f;
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
        spawnPos = transform.position;
        AttackCurrentTime = AttackSpeed;

        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth <= 0)
            Destroy(gameObject);
       
            if (ActiveVengefulStance != 1)
                ActiveVengefulStance = 1;

        if (Vector2.SqrMagnitude(Player.transform.position - transform.position) <= VisionRange)
        {
            Target = Player;
            Chase = true;
        }
        else
        {
            Target = null;
            Chase = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Animator>().SetBool("Move", false);
        }

        if (Stun)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Animator>().SetBool("Move", false);
        }
        else
        {

            if (Chase)
            {
                if (Vector2.SqrMagnitude(Target.transform.position - transform.position) <= Range)
                    Velocity = Vector2.zero;
                else
                {
                    if (Target.transform.position.y > transform.position.y)
                    {
                        Velocity.y = 1 * MoveSpeed * MoveSlow;
                        GetComponent<Animator>().SetBool("Top", true);
                    }
                    else if (Target.transform.position.y < transform.position.y)
                    {
                        Velocity.y = -1 * MoveSpeed * MoveSlow;
                        GetComponent<Animator>().SetBool("Top", false);
                    }

                    if (Target.transform.position.x > transform.position.x)
                    {
                        Velocity.x = 1 * MoveSpeed * MoveSlow;
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else if (Target.transform.position.x < transform.position.x)
                    {
                        Velocity.x = -1 * MoveSpeed * MoveSlow;
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                }



                GetComponent<Rigidbody2D>().velocity = Velocity;



                if (AttackCurrentTime >= AttackSpeed)
                {
                    if (Vector2.SqrMagnitude(Target.transform.position - transform.position) <= Range)
                    {
                        GetComponent<Animator>().SetTrigger("Attack");
                        AttackCurrentTime = 0;
                    }
                }
                else
                    AttackCurrentTime += Time.deltaTime;




                if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
                    GetComponent<Animator>().SetBool("Move", false);
                else
                    GetComponent<Animator>().SetBool("Move", true);
            }

        }

        //Wait until Stun ends
        if (StunTime <= 0)
        {
            if (Stun)
                Stun = !Stun;
        }
        else
        {
            StunTime -= Time.deltaTime;
        }
    }

    public void Attack()
    {       

            UseAbility = !UseAbility;

        if (Random.Range(0, 100) <= ChanceHit)
        {

            if (Random.Range(0, 100) <= ChanceCrit)
            {
                int tempcrit = (int)(Random.Range(MinBaseDmg, MaxBaseDmg) * CritMultiplier * DoubleDmg * ActiveVengefulStance);                
                Target.GetComponent<Stats>().Damage(tempcrit, true, gameObject);
                NagaSpirit(tempcrit);
            }
            else
                Target.GetComponent<Stats>().Damage((int)(Random.Range(MinBaseDmg, MaxBaseDmg) * DoubleDmg * ActiveVengefulStance), true, gameObject);
        }



            if (UseAbility)
            {
                if (Random.Range(0, 100) <= CurrentChanceAbillity)
                {
                    CurrentChanceAbillity = ChanceAbillity;
                    Useskills();

                }
                else
                {
                    CurrentChanceAbillity += AddChanceAbillity;
                }
            }
            else
                CurrentChanceAbillity += AddChanceAbillity;

        if (DoubleDmg > 1)
            DoubleDmg = 1;
    }

    public override void Damage(int dmg, bool Physical, GameObject DmgDealer)
    {
        if (VengefulStanceActive)
            DmgDealer.GetComponent<Stats>().Damage((int)(dmg * 0.5), false, gameObject);

        base.Damage(dmg, Physical, DmgDealer);
    }

    void Useskills()
    {
        if (Target)
        {
            do
            {
                NewSkill = (skills)Random.Range(0, (int)(skills.END));
            } while (NewSkill == LastSkill);

            switch (NewSkill)
            {
                case skills.BlackfathomHamstring:
                    BlackfathomHamstring();
                    break;
                case skills.Bash:
                    Bash();
                    break;
                case skills.MyrmidonSlash:
                    MyrmidonSlash();
                    break;
                case skills.VengefulStance:
                    VengefulStance();
                    break;
                default:
                    break;
            }
        }
    }


    #region Abilities
    void BlackfathomHamstring()
    {
        EffectManager.AddDebuff(new BlackfathomHamstring(Target, WeaponBaseDamage, ChanceCrit, CritMultiplier, IconBlackfathomHamstring, gameObject));
        //Enemy.GetComponent<Stats>().Damage((int)(WeaponBaseDamage * 1.80));
    }

    void Bash()
    {
        Target.GetComponent<Stats>().StunMe(5);
        EffectManager.AddDebuff(new Bash(Target, IconBash));
    }

    void MyrmidonSlash()
    {
        DoubleDmg = 2;
        Text.SetActive(true);
        Text.GetComponent<TextMesh>().text = "HYAAAH";
        StartCoroutine(MyrmidonSlashText());
    }


    IEnumerator MyrmidonSlashText()
    {
        yield return new WaitForSeconds(1.5f);
        if (Text.GetComponent<TextMesh>().text == "HYAAAH")
        {
            Text.SetActive(false);
            Text.GetComponent<TextMesh>().text = "";
        }
    }

    void VengefulStance()
    {
        ActiveVengefulStance = 0.6f;
        VengefulStanceActive = true;
        EffectManager.AddDebuff(new VengefulStance(gameObject, IconVengefulStance));

    }
    #endregion


    void NagaSpirit(int Dmg)
    {
        EffectManager.AddDebuff(new NagaSpirit(gameObject, Dmg, IconNagaSpirit));
    }

    
}

#region Effects
public class BlackfathomHamstring : Effect
{
      
    int TimeOfEfect = 8;
    float CurrentTime;
    float AttackSpeedTaken = 0;

    public BlackfathomHamstring(GameObject inputTarget, int WeaponBaseDamage, float Critchance, float critmulti, Sprite InpIcon, GameObject DmgDealer)
    {
        Icon = InpIcon;
        Target = inputTarget;

        CurrentTime = TimeOfEfect;
        if (Random.Range(0, 100) <= Critchance)
            Target.GetComponent<Paladin>().Damage((int)((WeaponBaseDamage * 1.80) * critmulti), false, DmgDealer);
        else
            Target.GetComponent<Paladin>().Damage((int)(WeaponBaseDamage * 1.80), false, DmgDealer);

        
        Target.GetComponent<Stats>().Slow(0.5f);

        AttackSpeedTaken = Target.GetComponent<Stats>().AttackSpeed / 2;
        Target.GetComponent<Stats>().AttackSpeed -= AttackSpeedTaken;

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));
    }


    public override void update()
    {
        if (Target == null)
        {
            Active = false;
        }
        else
        {
            if (CurrentTime <= 0)
            {
                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));
                Target.GetComponent<Stats>().Slow(-0.5f);
                Target.GetComponent<Stats>().AttackSpeed += AttackSpeedTaken;
                Active = false;
            }
            else
            {
                CurrentTime -= Time.deltaTime;
                Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position).Buff.Cooldown = CurrentTime;

            }
        }

    }

}



public class NagaSpirit : Effect
{
    int healing = 0;

   
    int TimeOfEfect = 10;
    int tic ;
    float CurrentTime;

    public NagaSpirit(GameObject inpTarget,int inpdmg, Sprite InpIcon)
    {
        Icon = InpIcon;
        Target = inpTarget;
        healing = (int)(inpdmg * 0.6 * 0.1);
        tic = TimeOfEfect;
        CurrentTime = TimeOfEfect;

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));
    }


    public override void update()
    {
        if (Target == null)
        {
            Active = false;
        }
        else
        {
            if (CurrentTime <= tic)
            {
                
                Target?.GetComponent<TheBlackfathomDeeps>().Heal(healing);
                
                --tic;

                if (tic <= 0)
                {
                    Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));
                    Active = false;
                }
            }
            else
            {
                CurrentTime -= Time.deltaTime;
                Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position).Buff.Cooldown = CurrentTime;

            }
        }
    }
}



public class Bash : Effect
{
    
    int TimeOfEfect = 5;
    float CurrentTime;


    public Bash(GameObject inputTarget, Sprite InpIcon)
    {
        Icon = InpIcon;
        Target = inputTarget;
        CurrentTime = TimeOfEfect;
        

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));
    }

    public override void update()
    {
        if (Target == null)
        {
            Active = false;
        }
        else
        {

            if (CurrentTime <= 0)
            {
                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                Active = false;
            }
            else
            {
                CurrentTime -= Time.deltaTime;
                Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position).Buff.Cooldown = CurrentTime;

            }
        }


    }


}



public class VengefulStance : Effect
{
    
    int TimeOfEfect = 10;
    float CurrentTime;

    public VengefulStance(GameObject inpTarget, Sprite InpIcon)
    {
        Icon = InpIcon;
        Target = inpTarget;
        CurrentTime = TimeOfEfect;
        
        Target.GetComponent<Stats>().Slow(1);
        Target.GetComponent<TheBlackfathomDeeps>().Text.SetActive(true);
        Target.GetComponent<TheBlackfathomDeeps>().Text.GetComponent<TextMesh>().text = "En Garde";
        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));
    }

    public override void update()
    {
        if (Target == null)
        {
            Active = false;
        }
        else
        {

            if (CurrentTime <= 0)
            {
                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));
                Target.GetComponent<Stats>().Slow(-1);
                Target.GetComponent<TheBlackfathomDeeps>().Text.SetActive(false);
                Target.GetComponent<TheBlackfathomDeeps>().Text.GetComponent<TextMesh>().text = "";
                Target.GetComponent<TheBlackfathomDeeps>().VengefulStanceActive = false;
                Active = false;
            }
            else
            {
                CurrentTime -= Time.deltaTime;
                Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position).Buff.Cooldown = CurrentTime;

            }
        }


    }
}
#endregion