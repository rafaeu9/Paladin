using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sets : MonoBehaviour
{
    GameObject Set1;
    GameObject Set2;
    GameObject Set3;
    GameObject Set4;
    GameObject Set4Skill1;
    GameObject Set4Skill2;
    GameObject Set4Skill3;
    GameObject Set4Skill4;


    enum Acticated {Set1, Set2, Set3, Set4 , NULL};

    enum Set4SkillActicated { Set4Skill1, Set4Skill2, Set4Skill3, Set4Skill4, NULL };

    
    Acticated ActiveSet;
    Set4SkillActicated ActiveSet4Skills;

    // Start is called before the first frame update
    void Start()
    {
        Set1 = gameObject.transform.Find("Set 1").gameObject;
        Set2 = gameObject.transform.Find("Set 2").gameObject;
        Set3 = gameObject.transform.Find("Set 3").gameObject;
        Set4 = gameObject.transform.Find("Set 4").gameObject;
        Set4Skill1 = gameObject.transform.Find("Set Skill 1").gameObject;
        Set4Skill2 = gameObject.transform.Find("Set Skill 2").gameObject;
        Set4Skill3 = gameObject.transform.Find("Set Skill 3").gameObject;
        Set4Skill4 = gameObject.transform.Find("Set Skill 4").gameObject;

    }

    #region SetsActivate
    public void ActivateSet1()
    {
        if (!Set1.activeSelf)
        {
            DeactivateSet();
            Set1.SetActive(true);
            Set1.GetComponent<Animation>().Play("SetApear");
        }

    }
    public void ActivateSet2()
    {
        if (!Set2.activeSelf)
        {
            DeactivateSet();
            Set2.SetActive(true);
            Set2.GetComponent<Animation>().Play("SetApear");
        }
    }

    public void ActivateSet3()
    {
        if (!Set3.activeSelf)
        {
            DeactivateSet();
            Set3.SetActive(true);
            Set3.GetComponent<Animation>().Play("SetApear");
        }
    }

    public void ActivateSet4()
    {
        if (!Set4.activeSelf)
        {
            DeactivateSet();
            Set4.SetActive(true);
            Set4.GetComponent<Animation>().Play("SetApear");
        }
    }
    #endregion

    public void ActivateSet4Skill1()
    {
        if (!Set4Skill1.activeSelf)
        {
            //DeactivateSet4Skill();
            Set4Skill1.SetActive(true);
            Set4Skill1.GetComponent<Animation>().Play("SetApear");
        }

    }
    public void ActivateSet4Skill2()
    {
        if (!Set4Skill2.activeSelf)
        {
            //DeactivateSet4Skill();
            Set4Skill2.SetActive(true);
            Set4Skill2.GetComponent<Animation>().Play("SetApear");
        }
    }

    public void ActivateSet4Skill3()
    {
        if (!Set4Skill3.activeSelf)
        {
            //DeactivateSet4Skill();
            Set4Skill3.SetActive(true);
            Set4Skill3.GetComponent<Animation>().Play("SetApear");
        }
    }

    public void ActivateSet4Skill4()
    {
        if (!Set4Skill4.activeSelf)
        {
            //DeactivateSet4Skill();
            Set4Skill4.SetActive(true);
            Set4Skill4.GetComponent<Animation>().Play("SetApear");
        }
    }

    public void DeactivateSet()
    {

        if (Set1.activeSelf)
            ActiveSet = Acticated.Set1;
        else if (Set2.activeSelf)
            ActiveSet = Acticated.Set2;
        else if (Set3.activeSelf)
            ActiveSet = Acticated.Set3;
        else if (Set4.activeSelf)
            ActiveSet = Acticated.Set4;
        else
            ActiveSet = Acticated.NULL;

        StartCoroutine(EnDeactivateSet());
    }
    public IEnumerator EnDeactivateSet()
    {


        switch (ActiveSet)
        {
            case Acticated.Set1:
                Set1.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set1.SetActive(false);
                break;
            case Acticated.Set2:
                Set2.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set2.SetActive(false);
                break;
            case Acticated.Set3:
                Set3.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set3.SetActive(false);
                break;
            case Acticated.Set4:
                DeactivateSet4Skill();
                Set4.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set4.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void DeactivateSet4Skill()
    {

        if (Set4Skill1.activeSelf)
            ActiveSet4Skills = Set4SkillActicated.Set4Skill1;
        else if (Set4Skill2.activeSelf)
            ActiveSet4Skills = Set4SkillActicated.Set4Skill2;
        else if (Set4Skill3.activeSelf)
            ActiveSet4Skills = Set4SkillActicated.Set4Skill3;
        else if (Set4Skill4.activeSelf)
            ActiveSet4Skills = Set4SkillActicated.Set4Skill4;
        else
            ActiveSet4Skills = Set4SkillActicated.NULL;

        StartCoroutine(EnDeactivateSet4Skill());
    }
    public IEnumerator EnDeactivateSet4Skill()
    {


        switch (ActiveSet4Skills)
        {
            case Set4SkillActicated.Set4Skill1:
                Set4Skill1.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set4Skill1.SetActive(false);
                break;
            case Set4SkillActicated.Set4Skill2:
                Set4Skill2.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set4Skill2.SetActive(false);
                break;
            case Set4SkillActicated.Set4Skill3:
                Set4Skill3.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set4Skill3.SetActive(false);
                break;
            case Set4SkillActicated.Set4Skill4:
                Set4Skill4.GetComponent<Animation>().Play("SetDisapear");
                yield return new WaitForSeconds(0.3f);
                Set4Skill4.SetActive(false);
                break;
            default:
                break;
        }
    }
}
