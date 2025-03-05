using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PuceBoard : Singleton<PuceBoard>
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
    public GameObject pucesParent;

    public List<GameObject> pucesToDestroy = new();

    [SerializeField]
    private Puce selectedPuce; //puce 1 sélectionnée par le joueur au clik
    private Puce selectedPuce2; //puce 2 sélectionnée par le joueur au relachement du clik

    [SerializeField]
    private bool isProcessingMove;

    private bool gameassarted = false;

    //layoutArray
    public ArrayLayout arrayLayout;
    //public static of puceboard
    // public static PuceBoard Instance;

    //pour empécher le joueur de jouer quand la parti est fini
    public bool isGameFinish = false;

    private List<Puce> pucesToRemove = new();

    //private void Awake()
    //{
    //    Instance = this;
    //}

    void Start()
    {
        selectedPuce = null;
        selectedPuce2 = null;
        InitializeBoard();
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0)) 
    //    {
    //        UserClicLeftDown();
    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        UserClicLeftUp();
    //    }
    //}

    //fonction pour quand l'utilisateur click sur une puce
    public void UserClicLeftDown()
    {
        if (isGameFinish) { return; }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<Puce>())
        {
            if (isProcessingMove) { return; }
            Puce puce = hit.collider.gameObject.GetComponent<Puce>();
            puce = hit.collider.gameObject.GetComponent<Puce>();
            Debug.Log("i have clicked a puce :" + puce.gameObject);

            SelectPuce(puce);
        }
    }

    //fonction pour quand l'utilisateur relache le click sur une puce
    public void UserClicLeftUp()
    {
        if (isGameFinish) { return; }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<Puce>())
        {
            if (isProcessingMove) { return; }
            Puce puce = hit.collider.gameObject.GetComponent<Puce>();
            //selectedPuce2 = hit.collider.gameObject.GetComponent<Puce>();
            Debug.Log("i have clicked a puce :" + puce.gameObject);

            SelectPuce(puce);
        }

    }

    public void InitializeBoard()
    {
        DestroyPuces();
        puceBoard = new Node[width, height];

        spacingX = (float)((width - 1) / 2) - 3;
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
                    puce.transform.SetParent(pucesParent.transform);
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
            gameassarted = true;
        }
    }

    public void DestroyPuces()
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

        pucesToRemove.Clear();

        foreach (Node node in puceBoard) 
        { 
            if(node.puce != null)
            {
                node.puce.GetComponent<Puce>().isMatched = false;
            }
        }

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
                            MatchResult superMatchedPuce = SuperMatch(matchedPuces);

                            pucesToRemove.AddRange(superMatchedPuce.connectedPuces);

                            foreach (Puce p in superMatchedPuce.connectedPuces)
                                p.isMatched = true;

                            hasMatched = true;
                        }
                    }
                }
            }
        }

        return hasMatched;
    }

    public IEnumerator ProcessTurnOnMatchedBoard(bool _subtractMoves)
    {
        foreach (Puce puceToRemove in pucesToRemove)
        {
            puceToRemove.isMatched = false;
        }

        RemoveAndRefill(pucesToRemove);
        yield return new WaitForSeconds(0.4f);

        if (CheckBoard())
        {
            StartCoroutine(ProcessTurnOnMatchedBoard(false));
        }
    }

    private void RemoveAndRefill(List<Puce> pucesToRemove)
    {
        foreach (Puce p in pucesToRemove) 
        {
            int xIndex = p.xIndex;
            int yIndex = p.yIndex;

            Destroy(p.gameObject);

            puceBoard[xIndex, yIndex] = new Node(true, null);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (puceBoard[x,y].puce == null)
                    RefillPotion(x, y);
            }
        } 
    }

    private void RefillPotion(int x, int y)
    {
        int yOffset = 1;

        while (y + yOffset < height && puceBoard[x,y+yOffset].puce == null) 
        {
            yOffset++;
        }
        if (y + yOffset < height && puceBoard[x, y + yOffset].puce != null)
        {
            Puce puceAbove = puceBoard[x, y+yOffset].puce.GetComponent<Puce>();
            Vector3 targetPos = new Vector3(x - spacingX, y - spacingY, puceAbove.transform.position.z);
            puceAbove.MoveToTarget(targetPos);
            puceAbove.SetIndicies(x, y);
            puceBoard[x, y] = puceBoard[x, y + yOffset];
            puceBoard[x, y + yOffset] = new Node(true,null);
        }
        if (y + yOffset == height)
        {
            SpawnPuceAtTop(x);
        }


    }

    private void SpawnPuceAtTop(int x)
    {
        int index = FindIndexOfLowestNull(x);
        int locationToMoveTo = 8 - index;
        
        int randomIndex = Random.Range(0,pucePrefabs.Length);
        GameObject newPuce = Instantiate(pucePrefabs[randomIndex], new Vector2(x - spacingX,height - spacingY), Quaternion.identity);
        newPuce.transform.SetParent(pucesParent.transform);

        newPuce.GetComponent<Puce>().SetIndicies(x,index);
        puceBoard[x,index]= new Node(true,newPuce);
        Vector3 targetPosition = new Vector3(newPuce.transform.position.x, newPuce.transform.position.y - locationToMoveTo, newPuce.transform.position.z);
        newPuce.GetComponent<Puce>().MoveToTarget(targetPosition);
    }

    private int FindIndexOfLowestNull(int x) 
    {
        int lowestNull = 99;
        for (int y = 7; y >= 0; y--)
        {
            if (puceBoard[x,y].puce == null)
            {
                lowestNull = y;
            }
        }
        return lowestNull; 
    }

    private MatchResult SuperMatch(MatchResult matchedResults)
    {
        if(matchedResults.direction == MatchDirection.Horizontal || matchedResults.direction == MatchDirection.LongHorizontal)
        {
            foreach(Puce p in matchedResults.connectedPuces)
            {
                List<Puce> extraConnectedPuces = new();

                CheckDirection(p, new Vector2Int(0, 1), extraConnectedPuces);
                CheckDirection(p, new Vector2Int(0, -1), extraConnectedPuces);

                if (extraConnectedPuces.Count >= 2) 
                {
                    Debug.Log("I have a super Horizontal Match");
                    extraConnectedPuces.AddRange(matchedResults.connectedPuces);
                    if (gameassarted)
                        CandyGameManager.Instance.ProcessTurn(1000, 2);

                    return new MatchResult
                    {
                        connectedPuces = extraConnectedPuces,
                        direction = MatchDirection.Super,
                    };
                }
            }
            return new MatchResult
            {
                connectedPuces = matchedResults.connectedPuces,
                direction = matchedResults.direction
            };
        }
        else if (matchedResults.direction == MatchDirection.Vertical || matchedResults.direction == MatchDirection.LongVertical)
        {
            foreach (Puce p in matchedResults.connectedPuces)
            {
                List<Puce> extraConnectedPuces = new();

                CheckDirection(p, new Vector2Int(1, 0), extraConnectedPuces);
                CheckDirection(p, new Vector2Int(-1, 0), extraConnectedPuces);

                if (extraConnectedPuces.Count >= 2)
                {
                    Debug.Log("I have a super Vertical Match");
                    extraConnectedPuces.AddRange(matchedResults.connectedPuces);
                    if (gameassarted)
                        CandyGameManager.Instance.ProcessTurn(1000, 2);

                    return new MatchResult
                    {
                        connectedPuces = extraConnectedPuces,
                        direction = MatchDirection.Super,
                    };
                }
            }
            return new MatchResult
            {
                connectedPuces = matchedResults.connectedPuces,
                direction = matchedResults.direction
            };
        }
        return null;
    }

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
            if (gameassarted)
                CandyGameManager.Instance.ProcessTurn(100, 1);

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
            if (gameassarted)
                CandyGameManager.Instance.ProcessTurn(500, 1);

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
            if (gameassarted)
                CandyGameManager.Instance.ProcessTurn(100, 1);
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
            if (gameassarted)
                CandyGameManager.Instance.ProcessTurn(500, 1);
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
        //Si on a pour l'instant sélectionne aucune PUCE
        if(selectedPuce == null && selectedPuce2 == null)
        {
            print("Selection de la puce 1");
            selectedPuce = _puce;
        }
        //Si on a déjà sélectionné cette PUCE
        else if (selectedPuce == _puce)
        {
            print("Deselection de la puce 1");
            selectedPuce = null;
            selectedPuce2 = null; //par précaution
        }
        else if (selectedPuce != _puce && selectedPuce2 == null)
        {
            print("Selection de la puce 2");
            selectedPuce2 = _puce;
            SwapPuce(selectedPuce, selectedPuce2);
            selectedPuce = null;
            selectedPuce2 = null;
        }
    }
    private void SwapPuce(Puce _currentPuce, Puce _targetPuce)
    {
        if (!IsAdjacent (_currentPuce, _targetPuce))
        {
            return;
        }
        Debug.LogWarning("Les puce sont adjacentes, on peut les échanger!");
        DoSwap(_currentPuce, _targetPuce);

        isProcessingMove = true;

        StartCoroutine(ProcessMatches(_currentPuce, _targetPuce));

    }

    private void DoSwap(Puce _currentPuce, Puce _targetPuce)
    {
        Debug.LogWarning("Echange des puces");
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

        if (CheckBoard())
        {
            StartCoroutine(ProcessTurnOnMatchedBoard(true));
        }

        else 
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

#region Match 
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

#endregion

#region cascadin puce




#endregion