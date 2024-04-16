
#include <span>
#include <queue>
#include <vector>
using namespace std;

class Solution {

    struct Point {
        size_t index;
        int edgeTime;
        Point() = default;
        Point(size_t index, int edgeTime): index{index}, edgeTime{edgeTime}{}
    };

    struct Step {
        size_t index;
        int timeFromSart;
        Step() = default;
        Step(size_t index, int timeFromSart): index{index}, timeFromSart{timeFromSart}{}
    };

    struct CompareSteps {
        auto operator()(const Step& first, const Step& second)const {
            return first.timeFromSart > second.timeFromSart;
        };
    };

    vector<vector<Point>> graph;
    static const int CAN_NOT_BE_REACHED = -1;


public:
    vector<int> minimumTime(int numberOfNodes, const vector<vector<int>>& edges, const vector<int>& disappearTime) {
        initializeGraph(edges, numberOfNodes);
        return dijkstraSearchForMinTimePath(disappearTime, numberOfNodes);
    }

private:
    void initializeGraph(span<const vector<int>> edges, int numberOfNodes) {
        graph.resize(numberOfNodes);

        for (const auto& edge : edges) {
            int from = edge[0];
            int to = edge[1];
            int time = edge[2];

            if (from != to) {
                graph[from].emplace_back(to, time);
                graph[to].emplace_back(from, time);
            }
        }
    }

    vector<int> dijkstraSearchForMinTimePath(span<const int> disappearTime, int numberOfNodes) const {
        priority_queue<Step, vector<Step>, CompareSteps> minHeap;
        minHeap.emplace(0, 0);

        vector<int>minTime {disappearTime.begin(), disappearTime.end()};
        minTime[0] = 0;

        while (!minHeap.empty()) {
            Step current = minHeap.top();
            minHeap.pop();
            if (current.timeFromSart > minTime[current.index]) {
                continue;
            }

            for (const auto& next : graph[current.index]) {
                if (minTime[next.index] > current.timeFromSart + next.edgeTime) {
                    minTime[next.index] = current.timeFromSart + next.edgeTime;
                    minHeap.emplace(next.index, minTime[next.index]);
                }
            }
        }

        markUnreachableNodes(minTime, disappearTime, numberOfNodes);
        return minTime;
    }

    void markUnreachableNodes(span<int> minTime, span<const int> disappearTime, int numberOfNodes) const {
        for (int i = 0; i < numberOfNodes; ++i) {
            if (minTime[i] == disappearTime[i]) {
                minTime[i] = CAN_NOT_BE_REACHED;
            }
        }
    }
};
