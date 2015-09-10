using UnityEngine;
using System.Collections;

public class Boton : MonoBehaviour {

	// Declaramos una variable que nos indicara si el boton esta pulsado
	[HideInInspector] // Ocultamos la variable al inspector
	public bool pulsado;

	/* Con 'SerializeField' hacemos que una variable privada salga en el inspector
	[SerializeField]
	private bool probando; */

	public void BotonPresionado () {
		// Estamos pulsando el boton
		pulsado = true;
	}

	public void BotonLevantado () {
		// Hemos soltado el boton
		pulsado = false;
	}
}
