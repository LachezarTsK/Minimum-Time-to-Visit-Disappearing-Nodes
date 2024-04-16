
/**
 * @param {number} numberOfNodes
 * @param {number[][]} edges
 * @param {number[]} disappearTime
 * @return {number[]}
 */
var minimumTime = function (numberOfNodes, edges, disappearTime) {
    this.graph = Array.from(new Array(numberOfNodes), () => new Array());
    this.CAN_NOT_BE_REACHED = -1;

    initializeGraph(edges);
    return dijkstraSearchForMinTimePath(disappearTime, numberOfNodes);
};

/**
 * @param {number} index
 * @param {number} edgeTime
 */
function Point(index, edgeTime) {
    this.index = index;
    this.edgeTime = edgeTime;
}

/**
 * @param {number} index
 * @param {number} timeFromSart
 */
function Step(index, timeFromSart) {
    this.index = index;
    this.timeFromSart = timeFromSart;
}

/**
 * @param {number[][]} edges
 * @return {void}
 */
function initializeGraph(edges) {
    for (let [from, to, time] of edges) {
        if (from !== to) {
            this.graph[from].push(new Point(to, time));
            this.graph[to].push(new Point(from, time));
        }
    }
}

/**
 * @param {number[]} disappearTime
 * @param {number} numberOfNodes
 * @return {number[]}
 */
function dijkstraSearchForMinTimePath(disappearTime, numberOfNodes) {
    // MinPriorityQueue<Step>
    // const {MinPriorityQueue} = require('@datastructures-js/priority-queue');
    const minHeap = new MinPriorityQueue({compare: (x, y) => x.timeFromSart - y.timeFromSart});
    minHeap.enqueue(new Step(0, 0));

    const minTime = Array.from(disappearTime);
    minTime[0] = 0;

    while (!minHeap.isEmpty()) {
        const current = minHeap.dequeue();
        if (current.timeFromSart > minTime[current.index]) {
            continue;
        }

        for (let next of this.graph[current.index]) {
            if (minTime[next.index] > current.timeFromSart + next.edgeTime) {
                minTime[next.index] = current.timeFromSart + next.edgeTime;
                minHeap.enqueue(new Step(next.index, minTime[next.index]));
            }
        }
    }

    markUnreachableNodes(minTime, disappearTime, numberOfNodes);
    return minTime;
}

/**
 * @param {number[]} minTime
 * @param {number[]} disappearTime
 * @param {number} numberOfNodes
 * @return {void}
 */
function markUnreachableNodes(minTime, disappearTime, numberOfNodes) {
    for (let i = 0; i < numberOfNodes; ++i) {
        if (minTime[i] === disappearTime[i]) {
            minTime[i] = this.CAN_NOT_BE_REACHED;
        }
    }
}
