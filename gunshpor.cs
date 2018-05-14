using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class gunshpor : MonoBehaviour {
	Rigidbody rb;
	Transform tf;
	[Header("Аудио")]
	public AudioClip fire;
	public AudioClip reload;
	[Header("Текст")]
	public Text GUI_ammo;
	public Text GUI_hp;
	public Slider GUI_Slider;
	[Header("Статистика Игрока")]
	public float hp = 100;
	public int speed = 500;
	//[Header("Для хождения")]
	Quaternion OriginRotation;
	Vector3 OriginPosition;
	Vector3 newVelocity;
	bool run;
	bool checkwall = true;
	float v;
	float h;

	//[Header("Input вещи и бег")]
	int recoildown;
	int recoilright;
	float Mouse_X;
	float Mouse_Y;
	float rbm;
	[Header("Нужные компоненты")]
	public Rigidbody objbul;
	public Transform Camera;

	[Header("Компоненты Снайперки")]
	public GameObject s_obj;
	public Animator s_anime;
	public GameObject s_dulo;

	[Header("Компоненты Макарова")]
	public GameObject m_obj;
	public Animator m_anime;
	public GameObject m_dulo;

	public Gun[] guns = new Gun[2];

	public class Gun{
		public string name;
		public bool enabled = false;
		public GameObject obj;
		public GameObject dulo;
		public Animator anime;
		public int ammo;
		public int magazin;
		public float Mouse_Sens = 5;
		public int ammoempty;
		public void Fire (int Irecoilright,int Irecoildown,Transform ICamera,Rigidbody Iobjbul){
			Quaternion bulrotX = Quaternion.AngleAxis (1,Vector3.up+new Vector3(0,Irecoilright,0));
			Quaternion bulrotY = Quaternion.AngleAxis (1,Vector3.right+new Vector3(0,Irecoildown,0));
			Rigidbody bullet = Instantiate (Iobjbul, dulo.GetComponent<Transform>().position, ICamera.rotation * bulrotY * bulrotX);
			bullet.AddForce (bullet.GetComponent<Transform>().forward * 7000f);
			ammo--;
		}
		public Gun(string aname,bool _enabled,int aammo,int amagazin,float asense,Animator aanime, GameObject aobj,GameObject adulo,int aammoempty){
			name = aname;
			aammo = aammo;
			enabled = _enabled;
			magazin = amagazin;
			Mouse_Sens = asense;
			anime = aanime;
			dulo = adulo;
			obj = aobj;
			ammoempty = aammoempty;
		}
		public Gun(){

		}
	}

	void Start(){

		rb = GetComponent<Rigidbody> ();
		tf = GetComponent<Transform>();
		OriginRotation = transform.rotation;
		OriginPosition = new Vector3(0,1,0);
		Cursor.lockState = CursorLockMode.Locked;
		guns[0] = new Gun ("Sniper",false,30,90,5f,s_anime,s_obj,s_dulo,30);
		guns[1] = new Gun ("makarov",true,8,40,8f,m_anime,m_obj,m_dulo,8);
	}
	void Update(){
		// Все коды с настройкой инпутов
		rbm = Input.GetAxis("Fire2");
		Mouse_Y = Mathf.Clamp (Mouse_Y, -70, 60);
		// ^^^ Все коды с настройкой инпутов
		// Все коды с настройкой шага
		v = Input.GetAxis("Vertical");
		h = Input.GetAxis("Horizontal");
		Quaternion rotationX = Quaternion.AngleAxis (Mouse_X,Vector3.up);
		Quaternion rotationY = Quaternion.AngleAxis (-Mouse_Y,Vector3.right);
		Camera.rotation = OriginRotation * rotationX * rotationY;
		// ^^^ Все коды с настройкой шага

		// Все коды с классом gun
		if (Input.GetButtonDown("Fire1"))
		{
			for (int i = 0; i < guns.Length; i++) {
				if (guns [i].enabled) {
					if (guns [i].ammo > 0) {
						recoildown = Random.Range (-4, 3);
						recoilright = Random.Range (-5, 5);
						guns [i].Fire (recoilright, recoildown, Camera, objbul);
					}
				}
			}
		}
		if(rbm > 0.1f)
		{
			for (int i = 0; i < guns.Length; i++) {
				if (guns [i].enabled) { 
					recoildown = Random.Range (-12, -10);
					recoilright = Random.Range (-12, -10);
					guns[i].anime.SetBool ("is_aim", true); 
				}
			}
		}
		if (rbm < 0.1f)
		{
			for (int i = 0; i < guns.Length; i++) {
				if (guns [i].enabled) {
					guns [i].anime.SetBool ("is_aim", false);
				}
			}
		}
		// НАСТРОЙКА ИНПУТОВ И ТЕКСТОВ
		for (int i = 0; i < guns.Length; i++) {
			if (guns [i].enabled) {
				GUI_ammo.text = guns [i].ammo + "/" + guns [i].magazin;
				Mouse_X += Input.GetAxis ("Mouse X") * guns [i].Mouse_Sens;
				Mouse_Y += Input.GetAxis ("Mouse Y") * guns [i].Mouse_Sens;
			}
		}
		// ^^^ Все строки с инпутами и текстами
		// ^^^ Все строки с работой классами gun
		// Работа с текстом и слайдером
		GUI_Slider.value = hp;
		GUI_hp.text = "HP:"+hp;
		// ^^^ Все строки работой с текстом
		//Работа с гравитацией
		newVelocity = rb.velocity + new Vector3(0, -9.81f, 0) * rb.mass * Time.deltaTime;
		// ^^^ Все строки с работой Gravity
		// Работа с шагом
		if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
			run = true;
			speed = 800;
			for (int i = 0; i < guns.Length; i++) {
				if (guns [i].enabled) {
					guns [i].Mouse_Sens = 4;
					guns [i].anime.SetBool ("is_run", true);
				}
			}
			newVelocity =  newVelocity - new Vector3(newVelocity.x,0,newVelocity.z)*4*Time.deltaTime;
		} else if (Input.GetKeyUp (KeyCode.LeftShift) == true || Input.GetKeyUp (KeyCode.RightShift) == true){
			run = false;
			speed = 500;
			for (int i = 0; i < guns.Length; i++) {
				if (guns [i].enabled) {
					guns [i].Mouse_Sens = 5;
					guns [i].anime.SetBool ("is_run", false);
				}
			}
			newVelocity =  newVelocity - new Vector3(newVelocity.x,0,newVelocity.z)*8*Time.deltaTime;
		}
		if (v < 0 || v > 0) {
			move_z ();
		}
		if (h < 0 || h > 0) {
			move_x ();
		}
		// ^^^ вся Работа с шагом
		// Работа с перезарядкой
		if (Input.GetKeyDown (KeyCode.R) == true)
		{
			for (int i = 0; i < guns.Length; i++) {
				if (guns [i].enabled) {
					if (guns[i].ammo != guns[i].ammoempty && guns[i].magazin !=0) {
						if ((guns [i].magazin / guns[i].ammoempty).ToString ("0") != "0") {
							guns [i].magazin -= guns[i].ammoempty - guns [i].ammo;
							guns [i].ammo += guns[i].ammoempty - guns [i].ammo;
							gameObject.GetComponent<AudioSource> ().clip = reload;
							gameObject.GetComponent<AudioSource> ().Play ();
						}else if ((guns [i].magazin / guns[i].ammoempty).ToString ("0") == "0" && guns[i].ammo + guns[i].magazin <= guns[i].ammoempty) {
							guns [i].ammo += guns [i].magazin;
							guns [i].magazin = 0;
							gameObject.GetComponent<AudioSource> ().clip = reload;
							gameObject.GetComponent<AudioSource> ().Play ();

						} else {
							guns [i].ammo += guns [i].magazin;
							guns [i].magazin -= guns [i].magazin;
							gameObject.GetComponent<AudioSource> ().clip = reload;
							gameObject.GetComponent<AudioSource> ().Play ();
						}
					}
				}
			}
		}
		// ^^^ вся работа с перезарядкой
		// Работа с прыжком
		if (Input.GetKeyDown (KeyCode.Space) == true && checkwall == true)
		{
			checkwall = false;
			rb.AddForce(tf.up * 1500);
		}
		// ^^^ вся работа с прыжком
	}
	void move_z (){
		rb.AddForce(Vector3.Cross(Camera.right * v * speed,OriginPosition));
		newVelocity =  newVelocity - new Vector3(newVelocity.x,0,newVelocity.z)*8*Time.deltaTime;
		rb.velocity = newVelocity;
	}
	void move_x (){
		rb.AddForce(Vector3.Cross(Camera.forward * -h * speed,OriginPosition));
		newVelocity =  newVelocity - new Vector3(newVelocity.x,0,newVelocity.z)*8*Time.deltaTime;
		rb.velocity = newVelocity;
	}
	void OnCollisionEnter(Collision arg){
		if (arg.gameObject.layer == 9 || arg.gameObject.tag == "doroga") {
			checkwall = true;
		}
	}
}
