using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private Unit unit1, unit2;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private float distance;

    void Start()
    {
        unit1 = GetComponentInParent<Unit>();
        buttonContainer.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ally"))
        {
            unit2 = other.GetComponent<Unit>();
            
            if (unit2.className == "Sniper")
            {
                buttonContainer.SetActive(true);
            }
            else
            {
                StartCoroutine(CalDistance(unit1, unit2));
            }

            if (unit2.hasAttacked)
            {
                buttonContainer.SetActive(false);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        //var distance = Vector3.Distance(unit1.transform.position, unit2.transform.position);
        
        if (other.CompareTag("Ally"))
        {
            if (unit2.unitName == "Sniper")
            {
                buttonContainer.SetActive(false);
            }
            else
            {
                // if (distance > 0.6)
                // {
                //     buttonContainer.SetActive(false);
                // }
            }
        }
    }

    public void Combat()
    {
        if (unit2.hasAttacked == false)
        {
            StartCoroutine(unit2.Attack(unit1));

            unit2.hasAttacked = true;
            buttonContainer.SetActive(false);
        }
    }

    public IEnumerator CalDistance(Unit unit1, Unit unit2)
    {
        yield return new WaitForSeconds(0.5f);
        
        distance = Vector3.Distance(unit1.transform.position, unit2.transform.position);
        
        if (distance <= 1)
        {
            buttonContainer.SetActive(true);
        }
    }
}
