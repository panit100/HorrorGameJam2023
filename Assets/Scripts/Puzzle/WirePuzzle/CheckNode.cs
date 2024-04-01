using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNode : MonoBehaviour
{
    public WireNode mainWireNode;
    [SerializeField] int checkArea = 20;

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = new Color(0, 1, 0, 0.5f);
    //    Gizmos.DrawCube(transform.position, new Vector3(checkArea, checkArea, 0));
    //}

    //public void CheckOverlapNode()
    //{
    //    Collider2D collider2D = Physics2D.OverlapBox(new Vector2(transform.position.x,transform.position.y) , new Vector2 (checkArea,checkArea),0f);
    //    if (collider2D != null)
    //    {
    //        if (collider2D.GetComponent<CheckNode>().mainWireNode.GetComponent<WireNode>().haveEnergy == true)
    //        {
    //            print(collider2D.GetComponent<CheckNode>().mainWireNode.name);
    //            mainWireNode.haveEnergy = true;
    //        }
    //    }
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {
        print("Check");
        if (mainWireNode.isStart == true)
        {
            mainWireNode.haveEnergy = true;
        }
        else
        {
            mainWireNode.haveEnergy = false;
        }
        if (collision.GetComponent<CheckNode>() != null)
        {
            //print("OK");
            if (collision.GetComponent<CheckNode>().mainWireNode.GetComponent<WireNode>().haveEnergy == true)
            {
                mainWireNode.haveEnergy = true;
            }
        }
        mainWireNode.DisplayEnergy();
        //mainWireNode.wirePuzzleController.CheckConnectionNodes();
    }
}
