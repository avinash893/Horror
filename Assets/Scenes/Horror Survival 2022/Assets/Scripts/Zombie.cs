using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{

    public enum ZombieType
    {
        shuffle,
        dizzy,
        alert
    }
    public enum zombieSate
    {
        Idle,
        Walking,
        Eating
    }

    public ZombieType ZombieStyle;
    public zombieSate chooseState;  
    public float yadj = 0.0f;

    private Animator anim;
    private AnimatorStateInfo animInfo;
    private NavMeshAgent agent;
    public bool randomState = false;
    private int newState = 0;
    private int currentState;

    public float randomTiming=5f;

    private GameObject[] target;
    public float[] walkSpeed = { 0.75f, 1.25f, 1f };
    private float disToTarget;
    private int currentTarget=0;
    private float zombieAlertrange = 30f;
    public GameObject Player;
    private float distanceToPlayer;
    private bool awareofPlayer = false;
    private bool adding = true;
    private AudioSource chaseMusicPlayer;
    public static AudioSource backgroundMusicPlayer;
   
    private float currentChaseVol=0f;

    public float attackDistance = 1f;

    public float rotateSpeed = 3f;
    private AudioSource ZombieSound;

    private float gunAlertRange = 100f;

    public bool isAngry=false;//reacting even if chasing bottle and attack u
    private float hiddenRange = 0f;


    // Start is called before the first frame update
    void Start()
    {

        backgroundMusicPlayer=GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        chaseMusicPlayer = GameObject.Find("ChaseMusic").GetComponent<AudioSource>();
        ZombieSound=GetComponent<AudioSource>();


        anim=GetComponent<Animator>();
        anim.SetLayerWeight(((int)ZombieStyle+1), 1);
        agent=GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectsWithTag("Target");
        Player = GameObject.Find("FPSController");
        zombieAlertrange = Random.Range(6f, 35f);

        if (ZombieStyle == ZombieType.shuffle)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y+yadj,transform.position.z);
        }
        anim.SetTrigger(chooseState.ToString());
    
        currentState=(int)chooseState;

        if (randomState == true) 
        {
            InvokeRepeating("SetAnimState", randomTiming, randomTiming);
        }


        
            agent.destination = target[0].transform.position;
        
        
        agent.speed = walkSpeed[(int)ZombieStyle];
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("IsDead") == false)
        {

            distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            if (SaveScript.bottlePos == Vector3.zero)
            {
                isAngry = false;

            }
            if (SaveScript.bottlePos != Vector3.zero && distanceToPlayer > attackDistance && isAngry == false)
            {
                agent.destination = SaveScript.bottlePos;
                anim.SetBool("Attacking", false);
                chooseState = zombieSate.Walking;

            }
            else
            {





                if (distanceToPlayer <= attackDistance)
                {
                    agent.isStopped = true;
                    anim.SetBool("Attacking", true);

                    Vector3 pos = (Player.transform.position - transform.position).normalized;
                    Quaternion posRotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, posRotation, rotateSpeed * Time.deltaTime);
                }


                else
                {
                    anim.SetBool("Attacking", false);

                    if (SaveScript.zommbieChasing.Count > 0)
                    {
                        backgroundMusicPlayer.Stop();

                        if (chaseMusicPlayer.volume < 0.6f)
                        {
                            if (chaseMusicPlayer.isPlaying == false)
                            {
                                chaseMusicPlayer.Play();
                            }
                           
                            chaseMusicPlayer.volume += 0.5f * Time.deltaTime;


                        }
                    }

                    if (SaveScript.zommbieChasing.Count == 0)
                    {
                        if (chaseMusicPlayer.volume > 0.0f)
                        {
                            chaseMusicPlayer.volume -= 0.5f * Time.deltaTime;
                        }
                        if (chaseMusicPlayer.volume == 0.0f)
                        {
                            chaseMusicPlayer.Stop();
                        }
                    }




                    disToTarget = Vector3.Distance(transform.position, target[currentTarget].transform.position);
                    animInfo = anim.GetCurrentAnimatorStateInfo((int)ZombieStyle);



                    if (distanceToPlayer < zombieAlertrange && chooseState == zombieSate.Walking)
                    {
                        agent.destination = Player.transform.position;
                        awareofPlayer = true;
                        if (adding == true)
                        {
                            if (SaveScript.zommbieChasing.Contains(this.gameObject))
                            {
                                adding = false;
                                return;
                            }
                            else
                            {
                                SaveScript.zommbieChasing.Add(this.gameObject);
                                adding = false;
                            }
                        }
                    }
                    if (distanceToPlayer > zombieAlertrange)
                    {
                        awareofPlayer = false;
                        if (SaveScript.zommbieChasing.Contains(this.gameObject))
                        {
                            SaveScript.zommbieChasing.Remove(this.gameObject);
                            adding = true;
                        }
                    }

                    if (distanceToPlayer > 180)
                    {
                        if (distanceToPlayer > zombieAlertrange)
                        {
                            awareofPlayer = false;
                            if (SaveScript.zommbieChasing.Contains(this.gameObject))
                            {
                                SaveScript.zommbieChasing.Remove(this.gameObject);
                                adding = true;
                            }
                        }
                        SaveScript.zombiesInGameAmt--;
                        Destroy(gameObject);
                    }

                    if (animInfo.IsTag("motion"))
                    {
                        if (anim.IsInTransition((int)ZombieStyle))
                        {
                            agent.isStopped = true;
                        }
                    }

                    if (chooseState == zombieSate.Walking && disToTarget < 1.5f)
                    {
                        currentTarget = Random.Range(0, target.Length);
                    }
                }



            }

        }





        else
        {
            if (SaveScript.zommbieChasing.Contains(this.gameObject))
            {
                SaveScript.zommbieChasing.Remove(this.gameObject);
                adding = true;
            }
            if (SaveScript.zommbieChasing.Count == 0)
            {
                if (chaseMusicPlayer.volume > 0.0f)
                {
                    chaseMusicPlayer.volume -= 0.5f * Time.deltaTime;
                }
                if (chaseMusicPlayer.volume == 0.0f)
                {
                    chaseMusicPlayer.Stop();
                    if (!backgroundMusicPlayer.isPlaying)
                    {
                        backgroundMusicPlayer.Play();
                    }
                }
            }

            CancelInvoke();
            Destroy(gameObject, 5f);
        }
        if (SaveScript.gunUsed == true)
        {
            zombieAlertrange=gunAlertRange;
            StartCoroutine(ResetGunRange());
        }
        else if (SaveScript.isHidden == true)
        {
            zombieAlertrange=hiddenRange;
        }
        else
        {
            zombieAlertrange = 20f;
        }
    }
    void SetAnimState()
    {

        if (awareofPlayer == false)
        {
            newState = Random.Range(0, 3);  // Randomly selects a state
                                            // (0 = Idle, 1 = Walking, 2 = Eating)
            if (newState != currentState)
            {
                chooseState = (zombieSate)newState;
                currentState = newState;
                anim.SetTrigger(chooseState.ToString());
            }
        }
        if (awareofPlayer==true) 
        {
            chooseState = zombieSate.Walking;
        }
        ZombieSound.Play();
    }









    public void Walkon()
    {
        agent.isStopped = false;
        agent.destination = target[currentTarget].transform.position;
    }

    public void Walkoff()
    {
        agent.isStopped = true;
    }





    IEnumerator ResetGunRange()
    {
        yield return new WaitForSeconds(5f);
        SaveScript.gunUsed = false;
    }

}

