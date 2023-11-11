using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerCtrlrNormalized : MonoBehaviour
{
	[Header("Script configurations")]
	//VARIAVEIS
	private Animator PlayerAnimator;
	private CharacterController CharCtrlr;
	private Transform transformCamera;

	//Controla a velocidade do personagem
	private float VelocityCtrlr;
	[Tooltip("Velocidade do personagem caminhando")]
	public float VelWalk;
	[Tooltip("Velocidade do personagem corendo")]
	public float VelRun;
	[Tooltip("Velocidade do personagem rotacionando")]
	public float VelRot;
	//variavel usada para receber um angulo de rotação em radianos
	private float RotationDirection;
	private float MoveForward; //variavel que move o player para frente
	[Tooltip("Força do pulo")]
	public float JumpForce;

	private bool isGrounded;         //variável que informa se o player está no chao
	private bool isJumping;          //variável que informa se o player está pulando

	private Vector3 VectorJump;		//variavel usada para fazer o player pular
	private Vector3 frenteCamera;	//recebe a direção da camera
	private Vector3 MoveDirection;	//variavel usada para mover o personagem na direção
	private Vector3 normalizaZeroPiso = new Vector3(0, 0, 0);

	private float InputAxis_V;	//variavel usada para receber um valor de direção vertical
	private float InputAxis_H;  //variavel usada para receber um valor de direção Horizontal
	private bool InputButton_V; //variavel usada para informar se está movendo nas verticais
	private bool InputButton_H; //variavel usada para informar se está movendo nas horizontais
	private bool InputKey_Run;  //variavel usada para informar se está correndo

	// Use this for initialization
	void Start()
	{
		CharCtrlr = GetComponent<CharacterController>();	//pega o componente CharacterController do proprio gameobject
		PlayerAnimator = GetComponent<Animator>();          //pega o componente Animator do proprio gameobject
		transformCamera = Camera.main.transform;            //pega o componente Transform da camera principal da cena
	}

	// Update is called once per frame
	void Update()
	{
		InputMovements();		//função que recebe os inputs de movimento
		AnimationCtrlrKeys();	//função que chama as animações
		InputJump();			//função que chama a mecanica do pulo

		//pega a direção que a vcamera está olhando
		frenteCamera = Vector3.Scale(transformCamera.forward, new Vector3(1, 0, 1)).normalized;
		//
		MoveDirection = InputAxis_V * frenteCamera + InputAxis_H * transformCamera.right;
		//move o personagem na direção multiplicando a velocidade em função do tempo
		CharCtrlr.Move(MoveDirection * VelocityCtrlr * Time.deltaTime);

		//normaliza a direção para que não ante mais rápido em diagonais
		if (MoveDirection.magnitude > 1.0f) {
			MoveDirection.Normalize();
		}
		//transforma essa direção do mundo para uma direção local
		MoveDirection = transform.InverseTransformDirection(MoveDirection);
		//recebe o valor de direção em um plano (chao) para mover na direção certa usando 2 eixos
		MoveDirection = Vector3.ProjectOnPlane(MoveDirection, normalizaZeroPiso);
		RotationDirection = Mathf.Atan2(MoveDirection.x, MoveDirection.z);			//recebe a direção em 2 eixos usando o tangente para rotacionar depois
		MoveForward = MoveDirection.z; // move o personagem na direção z

		CharCtrlr.SimpleMove(Physics.gravity); // faz a gravidade
		Grounded(); // chama a função que verifica se o personagem está no chao
		AplicaRotacao(); //aplica a rotação
	}
	void Grounded() {                       //função de checar se está no chao
		isGrounded = CharCtrlr.isGrounded;  //variável isGrounded recebe a checagem do Character controller se for true ou false
		if (isGrounded) {					//se isGrouded for true 
			VectorJump.y = 0.0f;            //vetor de pulo receberá valor 0
			isJumping = false;				//garante que se está no chao não está pulando
		}
	}

	void InputJump() {									//função que faz o personagem pular
		if (Input.GetButton("Jump") && isGrounded) {	//se apertou Jump e está no chão então...
			VectorJump.y = JumpForce;					//o vetor de pulo recebe o valor do pulo
			isJumping = true;							//informa se está pulando (usado para animar)
		}
		CharCtrlr.Move(VectorJump * Time.deltaTime);	//move o personagem para cima em função do tempo (para pular)
	}

	void InputMovements() {									//função que recebe os inputs de movimento
		InputAxis_V = Input.GetAxis("Vertical");			//recebe o valor do input vertical
		InputAxis_H = Input.GetAxis("Horizontal");          //recebe o valor do input horizontal
		InputButton_V = Input.GetButton("Vertical");		//informa se é verdadeiro o movimento vertical
		InputButton_H = Input.GetButton("Horizontal");      //informa se é verdadeiro o movimento horizontal
		InputKey_Run = Input.GetKey(KeyCode.LeftShift);     //informa se é está aperntado o botão de correr
	}

	void AnimationCtrlrKeys() { // função que controla as animações do personagem

		if (isJumping) {							//valida se está pulando então...
			PlayerAnimator.SetInteger("Jump", 2);	//...o parametro de animação Jump receberá 2
			isJumping = false;						//valida que não está mais podendo pular
		}
		else {										//se não ...
			PlayerAnimator.SetInteger("Jump", 0);	//... o parametro de animação Jump receberá 0
		}

		if ((InputButton_H || InputButton_V) && InputKey_Run) {			//se está movendo em alguma direção (vertical ou horizontal) e está aperntando o ~botão de correr, então...
			PlayerAnimator.SetFloat("AnimatorVel", MoveForward);	    //o parametro de animação AnimatorVel receberá o valor total de andar para frente
			VelocityCtrlr = VelRun;										//o controlador de velocidade de movimento recebe velcidade de correr
		} 
		else {															//se não
			PlayerAnimator.SetFloat("AnimatorVel", MoveForward / 2.0f); //o parametro de animação AnimatorVel receberá o valor total de andar para frente dividido por 2
			VelocityCtrlr = VelWalk;                                    //o controlador de velocidade de movimento recebe velcidade de caminhar
		}
		//faz a animação press
		if (Input.GetKeyDown(KeyCode.E)) {
			PlayerAnimator.SetTrigger("Press");
		}
		//faz a animação bye
		if (Input.GetKeyDown(KeyCode.Q)) {
			PlayerAnimator.SetTrigger("Bye");
		}
	}

	void AplicaRotacao()    //função que aplica a rotação
    {   //a variavel recebe o valor matemático de interpolação entre dois pontos
        float velocidadeGiro = Mathf.Lerp(180, 360, MoveForward);
		//rotacina o personagem até a direção de rotação em função do tempo no eixo Y
        transform.Rotate(0, RotationDirection * (velocidadeGiro * VelRot) * Time.deltaTime, 0);
	}
}
