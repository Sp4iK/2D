using UnityEngine;
using System.Collections;

public class FondoParallax : MonoBehaviour {

	// Declaramos la velocidad a la que queremos que se mueva el fondo
	public float velocidad;
	// Declaramos el Renderer que dibuja el material
	Renderer rendererParallax;
	
	void Start () {
		rendererParallax = GetComponent<Renderer>();
	}

	void Update () {
		// Calcular el desplazamiento de la textura
		Vector2 desplamientoActual = rendererParallax.material.mainTextureOffset;
		desplamientoActual = desplamientoActual + Vector2.right * velocidad * Time.deltaTime;
		rendererParallax.material.mainTextureOffset = desplamientoActual;
	}
}
