using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public UnityEngine.Rendering.Volume volume;
    public UnityEngine.Rendering.VolumeProfile whiteBlackVolume;
    public UnityEngine.Rendering.VolumeProfile simple;
    public Vector3 pos;
    public Transform upperPos;
    private CharacterController cc;
    public GameObject _death;
    private bool f;

    public List<GameObject> obj = new List<GameObject>();

    void Start()
    {
        cc = upperPos.gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(AttackDelay());
            //TODO: Start movement
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!obj.Contains(other.gameObject))
            {
                obj.Add(other.gameObject);
            }
        }
    }
    private IEnumerator AttackDelay()
    {
        for (int i = 0; i < obj.Count; i++)
        {
            StartCoroutine(nve(obj[i]));
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator nve(GameObject en)
    {
        cc.enabled = false;
        f = true;
        volume.profile = whiteBlackVolume;
        while (Vector3.Distance(upperPos.position, en.transform.position) > 4f)
        {
            var _dir = (en.transform.position - upperPos.position).normalized;
            var _lookRot = Quaternion.LookRotation(_dir);
            upperPos.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,_lookRot.y,0), Time.deltaTime * 15f);
            upperPos.position = Vector3.Lerp(transform.position, new Vector3(en.transform.position.x, upperPos.position.y, en.transform.position.z), 3f * Time.deltaTime);
            // if(Vector3.Distance(upperPos.position, en.transform.position)<7f){
            //     Time.timeScale -= 0.09f;
            //     if(Time.timeScale<0.1f){
            //         Time.timeScale = 0.2f;
            //     }
            // }
            yield return new WaitForEndOfFrame();
        }
        Instantiate(_death, en.transform.position, Quaternion.identity);
        Destroy(en.gameObject);
        // Time.timeScale = 1f;
        cc.enabled = true;
        volume.profile = simple;
        f = false;
    }

}
