using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mouse_actice : MonoBehaviour {
	public AudioClip fire;
	public AudioClip reload;
	public Transform bulfly;
	public Transform Camera;
	public Text GUI_ammo;
	public Text GUI_hp;
	public Rigidbody objbul;
	public Animator Anime;
	public Slider GUI_Slider;
	public int ammo = 30;
	public int magazin = 60;
	public float hp = 100;
	public int speed = 500;
	float v;
	float h;
	Quaternion OriginRotation;
	Vector3 OriginPosition;
	Vector3 newVelocity;
    Rigidbody rb;
	Transform tf;
	int recoildown;
	int recoilright;
    float Mouse_X;
	float Mouse_Y;
	float Mouse_Sens = 5;
	float rbm;
	bool run;
	bool checkwall = true;
    
    void Start () {
        rb = GetComponent<Rigidbody> ();
		tf = GetComponent<Transform>();
		OriginRotation = transform.rotation;
		OriginPosition = new Vector3(0,1,0);
		Cursor.lockState = CursorLockMode.Locked;
    }
	void Fire (){
		Quaternion bulrotX = Quaternion.AngleAxis (1,Vector3.up+new Vector3(0,recoilright,0));
		Quaternion bulrotY = Quaternion.AngleAxis (1,Vector3.right+new Vector3(0,recoildown,0));
		Rigidbody bullet = Instantiate (objbul, bulfly.position, Camera.rotation * bulrotY * bulrotX);
		bullet.AddForce (bullet.GetComponent<Transform>().forward * 7000f);
		gameObject.GetComponent<AudioSource>().clip = fire;
		gameObject.GetComponent<AudioSource> ().Play ();
		ammo--;
        Mouse_Y+=5f;
    }
	void Update () {
		GUI_Slider.value = hp;
		GUI_ammo.text = ammo + "/" + magazin;
		GUI_hp.text = "HP:"+hp;
		//Gravity
		newVelocity = rb.velocity + new Vector3(0, -9.81f, 0) * rb.mass * Time.deltaTime;
		//End Gravity
		Mouse_X += Input.GetAxis ("Mouse X") * Mouse_Sens;
		Mouse_Y += Input.GetAxis ("Mouse Y") * Mouse_Sens;
		Mouse_Y = Mathf.Clamp (Mouse_Y, -70, 60);
        rbm = Input.GetAxis("Fire2");
		v = Input.GetAxis("Vertical");
		h = Input.GetAxis("Horizontal");
		Quaternion rotationX = Quaternion.AngleAxis (Mouse_X,Vector3.up);
		Quaternion rotationY = Quaternion.AngleAxis (-Mouse_Y,Vector3.right);
		Camera.rotation = OriginRotation * rotationX * rotationY;
        
        recoildown = Random.Range(-3,4);
        recoilright = Random.Range(-6,6);

		if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
			run = true;
			speed = 700;
			Mouse_Sens = 4;
			Anime.SetBool("is_run", true);
			newVelocity =  newVelocity - new Vector3(newVelocity.x,0,newVelocity.z)*3*Time.deltaTime;
		} else if (Input.GetKeyUp (KeyCode.LeftShift) == true || Input.GetKeyUp (KeyCode.RightShift) == true){
			run = false;
			speed = 500;
			Mouse_Sens = 5;
			Anime.SetBool("is_run", false);
			newVelocity =  newVelocity - new Vector3(newVelocity.x,0,newVelocity.z)*8*Time.deltaTime;
		}
		if (v < 0 || v > 0) {
			move_z ();
		}
		if (h < 0 || h > 0) {
			move_x ();
		}
        if(rbm > 0.1f)
        {
            Anime.SetBool("is_aim", true);  
            recoildown = Random.Range(-1,1);
            recoilright = Random.Range(-1,1);
        }
        if (rbm < 0.1f)
        {
            Anime.SetBool("is_aim", false);
        }
        if (Input.GetButtonDown("Fire1"))
        {
			if (ammo > 0) {
				Fire ();
			}
        }
		if (Input.GetKeyDown (KeyCode.R) == true)
		{
			if (ammo != 30 && magazin != 0) {
				if ((magazin / 30f).ToString ("0") != "0") {
					magazin -= 30 - ammo;
					ammo += 30 - ammo;
					gameObject.GetComponent<AudioSource>().clip = reload;
					gameObject.GetComponent<AudioSource> ().Play ();
				} else if ((magazin / 30f).ToString ("0") == "0" && ammo + magazin <= 30) {
					ammo += magazin;
					magazin -= magazin;
					gameObject.GetComponent<AudioSource>().clip = reload;
					gameObject.GetComponent<AudioSource> ().Play ();

				} else {
					ammo += magazin;
					magazin -= magazin;
					gameObject.GetComponent<AudioSource>().clip = reload;
					gameObject.GetComponent<AudioSource> ().Play ();
				}
			}
        }
		if (Input.GetKeyDown (KeyCode.Space) == true && checkwall == true)
		{
			checkwall = false;
			rb.AddForce(tf.up * 1500);
		}
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
