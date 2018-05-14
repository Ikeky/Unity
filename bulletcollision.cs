using UnityEngine;
using UnityEngine.UI;

public class bulletcollision : MonoBehaviour
{
    public GameInstaller gameinstaller;
    public int Damage;
    public Vector3 p_tr;
    Transform instancebul;
    Transform tr;
    int damage;
    // Use this for initialization
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision arg)
    {
        Destroy(gameObject);
        if (arg.gameObject.tag == "glass")
        {
            Destroy(arg.gameObject);
        }
        else if(arg.gameObject.tag == "Enemy_Body" || arg.gameObject.tag == "Enemy_Head"){
            arg.gameObject.GetComponent<AiSystem>().takeDamage(Damage,p_tr);
        }
    }
}
