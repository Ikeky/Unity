using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AiSystem : MonoBehaviour {
	public Slider GUI_Slider;
    public Text GUI_Identity;
	public Transform target;
    public GameObject waypoint;
	public float hp = 100;
    public GameObject HPLABEL;
    public GameInstaller gameinstaller;
	GameObject way;
	float time = 2; 
	bool pusk; 
	NavMeshAgent nav;
    Transform tf;
    public Vector3 setTarget;
	void Start () {
		way = Instantiate(waypoint,target.position,target.rotation);
		nav = GetComponent<NavMeshAgent> ();
        tf = GetComponent<Transform> ();
	}
	
	void Update () {
		GUI_Slider.value = hp;
        nav.SetDestination (setTarget);
		if (pusk) { 
			if (time >= 0) { 
				time -= Time.deltaTime;
			} else {
				GUI_Identity.gameObject.SetActive (false);
				pusk = false;
				time = 5;
			}
		}
	}
	void OnCollisionEnter(Collision arg){
		if (arg.gameObject.tag == "Player") {
			arg.gameObject.GetComponent<move> ().hp -= Random.Range(5,30);
			if (arg.gameObject.GetComponent<move> ().hp <= 0) {
				SceneManager.LoadScene ("scene_2");
			}
		} 
	}
    void OnTriggerEnter(Collider arg){
        if(arg.gameObject.tag == "steellabber"){
            Debug.Log("Он столкнулся");
        } else if(arg.gameObject.tag == "Player"){
			pusk = true;
			GUI_Identity.gameObject.SetActive (true);
            setTarget = target.position;
        }
    }
    void OnTriggerExit(Collider arg){
        if(arg.gameObject.tag == "player"){
			pusk = false;
			way.GetComponent<Transform> ().position = target.position;
            setTarget = way.gameObject.GetComponent<Transform>().position;
        }
    }
    public void takeDamage(int damage,Vector3 Player_tr)
    {
		way.GetComponent<Transform> ().position = target.position;
		setTarget = way.gameObject.GetComponent<Transform>().position;
		pusk = true;
        float Distance = Vector3.Distance(Player_tr, tf.position);
        if (Distance > 5)
        {
            damage += Random.Range(15,20);
        }
        else if (Distance < 5)
        {
            damage += Random.Range(50,60);
        }
        GameObject hplabel = Instantiate(HPLABEL, tf.position + new Vector3(Random.Range(-1, 1), 4, Random.Range(-1, 1)), tf.rotation);
        hplabel.GetComponent<AlwaysFace>().Target = target.gameObject;
        hplabel.GetComponent<Transform>().GetChild(0).GetComponent<TextMesh>().text = -damage + "HP";
        hp-=damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            gameinstaller.GUI_CASH += 140;
        }
    }
}
