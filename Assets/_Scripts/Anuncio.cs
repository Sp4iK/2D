using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class Anuncio : MonoBehaviour {

	// Declaramos una variable que nos dice si ya hemos mostrado el anuncio
	static bool anuncioMostrado = false;
	
	void Start () {
		Advertisement.Initialize ("55533", true);
		//anuncioMostrado = false;
	}
	
	void Update () {
		// Si el anuncio esta listo y no se ha mostrado aun...
		if (Advertisement.isReady() && !anuncioMostrado) {
			// ...paramos el juego y mostramos el anuncio
			Jugador.estadoJuego = EstadoJuego.Parado;
			Advertisement.Show();
			anuncioMostrado = true;
		} else {
			// sino significa que ya se ha mostrado por tanto reanudamos el juego
			Jugador.estadoJuego = EstadoJuego.Jugando;
		}
	}
}