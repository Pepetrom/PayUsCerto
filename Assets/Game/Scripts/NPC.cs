using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public int job;
    public float timeForMove;
    public NavMeshAgent ai;
    int tempJob;
    bool agressive = false;
    public GameObject AgressiveForm;
    public GameObject ore, pickaxe;
    private void Start()
    {
        ai = GetComponent<NavMeshAgent>();       
    }
    public void StartNPC(int job)
    {
        ChangeJob(job);
        StartCoroutine(DoJob());
    }
    public void ChangeJob(int job)
    {
        this.job = job;
    }
    private IEnumerator DoJob()
    {
        ore.SetActive(false);
        pickaxe.SetActive(false);
        if (GameManager.instance.NPCManager.UseFuel())
        {
            AgressiveForm.SetActive(false);
            agressive = false;
            ore.SetActive(false);
            pickaxe.SetActive(true);
            ai.speed = 3.5f;
            tempJob = job;
            ai.SetDestination(GameManager.instance.NPCManager.holes[job].position);
            yield return new WaitForSeconds(timeForMove + GameManager.instance.NPCManager.jobTime);
            pickaxe.SetActive(false);
            ore.SetActive(true);
            ai.SetDestination(GameManager.instance.NPCManager.baseReturn.position);
            yield return new WaitForSeconds(timeForMove);
            GameManager.instance.inventory.AddItems(tempJob);
            StartCoroutine(DoJob());
        }
        else
        {
            AgressiveForm.SetActive(true);
            agressive = true;
            ai.speed = 7;
            ai.SetDestination(GameManager.instance.playerMovement.transform.position);         
            yield return new WaitForSeconds(3);
            StartCoroutine(DoJob());
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && agressive)
        {
            GameManager.instance.SceneChange("Defeat");
        }
    }
}
