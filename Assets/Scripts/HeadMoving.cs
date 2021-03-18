using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
public class HeadMoving : MonoBehaviour
{
    private float time;
    private float deltaTime = 0.16f;
    private Vector3 step = new Vector3(100f, 0f, 0f);
    private bool isPlaying = true;
    private enum State { right = 0, left = 1, up = 2, down = 3, downLeft = 4, upLeft = 5, rightUp = 6, rightDown = 7, leftUp = 8, downRight = 9, upRight = 10, leftDown = 11 };
    private State state = State.right;
    private State stateToChange;
    private State tailState = State.right;
    private int addCount = 0;
    private Vector3 tailPosition = new Vector3(0f, 0f, 0f);
    private Quaternion rotation = Quaternion.Euler(0, 0, 0);
    private bool addFlag = false;
    private bool isChanged = false;
    private State turnDirection;
    public int chereshnyaCount = 0;

    public GameObject bodyPart;
    public GameObject chershnyaObj;
    private GameObject chereshnya;
    //public GameObject tailObj;
    public GameObject tail;
    public GameObject angleObj;
    private List<GameObject> snake = new List<GameObject>();
    private List<State> directions = new List<State>();

    private Animator anim;
    private Animator tailAnim;
    private void OnGUI()
    {
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.font = new Font("Montserrat");
        gUIStyle.normal.background = Texture2D.redTexture;
        gUIStyle.normal.textColor = Color.black;
        
        if (isPlaying == false)
        {
            if (GUI.Button(new Rect(715, 288, 75, 75), "Restart"))
            {
                foreach (GameObject x in snake)
                {
                    Destroy(x);
                }
                snake.Clear();
                transform.position = new Vector3(0f, 0f, 0f);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                state = State.right;
                tailState = State.right;
                anim.enabled = true;
                tailAnim.enabled = true;
                step = new Vector3(100f, 0f, 0f);
                directions.Clear();

                snake.Add(Instantiate(bodyPart));
                snake[0].transform.position = new Vector3(-100f, 0f, 0f);
                directions.Add(State.right);
                //tail = Instantiate(tailObj);
                tail.transform.position = new Vector3(-200f, 0f, 0f);
                Destroy(chereshnya);
                chereshnya = Instantiate(chershnyaObj);
                chereshnya.transform.position = new Vector3(Random.Range(-8, 8) * 100, Random.Range(-7, 7) * 100, 0f);
                time = Time.time;
                chereshnyaCount = 0;
                isPlaying = true;
                /*
                            anim = GetComponent<Animator>();
                            //anim.enabled = false;
                            tailAnim = tail.GetComponent<Animator>();*/
            }
        }
        
    }
    void Start()
    {
        snake.Add(Instantiate(bodyPart));
        snake[0].transform.position = new Vector3(-100f, 0f, 0f);
        directions.Add(State.right);
        //tail = Instantiate(tailObj);
        tail.transform.position = new Vector3(-200f, 0f, 0f);
        chereshnya = Instantiate(chershnyaObj);
        chereshnya.transform.position = new Vector3(Random.Range(-8, 8) * 100, Random.Range(-7, 7) * 100, 0f);
        time = Time.time;

        anim = GetComponent<Animator>();
        //anim.enabled = false;
        tailAnim = tail.GetComponent<Animator>();
        //tailAnim.enabled = false;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            SceneManager.LoadScene(2);
        }
        if (isPlaying)
        {
            if (isChanged)
            {
                isChanged = false;
                //transform.rotation = rot;
                if (turnDirection == State.right)
                    anim.Play("layer.HeadTurnRight", 0, 0f);
                else
                    anim.Play("layer.HeadTurnLeft", 0, 0f);

            }
            else
            {
                if (Time.time - time > deltaTime)
                {
                    transform.rotation = rotation;
                    time = Time.time;
                    tailPosition[0] = snake[snake.Count - 1].transform.position.x;
                    tailPosition[1] = snake[snake.Count - 1].transform.position.y;
                    tailPosition[2] = snake[snake.Count - 1].transform.position.z;
                    state = stateToChange;
                    moveSnake();
                    if (addCount > 0)
                    {
                        directions.Add(tailState);
                        addFlag = true;
                        addCount--;
                    }
                }
                if (Input.GetKey(KeyCode.UpArrow) && state != State.down && state != State.up)
                {
                    step[0] = 0f;
                    step[1] = 100f;
                    stateToChange = State.up;
                    rotation = Quaternion.Euler(0, 0, 90);
                    isChanged = true;
                    if (state == State.left)
                        turnDirection = State.right;
                    else
                        turnDirection = State.left;
                }
                else if (Input.GetKey(KeyCode.DownArrow) && state != State.up && state != State.down)
                {
                    step[0] = 0f;
                    step[1] = -100f;
                    stateToChange = State.down;
                    rotation = Quaternion.Euler(0, 0, 270);
                    isChanged = true;
                    if (state == State.right)
                        turnDirection = State.right;
                    else
                        turnDirection = State.left;
                }
                else if (Input.GetKey(KeyCode.RightArrow) && state != State.left && state != State.right)
                {
                    step[0] = 100f;
                    step[1] = 0f;
                    stateToChange = State.right;
                    rotation = Quaternion.Euler(0, 0, 0);
                    isChanged = true;
                    if (state == State.up)
                        turnDirection = State.right;
                    else
                        turnDirection = State.left;
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && state != State.right && state != State.left)
                {
                    step[0] = -100f;
                    step[1] = 0f;
                    stateToChange = State.left;
                    rotation = Quaternion.Euler(0, 0, 180);
                    isChanged = true;
                    if (state == State.down)
                        turnDirection = State.right;
                    else
                        turnDirection = State.left;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().color == Color.red)
        {
            Destroy(other.gameObject);
            chereshnya = Instantiate(chershnyaObj);
            chereshnya.transform.position = newChereshnyaPosition();
            addCount++;
            chereshnyaCount++;
        }
        else
        {
            isPlaying = false;
            anim.enabled = false;
          
            
        }
    }

    private void moveSnake()
    {
        if (addFlag)
        {
            snake.Add(Instantiate(bodyPart, snake[snake.Count - 1].transform.position, Quaternion.Euler(0, 0, 0)));
            addFlag = false;
        }
        else
        {
            if (directions[directions.Count - 1] == State.rightUp || directions[directions.Count - 1] == State.downRight || 
                directions[directions.Count - 1] == State.leftDown || directions[directions.Count - 1] == State.upLeft)
                tailAnim.Play("layer.TailTurnLeftAnimation", 0, 0f);
            else if (directions[directions.Count - 1] == State.downLeft || directions[directions.Count - 1] == State.leftUp ||
                    directions[directions.Count - 1] == State.upRight || directions[directions.Count - 1] == State.rightDown)
                tailAnim.Play("layer.TailTurnRightAnimation", 0, 0f);
            else
                tailAnim.Play("layer.TailMoveAnimation", 0, 0f);
            switch (tailState)
            {
                case State.left:
                    tail.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case State.right:
                    tail.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case State.down:
                    tail.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case State.up:
                    tail.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
            tail.transform.position = snake[snake.Count - 1].transform.position;
        }
        for (int i = snake.Count - 1; i > 0; i--)
        {
            if (directions[i] != directions[i - 1])
            {
                Destroy(snake[i]);
                snake[i] = bodyPartGeneration(directions[i - 1], snake[i - 1].transform.position);
                directions[i] = directions[i - 1];
            }
            else
                snake[i].transform.position = snake[i - 1].transform.position;
        }
        State dir = generateDirection();
        if (dir != directions[0])
        {
            directions[0] = dir;
            Destroy(snake[0]);
            snake[0] = bodyPartGeneration(directions[0], transform.position);
        }
        else
            snake[0].transform.position = transform.position;

        transform.position += step;
        //transform.rotation = rotation;

        anim.Play("layer.HeadMoveAnimation", 0, 0f);
        //tailAnim.Play("layer.TailMoveAnimation", 0, 0f);

        //anim.PlayInFixedTime("layer.HeadMoveAnimation", 0, 0.25f);
        //Debug.Log("play anim");

        if (tail.transform.position.x > snake[snake.Count - 1].transform.position.x)
        {
            //tail.transform.rotation = Quaternion.Euler(0, 0, 180);
            tailState = State.left;
        }
        else if (tail.transform.position.x < snake[snake.Count - 1].transform.position.x)
        {
            //tail.transform.rotation = Quaternion.Euler(0, 0, 0);
            tailState = State.right;
        }
        else if (tail.transform.position.y > snake[snake.Count - 1].transform.position.y)
        {
            //tail.transform.rotation = Quaternion.Euler(0, 0, 270);
            tailState = State.down;
        }
        else if (tail.transform.position.y < snake[snake.Count - 1].transform.position.y)
        {
            //tail.transform.rotation = Quaternion.Euler(0, 0, 90);
            tailState = State.up;
        }
    }

    private Vector3 newChereshnyaPosition()
    {
        //лучше тут сделать более детерменированный алгоритм
        bool flag = true;
        Vector3 result = new Vector3(0, 0, 0);
        while (flag)
        {
            flag = false;
            result[0] = Random.Range(-8, 8) * 100;
            result[1] = Random.Range(-7, 7) * 100;
            foreach (GameObject i in snake)
                if (i.transform.position == result)
                    flag = true;
        }
        return result;
    }

    private GameObject bodyPartGeneration(State state, Vector3 position)
    {
        switch (state)
        {
            case State.rightUp:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 0));
            case State.downLeft:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 0));
            case State.upLeft:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 90));
            case State.rightDown:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 90));
            case State.leftDown:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 180));
            case State.upRight:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 180));
            case State.leftUp:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 270));
            case State.downRight:
                return Instantiate(angleObj, position, Quaternion.Euler(0, 0, 270));
            case State.right:
            case State.left:
                return Instantiate(bodyPart, position, Quaternion.Euler(0, 0, 0));
            case State.up:
            case State.down:
                return Instantiate(bodyPart, position, Quaternion.Euler(0, 0, 90));
        }
        return Instantiate(bodyPart, position, Quaternion.Euler(0, 0, 0));
    }

    private State generateDirection()
    {
        State dir = State.right;
        switch (directions[0])
        {
            case State.down:
                switch (state)
                {
                    case State.left:
                        dir = State.downLeft;
                        break;
                    case State.right:
                        dir = State.downRight;
                        break;
                    case State.down:
                        dir = State.down;
                        break;
                }
                break;
            case State.rightDown:
                switch (state)
                {
                    case State.left:
                        dir = State.downLeft;
                        break;
                    case State.right:
                        dir = State.downRight;
                        break;
                    case State.down:
                        dir = State.down;
                        break;
                }
                break;
            case State.leftDown:
                switch (state)
                {
                    case State.left:
                        dir = State.downLeft;
                        break;
                    case State.right:
                        dir = State.downRight;
                        break;
                    case State.down:
                        dir = State.down;
                        break;
                }
                break;

            case State.up:
                switch (state)
                {
                    case State.left:
                        dir = State.upLeft;
                        break;
                    case State.right:
                        dir = State.upRight;
                        break;
                    case State.up:
                        dir = State.up;
                        break;
                }
                break;
            case State.leftUp:
                switch (state)
                {
                    case State.left:
                        dir = State.upLeft;
                        break;
                    case State.right:
                        dir = State.upRight;
                        break;
                    case State.up:
                        dir = State.up;
                        break;
                }
                break;
            case State.rightUp:
                switch (state)
                {
                    case State.left:
                        dir = State.upLeft;
                        break;
                    case State.right:
                        dir = State.upRight;
                        break;
                    case State.up:
                        dir = State.up;
                        break;
                }
                break;

            case State.right:
                switch (state)
                {
                    case State.up:
                        dir = State.rightUp;
                        break;
                    case State.down:
                        dir = State.rightDown;
                        break;
                    case State.right:
                        dir = State.right;
                        break;
                }
                break;
            case State.downRight:
                switch (state)
                {
                    case State.up:
                        dir = State.rightUp;
                        break;
                    case State.down:
                        dir = State.rightDown;
                        break;
                    case State.right:
                        dir = State.right;
                        break;
                }
                break;
            case State.upRight:
                switch (state)
                {
                    case State.up:
                        dir = State.rightUp;
                        break;
                    case State.down:
                        dir = State.rightDown;
                        break;
                    case State.right:
                        dir = State.right;
                        break;
                }
                break;

            case State.left:
                switch (state)
                {
                    case State.up:
                        dir = State.leftUp;
                        break;
                    case State.down:
                        dir = State.leftDown;
                        break;
                    case State.left:
                        dir = State.left;
                        break;
                }
                break;
            case State.upLeft:
                switch (state)
                {
                    case State.up:
                        dir = State.leftUp;
                        break;
                    case State.down:
                        dir = State.leftDown;
                        break;
                    case State.left:
                        dir = State.left;
                        break;
                }
                break;
            case State.downLeft:
                switch (state)
                {
                    case State.up:
                        dir = State.leftUp;
                        break;
                    case State.down:
                        dir = State.leftDown;
                        break;
                    case State.left:
                        dir = State.left;
                        break;
                }
                break;
        }
        return dir;
    }
}