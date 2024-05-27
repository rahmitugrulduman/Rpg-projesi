using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {

        Health health;
        private void Start()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if(health.IsDead() == true)
            {
                return;
            }

            if (InteractWithCombat() == true)
            {
                return;
            }
            if (InteractWithMovement()==true)
            {
                
                return;
            }
            
        }

     

        private bool InteractWithCombat()
        {
            
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if(target ==null)

                {
                    continue;
                }
                
                if(!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if(target==null)
                {
                    continue;
                }
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                    
                }
                return true;
            }
            return false;

        }

        private bool InteractWithMovement()
{
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit == true)
            {
                if(Input.GetMouseButton(0))
                {
                   
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
                

            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}

