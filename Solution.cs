
using System;
using System.Collections.Generic;

public class Solution
{
    private sealed record Point(int index, int edgeTime){}
    private sealed record Step(int index, int timeFromSart){}

    private List<Point>[]? Graph;
    private static readonly int CAN_NOT_BE_REACHED = -1;

    public int[] MinimumTime(int numberOfNodes, int[][] edges, int[] disappearTime)
    {
        InitializeGraph(edges, numberOfNodes);
        return DijkstraSearchForMinTimePath(disappearTime, numberOfNodes);
    }

    private void InitializeGraph(int[][] edges, int numberOfNodes)
    {
        Graph = new List<Point>[numberOfNodes];
        for (int i = 0; i < numberOfNodes; ++i)
        {
            Graph[i] = new List<Point>();
        }

        foreach (int[] edge in edges)
        {
            int from = edge[0];
            int to = edge[1];
            int time = edge[2];

            if (from != to)
            {
                Graph[from].Add(new Point(to, time));
                Graph[to].Add(new Point(from, time));
            }
        }
    }

    private int[] DijkstraSearchForMinTimePath(int[] disappearTime, int numberOfNodes)
    {
        PriorityQueue<Step, int> minHeap = new PriorityQueue<Step, int>();
        minHeap.Enqueue(new Step(0, 0), 0);

        int[] minTime = new int[numberOfNodes];
        Array.Copy(disappearTime, minTime, disappearTime.Length);
        minTime[0] = 0;

        while (minHeap.Count > 0)
        {
            Step current = minHeap.Dequeue();
            if (current.timeFromSart > minTime[current.index])
            {
                continue;
            }

            foreach (Point next in Graph[current.index])
            {
                if (minTime[next.index] > current.timeFromSart + next.edgeTime)
                {
                    minTime[next.index] = current.timeFromSart + next.edgeTime;
                    minHeap.Enqueue(new Step(next.index, minTime[next.index]), minTime[next.index]);
                }
            }
        }

        MarkUnreachableNodes(minTime, disappearTime, numberOfNodes);
        return minTime;
    }

    private void MarkUnreachableNodes(int[] minTime, int[] disappearTime, int numberOfNodes)
    {
        for (int i = 0; i < numberOfNodes; ++i)
        {
            if (minTime[i] == disappearTime[i])
            {
                minTime[i] = CAN_NOT_BE_REACHED;
            }
        }
    }
}
