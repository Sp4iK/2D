using UnityEngine;
using System.Collections;

public class Jugador : MonoBehaviour {

	// Declaramos una variable para controlar el estado del juego
	[HideInInspector]
	public static EstadoJuego estadoJuego;

	// Declaramos una velocidad a la que se mueve el pollo
	public float velocidad;
	// Declaramos una fuerza de impulso para el salto
	public float fuerzaSalto;
	// Declaramos el componente Rigidbody2D del pollo
	Rigidbody2D rigidbodyPollo;
	// Declaramos el componente Animator del pollo
	Animator animatorPollo;
	// Declaramos una variable que nos indica si el pollo esta saltando
	bool saltando;
	/* Declaramos una variable que nos indica si el pollo esta suspendido
	en el aire sin que hayamos saltado primero */
	bool cayendo;

	// Declaramos el Transform de la posicion de inicio
	public Transform transformInicio;
	// Declaramos el Transform del pollo
	Transform transformPollo;

	/*SONIDO*/
	// Declaramos el AudioSource para hacer sonar los sonidos
	AudioSource audioSourcePollo;
	// Declaramos una zona para los sonidos
	public AudioClip clipSalto, clipMuerte, clipMoneda;

	/*EVENTOS*/
	// Declaramos los eventos que lanzara el pollo
	public delegate void PolloAction();
	public delegate void PuntosAction(int puntos);
	public static event PolloAction OnPerderVida;
	public static event PuntosAction OnCogerPuntos;

#if UNITY_ANDROID || UNITY_IOS
	// Declaramos los botones de izquierda y derecha
	public Boton botonDerecha, botonIzquierda;
#endif

	void Awake () {
		OnPerderVida = null;
		OnCogerPuntos = null;
	}

	void Start () {
		// Fijamos el estado del juego
		estadoJuego = EstadoJuego.Jugando;

		// Obtenemos el componente Rigidbody2D
		rigidbodyPollo = GetComponent<Rigidbody2D>();
		// Obtenemos el componente Animator
		animatorPollo = GetComponent<Animator>();
		// Obtenemos el AudioSource
		audioSourcePollo = GetComponent<AudioSource>();
		// Obtenemos el Transform del pollo
		transformPollo = GetComponent<Transform>();
	}

