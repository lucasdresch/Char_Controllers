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

    [SerializeField]
    private bool isJumping;         //variável que informa se o player está pulando
    [SerializeField]
    private bool isGrounded;         //variável que informa se o player está pulando

    private float VelocityCtrlr;    //controla a velocidade que o player está andando (caminhar ou correr)
    private float InputForward;     //recebe o input para andar para frente e para trás
    private float InputRotate;      //recebe o input para rotacionar
    //variaveis que recebem os inputs nos vetores para cada ação separada
    [SerializeField]
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
        //chama o função de pular
        InputJump();
    }
    
    void InputJump() { //recebe o comando de pular
        if (Input.GetButton("Jump") && isGrounded) {  //se apertou o botão de pulo e isJumping for falso                                          
            VectorJump.y = Jump;    //aplica a força para subir (pular)
            isJumping = true;       //atribui verdadeiro na variável que informa se está pulando
            isGrounded = false;
            print("pulou!");
        }
        //faz a ação de pular
        CharCtrlr.Move(VectorJump * Time.deltaTime);
    }
    private void OnControllerColliderHit() { //verifca o momento em que houve colisão com algo retornando verdadeiro estver colidindo
        VectorJump.y = 0.0f;    //atribui o valor 0 ao vetor de pulo
        isJumping = false;      //atribui falso na variável que informa se está pulando quando tocar no chão
        isGrounded = true;
    }

    void InputMovements() { //recebe os inputs relacionado a movimentação
        InputForward = Input.GetAxis("Vertical");   //recebe o input para mover para frente no sentido +/-
        InputRotate = Input.GetAxis("Horizontal");  //recebe o input para rotacionar no sentido +/-
        PlayerRotation.y = InputRotate;             //recebe variável do input de rotação no eixo Y de vetor3 de rotação
    }

    void AnimationsCtrlrKeys() { //função que recebe os inputs de movimento para animar o player
        if (Input.GetKey(KeyCode.LeftShift)) {
            //se Shift estiver apertado
            //insere valor do input na variavel controladora da animção de movimentar para frente no sentido +/- que está abaixo
            //o animator verifica o valor e deve chamar a animação de correr
            PlayerAnimator.SetFloat("AnimatorVel", InputForward);
            //insere valor da velocidade de movimento para correr na variavel controladora de velocidade
            VelocityCtrlr = VelRun;
        } else {
            //se Shift não estiver apertado
            //insere valor do input na variavel controladora da animção de movimentar para frente no sentido +/-
            //o animator verifica o valor e deve chamar a animação de caminhar pois o valor está dividido pela metade
            PlayerAnimator.SetFloat("AnimatorVel", InputForward / 2.0f);
            //insere valor da velocidade de movimento para caminhar na variavel controladora de velocidade
            VelocityCtrlr = VelWalk;
        }

        if (Input.GetButton("Jump") && isGrounded) {
            //se apertou o botão jump
            //insere valor 2 na variavel que controla a animação Jump
            PlayerAnimator.SetInteger("Jump", 2);
            print("no chao!");
        } else {
            //se não apertou
            //insere valor 1 na variavel que controla a animação Jump
            PlayerAnimator.SetInteger("Jump", 1);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            //se apertar E
            //ativa a variavel de animação trigger de animação Press
            PlayerAnimator.SetTrigger("Press");
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            //se apertou Q
            //ativa a variavel de animação trigger de animação Bye
            PlayerAnimator.SetTrigger("Bye");
        }
    }
}
