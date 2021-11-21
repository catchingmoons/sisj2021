using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCoffee : MonoBehaviour
{
    CoffeeController coffee;
    public Reactive react;
    public bool on;

    void Awake()
    {
        react = GetComponent<Reactive>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!on) return;

        if (coffee == null && other.CompareTag("coffee"))
        {
            var newCoffee = other.GetComponent<CoffeeController>();
            if (newCoffee.fullness > 0.5f)
            {
                coffee = newCoffee;
                coffee.transform.parent = transform;
                coffee.transform.position = transform.position;
                coffee.fullness = 0f;

                react.react_pos();
            }
        }
    }
}
