
import java.util.*

class Solution {

    data class Point(val index: Int, val edgeTime: Int){}
    data class Step(val index: Int, val timeFromSart: Int){}

    companion object {
        const val CAN_NOT_BE_REACHED = -1
    }

    private var graph = arrayOfNulls<ArrayList<Point>>(0)

    fun minimumTime(numberOfNodes: Int, edges: Array<IntArray>, disappearTime: IntArray): IntArray {
        initializeGraph(edges, numberOfNodes)
        return dijkstraSearchForMinTimePath(disappearTime, numberOfNodes)
    }

    private fun dijkstraSearchForMinTimePath(disappearTime: IntArray, numberOfNodes: Int): IntArray {
        val minHeap = PriorityQueue<Step> { x, y -> x.timeFromSart - y.timeFromSart }
        minHeap.add(Step(0, 0))

        val minTime = disappearTime.copyOf(disappearTime.size)
        minTime[0] = 0

        while (!minHeap.isEmpty()) {
            val current = minHeap.poll()
            if (current.timeFromSart > minTime[current.index]) {
                continue
            }

            for (next in graph[current.index]!!) {
                if (minTime[next.index] > current.timeFromSart + next.edgeTime) {
                    minTime[next.index] = current.timeFromSart + next.edgeTime
                    minHeap.add(Step(next.index, minTime[next.index]))
                }
            }
        }

        markUnreachableNodes(minTime, disappearTime, numberOfNodes)
        return minTime
    }

    private fun markUnreachableNodes(minTime: IntArray, disappearTime: IntArray, numberOfNodes: Int) {
        for (i in 0..<numberOfNodes) {
            if (minTime[i] == disappearTime[i]) {
                minTime[i] = CAN_NOT_BE_REACHED
            }
        }
    }

    private fun initializeGraph(edges: Array<IntArray>, numberOfNodes: Int) {
        graph = arrayOfNulls<ArrayList<Point>>(numberOfNodes)
        for (i in 0..<numberOfNodes) {
            graph[i] = ArrayList<Point>()
        }

        for ((from, to, time) in edges) {
            if (from != to) {
                graph[from]!!.add(Point(to, time))
                graph[to]!!.add(Point(from, time))
            }
        }
    }
}
