using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public UnityEngine.Rendering.Volume volume;
    public UnityEngine.Rendering.VolumeProfile whiteBlackVolume;
    public UnityEngine.Rendering.VolumeProfile simple;
    public UnityEngine.Rendering.Universal.ChromaticAberration chromatic;
    public Vector3 pos;
    public Transform upperPos;
    private CharacterController cc;
    public GameObject _death;
    public PlayerController playerc;
    private bool f = true;

    public List<GameObject> obj = new List<GameObject>();

    void Start()
    {
        playerc = upperPos.gameObject.GetComponent<PlayerController>();
        Debug.Log(whiteBlackVolume.components);
        cc = upperPos.gameObject.GetComponent<CharacterController>();
        if (!whiteBlackVolume.TryGet(out chromatic)) throw new System.NullReferenceException(nameof(chromatic));
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
        playerc.ff = false;
        for (int i = 0; i < obj.Count; i++)
        {
            StartCoroutine(nve(obj[i]));
            yield return new WaitForSeconds(1.5f);
            //TODO: REMAKE COROUTINE
        }
    }
    private IEnumerator nve(GameObject en)
    {
        cc.enabled = false;
        f = false;
        volume.profile = whiteBlackVolume;
        while (Vector3.Distance(upperPos.position, en.transform.position) > 3f)
        {
            upperPos.transform.LookAt(en.transform.position);
            upperPos.position = Vector3.Lerp(transform.position, new Vector3(en.transform.position.x, upperPos.position.y, en.transform.position.z), 3f * Time.deltaTime);
            if (Vector3.Distance(upperPos.position, en.transform.position) < 6f)
            {
                Time.timeScale -= 0.09f;
                chromatic.intensity.value += 0.08f;
                if (Time.timeScale < 0.1f)
                {
                    Time.timeScale = 0.2f;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        Instantiate(_death, en.transform.position, Quaternion.identity);
        Destroy(en.gameObject);
        chromatic.intensity.value = 0;
        cc.enabled = true;
        volume.profile = simple;
        Time.timeScale = 1;
        Debug.Log(f);
        if (en == obj[obj.Count-1])
        {
            playerc.ff = true;
            Debug.Log(en.name);
            Debug.Log(obj[obj.Count-1].name);
            upperPos.transform.position = new Vector3(0,1,upperPos.transform.position.z);
            obj.Clear();
            f = true;
        }
        //TODO: Animation attack
    }

}
