using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.ObjectSpin;

public class PuceBoard : MonoBehaviour
{
    //define the size of the board
    public int width = 6;
    public int height = 8;
    //define some spacing for the board
    public float spacingX;
    public float spacingY;
    //get a reference to our puce prefabs
    public GameObject[] pucePrefabs;
    //get a reference to the collection nodes puceBoard + GO
    public Node[,] puceBoard;
    public GameObject puceBoardGO;

    public List<GameObject> pucesToDestroy = new();

    [SerializeField]
    private Puce selectedPuce;

    [SerializeField]
    private bool isProcessingMove;


    //layoutArray
    public ArrayLayout arrayLayout;
    //public static of puceboard
    public static PuceBoard Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeBoard();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Puce>())
            {
                if (isProcessingMove) { return; }
                Puce puce = hit.collider.gameObject.GetComponent<Puce>();
                Debug.Log("i have clicked a puce :" + puce.gameObject);

                SelectPuce(puce);
            }
        }
    }

    void InitializeBoard()
    {
        DestroyPuces();
        puceBoard = new Node[width, height];

        spacingX = (float)(width - 1) / 2;
        spacingY = (float)((height - 1) / 2) + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new Vector2(x - spacingX, y - spacingY);
                if (arrayLayout.rows[y].row[x])
                {
                    puceBoard[x, y] = new Node(false, null);
                }
                else
                {
                    int randomIndex = Random.Range(0, pucePrefabs.Length);

                    GameObject puce = Instantiate(pucePrefabs[randomIndex], position, Quaternion.identity);
                    puce.GetComponent<Puce>().SetIndicies(x, y);
                    puceBoard[x, y] = new Node(true, puce);

                    pucesToDestroy.Add(puce);
                }
            }
        }

        if (CheckBoard())
        {
            Debug.Log("We have matches let's re-create the board");
            InitializeBoard();
        }
        else
        {
            Debug.Log("There are no matches, it's time to start the game!");
        }
    }

    private void DestroyPuces()
    {
        if (pucesToDestroy != null)
        {
            foreach(GameObject puce in  pucesToDestroy)
            {
                Destroy(puce);
            }
            pucesToDestroy.Clear();
        }
    }

    public bool CheckBoard()
    {
        Debug.Log("Checking Board");
        bool hasMatched = false;

        List<Puce> pucesToRemove = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //checking if puce node is usable
                if (puceBoard[x, y].isUsable)
                {
                    //then proceed to get puce class in node.
                    Puce puce = puceBoard[x, y].puce.GetComponent<Puce>();

                    //ensure its not matched
                    if (!puce.isMatched)
                    {
                        //run some matching logic

                        MatchResult matchedPuces = IsConnected(puce);

                        if (matchedPuces.connectedPuces.Count >= 3)
                        {
                            //MatchResult superMatchedPuce = SuperMatch(matchedPuces);

                            pucesToRemove.AddRange(matchedPuces.connectedPuces);

                            foreach (Puce p in matchedPuces.connectedPuces)
                                p.isMatched = true;

                            hasMatched = true;
                        }
                    }
                }
            }
        }

        return hasMatched;
    }

    //private MatchResult SuperMatch(MatchResult matchedPuces)
    //{
        
    //}

    MatchResult IsConnected(Puce puce)
    {
        List<Puce> connectedPuces = new();
        PuceType puceType = puce.puceType;

        connectedPuces.Add(puce);

        //check right
        CheckDirection(puce, new Vector2Int(1, 0), connectedPuces);
        //check left
        CheckDirection(puce, new Vector2Int(-1, 0), connectedPuces);
        //have we made a 3 match? (Horizontal Match)
        if (connectedPuces.Count == 3)
        {
            Debug.Log("I have a normal horizontal match, the color of my match is: " + connectedPuces[0].puceType);

            return new MatchResult
            {
                connectedPuces = connectedPuces,
                direction = MatchDirection.Horizontal
            };
        }
        //checking for more than 3 (Long horizontal Match)
        else if (connectedPuces.Count > 3)
        {
            Debug.Log("I have a Long horizontal match, the color of my match is: " + connectedPuces[0].puceType);

            return new MatchResult
            {
                connectedPuces = connectedPuces,
                direction = MatchDirection.LongHorizontal
            };
        }
        //clear out the connectedpuces
        connectedPuces.Clear();
        //readd our initial puce
        connectedPuces.Add(puce);

        //check up
        CheckDirection(puce, new Vector2Int(0, 1), connectedPuces);
        //check down
        CheckDirection(puce, new Vector2Int(0, -1), connectedPuces);

        //have we made a 3 match? (Vertical Match)
        if (connectedPuces.Count == 3)
        {
            Debug.Log("I have a normal vertical match, the color of my match is: " + connectedPuces[0].puceType);

            return new MatchResult
            {
                connectedPuces = connectedPuces,
                direction = MatchDirection.Vertical
            };
        }
        //checking for more than 3 (Long Vertical Match)
        else if (connectedPuces.Count > 3)
        {
            Debug.Log("I have a Long vertical match, the color of my match is: " + connectedPuces[0].puceType);

            return new MatchResult
            {
                connectedPuces = connectedPuces,
                direction = MatchDirection.LongVertical
            };
        }
        else
        {
            return new MatchResult
            {
                connectedPuces = connectedPuces,
                direction = MatchDirection.None
            };
        }
    }

    void CheckDirection(Puce p, Vector2Int direction, List<Puce> connectedpuces)
    {
        PuceType puceType = p.puceType;
        int x = p.xIndex + direction.x;
        int y = p.yIndex + direction.y;

        //check that we're within the boundaries of the board
        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            if (puceBoard[x, y].isUsable)
            {
                Puce neighbourpuce = puceBoard[x, y].puce.GetComponent<Puce>();

                //does our puceType Match? it must also not be matched
                if (!neighbourpuce.isMatched && neighbourpuce.puceType == puceType)
                {
                    connectedpuces.Add(neighbourpuce);

                    x += direction.x;
                    y += direction.y;
                }
                else
                {
                    break;
                }

            }
            else
            {
                break;
            }
        }
    }
    #region Swapping Puces

    public void SelectPuce(Puce _puce) 
    { 
        if(selectedPuce == null)
        {
            Debug.Log(_puce);
            selectedPuce = _puce;
        }
        else if(selectedPuce == _puce)
        {
            selectedPuce = null;
        }
        else if (selectedPuce != _puce)
        {
            SwapPuce(selectedPuce, _puce);
            selectedPuce = null;
        }
    }
    private void SwapPuce(Puce _currentPuce, Puce _targetPuce)
    {
        if (!IsAdjacent (_currentPuce, _targetPuce))
        {
            return;
        }

        DoSwap(_currentPuce, _targetPuce);

        isProcessingMove = true;

        StartCoroutine(ProcessMatches(_currentPuce, _targetPuce));

    }

    private void DoSwap(Puce _currentPuce, Puce _targetPuce)
    {
        GameObject temp = puceBoard[_currentPuce.xIndex,_currentPuce.yIndex].puce;
        puceBoard[_currentPuce.xIndex, _currentPuce.yIndex].puce = puceBoard[_targetPuce.xIndex, _targetPuce.yIndex].puce;
        puceBoard[_targetPuce.xIndex, _targetPuce.yIndex].puce = temp;

        int tempXIndex = _currentPuce.xIndex;
        int tempYIndex = _currentPuce.yIndex;
        _currentPuce.xIndex = _targetPuce.xIndex;
        _currentPuce.yIndex = _targetPuce.yIndex;

        _targetPuce.xIndex = tempXIndex;
        _targetPuce.yIndex = tempYIndex;

        _currentPuce.MoveToTarget(puceBoard[_targetPuce.xIndex, _targetPuce.yIndex].puce.transform.position);
        _targetPuce.MoveToTarget(puceBoard[_currentPuce.xIndex, _currentPuce.yIndex].puce.transform.position);
    }

    private IEnumerator ProcessMatches(Puce crurrentPuce,Puce targetPuce)
    {
        yield return new WaitForSeconds(0.2f);

        bool hasMatch = CheckBoard();

        if (!hasMatch)
        {
            DoSwap(crurrentPuce, targetPuce);
        }
        isProcessingMove = false;

    }
    private bool IsAdjacent(Puce _currentpuce, Puce _targetPuce)
    {
        return Mathf.Abs(_currentpuce.xIndex - _targetPuce.xIndex) + Mathf.Abs(_currentpuce.yIndex - _targetPuce.yIndex) == 1;
    }

    #endregion

}

public class MatchResult
{
    public List<Puce> connectedPuces;
    public MatchDirection direction;
}

public enum MatchDirection
{
    Vertical,
    Horizontal,
    LongVertical,
    LongHorizontal,
    Super,
    None
}


