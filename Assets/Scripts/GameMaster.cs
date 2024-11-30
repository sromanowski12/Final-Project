// Seth Romanowski
// CSCI-C490

// GameMaster.cs
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{

    public enum State { idle, moveRoll, professor, library, gameWon, purchase };
    public enum Tile { college, library, library2, professor1, professor2, professor3, professor4, grades };
    public enum TileState { forSale, purchased, unavailable };
    // Tile 1, 2, 3 (Color: Purple)
    // ENG-W101, Price: 
    // ENG-W201, Price:
    // ENG-W202, Price: 

    // Tile 5, 6, 7 (Color: Light Blue)
    //
    //
    //

    // Tile 9, 10, 11 (Color: Pink)
    //
    //
    //

    public Material[] mats;

    public GameObject player, player2;
    public GameObject[] tiles;
    public Tile[] tileType;
    public TileState[] tileState;
    public State state;

    public Text btn1text, btn2text, btn3text, infoText, scoreText, scoreText2, tuitionText, tuitionText2;
    public Camera mainCam;
    public Button btn1, btn2, btn3;

    int dieValue, playerTuition = 5000, playerTuition2 = 5000, playerScore, playerScore2;
    public GameObject dieObject, cardObject;
    static float[,] dieRotations = { { 0, 0, 0 }, { -90, 0, 0 }, { 0, 0, -90 }, { 0, 0, 90 }, { 90, 0, 0 }, { 180, 0, 0 } };
    public int[] tilePrice = { 0, 60, 60, 60, 200, 100, 100, 120, 0, 140, 140, 160, 200, 180, 180, 200, 0, 220, 220, 240, 200, 260, 260, 280, 0, 300, 300, 320, 200, 350, 350, 400 };
    public int[] tileCredit = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };


    public int playerPos, playerPos2, playerTurn=1;
    Vector3 playerOffset, playerOffset2;

    // Use this for initialization
    void Start()
    {
        int i, j;
        tileType = new Tile[tiles.Length];
        tileState = new TileState[tiles.Length];
        // tilePrice = new int[tiles.Length];

        for (i = 0; i < tiles.Length; i++)
        {
            // use the name of the tiles to populate the array
            tiles[i] = GameObject.Find("tile" + i);
            if (i == 0 || i == 8 || i == 16 || i == 24)
            {
                tileState[i] = TileState.unavailable;
                // tilePrice[i] = 0;
            }
            
            else
            {
                tileState[i] = TileState.forSale;
            }

            if (tiles[i])
            {
                // use the material's name to set the type
                Material mat = tiles[i].GetComponent<Renderer>().material;
                tileType[i] = Tile.college;
                for (j = 0; j < mats.Length; j++)
                    if (mat.name.IndexOf(mats[j].name) >= 0)
                        tileType[i] = (Tile)j;
            }
        }
        RollDie();
        playerOffset = tiles[0].transform.position - player.transform.position;
        playerOffset2 = tiles[0].transform.position - player2.transform.position;
        PlacePlayer(0);     // starting position for the game
        PlaceButtons();     // place the buttons based on the screen size
        scoreText.text = "Player 1: 0";
        tuitionText.text = "Player 1: $5,000";
        state = State.idle; // initial state
        SetButtons();
        infoText.text = "";
        playerTurn = 2;
        PlacePlayer(0);     // starting position for the game
        scoreText2.text = "Player 2: 0";
        tuitionText2.text = "Player 2: $5,000";
        state = State.idle; // initial state
    }

    // Places the player on a tile given by the index in the tiles array.
    // Only uses the x and z of the tile.
    public void PlacePlayer(int place)
    {
        
        if (playerTurn == 1)
        {
            player.transform.position = new Vector3(tiles[place].transform.position.x,
                                                0.25f,
                                                tiles[place].transform.position.z);
            playerPos = place; // index in the array of tiles
            
        }
        else
        {
            player2.transform.position = new Vector3(tiles[place].transform.position.x,
                                                0.25f,
                                                tiles[place].transform.position.z);
            playerPos2 = place;
        }
    }

    // Update is called once per frame
    // Will probably need this to implement the actions
    void Update()
    {
        switch (state)
        {
            case State.moveRoll:
                // PlacePlayer(dieValue + playerPos);
                if (playerTurn == 1)
                {
                    if ((dieValue + playerPos) >= 32)               // Account for size of the tiles array
                    {
                        PlacePlayer(0);                            // Move player to the last tile
                                                                   // displayText.text = "GAME WON!!!";
                        state = State.gameWon;                      // Move R --> Game Won
                    }
                    else
                        PlacePlayer(dieValue + playerPos);          // Move R - player's roll, R = dieTop
                    Debug.Log(playerPos);

                    if (tileType[playerPos] == Tile.college && tileState[playerPos] == TileState.forSale)
                    {
                        state = State.purchase;
                    }

                    else if (tileType[playerPos] == Tile.professor1 || tileType[playerPos] == Tile.professor2 || tileType[playerPos] == Tile.professor3 || tileType[playerPos] == Tile.professor4)
                        state = State.professor;
                    else
                        state = State.idle;
                }
                else if (playerTurn == 2)
                {
                    if ((dieValue + playerPos2) >= 32)               // Account for size of the tiles array
                    {
                        PlacePlayer(0);                            // Move player to the last tile
                                                                   // displayText.text = "GAME WON!!!";
                        state = State.gameWon;                      // Move R --> Game Won
                    }
                    else
                        PlacePlayer(dieValue + playerPos2);          // Move R - player's roll, R = dieTop
                    Debug.Log(playerPos2);

                    if (tileType[playerPos2] == Tile.college && tileState[playerPos2] == TileState.forSale)
                    {
                        state = State.purchase;
                    }
                    else if (tileType[playerPos2] == Tile.professor1 || tileType[playerPos2] == Tile.professor2 || tileType[playerPos2] == Tile.professor3 || tileType[playerPos2] == Tile.professor4)
                        state = State.professor;
                    else
                        state = State.idle;
                    
                }
                break;
            
        }
    }

    // Sets the rotation of the die object based on the value of the die.
    void SetDie(int value)
    {
        float x, y, z;
        if (1 <= value && value <= 6)
        {
            x = dieRotations[value - 1, 0];
            y = dieRotations[value - 1, 1];
            z = dieRotations[value - 1, 2];
            dieObject.transform.eulerAngles = new Vector3(x, y, z);
        }
    }

    // Roll the die randomly, then set the die object and the info text.
    void RollDie()
    {
        dieValue = Random.Range(1, 6);       // random die
        SetDie(dieValue);
        infoText.text = "Roll: " + dieValue; // show it
    }

    //place the buttons based on the screen size
    void PlaceButtons()
    {
        //Camera cam = GameObject.Find("Main Camera");
        //Button btn = GameObject.Find("Button1") as Button;
        float ymax = Screen.height;
        float xmax = Screen.width;
        float wid, hei;
        wid = btn1.GetComponent<RectTransform>().rect.width;
        hei = btn1.GetComponent<RectTransform>().rect.height;
        btn1.transform.position = new Vector3(xmax / 2 - 10f - 1.5f * wid, ymax - hei / 2 - 5f,
                                              btn1.transform.position.z);
        btn2.transform.position = new Vector3(xmax / 2 - wid / 2, ymax - hei / 2 - 5f,
                                              btn2.transform.position.z);
        btn3.transform.position = new Vector3(xmax / 2 + wid / 2 + 10f, ymax - hei / 2 - 5f,
                                              btn3.transform.position.z);

        wid = scoreText.GetComponent<RectTransform>().rect.width;
        hei = scoreText.GetComponent<RectTransform>().rect.height;
        scoreText.transform.position = new Vector3(5f + wid / 2, 25f,
                                                   scoreText.transform.position.z);
        scoreText2.transform.position = new Vector3(5f + wid / 2, 5f,
                                                   scoreText.transform.position.z);

        tuitionText.transform.position = new Vector3(5f + wid / 2, 100f,
                                                   tuitionText.transform.position.z);
        tuitionText2.transform.position = new Vector3(5f + wid / 2, 80f,
                                                   tuitionText2.transform.position.z);
        wid = infoText.GetComponent<RectTransform>().rect.width;
        hei = infoText.GetComponent<RectTransform>().rect.height;

        infoText.transform.position = new Vector3(xmax - wid / 2 - 5f, ymax - hei / 2 - 5f,
                                                  infoText.transform.position.z);

    }

    // first button action
    public void Action1()
    {
        switch (state)
        {
            case State.idle:
                RollDie();
                state = State.moveRoll;              // next state
                if (playerTurn == 1)
                    playerTurn = 2;
                else
                    playerTurn = 1;
                break;

            case State.purchase:
                if (playerTurn == 1)
                {
                    tuitionText.text = "Player 1: $" + System.Convert.ToString(playerTuition - tilePrice[playerPos]);
                    tilePrice[playerPos] = 0;
                    tileState[playerPos] = TileState.purchased;
                    playerScore += tileCredit[playerPos];
                    scoreText.text = "Player 1: " + System.Convert.ToString(playerScore) + " credits";
                    tileCredit[playerPos] = 0;
                    state = State.idle;
                }
                else
                {
                    tuitionText2.text = "Player 2: $" + System.Convert.ToString(playerTuition2 - tilePrice[playerPos2]);
                    tilePrice[playerPos2] = 0;
                    tileState[playerPos2] = TileState.purchased;
                    playerScore2 += tileCredit[playerPos2];
                    scoreText2.text = "Player 2: " + System.Convert.ToString(playerScore2) + " credits";
                    tileCredit[playerPos2] = 0;
                    state = State.idle;
                }
                break;
        }
        SetButtons();
    }
    // 
    public void Action2()
    {
        switch (state)
        {
            case State.professor:
                // RollDie();
                cardObject.transform.eulerAngles = new Vector3(90f, cardObject.transform.rotation.y, 180f);
                state = State.idle;
                break;
            case State.purchase:
                state = State.idle;
                break;
        }
    }
    // 
    public void Action3()
    {

    }

    // Sets the text of the buttons based on the context.
    void SetButtons()
    {
        btn1text.text = "";
        btn2text.text = "";
        btn3text.text = "";

        switch (state)
        {
            case State.idle:
                btn1text.text = "Roll";
                break;

            case State.purchase:
                btn1text.text = "Purchase";
                btn2text.text = "Ignore";
                break;
        }
    }
}
