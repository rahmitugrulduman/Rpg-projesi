using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;


namespace RPG.Combat

{
    public class Fighter : MonoBehaviour,IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage = 10f;

        Health targetObject;
        float timeSinceLastAttack;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (targetObject == null)
            {
                return;
            }
            if(targetObject.IsDead()==true)
            {
                GetComponent<Animator>().ResetTrigger("attack");
                Cancel();
                return;
            }
            

            if (GetIsInRange() == false)
            {
                GetComponent<Mover>().MoveTo(targetObject.transform.position);
            }
            else
            {
                AttackMethod();
                GetComponent<Mover>().Cancel();
            }
        }

        private void AttackMethod()
        {
            transform.LookAt(targetObject.transform);
            if(timeSinceLastAttack>timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;


            }

        }
        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget==null)
            {
                return false;
            }
            Health healthToTest = GetComponent<Health>();
            return healthToTest != null && !healthToTest.IsDead();
        }

        public void Attack(GameObject target)

        {
            GetComponent<ActionScheduler>().StartAction(this);
            targetObject = target.GetComponent<Health>();
        }


        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void Hit()
        {
            if(targetObject==null)
            {
                return;
            }
            targetObject.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetObject.transform.position) < weaponRange;
        }

       

        public void Cancel()
        {
            StopAttack();
            targetObject = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}

