using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float atackRange;
    public bool canHit = true;
    RaycastHit _hit;
    Ray _ray;
    Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
        GameManager.instance.playerLogic = this;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            PauseMenu();
            TryHit();
        }
    }
    public void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.PauseMenu();
        }
    }
    public void TryHit()
    {
        //Play animation
        if (Input.GetKey(KeyCode.Mouse0) && canHit)
        {
            canHit = false;
            //animator.SetTrigger("Hit");
            GameManager.instance.playerMovement.UseStamina(0.1f);
            HitObject();
            StartCoroutine(Hit());
        }
    }
    IEnumerator Hit()
    {
        yield return new WaitForSeconds(0.5f);
        canHit = true;
    }
    public bool Raycast()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(_ray, out _hit);
        if(_hit.collider != null)
        {
            if (Vector3.Distance(transform.position, _hit.collider.transform.position) < atackRange)
            {
                return true;
            }
        }     
        return false;
    }
    public void HitObject()
    {
        if (Raycast())
        {
            if (_hit.collider.CompareTag("Breakable"))
            {
                _hit.collider.GetComponent<Breakable>().TakeHit(1);
            }
        }   
    }
}