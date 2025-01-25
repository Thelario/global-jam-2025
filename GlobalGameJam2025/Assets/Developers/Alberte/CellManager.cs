using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    public GameObject[] cells;
    public bool randomizing = false;
    public int level = 10;
    public Text count;
    
    public Material[] materials;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cells = GameObject.FindGameObjectsWithTag("Cell");
        StartCoroutine(Randomize());
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Randomize()
    {

        while (true)
        {
            count.text = "3";
            yield return new WaitForSeconds(1f);
            count.text = "2";
            yield return new WaitForSeconds(1f);
            count.text = "1";
            yield return new WaitForSeconds(1f);
            count.text = "";
            yield return new WaitForSeconds(1f);

            
            for (int x = 0; x < 10; x++)
            {

                int random = Random.Range(0, cells.Length);
                while (cells[random].GetComponent<Cell>().trap)
                {
                    random = Random.Range(0, cells.Length);
                }
                cells[random].GetComponent<Cell>().trap = true;
            }
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].GetComponent<Cell>().trap == true)
                {
                    cells[i].GetComponent<Renderer>().material = materials[0];

                }
                else
                {

                    cells[i].GetComponent<Renderer>().material = materials[1];

                }

            }

            yield return new WaitForSeconds(4f);
            for (int i = 0; i < cells.Length; i++)
            {
                if (!cells[i].GetComponent<Cell>().trap)
                {
                    cells[i].SetActive(false);

                }


            }
            yield return new WaitForSeconds(3);
            for (int i = 0; i < cells.Length; i++)
            {

                cells[i].SetActive(true);
                cells[i].GetComponent<Renderer>().material = materials[2];

            }
            level--;
            if (level == 1)
            {
                level = 1;
            }
        }
        







        //while (randomizing)
        //{

        //    for (int i = 0; i < cells.Length; i++)
        //    {
        //        if (cells[i].GetComponent<Cell>().trap == true)
        //        {
        //            cells[i].GetComponent<Renderer>().material = materials[0];
        //        }
        //        else
        //        {
        //            cells[i].GetComponent<Renderer>().material = materials[1];
        //        }

        //    }
        //    yield return new WaitForSeconds(0.3f);
        //    randomizing = false;
        //}
        //Debug.Log("SALEEE");


        //yield return new WaitForSeconds(0.3f);
        //randomizing = false;
        //for (int i = 0; i < cells.Length; i++)
        //{
        //    if (cells[i].GetComponent<Cell>().trap == true)
        //    {
        //        cells[i].GetComponent<Renderer>().material = materials[0];
        //    }
        //    else
        //    {
        //        cells[i].GetComponent<Renderer>().material = materials[1];
        //    }

        //}
        //yield return new WaitForSeconds(0.3f);
        //randomizing = false;


        //for( int z=0; z<level; z++)
        //{
        //    int random = Random.Range(0, cells.Length);
        //    while (cells[random].GetComponent<Cell>().trap)
        //    {
        //        random = Random.Range(0, cells.Length);
        //    }
        //    cells[random].GetComponent<Cell>().trap = true;
        //    cells[random].GetComponent<Renderer>().material = materials[1];
        //}
        //for (int i = 0; i < cells.Length; i++)
        //{
        //    if (cells[i].GetComponent<Cell>().trap)
        //    {
        //        cells[i].GetComponent<Renderer>().material = materials[0];
        //    }



        //}
        //yield return new WaitForSeconds(3f);
        //for (int i = 0; i < cells.Length; i++)
        //{
        //    if (!cells[i].GetComponent<Cell>().trap)
        //    {
        //        cells[i].SetActive(false);
        //    }



        //}


    }
}
