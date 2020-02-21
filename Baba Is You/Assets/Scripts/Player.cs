using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int initialRow, initialCol;
    int currentRow, currentCol;

    private void Start()
    {
        transform.position = new Vector3(initialCol, initialRow, 0);
        currentRow = initialRow;
        currentCol = initialCol;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            currentRow += 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            currentRow -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
            currentCol += 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y-1, transform.position.z);
            currentCol -= 1;
        }
    }
}
