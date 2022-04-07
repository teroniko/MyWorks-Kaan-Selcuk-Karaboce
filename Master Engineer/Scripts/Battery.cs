using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public float volt, amper, actual_mass;
    public float capasity;
    //Hız pili olsun o pilden çok bağlayınca hızı artsın
    //amperi fazla olan pil süre bakımından
    void Start()
    {
        actual_mass = GetComponent<Rigidbody>().mass *
            calculate_cylinder_volume(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.Find("Field").gameObject.SetActive(true);
        volt = transform.localScale.x / (transform.localScale.y * 2);
        amper = transform.localScale.y * 2 / transform.localScale.x;
        capasity = actual_mass * 20;
    }
    void Update()
    {
    }
    public float calculate_cylinder_volume(float x, float y, float z) {
        return 2 * Mathf.PI * x * z * y / 3000;
    }
}
