using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Paladin : Stats
{
    GameObject UI;

    ///Abilitys Var

    //CrusaiderStrike
    float ActiveCrusaiderStrike = 1;

    public Sets Sets;
    public CooldownManager Set1;
    public CooldownManager Set2;
    public CooldownManager Set3;
    public CooldownManager Set4;

    public GameObject ConcecratedLand;
    bool ConcecrationLandActive = false;
    float ConcecrationLandTime = 12;
    float ConcecrationLandCurrentTime = 0;
    internal bool OnConcecrationLand = false;

    internal bool CastingActive = false;
    internal float TotalCastingTime = 0;
    internal float CurrentCastingTime = 0;

    bool HolyLightActive = false;
    bool ExorcismActive = false;

    public Sprite IconHammerOfJustice;
    public Sprite IconLayOnHands;

    
    bool ActiveDivineShild = false;

    bool ActiveArdentDefender = false;

    bool ActiveHandOfFreedom = false;

    float TotalManaTime = 5;
    float CurrentManaTime = 0;

    public Sprite IconDevotionAura;
    public bool ActiveDevotionAura = false;
    public Sprite IconMagicalAura;
    public bool ActiveMagicalAura = false;
    public Sprite IconRetributionAura;
    public bool ActiveRetributionAura = false;
    

    public Sprite IconSealOfRighteousness;
    public bool ActiveSealOfRighteousness = false;
    public Sprite IconSealOfLight;
    public bool ActiveSealOfLight = false;
    public Sprite IconSealOfJustice;
    public bool ActiveSealOfJustice = false;


    public Sprite IconJudgementOfRighteousness;
    public bool ActiveJudgementOfRighteousness = false;
    public Sprite IconJudgementOfWisdom;
    public bool ActiveJudgementOfWisdom = false;
    public Sprite IconJudgementOfWeakness;
    public bool ActiveJudgementOfWeakness = false;
    
    
    public Sprite IconBlessingOfMight;    
    public bool ActiveBlessingOfMight = false;
    public Sprite IconBlessingOfWisdom;
    public bool ActiveBlessingOfWisdom = false;
    public Sprite IconBlessingOfKings;
    public bool ActiveBlessingOfKings = false;

    internal Vector3 SpawnPos;
    public bool Dead = false;

    Vector3 touchPosWorld;

    //Change me to change the touch phase used.
    TouchPhase touchPhase = TouchPhase.Ended;

    public GameObject TargetArrow;


    private void Awake()
    {
        MaxHealth = 800;
        CurrentHealth = 0;
        MaxMana = 500;
        CurrentMana = 0;
        PhysicalDmgReducion = 0.40f;
        AttackSpeed = 2.4f;
        CurrentAttackTime = 0;
        MinBaseDmg = 37;
        MaxBaseDmg = 58;
        WeaponBaseDamage = 48;
        ChanceHit = 87;
        ChanceCrit = 11;
        CritMultiplier = 1.7f;
        SpellChanceCrit = 17;
        SpellCritMultiplier = 1.7f;
        ManaRegenBase = 34;
        ManaRegenWhileCombat = 15;
        ManaRegenWhileCasting = 6;
        Casting = false;
        Combat = false;
        Range = 5;
    }




    // Start is called before the first frame update
    void Start()
    {
        SpawnPos = transform.position;

        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;

        UI = GameObject.Find("Canvas");

        EffectManager.AddDebuff(new DevotionAura(gameObject, IconDevotionAura));
        ActiveDevotionAura = true;

        //Set1 = GameObject.Find("Set 1").GetComponent<CooldownManager>();
        //Set2 = GameObject.Find("Set 2").GetComponent<CooldownManager>();
        //Set3 = GameObject.Find("Set 3").GetComponent<CooldownManager>();
        //Set4 = GameObject.Find("Set 4").GetComponent<CooldownManager>();


    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            #region Target

            //We check if we have more than one touch happening.
            //We also check if the first touches phase is Ended (that the finger was lifted)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase)
            {
                //We transform the touch position into word space from screen space and store it.
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                //We now raycast with this information. If we have hit something we can process it.
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld, Camera.main.transform.forward);

                if (hitInformation.transform?.gameObject.GetComponent<Stats>())
                {
                    Target = hitInformation.transform.gameObject;
                    TargetArrow.SetActive(true);                    
                   

                }

            }

            if (!Target)
            {
                Target = null;
                TargetArrow.SetActive(false);

            }
            else
            {
                TargetArrow.transform.position = Target.transform.position;
            }
            #endregion



            #region Movement
            if (ActiveHandOfFreedom)
                GetComponent<Rigidbody2D>().velocity = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal") * MoveSpeed * MoveSlow, CrossPlatformInputManager.GetAxisRaw("Vertical") * MoveSpeed * MoveSlow);

            else if (Stun)
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal") * MoveSpeed, CrossPlatformInputManager.GetAxisRaw("Vertical") * MoveSpeed);

            GetComponent<Animator>().SetFloat("direction", GetComponent<Rigidbody2D>().velocity.y);

            if (GetComponent<Rigidbody2D>().velocity.x < 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else if (GetComponent<Rigidbody2D>().velocity.x > 0)
                GetComponent<SpriteRenderer>().flipX = false;


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

            #endregion


            // Update Health and Mana on UI
            //UI.GetComponent<UIStats>().UpdatePlayer(MaxHealth, CurrentHealth, MaxMana, CurrentMana);        

            if (ConcecrationLandActive)
                ConcecrationLandUpdate();

            if (CastingActive)
                CastingUpdate();


            if (Combat && !ActiveDivineShild && Target && Target.tag.Equals("Enemy"))
                if (CurrentAttackTime >= AttackSpeed)
                {
                    if (Vector2.SqrMagnitude(Target.transform.position - transform.position) <= Range)
                    {
                        GetComponent<Animator>().SetTrigger("attack");
                        CurrentAttackTime = 0;
                    }
                }
                else
                    CurrentAttackTime += Time.deltaTime;


            if (CurrentMana <= MaxMana)
            {
                CurrentManaTime += Time.deltaTime;
                if (CurrentManaTime >= TotalManaTime)
                {
                    CurrentManaTime = 0;
                    ManaRegen();
                }
            }
        }
    }

    public void StartDead()
    {
        Dead = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().SetFloat("direction", GetComponent<Rigidbody2D>().velocity.y);
        UI.SetActive(false);
        GetComponent<Animator>().SetBool("Dead", Dead);
        StartCoroutine(WaitDead());
    }

    IEnumerator WaitDead()
    {
        yield return new WaitForSeconds(3);
        transform.position = SpawnPos;
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
        UI.SetActive(true);
        Dead = false;
        GetComponent<Animator>().SetBool("Dead", Dead);
    }

    void ManaRegen()
    {
        if (ActiveBlessingOfWisdom)
            CurrentMana += 10;

        if (ActiveMagicalAura)
            CurrentMana += (int)(MaxMana * 0.02);

            if (CastingActive)
                CurrentMana += ManaRegenWhileCasting;
            else if (Combat)
                CurrentMana += ManaRegenWhileCombat;
            else
                CurrentMana += ManaRegenBase;

            if (CurrentMana > MaxMana)
                CurrentMana = MaxMana;
        
    }

    public void AttackAnim()
    {
        Combat = !Combat;

        if (Combat)
            UI.GetComponent<Sets>().ActivateSet1();
        else
            UI.GetComponent<Sets>().DeactivateSet();
    }

    public void Attack()
    {

        if (Target)
        {
            if (Vector2.SqrMagnitude(Target.transform.position - transform.position) <= Range)
                if (Random.Range(0, 100) <= ChanceHit)
                {
                    int dmg = Random.Range(MinBaseDmg, MaxBaseDmg);

                    if (ActiveBlessingOfMight)
                        dmg = (int)(dmg + dmg * 0.1);

                    if (ActiveSealOfRighteousness)
                        DealSpellDamage((int)(WeaponBaseDamage * 0.15));

                    if (ActiveSealOfJustice)
                        if (Random.Range(0, 100) <= 10)
                            Target.GetComponent<Stats>().StunMe(2);

                    if (ActiveSealOfLight)
                        if (Random.Range(0, 100) <= 20)
                            Heal((int)(dmg * 0.2));


                    DealPhysicalDamage(dmg);
                }
        }

        CurrentAttackTime = 0;


    }

    void CastingTime(float Time)
    {
        TotalCastingTime = Time;
        CastingActive = true;
        CurrentCastingTime = 0;


    }

    public override void Damage(int dmg, bool Physical, GameObject DmgDealer)
    {



        if (ActiveRetributionAura)
            DmgDealer.GetComponent<Stats>().Damage((int)(WeaponBaseDamage * 0.02), false, gameObject);

        if (!ActiveDivineShild)
        {

            if (ActiveDevotionAura)
                dmg = (int)(dmg - dmg * 0.1);

            if (ActiveArdentDefender)
            {
                dmg -= (int)(dmg * 0.2);

                if (dmg >= CurrentHealth)
                    CurrentHealth = (int)(MaxHealth * 0.12);
                else
                    base.Damage(dmg, Physical, DmgDealer);
            }else
            base.Damage(dmg, Physical, DmgDealer);

            if(CurrentHealth <=0)
            {
                DmgDealer.GetComponent<Stats>().CurrentHealth = DmgDealer.GetComponent<Stats>().MaxHealth;
                StartDead();
            }

        }
    }


    //void Abilities()
    //{

    //}

    #region Set 1

    public void CrusaiderStrike()
    {
        if (Target)
        {
            Set1.FSkill1();

            ActiveCrusaiderStrike = 1.4f;
            DealPhysicalDamage((int)(Random.Range(MinBaseDmg, MaxBaseDmg) * ActiveCrusaiderStrike));
        }
        
    }

    public void HammerOfJustice()
    {
        if (Target)
        {
            Set1.FSkill2();
            EffectManager.AddDebuff(new HammerOfJustice(Target, IconHammerOfJustice));
        }
    }

    public void DivineStorm()
    {
        if (Target)
        {
            Set1.FSkill3();
            int dmg = 0;
            int dmgDeal = 0;
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");



            for (int i = 0; i < Enemys.Length; i++)
            {
                if (Vector2.SqrMagnitude(Enemys[i].transform.position - transform.position) <= Range)
                {
                    dmg = Random.Range(MinBaseDmg, MaxBaseDmg);
                    dmgDeal += dmg;
                    Enemys[i].GetComponent<Stats>()?.Damage(dmg, false, gameObject);
                }

            }

            Heal(dmgDeal);
        }
    }

    public void Judgement()
    {
        if (Target)
        {
            Set1.FSkill4();
            int Dmg = 0;
            if (Random.Range(0, 100) <= ChanceCrit)
            {
                Dmg = (int)((WeaponBaseDamage * 0.2) * CritMultiplier);
            }
            else
                Dmg = (int)(WeaponBaseDamage * 0.2);

            

            if(ActiveJudgementOfRighteousness)
            EffectManager.AddDebuff(new JudgementOfRighteousness(Target, IconJudgementOfRighteousness));
            else if (ActiveJudgementOfWisdom)
                EffectManager.AddDebuff(new JudgementOfWisdom(Target, IconJudgementOfWisdom));
            else if(ActiveJudgementOfWeakness)
            EffectManager.AddDebuff(new JudgementOfWeakness(Target, IconJudgementOfWeakness));

            Target.GetComponent<Stats>().Damage(Dmg, true, gameObject);
        }
    }
    #endregion

    #region Set 2
    public void HolyLight()
    {
        if (Target)
        {
            if (!CastingActive)
            {
                Set2.FSkill1();
                TotalCastingTime = 2.5f;
                CastingActive = true;
                CurrentCastingTime = 0;
                HolyLightActive = true;
                gameObject.GetComponent<Animator>().SetBool("Casting", true);

            }
            else
            {
                if (Target.tag.Equals("Ally") || Target.Equals(gameObject))
                {
                    Target.GetComponent<Stats>().Heal(Random.Range(130, 190));

                    HolyLightActive = false;
                }
            }
        }
    }

    public void Exorcism()
    {
        if (!CastingActive)
        {
            if (Target)
            {
                Set2.FSkill2();
                TotalCastingTime = 1.5f;
                CastingActive = true;
                CurrentCastingTime = 0;
                ExorcismActive = true;
                gameObject.GetComponent<Animator>().SetBool("Casting", true);
            }
        }
        else
        {
            if (Target.tag.Equals("Enemy"))
            {
                DealSpellDamage(Random.Range(75, 125));
                ExorcismActive = false;
            }
            
        }
        
    }

    void CastingUpdate()
    {
        //Wait until Stun ends
        CurrentCastingTime += Time.deltaTime;
        if (CurrentCastingTime >= TotalCastingTime)
        {
            gameObject.GetComponent<Animator>().SetBool("Casting", false);

            if (HolyLightActive)
                HolyLight();
            else if(ExorcismActive)
                Exorcism();

            CastingActive = false;
        }

    }

    public void Concecration()
    {
        
            Set2.FSkill3();

            ConcecratedLand.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            ConcecratedLand.SetActive(true);
            ConcecratedLand.transform.parent = null;
            ConcecrationLandActive = true;
            ConcecrationLandCurrentTime = ConcecrationLandTime;
        
    }

    void ConcecrationLandUpdate()
    {
        if (ConcecrationLandCurrentTime <= 0)
        {
            ConcecrationLandActive = false;
            
            ConcecratedLand.SetActive(false);
            ConcecratedLand.transform.parent = gameObject.transform;
            ConcecratedLand.transform.position = gameObject.transform.position;

        }
        else
            ConcecrationLandCurrentTime -= Time.deltaTime;
    }

    public void LayOnHands()
    {
        if (Target && !(Target.GetComponent<Stats>().Buff.Find(x => x.Buff.Icon == IconLayOnHands) != null))
        {
            Set2.FSkill4();
            EffectManager.AddDebuff(new Forbearance(Target, IconLayOnHands));
            Target.GetComponent<Stats>().Heal(MaxHealth);
        }
    }
    #endregion

    #region Set 3

    public void DivineShild()
    {
        if (Target)
        {
            Set3.FSkill1();
            ActiveDivineShild = true;
            StartCoroutine(DivineShildTime());
        }
    }

    IEnumerator DivineShildTime()
    {
        yield return new WaitForSeconds(8);
        ActiveDivineShild = false;
    }
    public void HandOfFreedom()
    {
        if (Target)
        {
            Set3.FSkill2();
            ActiveHandOfFreedom = true;
            StartCoroutine(HandOfFreedomTime());
        }
    }

    IEnumerator HandOfFreedomTime()
    {
        yield return new WaitForSeconds(8);
        ActiveHandOfFreedom = false;
    }

    public void ArdentDefender()
    {
        if (Target)
        {
            Set3.FSkill3();
            ActiveArdentDefender = true;
            StartCoroutine(ArdentDefenderTime());
        }
    }

    IEnumerator ArdentDefenderTime()
    {
        yield return new WaitForSeconds(8);
        ActiveArdentDefender = false;
    }

    public void LightoftheProtector()
    {
        if (Target)
        {
            Set3.FSkill4();
            if (OnConcecrationLand)
            {
                Heal((int)((MaxHealth - CurrentHealth) * 0.5));

            }
            else
                Heal((int)((MaxHealth - CurrentHealth) * 0.2));
        }
    }

    #endregion

    #region Set 4

    #region Skill 1
    public void DevotionAura()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveDevotionAura)
        {
            EffectManager.AddDebuff(new DevotionAura(gameObject, IconDevotionAura));
            ActiveDevotionAura = true;
            ActiveMagicalAura = false;
            ActiveRetributionAura = false;
        }

    }

    public void MagicalAura()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveMagicalAura)
        {
            EffectManager.AddDebuff(new MagicalAura(gameObject, IconMagicalAura));
            ActiveDevotionAura = false;
            ActiveMagicalAura = true;
            ActiveRetributionAura = false;
        }
    }

    public void RetributionAura()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveRetributionAura)
        {
            EffectManager.AddDebuff(new RetributionAura(gameObject, IconRetributionAura));
            ActiveDevotionAura = false;
            ActiveMagicalAura = false;
            ActiveRetributionAura = true;
        }
    }
    #endregion

    #region Skill 2
    public void SealOfRighteousness()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveSealOfRighteousness)
        {
            EffectManager.AddDebuff(new SealOfRighteousness(gameObject, IconSealOfRighteousness));
            ActiveSealOfRighteousness = true;
            ActiveSealOfLight = false;
            ActiveSealOfJustice = false;
        }
    }

    public void SealOfLight()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveSealOfLight)
        {
            EffectManager.AddDebuff(new SealOfLight(gameObject, IconSealOfLight));
            ActiveSealOfRighteousness = false;
            ActiveSealOfLight = true;
            ActiveSealOfJustice = false;
        }
    }

    public void SealOfJustice()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveSealOfJustice)
        {
            EffectManager.AddDebuff(new SealOfJustice(gameObject, IconSealOfJustice));
            ActiveSealOfRighteousness = false;
            ActiveSealOfLight = false;
            ActiveSealOfJustice = true;
        }
    }
    #endregion

    #region Skill 3
    public void JudgementOfRighteousness()
    {
        Sets.DeactivateSet4Skill();
        ActiveJudgementOfRighteousness = true;
        ActiveJudgementOfWisdom = false;
        ActiveJudgementOfWeakness = false;
    }

    public void JudgementOfWisdom()
    {
        Sets.DeactivateSet4Skill();
        ActiveJudgementOfRighteousness = false;
        ActiveJudgementOfWisdom = true;
        ActiveJudgementOfWeakness = false;
    }

    public void JudgementOfWeakness()
    {
        Sets.DeactivateSet4Skill();
        ActiveJudgementOfRighteousness = false;
        ActiveJudgementOfWisdom = false;
        ActiveJudgementOfWeakness = true;
    }
    #endregion

    #region Skill 4
    public void BlessingOfMight()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveBlessingOfMight)
        {
            EffectManager.AddDebuff(new BlessingOfMight(gameObject, IconBlessingOfMight));
            ActiveBlessingOfMight = true;
            ActiveBlessingOfWisdom = false;
            ActiveBlessingOfKings = false;
        }
    }

    public void BlessingOfWisdom()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveBlessingOfWisdom)
        {
            EffectManager.AddDebuff(new BlessingOfWisdom(gameObject, IconBlessingOfWisdom));
            ActiveBlessingOfMight = false;
            ActiveBlessingOfWisdom = true;
            ActiveBlessingOfKings = false;
        }
    }

    public void BlessingOfKings()
    {
        Sets.DeactivateSet4Skill();
        if (!ActiveBlessingOfKings)
        {
            EffectManager.AddDebuff(new BlessingOfKings(gameObject, IconBlessingOfKings));

            MaxHealth += (int)(MaxHealth * 0.1);
            MaxMana += (int)(MaxMana * 0.1);
            AttackSpeed -= 0.1f;
            ChanceCrit += 2;
            WeaponBaseDamage += (int)(WeaponBaseDamage * 0.05);

            ActiveBlessingOfMight = false;
            ActiveBlessingOfWisdom = false;
            ActiveBlessingOfKings = true;
        }

    }
    #endregion

    #endregion

}

