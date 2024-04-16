
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.PriorityQueue;

public class Solution {

    private record Point(int index, int edgeTime){}
    private record Step(int index, int timeFromSart){}

    private List<Point>[] graph;
    private static final int CAN_NOT_BE_REACHED = -1;

    public int[] minimumTime(int numberOfNodes, int[][] edges, int[] disappearTime) {
        initializeGraph(edges, numberOfNodes);
        return dijkstraSearchForMinTimePath(disappearTime, numberOfNodes);
    }

    private void initializeGraph(int[][] edges, int numberOfNodes) {
        graph = new List[numberOfNodes];
        for (int i = 0; i < numberOfNodes; ++i) {
            graph[i] = new ArrayList<>();
        }

        for (int[] edge : edges) {
            int from = edge[0];
            int to = edge[1];
            int time = edge[2];

            if (from != to) {
                graph[from].add(new Point(to, time));
                graph[to].add(new Point(from, time));
            }
        }
    }

    private int[] dijkstraSearchForMinTimePath(int[] disappearTime, int numberOfNodes) {
        PriorityQueue<Step> minHeap = new PriorityQueue<>((x, y) -> x.timeFromSart - y.timeFromSart);
        minHeap.add(new Step(0, 0));

        int[] minTime = Arrays.copyOf(disappearTime, disappearTime.length);
        minTime[0] = 0;

        while (!minHeap.isEmpty()) {
            Step current = minHeap.poll();
            if (current.timeFromSart > minTime[current.index]) {
                continue;
            }

            for (Point next : graph[current.index]) {
                if (minTime[next.index] > current.timeFromSart + next.edgeTime) {
                    minTime[next.index] = current.timeFromSart + next.edgeTime;
                    minHeap.add(new Step(next.index, minTime[next.index]));
                }
            }
        }

        markUnreachableNodes(minTime, disappearTime, numberOfNodes);
        return minTime;
    }

    private void markUnreachableNodes(int[] minTime, int[] disappearTime, int numberOfNodes) {
        for (int i = 0; i < numberOfNodes; ++i) {
            if (minTime[i] == disappearTime[i]) {
                minTime[i] = CAN_NOT_BE_REACHED;
            }
        }
    }
}
