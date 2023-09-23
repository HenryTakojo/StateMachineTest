using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    private float lifeTime = 2.0f;
    private float currentTime = 0;

    [SerializeField] private Collider myCollider;

    [SerializeField] private int damage;

    private List<Collider> alreadyCollidedWith = new List<Collider>();
    

    // Update is called once per frame
    void Update()
    {
        PhysicsCollide();

        currentTime += Time.deltaTime;
        if (currentTime > lifeTime)
        {
            if (this.name == "IceArrow(Clone)")
            {
                Destroy(gameObject);
                currentTime = 0;
            }
            this.gameObject.SetActive(true);
            currentTime = 0;
        }

        transform.position += transform.forward * 20 * Time.deltaTime;
    }

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void PhysicsCollide()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                if (alreadyCollidedWith.Contains(collider)) { return; }
                alreadyCollidedWith.Add(collider);
                DealDamage(collider.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //適用於角色身上有武器也有collider，可套用在丘丘人
        //if(collision == myCollider) { return; }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (alreadyCollidedWith.Contains(collision)) { return; }
            alreadyCollidedWith.Add(collision);

            DealDamage(collision.gameObject);
        }
    }

    private void DealDamage(GameObject Enemy)
    {
        // Enemy Health Decrease
        if(Enemy.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(this.damage);
        }
    }
    public void SetAttack(int damage)
    {
        this.damage = damage;
    }
}
