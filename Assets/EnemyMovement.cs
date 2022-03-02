using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.Tilemaps;
using System.Linq;

namespace PathFinding
{





    public class EnemyMovement : MonoBehaviour
    {

        [SerializeField] private Pathfinding2D enemyMovement;
        public GameObject[] enemies;
        [SerializeField] public List<Unit> selectedEnemies;
        [SerializeField] Unit currentEnemy;
        [SerializeField] private Tilemap map;
        private bool grabed;
        private bool selectedNewSpace;
        GameObject[] allies;

        [SerializeField] private TurnSystem.TurnSystem turnSystem;

        void Start()
        {
           
           

        }


        void Update()
        {

            if (enemies.Length == 0)
            {
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                allies = GameObject.FindGameObjectsWithTag("Ally");

                ConvertUnits();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentEnemy = selectedEnemies[0];
                enemyMovement = currentEnemy.GetComponent<Pathfinding2D>();
                SearchForAllies();
            }
            

        }


        void ConvertUnits()
        {

            foreach (GameObject unit in enemies)
            {
                selectedEnemies.Add(unit.GetComponent<Unit>());
            }

        }

        void SearchForAllies()
        {
            float distanciaMinima = 5f;
            GameObject currentTarget = null;
            float distanciaTmp = distanciaMinima;

            for (int i = 0; i < allies.Length; i++)
            {
                float distanciaMinimaActual = Vector3.Distance(currentEnemy.transform.position, allies[i].transform.position);

                if (distanciaMinimaActual <= distanciaTmp)
                {
                    distanciaTmp = distanciaMinimaActual;
                    currentTarget = allies[i];
                }
            }

            if (currentTarget != null)
            {
                enemyMovement.FindPath(currentEnemy.transform.position, currentTarget.transform.position);
                currentTarget = null;
                Move(enemyMovement);
            }

        }


        void Move(Pathfinding2D unitPath)
        {
            currentEnemy.path.SetActive(false);
            foreach (var t in unitPath.path.Take(unitPath.path.Count - 1))
            {
                currentEnemy.transform.DOMove(t.worldPosition, 1f, true);
                currentEnemy.hasMoved = true;
            }
        }

    }

}