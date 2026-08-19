using System.Threading;
using UnityEngine;

public class Lampara : Interactuable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public GameObject Bombilla;
	Light Luz;
    
    private bool bSeEstaPrendiendo;
    public float TimerTime;
    private float TimerCount;

    enum EstadoLampara
    {
        None = -1,
        Encendido,
        Apagado,
        Pipa
    }

    EstadoLampara estado;
	void Start()
    {
        Luz = Bombilla.GetComponent<Light>();

        TimerCount = 0;
        bSeEstaPrendiendo = false;

        estado = EstadoLampara.Apagado;
	}

    // Update is called once per frame
    void Update()
    {
        if(bSeEstaPrendiendo)
        {
            TimerCount += Time.deltaTime;
            Luz.intensity = Mathf.Lerp(0, 10, TimerCount / TimerTime);
			if (TimerCount >= TimerTime)
            {
		        //Luz.enabled = !Luz.enabled;
                bSeEstaPrendiendo = false;
                TimerCount = 0;
                estado = EstadoLampara.Encendido;
            }
        }
	}

	public override void Interactuar()
    {
        estado = EstadoLampara.Encendido;
        switch (estado)
        {
            case EstadoLampara.None:
                break;
            case EstadoLampara.Encendido:
                bSeEstaPrendiendo = true;
                //Luz.enabled = !Luz.enabled;
                Luz.enabled = true;
                break;
            case EstadoLampara.Apagado:
                break;
            case EstadoLampara.Pipa:
                break;
            default:
                break;
        }

        
	}
}
