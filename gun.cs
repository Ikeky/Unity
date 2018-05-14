using UnityEngine;
using UnityEngine.UI;
public class gun : MonoBehaviour {
	Rigidbody rb;
	Transform tf;
	[Header("Аудио")]
	public AudioClip fire;
	public AudioClip reload;
	[Header("Текст")]
	public Text GUI_ammo;
	//[Header("Для хождения")]
	Quaternion OriginRotation;
	bool checkwall = true;

	//[Header("Input вещи и бег")]
	int recoildown;
	int recoilright;
	float Mouse_X;
	float Mouse_Y;
	float rbm;
	[Header("Нужные компоненты")]
	public Rigidbody objbul;
	public Transform Camera;
	float timefire = 0f;

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
        public int minrange;
        public int maxrange;
        public int Damage;
		public float firerate;
		public float timefire;
		public float dist;
		public void Fire (int Irecoilright,int Irecoildown,Transform ICamera,Rigidbody Iobjbul,int _damage,Vector3 p_tr){
            
			Quaternion bulrotX = Quaternion.AngleAxis (1,Vector3.up+new Vector3(0,Irecoilright,0));
			Quaternion bulrotY = Quaternion.AngleAxis (1,Vector3.right+new Vector3(0,Irecoildown,0));
			Rigidbody bullet = Instantiate (Iobjbul, dulo.GetComponent<Transform>().position, ICamera.rotation * bulrotY * bulrotX);
			bullet.AddForce (bullet.GetComponent<Transform>().forward * 7000f);
            bullet.GetComponent<bulletcollision>().Damage = _damage;
            bullet.GetComponent<bulletcollision>().p_tr = p_tr;
			ammo--;
            }
		public Gun(string aname,bool _enabled,int aammo,int amagazin,float asense,Animator aanime, GameObject aobj,GameObject adulo,int aammoempty,float afirerate, int adamage , float adist){
			name = aname;
			aammo = aammo;
			enabled = _enabled;
			magazin = amagazin;
			Mouse_Sens = asense;
			anime = aanime;
			dulo = adulo;
			obj = aobj;
			ammoempty = aammoempty;
			firerate = afirerate;
			Damage = adamage;
			dist = adist;
		}
		public Gun(){
			
		}
	}

	void Start(){
		
		rb = GetComponent<Rigidbody> ();
		tf = GetComponent<Transform>();
		OriginRotation = transform.rotation;
		//OriginPosition = new Vector3(0,1,0);
		Cursor.lockState = CursorLockMode.Locked;
		guns[0] = new Gun ("Sniper",false,30,90,5f,s_anime,s_obj,s_dulo,30,0.8f,70,5f);
		guns[1] = new Gun ("makarov",true,8,40,8f,m_anime,m_obj,m_dulo,8,2f,30,0.5f);
	}
	void Update(){
		rbm = Input.GetAxis("Fire2");
		Mouse_Y = Mathf.Clamp (Mouse_Y, -70, 60);
		Quaternion rotationX = Quaternion.AngleAxis (Mouse_X,Vector3.up);
		Quaternion rotationY = Quaternion.AngleAxis (-Mouse_Y,Vector3.right);
		Camera.rotation = OriginRotation * rotationX * rotationY;
		for (int i = 0; i < guns.Length; i++) {
			if (guns [i].enabled) {
				if (Input.GetButtonDown("Fire1") && Time.time >= timefire)
				{
					if (guns [i].ammo > 0) {
						timefire = Time.time + 1f / guns [i].firerate;
						recoildown = Random.Range (-4, 3);
						recoilright = Random.Range (-5, 5);
						guns [i].Fire (recoilright, recoildown, Camera, objbul, guns[i].Damage , tf.position );
						Mouse_Y += guns [i].dist;
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
			for (int i = 0; i < guns.Length; i++) {
				if (guns [i].enabled) {
					GUI_ammo.text = guns [i].ammo + "/" + guns [i].magazin;
					Mouse_X += Input.GetAxis ("Mouse X") * guns [i].Mouse_Sens;
					Mouse_Y += Input.GetAxis ("Mouse Y") * guns [i].Mouse_Sens;
				}
			}
		if (Input.GetKeyDown (KeyCode.R))
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
        if (Input.GetKeyDown (KeyCode.Keypad1)){
            guns[0].enabled = true;
            guns[0].obj.SetActive(true);
            guns[1].enabled = false;
            guns[1].obj.SetActive(false);
        } else if(Input.GetKeyDown (KeyCode.Keypad2)){
            guns[1].enabled = true;
            guns[1].obj.SetActive(true);
            guns[0].enabled = false;
            guns[0].obj.SetActive(false);
        }
        
		if (Input.GetKeyDown (KeyCode.Space)&& checkwall)
		{
			checkwall = false;
			rb.AddForce(tf.up * 5000);
		}
	}
	void OnCollisionEnter(Collision arg){
		if (arg.gameObject.layer == 9) {
			checkwall = true;
		}
	}
}
