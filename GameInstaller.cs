using UnityEngine;
using UnityEngine.UI;

public class GameInstaller : MonoBehaviour {
	public int GUI_CASH = 0;
	public Text GUI_textcash;
	public bulletcollision bul_script;
    public GameObject Zombie;
	public GameObject Player;
    public Transform[] spawn;
    public Transform[] WaySpawn;
    GameObject _Zombie;

	void Start () {
        InvokeRepeating("SpawnMob",6f,20f);
		bul_script.gameinstaller = GetComponent<GameInstaller> ();
	}

	void Update () {
		if (GUI_CASH == 0) {
			GUI_textcash.text = "$CASH: 0$";
		} else {
			GUI_textcash.text = "$CASH: "+GUI_CASH+"$";
		}
	}
    void SpawnMob(){
        Debug.Log(spawn[0].position);
        _Zombie = Instantiate(Zombie,spawn[0].position,spawn[0].rotation);
        _Zombie.SetActive(true);
        _Zombie.GetComponent<AiSystem>().setTarget = WaySpawn[0].position;
        Debug.Log("Он идёт");
    }
}
