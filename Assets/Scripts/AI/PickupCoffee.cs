using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCoffee : MonoBehaviour
{
    CoffeeController coffee;
    public Reactive react;
    public float dist;
    public bool on;

    void Awake()
    {
        react = GetComponent<Reactive>();
    }

    void Update()
    {
        if (!on) return;

        if (coffee == null)
        {
            var allCoffees = FindObjectsOfType<CoffeeController>(); //very slow - but oh well!
            foreach (var coffee in allCoffees)
            {
                if ((Possession.currentFocus == null || Possession.currentFocus.gameObject != coffee.gameObject) && coffee.fullness > 0.5f 
                    && (coffee.transform.position - transform.position).sqrMagnitude <= dist)
                {
                    this.coffee = coffee;
                    coffee.transform.parent = transform;
                    coffee.transform.position = transform.position;
                    coffee.fullness = 0f;
                    react.react_pos();
                    return;
                }
            }
        }
    }
}
