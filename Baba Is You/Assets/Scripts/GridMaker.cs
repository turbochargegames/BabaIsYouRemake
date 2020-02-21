using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMaker : MonoBehaviour
{
    int rows, cols;
    public GameObject cellHolder;
    public List<LevelCreator> levelHolder = new List<LevelCreator>();
    public List<GameObject> cells = new List<GameObject>();
    public List<SpriteLibrary> spriteLibrary = new List<SpriteLibrary>();
    public static GridMaker instance=null;
    int currentLevel = 0;
    public int Rows
    {
        get { return rows; }
    }

    public int Cols
    {
        get { return cols; }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", 0);
        }
        currentLevel = PlayerPrefs.GetInt("Level");

        float count = levelHolder[currentLevel].level.Count;
        rows = (int)Mathf.Sqrt(count);
        cols = rows;

        CreateGrid();
        CompileRules();
    }

    public void CreateGrid()
    {
        int counter = 0;
        for (int i = 0; i < levelHolder[currentLevel].level.Count; i++)
        {
            if (levelHolder[currentLevel].level[i] != ElementTypes.Empty)
            {
                GameObject g = Instantiate(cellHolder, new Vector3(counter % cols, counter / rows, 0), Quaternion.identity);
                cells.Add(g);
                ElementTypes currentElement = levelHolder[currentLevel].level[i];

                g.GetComponent<CellProperty>().AssignInfo(counter / rows, counter % cols, currentElement);
                //Debug.Log( currentElement.ToString() + "R : " + i / rows + " C : " + i % cols);


            }
            counter++;
        }

    }

    public Sprite ReturnSpriteOf(ElementTypes e)
    {
        return spriteLibrary.Find(x => x.element == e).sprite;
    }

    public Vector2 Return2D(int i)
    {
        return new Vector2(i % cols, i / rows);
    }


    public bool IsStop(int r,int c, Vector2 dir)
    {
        bool isPush = false;
        int curRow = r, curCol = c;
        List<GameObject> atRC = FindObjectsAt(curRow, curCol);
        if (r >= rows || c >= cols || r < 0 || c < 0)
            return true;
        foreach(GameObject g in atRC)
        {
            CellProperty currentCell = g.GetComponent<CellProperty>();
            
                if (currentCell.IsStop)
                {
                    return true;
                }
                else if (currentCell.IsPushable)
                {
                    isPush = true;
                }
        }

        if (!isPush)
            return false;

        if (dir == Vector2.right)
        {
            return IsStop(curRow, curCol + 1, Vector2.right);
        }
        else if (dir == Vector2.left)
        {
            return IsStop(curRow, curCol - 1, Vector2.left);
        }
        else if (dir == Vector2.up)
        {
            return IsStop(curRow+1, curCol, Vector2.up);
        }
        else if (dir == Vector2.down)
        {
            return IsStop(curRow-1, curCol , Vector2.down);
        }
        return true;
    }

    public void CompileRules()
    {
        ResetData();
        for(int i = 0; i < cells.Count; i++)
        {
            CellProperty currentcell = cells[i].GetComponent<CellProperty>();

            if (IsElementStartingWord(currentcell.Element))
            {

                /*if (DoesListContainElement(FindObjectsAt(currentcell.CurrentRow + 1, currentcell.CurrentCol), ElementTypes.IsWord))
                {
                    if (DoesListContainWord(FindObjectsAt(currentcell.CurrentRow +2, currentcell.CurrentCol)))
                    {
                        Rule(currentcell.Element, ReturnWordAt(currentcell.CurrentRow + 2, currentcell.CurrentCol));
                    }
                }*/
                if (DoesListContainElement(FindObjectsAt(currentcell.CurrentRow - 1, currentcell.CurrentCol), ElementTypes.IsWord))
                {
                    if (DoesListContainWord(FindObjectsAt(currentcell.CurrentRow - 2, currentcell.CurrentCol)))
                    {
                        Rule(currentcell.Element, ReturnWordAt(currentcell.CurrentRow - 2, currentcell.CurrentCol));
                    }
                }
                if (DoesListContainElement(FindObjectsAt(currentcell.CurrentRow , currentcell.CurrentCol+1), ElementTypes.IsWord))
                {
                    if (DoesListContainWord(FindObjectsAt(currentcell.CurrentRow , currentcell.CurrentCol+2)))
                    {
                        Rule(currentcell.Element, ReturnWordAt(currentcell.CurrentRow, currentcell.CurrentCol+2));
                    }
                }

                /*if (DoesListContainElement(FindObjectsAt(currentcell.CurrentRow, currentcell.CurrentCol -1), ElementTypes.IsWord))
                {
                    if (DoesListContainWord(FindObjectsAt(currentcell.CurrentRow, currentcell.CurrentCol - 2)))
                    {
                        Rule(currentcell.Element, ReturnWordAt(currentcell.CurrentRow, currentcell.CurrentCol - 2));
                    }
                }*/
            }

        }
    }


    public ElementTypes GetActualObjectFromWord(ElementTypes e) {
        if (e == ElementTypes.BabaWord)
        {
            return ElementTypes.Baba;
        }
        else if (e == ElementTypes.FlagWord)
        {
            return ElementTypes.Flag;
        }
        else if (e == ElementTypes.RockWord)
        {
            return ElementTypes.Rock;
        }
        else if (e == ElementTypes.WallWord)
        {
            return ElementTypes.Wall;
        }
        return ElementTypes.Empty;

    }


    public void Rule(ElementTypes a, ElementTypes b)
    {
        if ((int)b >= 100 && (int)b<150)
        {
            //Replace all a objects to b
            List<CellProperty> cellsOf = GetAllCellsOf(GetActualObjectFromWord(a));
            for (int i= 0;i<cellsOf.Count;i++)
            {
                cellsOf[i].ChangeObject(GetCellOf(GetActualObjectFromWord(b)));
            }
        }
        else if ((int)b >= 150)
        {
            //Properties change
            if(b== ElementTypes.YouWord)
            {
                foreach(CellProperty p in GetAllCellsOf(GetActualObjectFromWord(a)))
                {
                    p.IsPlayer(true);
                }
                //player property true
            }
            else if (b == ElementTypes.PushWord)
            {
                //pushable property true
                foreach (CellProperty p in GetAllCellsOf(GetActualObjectFromWord(a)))
                {
                    p.IsItPushable(true);
                }
            }
            else if (b == ElementTypes.WinWord)
            {
                //win property true
                foreach (CellProperty p in GetAllCellsOf(GetActualObjectFromWord(a)))
                {
                    p.IsItWin(true);
                }
            }
            else if (b == ElementTypes.StopWord)
            {
                //stop property true
                foreach (CellProperty p in GetAllCellsOf(GetActualObjectFromWord(a)))
                {
                    p.IsItStop(true);
                }
            }
        }

    }

    public void ResetData()
    {
        foreach(GameObject g in cells)
        {
            g.GetComponent<CellProperty>().Initialize();
        }
    }

    public CellProperty GetCellOf(ElementTypes e)
    {
        foreach (GameObject g in cells)
        {
            if (g.GetComponent<CellProperty>().Element == e)
            {
                return g.GetComponent<CellProperty>();
            }
        }
        return null;
    }

    public List<CellProperty> GetAllCellsOf(ElementTypes e)
    {
        List<CellProperty> cellProp = new List<CellProperty>();

        foreach(GameObject g in cells)
        {
            if(g.GetComponent<CellProperty>().Element == e)
            {
                cellProp.Add(g.GetComponent<CellProperty>());
            }
        }
        return cellProp;

    }

    public bool IsTherePushableObjectAt(int r, int c)
    {
        List<GameObject> objectsAtRC = FindObjectsAt(r, c);

        foreach (GameObject g in objectsAtRC)
        {
            if (g.GetComponent<CellProperty>().IsPushable)
            {
                return true;
            }
        }
        return false;

    }

    public GameObject GetPushableObjectAt(int r, int c)
    {
        List<GameObject> objectsAtRC = FindObjectsAt(r, c);

        foreach (GameObject g in objectsAtRC)
        {
            if (g.GetComponent<CellProperty>().IsPushable)
            {
                return g;
            }
        }

        return null;
    }

    public bool IsElementStartingWord(ElementTypes e)
    {
        if((int)e>=100 && (int)e < 150)
        {
            return true;
        }
        return false;
    }

    public List<GameObject> FindObjectsAt(int r,int c)
    {
        return cells.FindAll(x => x.GetComponent<CellProperty>().CurrentRow == r && x.GetComponent<CellProperty>().CurrentCol == c);
    }

    public ElementTypes ReturnWordAt(int r,int c)
    {
        List<GameObject> l = FindObjectsAt(r, c);
        
        foreach(GameObject g in l)
        {
            ElementTypes e = g.GetComponent<CellProperty>().Element;
            if ((int)e >= 100)
            {
                return e;
            }
           
        }
        return ElementTypes.Empty;
    }

    public bool DoesListContainElement(List<GameObject> l, ElementTypes e)
    {
        foreach(GameObject g in l)
        {
            if(g.GetComponent<CellProperty>().Element == e)
            {
                return true;
            }
        }
        return false;
    }

    public bool DoesListContainWord(List<GameObject> l)
    {
        foreach (GameObject g in l)
        {
            if ((int)g.GetComponent<CellProperty>().Element >= 100)
            {
                return true;
            }
        }
        return false;
    }


    public bool IsElementIsWord(ElementTypes e)
    {
        if ((int)e == 99)
        {
            return true;
        }
        return false;
    }

    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("Level") >= levelHolder.Count)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
[System.Serializable]
public class SpriteLibrary{

    public ElementTypes element;
    public Sprite sprite;

}
