using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Script_PlayerCtrlr : MonoBehaviour
{
    //VARIAVEIS
    private Animator PlayerAnimator;
    private CharacterController CharCtrlr;
    [Tooltip("Velocidade de caminhar")]
    public float VelWalk;
    [Tooltip("Velocidade de correr")]
    public float VelRun;
    [Tooltip("Velocidade de rotacionar")]
    public float VelRot;
    [Tooltip("Força do pulo")]
    public float Jump;

    private bool isGrounded;        //variável que informa se o player está no chao
    private bool isJumping;         //variável que informa se o player está pulando

    private float VelocityCtrlr;    //controla a velocidade que o player está andando (caminhar ou correr)
    private float InputForward;     //recebe o input para andar para frente e para trás
    private float InputRotate;      //recebe o input para rotacionar
    
    private bool InputRun;      //recebe o input de correr
    private bool InputAction1;  //recebe o input da ação 1
    private bool InputAction2;  //recebe o input de ação 2

    public KeyCode RunKey;      //recebe a tecla de correr
    public KeyCode ActionKey1;  //recebe a tecla de ação 1
    public KeyCode ActionKey2;  //recebe a tecla de ação 2

    //variaveis que recebem os inputs nos vetores para cada ação separada
    private Vector3 PlayerMovement, PlayerRotation, VectorJump;

    // Start is called before the first frame update
    void Start() {
        PlayerAnimator = GetComponent<Animator>();          //pega o componente Animator do objeto que tem o script
        CharCtrlr = GetComponent<CharacterController>();    //pega o componente CharacterController do objeto que tem o script 
    }

    // Update is called once per frame
    void Update()
    {
        InputMovements(); //chama a função que recebe os inputs relacionados a movimentação (mover pra frente e rotacionar)
        AnimationsCtrlrKeys(); //chama a função que os inputs recebidos para chamar as animações
        
        //variavel recebe o input no vetor de mover para frente no sentido +/-
        PlayerMovement = InputForward * transform.TransformDirection(Vector3.forward) * VelocityCtrlr;
        //faz o personagem mover para frente no sentido +/-
        CharCtrlr.Move(PlayerMovement * Time.deltaTime);
        //faz o personagem rotacionar em Y no sentido +/-
        transform.Rotate(new Vector3(0, InputRotate * VelRot * Time.deltaTime, 0));

        //faz a gravidade
        CharCtrlr.SimpleMove(Physics.gravity);
        //chama função de checar se está no chão
        Grounded();
        //chama o função de pular
        InputJump();
        //chama a função das ações
        InputActions();
    }
    void Grounded() {                       //função de checar se está no chao
        isGrounded = CharCtrlr.isGrounded;  //variável isGrounded recebe a checagem do Character controller se for true ou false
        if (CharCtrlr.isGrounded) {         //se isGrouded for true 
            VectorJump.y = 0.0f;            //vetor de pulo receberá valor 0
        }
    }
    void InputJump() {                                      //função que faz a ação de pular
        if (Input.GetButton("Jump") && isGrounded) {        //se está apertando o botão de pulo e está no chão, então...
            isJumping = true;                               //informa a varável que está pulando
            VectorJump.y = Jump;                            //insere no vetor de controle de pulo o valor da força do pulo
        }
        CharCtrlr.Move(VectorJump * Time.deltaTime);        //aplica a força para o personagem subir/mover no eixo Y (pular)
    }
    void InputActions() {                               //função que faz as ações
        InputRun = Input.GetKey(RunKey);                //recebe o input de correr
        InputAction1 = Input.GetKeyDown(ActionKey1);    //recebe o input da ação 1
        InputAction2 = Input.GetKeyDown(ActionKey2);    //recebe o input da ação 2
    }
    void InputMovements() { //recebe os inputs relacionado a movimentação
        InputForward = Input.GetAxis("Vertical");   //recebe o input para mover para frente no sentido +/-
        InputRotate = Input.GetAxis("Horizontal");  //recebe o input para rotacionar no sentido +/-
        PlayerRotation.y = InputRotate;             //recebe variável do input de rotação no eixo Y de vetor3 de rotação
    }

    void AnimationsCtrlrKeys() { //função que recebe os inputs de movimento para animar o player
        if (InputRun) {
            //se botão de correr estiver apertado
            //insere valor do input na variavel controladora da animção de movimentar para frente no sentido +/- que está abaixo
            //o animator verifica o valor e deve chamar a animação de correr
            PlayerAnimator.SetFloat("AnimatorVel", InputForward);
            //insere valor da velocidade de movimento para correr na variavel controladora de velocidade
            VelocityCtrlr = VelRun;
        } 
        else {
            //se Shift não estiver apertado
            //insere valor do input na variavel controladora da animção de movimentar para frente no sentido +/-
            //o animator verifica o valor e deve chamar a animação de caminhar pois o valor está dividido pela metade
            PlayerAnimator.SetFloat("AnimatorVel", InputForward / 2.0f);
            //insere valor da velocidade de movimento para caminhar na variavel controladora de velocidade
            VelocityCtrlr = VelWalk;
        }

        if (isJumping) {
            //se está pulando, então...
            //insere valor 2 na variavel que controla a animação Jump para animar Jump
            PlayerAnimator.SetInteger("Jump", 2);
            isJumping = false; //informa que está pulando é falso
        } 
        else { 
            //se não apertou
            //insere valor 0 na variavel que controla a animação Jump
            PlayerAnimator.SetInteger("Jump", 0);
        }

        if (InputAction1) {
            //se apertar botão da ação 1
            //ativa a variavel de animação trigger de animação Press
            PlayerAnimator.SetTrigger("Press");
        }

        if (InputAction2) {
            //se apertou botão da ação 2
            //ativa a variavel de animação trigger de animação Bye
            PlayerAnimator.SetTrigger("Bye");
        }
    }
}