public class HammerOfJustice : Effect
{


    //Time Of the Effect
    int TimeOfEfect = 6;
    float CurrentTime;


    public HammerOfJustice(GameObject InpTarget, Sprite InpIcon)
    {
        Target = InpTarget;
        CurrentTime = TimeOfEfect;

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));



        //Stop Target
        Target.GetComponent<Stats>().Slow(1);
    }
    public override void update()
    {
        //Wait until Stun ends
        if (CurrentTime <= 0)
        {
            Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));
            //Activate Target Movement
            Target.GetComponent<Stats>().Slow(-1);


            //Delete Effect
            Active = false;
        }
        else
        {
            CurrentTime -= Time.deltaTime;
            Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position).Buff.Cooldown = CurrentTime;

        }
    }

    
}

public class Forbearance : Effect
{
    //Time Of the Effect
    int TimeOfEfect = 30;
    float CurrentTime;


    public Forbearance(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
        CurrentTime = TimeOfEfect;

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));
    }

    public override void update()
    {
        //Wait until Stun ends
        if (CurrentTime <= 0)
        {

            Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

            //Delete Effect
            Active = false;
        }
        else
        {
            CurrentTime -= Time.deltaTime;
            Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position).Buff.Cooldown = CurrentTime;

        }
    }
}

public class DevotionAura : Effect
{
    public DevotionAura(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;        

        Me.Icon = InpIcon;
        Me.Cooldown = 0;

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
        else { 
        if (!Target.GetComponent<Paladin>().ActiveDevotionAura)
        {
            Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));
            //Delete Effect
            Active = false;
        }
    }
    }
}

