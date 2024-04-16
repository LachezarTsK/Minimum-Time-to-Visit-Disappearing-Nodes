
function minimumTime(numberOfNodes: number, edges: number[][], disappearTime: number[]): number[] {
    this.graph = Array.from(new Array(numberOfNodes), () => new Array());
    this.CAN_NOT_BE_REACHED = -1;

    initializeGraph(edges);
    return dijkstraSearchForMinTimePath(disappearTime, numberOfNodes);
};

class Point {
    constructor(public index: number, public edgeTime: number){}
}

class Step {
    constructor(public index: number, public timeFromSart: number){}
}

function initializeGraph(edges: number[][]): void {
    for (let [from, to, time] of edges) {
        if (from !== to) {
            this.graph[from].push(new Point(to, time));
            this.graph[to].push(new Point(from, time));
        }
    }
}

function dijkstraSearchForMinTimePath(disappearTime: number[], numberOfNodes: number): number[] {
    // const {MinPriorityQueue} = require('@datastructures-js/priority-queue');
    const minHeap = new MinPriorityQueue({ compare: (x, y) => x.timeFromSart - y.timeFromSart });
    minHeap.enqueue(new Step(0, 0));

    const minTime: number[] = Array.from(disappearTime);
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


function markUnreachableNodes(minTime: number[], disappearTime: number[], numberOfNodes: number): void {
    for (let i = 0; i < numberOfNodes; ++i) {
        if (minTime[i] === disappearTime[i]) {
            minTime[i] = this.CAN_NOT_BE_REACHED;
        }
    }
}
