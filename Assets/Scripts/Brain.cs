using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainData
{
    string Name;

    public BrainData(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

public class Brain : MonoBehaviour
{
    List<BrainNode> Nodes = new List<BrainNode>();

    private void Start()
    {
        GatherBrainNodes(transform);

        Setup(new List<object>() { new BrainData("A"), new BrainData("B"), new BrainData("C"), new BrainData("D"), new BrainData("E"), new BrainData("F"), new BrainData("G"), new BrainData("H") });
    }

    public void Setup(List<object> dataList)
    {
        List<BrainNode> randomizeNodes = new List<BrainNode>();
        for(int i = 0; i < dataList.Count; ++i)
        {
            int randomIdx = UnityEngine.Random.Range(0, Nodes.Count);
            Nodes[randomIdx].data = dataList[i];
            randomizeNodes.Add(Nodes[randomIdx]);
            Nodes.RemoveAt(randomIdx);
        }

        for(int i = Nodes.Count - 1; i >= 0; --i)
        {
            if(Nodes[i].data == null)
            {
                UnityEngine.Object.Destroy(Nodes[i].gameObject);
            }
        }

        Nodes.Clear();
        Nodes.AddRange(randomizeNodes);
    }

    void GatherBrainNodes(Transform itTransform)
    {
        Nodes.AddRange(itTransform.gameObject.GetComponents<BrainNode>());

        for(int i = 0; i < itTransform.childCount; ++i)
        {
            GatherBrainNodes(itTransform.GetChild(i));
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(1))
        {
            foreach(BrainNode node in Nodes)
            {
                node.RightClickReleased();
            }
        }
    }
}
