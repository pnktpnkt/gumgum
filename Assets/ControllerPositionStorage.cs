using UnityEngine;
using System;

public class ControllerPositionStorage
{
    public int size;
    private int index;
    private Vector3[] posArray;

	public ControllerPositionStorage(int size)
	{
        this.size = size;
        index = 0;
        posArray = new Vector3[size];
	}

    public void add(Vector3 pos) {
        index++;
        if (index == size) index = 0;
        //velocityArray[index] = velocity.normalized;
        posArray[index] = pos;
    }

    public Vector3 getPunchDirection() {
        Vector3 vec = posArray[index] - posArray[(index+1)%size];
        //drawDebugAllLine();
        drawDebugDirectionLine(vec);
        return vec;
    }

    public void drawDebugDirectionLine(Vector3 vec) {
            GameObject newLine = new GameObject("Line");
            LineRenderer lRend = newLine.AddComponent<LineRenderer>();
            lRend.SetVertexCount(2);
            lRend.SetWidth(0.1f, 0.1f);
            Vector3 startVec = new Vector3(0.0f, 0.00f, 0.0f);
            lRend.SetPosition(0, startVec);
            lRend.SetPosition(1, vec * 2);
    }

    public void drawDebugAllLine() {
        //Debug.Log(index);
        for (int i = 0; i < size; i++) {
            GameObject newLine = new GameObject("Line");
            LineRenderer lRend = newLine.AddComponent<LineRenderer>();
            lRend.SetVertexCount(2);
            lRend.SetWidth(0.1f, 0.1f);
            Vector3 startVec = new Vector3(0.0f-5f+i, 0.00f, 0.0f);
            lRend.SetPosition(0, startVec);
            lRend.SetPosition(1, posArray[(index-i+size)%size] * 2);
           // Debug.Log((index - i + size) % size);
        }
    }
}
