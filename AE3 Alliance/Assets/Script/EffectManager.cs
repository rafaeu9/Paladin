using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    static List<Effect> Effects = new List<Effect>();
 
    void Update()
    {
        
        for (int i = 0; i < Effects.Count; i++)
        {
            Effects[i].update();

            if(!Effects[i].Active)
            {                                
                Effects.RemoveAt(i);                
            }
        }
    }

    static public void AddDebuff(Effect add)
    {
        Effects.Add(add);        
    }
}

public abstract class Effect
{
    public bool Active = true;
    protected GameObject Target;
    public Sprite Icon;

    protected int Position;
    protected Effects Me;


    //public abstract void init();
    public abstract void update();   

}
