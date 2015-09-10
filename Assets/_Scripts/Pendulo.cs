using UnityEngine;
using System.Collections;

public class Pendulo : MonoBehaviour {

	// Declaramos el Transform del pendulo
	Transform penduloTransform;
	// Declaramos la velocidad de giro
	public float velocidad;

	void Start () {
		// Obtenemos el componente Transform
		penduloTransform = GetComponent<Transform>();
	}

	void Update () {
		// Hacemos rotar el pendulo
		penduloTransform.Rotate(Vector3.forward * velocidad * Time.deltaTime);
	}
}
