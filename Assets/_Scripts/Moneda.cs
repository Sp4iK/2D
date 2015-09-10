using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Moneda : MonoBehaviour {

	/* Declaramos una variable que nos indica la cantidad
	de puntos que tiene */
	public int puntos;

	// Declaramos el prefab de los puntos
	public GameObject puntosPrefab;

	// Declaramos el Transform de la moneda
	Transform transformMoneda;
	// Declaramos el Canvas para emparentar el literal cuando lo instanciemos
	public Canvas canvasUI;

	void Start () {
		transformMoneda = GetComponent<Transform>();
	}

	void OnTriggerEnter2D (Collider2D col) {
		// Comprobar si es el jugador el que ha tocado el trigger
		if (col.tag == "Player") {
			MostrarPuntos();
		}
	}

	void MostrarPuntos () {
		// Obtenemos el punto de la pantalla equivalente a la posicion de la moneda
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(transformMoneda.position);
		// Creamos la instancia de los puntos
		GameObject literalPuntos = (GameObject) Instantiate(puntosPrefab, screenPoint, Quaternion.identity);
		// Emparentamos el literal de puntos al Canvas
		literalPuntos.GetComponent<RectTransform>().SetParent(canvasUI.transform);
		// Asignamos al texto el valor correspondiente a la moneda
		literalPuntos.GetComponentInChildren<Text>().text = puntos.ToString();
	}
}