	void Update () {
		// Comprobamos que no se este mostrando un menu
		if (estadoJuego != EstadoJuego.Parado) {
			#if UNITY_ANDROID || UNITY_IOS
			// Recogemos el input del teclado/gamepad o de la pantalla tactil
			float horizontal = (botonDerecha.pulsado) ? 1f : (botonIzquierda.pulsado) ? -1f : (Input.GetAxis("Horizontal") != 0f) ? Input.GetAxis("Horizontal") : 0f;

			// Recogemos todas las pulsaciones de los dedos
			Touch[] pulsaciones = Input.touches;
			// Creamos una variable para indicarnos si se ha pulsado la pantalla en la parte de salto
			bool salto = false;
			// Obtenemos la mitad del ancho de la pantalla
			int mitadPantalla = Screen.width / 2;

			// Ccomprobamos si se ha pulsado el boton A...
			salto = (Input.GetButtonDown("Jump")) ? true : false;

			// o en su defecto, recorremos todas las pulsaciones en pantalla para saber cual cumple la condicion
			foreach (Touch pulsacion in pulsaciones) {
				if (pulsacion.position.x <= mitadPantalla) {
					salto = true;
					break;
				}
			}
			
			// Miramos si el jugador salta y no esta cayendo
			if (salto && !saltando && !cayendo) {
				// Recogemos el estado de Mecanim para saber hacia donde mira el pollo
				EstadoPollo estadoPollo = (EstadoPollo) animatorPollo.GetInteger("Estado");
				
				// Evaluamos el estado
				switch (estadoPollo) {
				case EstadoPollo.AndandoDerecha:
					// Establecemos en Mecanim que salte a la derecha
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.SaltandoDerecha);
					break;
				case EstadoPollo.AndandoIzquierda:
					// Establecemos en Mecanim que salte a la izquierda
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.SaltandoIzquierda);
					break;
				}
				
				// Añadimos fuerza al Rigibody del pollo para hacerlo saltar
				rigidbodyPollo.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
				// Establecemos que el pollo esta saltando
				saltando = true;
				
				// Hacemos sonar el sonido de salto
				audioSourcePollo.PlayOneShot(clipSalto);
			} else if (horizontal != 0f && !saltando && !cayendo) {
				// Movemos el pollo en la direccion indicada
				rigidbodyPollo.velocity = Vector2.right * horizontal * velocidad * Time.deltaTime;
				//rigidbodyPollo.AddForce(Vector2.right * Mathf.Clamp(horizontal * velocidad * Time.deltaTime, 0f, 100f));
				
				if (horizontal > 0f) {
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.AndandoDerecha);
				} else if (horizontal < 0f) {
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.AndandoIzquierda);
				}
			}
			#else
			// Recogemos el input del teclado
			float horizontal = Input.GetAxis("Horizontal");

			// Miramos si el jugador ha pulsado el espacio y no ha saltado ya
			if (Input.GetKeyDown(KeyCode.Space) && !cayendo) {
				// Recogemos el estado de Mecanim para saber hacia donde mira el pollo
				EstadoPollo estadoPollo = (EstadoPollo) animatorPollo.GetInteger("Estado");

				// Evaluamos el estado
				switch (estadoPollo) {
				case EstadoPollo.AndandoDerecha:
					// Establecemos en Mecanim que salte a la derecha
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.SaltandoDerecha);
					break;
				case EstadoPollo.AndandoIzquierda:
					// Establecemos en Mecanim que salte a la izquierda
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.SaltandoIzquierda);
					break;
				}

				// Añadimos fuerza al Rigibody del pollo para hacerlo saltar
				rigidbodyPollo.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
				// Establecemos que el pollo esta saltando
				saltando = true;

				// Hacemos sonar el sonido de salto
				audioSourcePollo.PlayOneShot(clipSalto);
			} else if (horizontal != 0f && !saltando && !cayendo) {
				// Movemos el pollo en la direccion indicada
				rigidbodyPollo.velocity = Vector2.right * horizontal * velocidad * Time.deltaTime;
				//rigidbodyPollo.AddForce(Vector2.right * Mathf.Clamp(horizontal * velocidad * Time.deltaTime, 0f, 100f));
				
				if (horizontal > 0f) {
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.AndandoDerecha);
				} else if (horizontal < 0f) {
					animatorPollo.SetInteger("Estado", (int) EstadoPollo.AndandoIzquierda);
				}
			}
			#endif
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		// Recuperamos el estado del pollo
		EstadoPollo estadoPollo = (EstadoPollo) animatorPollo.GetInteger("Estado");

		if (saltando && col.gameObject.tag == "Mundo") {
			switch (estadoPollo) {
			case EstadoPollo.SaltandoDerecha:
				animatorPollo.SetInteger("Estado", (int) EstadoPollo.AndandoDerecha);
				break;
			case EstadoPollo.SaltandoIzquierda:
				animatorPollo.SetInteger("Estado", (int) EstadoPollo.AndandoIzquierda);
				break;
			}
			// Establecemos que el pollo ya no esta saltando
			saltando = false;
		}
	}

	void OnCollisionStay2D (Collision2D col) {
		if (col.gameObject.tag == "Mundo") {
			cayendo = false;
		}
	}

	void OnCollisionExit2D (Collision2D col) {
		//if (col.gameObject.tag != "Mundo") {
		if (!saltando) {
			// Si el tag es algo diferente al suelo, decimos que el pollo esta cayendo
			cayendo = true;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		// Comprobar contra que hemos entrado en contacto
		if (col.tag == "Enemigo") {
			// Colisionamos contra un enemigo
			ColisionEnemigo();
		} else if (col.tag == "Limite") {
			// Hemos llegado al limite inferior de la pantalla, por lo que perdemos una vida
			PerderVida();
		} else if (col.tag == "Moneda") {
			// Hemos colisionado con una moneda y vamos a sumar puntos
			CogerMoneda(col.gameObject.GetComponent<Moneda>());
		}
	}

	void CogerMoneda (Moneda moneda) {
		// Desactivamos la moneda
		moneda.gameObject.SetActive(false);
		// Hacemos sonar el sonido de coger la moneda
		audioSourcePollo.PlayOneShot(clipMoneda);
		// Lanzamos el evento de los puntos
		if (OnCogerPuntos != null) {
			OnCogerPuntos(moneda.puntos);
		}
	}

	void PerderVida () {
		// Reactivamos la colision entre capas
		Physics2D.IgnoreLayerCollision(8, 9, false);
		// Quitamos la velocidad de caida del pollo
		rigidbodyPollo.velocity = Vector2.zero;
		// Posicionamos el pollo en la posicion de inicio
		transformPollo.position = transformInicio.position;
		// Comprobamos si hay que lanzar el evento
		if (OnPerderVida != null) {
			// Lanzamos el evento en caso de que haya algun script que lo haya asociado
			OnPerderVida();
		}
	}

	void ColisionEnemigo () {
		// Hacemos que las capas del jugador y las del mundo se ignoren entre si
		Physics2D.IgnoreLayerCollision(8, 9);
		// Quitamos toda la velocidad al pollo
		rigidbodyPollo.velocity = Vector2.zero;
		// Añadimos velocidad para simular la muerte del pollo
		rigidbodyPollo.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

		// Hacemos sonar el sonido de muerte
		audioSourcePollo.PlayOneShot(clipMuerte);
	}
}

public enum EstadoPollo {
	AndandoDerecha,
	AndandoIzquierda,
	SaltandoDerecha,
	SaltandoIzquierda
}

public enum EstadoJuego {
	Jugando,
	Parado
}