public class MagicalAura : Effect
{
    public MagicalAura(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;

        Me.Icon = InpIcon;
        Me.Cooldown = 0;

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
            if (!Target.GetComponent<Paladin>().ActiveMagicalAura)
        {
            Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));
            //Delete Effect
            Active = false;
        }
    }
    }
}

public class RetributionAura : Effect
{
    public RetributionAura(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;

        Me.Icon = InpIcon;
        Me.Cooldown = 0;

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
            if (!Target.GetComponent<Paladin>().ActiveRetributionAura)
            {
                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));
                //Delete Effect
                Active = false;
            }
        }
    }
}

public class SealOfRighteousness : Effect
{    
    //Time Of the Effect
    int TimeOfEfect = 600;
    float CurrentTime;


    public SealOfRighteousness(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
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
            //Wait until Stun ends
            if (CurrentTime <= 0 || !Target.GetComponent<Paladin>().ActiveSealOfRighteousness)
        {
            Target.GetComponent<Paladin>().ActiveSealOfRighteousness = false;

            Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

            //Delete Effect
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
public class SealOfLight : Effect
{
    //Time Of the Effect
    int TimeOfEfect = 600;
    float CurrentTime;


    public SealOfLight(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
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
            //Wait until Stun ends
            if (CurrentTime <= 0 || !Target.GetComponent<Paladin>().ActiveSealOfLight)
            {
                Target.GetComponent<Paladin>().ActiveSealOfLight = false;

                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
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
public class SealOfJustice : Effect
{
    //Time Of the Effect
    int TimeOfEfect = 600;
    float CurrentTime;


    public SealOfJustice(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
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
            //Wait until Stun ends
            if (CurrentTime <= 0 || !Target.GetComponent<Paladin>().ActiveSealOfJustice)
            {
                Target.GetComponent<Paladin>().ActiveSealOfJustice = false;

                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
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

public class JudgementOfRighteousness : Effect
{
    //Time Of the Effect
    int TimeOfEfect = 20;
    float CurrentTime;


    public JudgementOfRighteousness(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
        CurrentTime = TimeOfEfect;

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));
        Target.GetComponent<TheBlackfathomDeeps>().TargetJudgementOfRighteousness = true;


    }

    public override void update()
    {
        if (Target == null)
        {
            Active = false;
        }
        else
        {
            //Wait until Stun ends
            if (CurrentTime <= 0)
            {

                Target.GetComponent<TheBlackfathomDeeps>().TargetJudgementOfRighteousness = false;

                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
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
public class JudgementOfWisdom : Effect
{
    //Time Of the Effect
    int TimeOfEfect = 20;
    float CurrentTime;


    public JudgementOfWisdom(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
        CurrentTime = TimeOfEfect;

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));
        Target.GetComponent<TheBlackfathomDeeps>().TargetJudgementOfWisdom = true;


    }

    public override void update()
    {
        if (Target == null)
        {
            Active = false;
        }
        else
        {
            //Wait until Stun ends
            if (CurrentTime <= 0)
            {

                Target.GetComponent<TheBlackfathomDeeps>().TargetJudgementOfWisdom = false;

                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
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
public class JudgementOfWeakness : Effect
{
    //Time Of the Effect
    int TimeOfEfect = 20;
    float CurrentTime;
    int MinBaseDmgTaken = 0;
    int MaxBaseDmgTaken = 0;

    public JudgementOfWeakness(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
        CurrentTime = TimeOfEfect;

        Me.Icon = InpIcon;
        Me.Cooldown = CurrentTime;

        Target.GetComponent<Stats>().EffectCount++;
        Position = Target.GetComponent<Stats>().EffectCount;
        Target.GetComponent<Stats>().Buff.Add(new Buffs(Me, Target.GetComponent<Stats>().EffectCount));

        MinBaseDmgTaken = (int)(Target.GetComponent<Stats>().MinBaseDmg * 0.08);
        MaxBaseDmgTaken = (int)(Target.GetComponent<Stats>().MaxBaseDmg * 0.08);
        Target.GetComponent<Stats>().MinBaseDmg -= MinBaseDmgTaken;
        Target.GetComponent<Stats>().MaxBaseDmg -= MaxBaseDmgTaken;
    }

    public override void update()
    {
        if (Target == null)
        {
            Active = false;
        }
        else
        {
            //Wait until Stun ends
            if (CurrentTime <= 0)
            {
                Target.GetComponent<Stats>().MinBaseDmg += MinBaseDmgTaken;
                Target.GetComponent<Stats>().MaxBaseDmg += MaxBaseDmgTaken;

                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
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

public class BlessingOfMight : Effect
{
    //Time Of the Effect

    public BlessingOfMight(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;

        Me.Icon = InpIcon;
        Me.Cooldown = 0;

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
            //Wait until Stun ends
            if (!Target.GetComponent<Paladin>().ActiveBlessingOfMight)
            {


                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
                Active = false;
            }
        }

    }
}
public class BlessingOfWisdom : Effect
{

    public BlessingOfWisdom(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;


        Me.Icon = InpIcon;
        Me.Cooldown = 0;

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
            //Wait until Stun ends
            if (!Target.GetComponent<Paladin>().ActiveBlessingOfWisdom)
            {


                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
                Active = false;
            }
        }

    }
}
public class BlessingOfKings : Effect
{

    public BlessingOfKings(GameObject inpTarget, Sprite InpIcon)
    {
        Target = inpTarget;
        

        Me.Icon = InpIcon;
        Me.Cooldown = 0;

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
            //Wait until Stun ends
            if (!Target.GetComponent<Paladin>().ActiveBlessingOfKings)
            {
                Target.GetComponent<Stats>().MaxHealth -= (int)(Target.GetComponent<Stats>().MaxHealth * 0.1);
                Target.GetComponent<Stats>().MaxMana -= (int)(Target.GetComponent<Stats>().MaxMana * 0.1);
                Target.GetComponent<Stats>().AttackSpeed += 0.1f;
                Target.GetComponent<Stats>().ChanceCrit -= 2;
                Target.GetComponent<Stats>().WeaponBaseDamage -= (int)(Target.GetComponent<Stats>().WeaponBaseDamage * 0.05);

                Target.GetComponent<Stats>().Buff.Remove(Target.GetComponent<Stats>().Buff.Find(x => x.ID == Position));

                //Delete Effect
                Active = false;
            }
        }
    }
}