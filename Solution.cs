
using System;
using System.Collections.Generic;

public class Solution
{
    private sealed record Point(int index, int edgeTime){}
    private sealed record Step(int index, int timeFromSart){}

    private List<Point>[]? graph;
    private static readonly int CAN_NOT_BE_REACHED = -1;

    public int[] MinimumTime(int numberOfNodes, int[][] edges, int[] disappearTime)
    {
        initializeGraph(edges, numberOfNodes);
        return dijkstraSearchForMinTimePath(disappearTime, numberOfNodes);
    }

    private void initializeGraph(int[][] edges, int numberOfNodes)
    {
        graph = new List<Point>[numberOfNodes];
        for (int i = 0; i < numberOfNodes; ++i)
        {
            graph[i] = new List<Point>();
        }

        foreach (int[] edge in edges)
        {
            int from = edge[0];
            int to = edge[1];
            int time = edge[2];

            if (from != to)
            {
                graph[from].Add(new Point(to, time));
                graph[to].Add(new Point(from, time));
            }
        }
    }

    private int[] dijkstraSearchForMinTimePath(int[] disappearTime, int numberOfNodes)
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

            foreach (Point next in graph[current.index])
            {
                if (minTime[next.index] > current.timeFromSart + next.edgeTime)
                {
                    minTime[next.index] = current.timeFromSart + next.edgeTime;
                    minHeap.Enqueue(new Step(next.index, minTime[next.index]), minTime[next.index]);
                }
            }
        }

        markUnreachableNodes(minTime, disappearTime, numberOfNodes);
        return minTime;
    }

    private void markUnreachableNodes(int[] minTime, int[] disappearTime, int numberOfNodes)
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
