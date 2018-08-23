using UnityEngine;
using System;

public class ControllerVelocityStorage
{
    public int size;
    private int index;
    private Vector3[] velocityArray;

	public ControllerVelocityStorage()
	{
        size = 10;
        index = 0;
        velocityArray = new Vector3[size];
	}

    public void add(Vector3 velocity) {
        index++;
        if (index == size) index = 0;
        //velocityArray[index] = velocity.normalized;
        velocityArray[index] = velocity;
    }

    public Vector3 getPunchDirection() {
        ///*
        Vector3 directionVec = new Vector3(0,0,0);
        for(int i = 0; i < size; i++) {
            directionVec += velocityArray[i];
        }
        directionVec = directionVec / (float)size;
        //*/
        /*
        Vector3 directionVec = new Vector3(0, 0, 0);
        for (int i = 0; i < size; i++) {
            int index_a = (index - i + size) % size;
            int index_b = (index - i -1 + size) % size;
            Vector3 velocity = velocityArray[index_a] - velocityArray[index_b];
            directionVec += velocity.normalized;
        }
        directionVec = directionVec / (float)size;
        */
        //drawDebugDirectionLine(directionVec);
        drawDebugAllLine();
        return directionVec;
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
            lRend.SetPosition(1, velocityArray[(index-i+size)%size] * 2);
           // Debug.Log((index - i + size) % size);
        }
    }
}
