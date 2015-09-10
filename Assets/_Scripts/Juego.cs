using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Juego : MonoBehaviour {

	// Declaramos las variables de vidas, puntos y el record
	public int vidas, puntos, record;

	// Declaramos las cajas de texto del UI
	public Text txtPuntos, txtVidas, txtRecord;

	// Declaramos el Animator del panel que muestra la ventana de Game Over y de Ganar
	public Animator animatorPanelGameOver, animatorPanelGanar;
	// Declaramos los Animators de las imagenes que muestran las vidas
	public Animator[] animatorsVidas;

	// Declaramos la cantidad de monedas que tiene el nivel
	int totalMonedas;

	void Start () {
		// Asociamos el evento de perder vida al metodo RestaVida
		Jugador.OnPerderVida += RestaVida;
		// Asociamos el evento de coger puntos al metodo SumaPuntos
		Jugador.OnCogerPuntos += SumaPuntos;

		// Recuperamos el record anterior y lo asignamos
		record = PlayerPrefs.GetInt("Record", 0);
		txtRecord.text = "RECORD: " + record.ToString();

		// Mostramos el numero de vidas al inicio
		txtVidas.text = vidas.ToString() + " VIDAS";

		// Contamos todas las monedas que hay en el nivel
		totalMonedas = FindObjectsOfType<Moneda>().Length;
	}

	void Update () {
		// Si pulsamos el escape salimos del juego
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	void SumaPuntos (int puntosRecividos) {
		// Sumamos los puntos de la moneda
		puntos += puntosRecividos;
		// Asignamos a la caja de texto los puntos
		txtPuntos.text = puntos.ToString() + " PUNTOS";
		// Restamos una moneda a la cantidad total de monedas
		totalMonedas--;

		// Comprobamos si ya hemos recogido todas las monedas
		if (totalMonedas == 0) {
			// Terminamos la partida
			animatorPanelGanar.enabled = true;

			// Recuperamos el record anterior
			record = PlayerPrefs.GetInt("Record", 0);
			// Comprobamos si el record anterior es inferior a los puntos actuales
			if (record < puntos) {
				// Grabamos el nuevo record
				PlayerPrefs.SetInt("Record", puntos);
			}
		}
	}

	void RestaVida () {
		// Quitamos una vida
		vidas--;
		// Ocultamos la imagen de la vida que hemos perdido
		animatorsVidas[vidas].enabled = true;
		txtVidas.text = vidas.ToString() + " VIDAS";
		// Restamos 10 puntos por penalizacion
		puntos -= 10;
		if (puntos < 0) {puntos = 0;} // Controlamos que no puedan haber puntos negativos
		txtPuntos.text = puntos.ToString() + " PUNTOS";

		// Si hemos terminado la partida, miramos si hay que grabar el record
		if (vidas == 0) {
			txtVidas.text = "¡HAS MUERTO!";
			// Recuperamos el record anterior
			record = PlayerPrefs.GetInt("Record", 0);
			// Comprobamos si el record anterior es inferior a los puntos actuales
			if (record < puntos) {
				// Grabamos el nuevo record
				PlayerPrefs.SetInt("Record", puntos);
			}

			GameOver();
		}
	}

	void GameOver () {
		// Paramos el juego
		Jugador.estadoJuego = EstadoJuego.Parado;
		// Habilitamos el Animator del panelGameOver
		animatorPanelGameOver.enabled = true;
	}

	public void Reiniciar () {
		// Recargamos la escena en la que nos encontramos
		Application.LoadLevel(Application.loadedLevel);
	}
}
