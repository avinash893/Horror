using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using static Zombie;

public class zombbietest : MonoBehaviour
{
    public enum ZombieType
    {
        shuffle,
        dizzy,
        alert
    }
    public enum ZombieState
    {
        Idle,
        Walking,
        Eating
    }
    public ZombieType zombieStyle;
    public ZombieState chooseState;
    public float yAdjustment = 0.0f;
    private Animator anim;
    private AnimatorStateInfo animInfo;
    public bool randomState = false;
    public float randomTiming = 5f;
    private int newState = 0;
    private int currentState;
    private NavMeshAgent agent;
    private GameObject[] targets;
    public float[] walkSpeed = { 0.75f, 1.25f, 1f };

    private float disToTarget;
    private float disToPlayer;

    private GameObject player;
    public float zombieAlertRange = 15f;

    private bool awareOfPlayer = false;
    private bool adding = true;
    private AudioSource chaseMusicPlayer;
    private AudioSource backgroundMusicPlayer;
    private float currentBackgroundVol = 0.8f;


    public float attackDistance = 1f;

    public float rotateSpeed = 3f;
    private AudioSource ZombieSound;



    private int currentTarget=0;

  


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FPSController");
        anim = GetComponent<Animator>();
        agent=GetComponent<NavMeshAgent>();
        targets = GameObject.FindGameObjectsWithTag("Target");
        chaseMusicPlayer = GameObject.Find("ChaseMusic").GetComponent<AudioSource>();
        backgroundMusicPlayer = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        zombieAlertRange = Random.Range(5.1f, 25f);



        anim.SetLayerWeight(((int)zombieStyle + 1), 1);

        if (zombieStyle == ZombieType.shuffle)
        {
            transform.position = new Vector3(transform.position.x,
            transform.position.y + yAdjustment, transform.position.z);
        }

        anim.SetTrigger(chooseState.ToString());
        currentState = (int)chooseState;

        if (randomState == true)
        {
            InvokeRepeating("SetAnimState", randomTiming, randomTiming);
        }

        agent.destination = targets[0].transform.position;
        agent.speed = walkSpeed[(int)zombieStyle];
    }

    // Update is called once per frame
    void Update()
    {
        disToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (disToPlayer <= attackDistance)
        {
            //headrotation
            agent.isStopped = true;
            anim.SetBool("Attacking", true);
            Vector3 pos= (player.transform.position-transform.position).normalized;
            Quaternion posRotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, posRotation,rotateSpeed*Time.deltaTime);
        }

        else
        {
            //if not attacked

            anim.SetBool("Attacking", false);
            if (SaveScript.zommbieChasing.Count > 0)
            {
                if (chaseMusicPlayer.volume < 0.78f)
                {
                    if (chaseMusicPlayer.isPlaying == false)
                    {
                        chaseMusicPlayer.Play();
                        backgroundMusicPlayer.Stop();

                    }
                    chaseMusicPlayer.volume += 0.5f * Time.deltaTime;

                }
            }


            if (SaveScript.zommbieChasing.Count == 0)
            {
                if (chaseMusicPlayer.volume > 0f)
                {
                    chaseMusicPlayer.volume -= 0.5f * Time.deltaTime;
                }

                if (chaseMusicPlayer.volume == 0.0f)
                {
                    chaseMusicPlayer.Stop();
                }
            }




            disToTarget = Vector3.Distance(transform.position, targets[currentTarget].transform.position);
            animInfo = anim.GetCurrentAnimatorStateInfo((int)chooseState);





            if (disToPlayer < zombieAlertRange && chooseState == ZombieState.Walking)
            {
                agent.destination = player.transform.position;
                awareOfPlayer = true;
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
                    }
                }
            }



            if (disToPlayer > zombieAlertRange)
            {
                awareOfPlayer = false;
                if (SaveScript.zommbieChasing.Contains(this.gameObject))
                {
                    SaveScript.zommbieChasing.Remove(this.gameObject);
                    adding = true;
                }
            }


            if (animInfo.IsTag("motion"))
            {
                if (anim.IsInTransition((int)chooseState))
                {
                    agent.isStopped = true;
                }
            }


            if (chooseState == ZombieState.Walking)
            {
                agent.isStopped = false;
            }
            if (chooseState != ZombieState.Walking)
            {
                agent.isStopped = true;

            }

            if (chooseState == ZombieState.Walking)
            {

                if (disToTarget < 1.5f)
                { currentTarget = Random.Range(0, targets.Length - 1); }



            }





            if (chooseState == ZombieState.Walking && disToTarget < 1.5f)
            {
                currentTarget = Random.Range(0, targets.Length);
            }


        }
    }

    void SetAnimState()
    {
        if (awareOfPlayer == false)
        { 
        newState = Random.Range(0, 3);
        if (newState != currentState)
         {
            chooseState = (ZombieState)newState;
            currentState = (int)chooseState;
            anim.SetTrigger(chooseState.ToString());
         }
        }
    }

    public void Walkon()
    {
        agent.isStopped = false;
        agent.destination = targets[currentTarget].transform.position;
    }

    public void Walkoff()
    {
        agent.isStopped = true;
    }
}
