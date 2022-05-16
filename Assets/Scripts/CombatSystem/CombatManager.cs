using System.Collections;
using DG.Tweening;
using PathFinding;
using UnityEditor;
using UnityEngine;

namespace CombatSystem
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private GameObject combatStation;
        [SerializeField] private CombatSpace combatSpace;
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject button;
        [SerializeField] private Camera mainCamera;
        private Vector3 _prevPos, _unit1PrevPos, _unit2PrevPos;
        private Quaternion _unit2PrevRot;

        [SerializeField] private ButtonBehaviour fader;

        [SerializeField] private UnitObstacle unitObstacle;
        [SerializeField] private Pathfinding2D movSystem;

        public IEnumerator MoveToCombat(Unit unit, Unit unit2)
        {
            yield return new WaitForSeconds(1);

            button.SetActive(false);
            _prevPos = new Vector3(0, 0, -10) + unit2.transform.position;
            mainCamera.transform.position = new Vector3(0, -39, -11);
            mainCamera.orthographicSize = 1.5f;
            canvas.SetActive(false);
            combatStation.SetActive(true);
            
            SetScene(unit, unit2);
        }

        private void SetScene(Unit unit, Unit unit2)
        {
            combatSpace.UpdateCardValue(unit, unit2);
            combatSpace.UpdateTitlesValue();

            _unit1PrevPos = unit.transform.position;
            _unit2PrevPos = unit2.transform.position;
            _unit2PrevRot = unit2.transform.rotation;
            unit.transform.position = new Vector3(0, 0, -10) + combatSpace.position1.position;
            unit2.transform.position = new Vector3(0, 0, -10) + combatSpace.position2.position;

            unit2.transform.rotation = combatSpace.position2.rotation;

            if (unit.unitSide == "Enemy" || unit2.unitSide == "Enemy")
            {
                StartCoroutine(StartCombat(unit, unit2));
            }
            else
            {
                StartCoroutine(StartHeal(unit, unit2));
            }
        }

        private IEnumerator StartCombat(Unit unit, Unit unit2)
        {
            unit.anim.SetTrigger("Attack");
            
            unit.Attack(unit2, unit);

            yield return new WaitForSeconds(2);
            
            unit2.anim.SetTrigger("Attack");
            

            if (unit.unitSide == "Ally")
            {
                unit.hasAttacked = true;
            }

            yield return new WaitForSeconds(1);

            combatSpace.UpdateCardValue(unit, unit2);
            combatSpace.UpdateTitlesValue();
            
            StartCoroutine(BackToOverWorld(unit, unit2));
        }
        
        private IEnumerator StartHeal(Unit unit, Unit unit2)
        {
            unit.anim.SetTrigger("Attack");

            yield return new WaitForSeconds(1);

            unit.Heal(unit2);
            unit.hasAttacked = true;
            
            yield return new WaitForSeconds(1);
            
            combatSpace.UpdateCardValue(unit, unit2);
            combatSpace.UpdateTitlesValue();
            
            StartCoroutine(BackToOverWorld(unit, unit2));
        }

        private IEnumerator BackToOverWorld(Unit unit, Unit unit2)
        {
            yield return new WaitForSeconds(1);
            
            fader.FadeOutCombat();

            unit.transform.position = _unit1PrevPos;
            unit2.transform.position = _unit2PrevPos;
            
            unit2.transform.rotation = _unit2PrevRot;

            movSystem = unit.GetComponent<Pathfinding2D>();
            unitObstacle.ClearObstacleMap();
            movSystem.UpdateGrid();

            mainCamera.transform.position = _prevPos;
            mainCamera.orthographicSize = 3.5f;
            canvas.SetActive(true);
            combatStation.SetActive(false);
            button.SetActive(false);
        }
    }
}
