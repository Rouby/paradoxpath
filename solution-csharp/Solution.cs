using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace solution_csharp
{
    public class Solution
    {
        class WorkItem
        {
            public int Index { get; set; }

            public int Weight { get; set; }

            public WorkItem Predecessor { get; set; }

            public bool IsExpanded { get; set; }
        }

        class WorkItemComparer : IComparer<double>
        {
            public int Compare(double x, double y)
            {
                return y > x ? 1 : -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nStartX">0-based coordinates of the start position</param>
        /// <param name="nStartY">0-based coordinates of the start position</param>
        /// <param name="nTargetX">0-based coordinates of the target position</param>
        /// <param name="nTargetY">0-based coordinates of the target position</param>
        /// <param name="pMap">describes a grid of width nMapWidth and height nMapHeight.
        /// The grid is given in row-major order, each row is given in order of increasing x-coordinate,
        /// and the rows are given in order of increasing y-coordinate. Traversable locations of the grid
        /// are indicated by 1, and impassable locations are indicated by 0. Locations are considered
        /// to be adjacent horizontally and vertically but not diagonally</param>
        /// <param name="nMapWidth"></param>
        /// <param name="nMapHeight"></param>
        /// <param name="pOutBuffer">is where you should store the positions visited in the found path,
        /// excluding the starting position but including the final position. Entries in pOutBuffer
        /// are indices into pMap</param>
        /// <param name="nOutBufferSize">is the maximum amount number of entries that can be written to pOutBuffer</param>
        /// <returns>the length of the shortest path between Start and Target, or −1 if no such path exists</returns>
        public static int FindPath(int nStartX, int nStartY, int nTargetX, int nTargetY, char[] pMap, int nMapWidth, int nMapHeight, int[] pOutBuffer, int nOutBufferSize)
        {
            if (nOutBufferSize == 0)
                return -1;

            var mapWeight = new WorkItem[pMap.Length];

            var sPos = nStartX + nStartY * nMapWidth;
            var tPos = nTargetX + nTargetY * nMapWidth;

            Action<int, WorkItem, ConcurrentPriorityQueue.ConcurrentPriorityQueue<WorkItem, double>, Func<int, double>> addItem = (index, predecessor, queue, heuristic) =>
            {
                if (index >= 0 && index < mapWeight.Length)
                {
                    var newWeight = predecessor.Weight + 1;
                    var distance = heuristic(index);
                    var item = mapWeight[index] ?? (mapWeight[index] = new WorkItem { Index = index });
                    if (item.IsExpanded)
                        return;
                    if (!queue.Contains(item))
                        queue.Enqueue(item, newWeight + distance);
                    else if (newWeight < item.Weight)
                        queue.UpdatePriority(item, newWeight + distance);
                    item.Weight = newWeight;
                    item.Predecessor = predecessor;
                }
            };

            Action<WorkItem, ConcurrentPriorityQueue.ConcurrentPriorityQueue<WorkItem, double>, Func<int, double>> expandItem = (item, queue, heuristic) =>
            {
                item.IsExpanded = true;
                var edge = item.Index % nMapWidth;
                if (edge > 0)
                    addItem(item.Index - 1, item, queue, heuristic);
                if (edge < nMapWidth - 1)
                    addItem(item.Index + 1, item, queue, heuristic);
                addItem(item.Index - nMapWidth, item, queue, heuristic);
                addItem(item.Index + nMapWidth, item, queue, heuristic);
            };

            Action floodWeight = () =>
            {
                var queue = new ConcurrentPriorityQueue.ConcurrentPriorityQueue<WorkItem, double>(new WorkItemComparer());
                queue.Enqueue(new WorkItem { Index = sPos, Weight = 1 }, 1);

                Func<int, double> heuristic = (index) => Math.Abs((double)nTargetX - index % nMapWidth) + Math.Abs((double)nTargetY - index / nMapWidth);

                while (queue.Count > 0)
                {
                    var next = queue.Dequeue();

                    if (next.Index == tPos)
                        return;

                    if (pMap[next.Index] == 1)
                    {
                        expandItem(next, queue, heuristic);
                    }
                }
            };

            floodWeight();

            Func<WorkItem, int, int> walk = null;
            walk = (item, depth) =>
            {
                if (item == null && depth == 0)
                    return -1;
                if (item.Predecessor == null)
                    return 0;

                var max = walk(item.Predecessor, depth + 1);
                pOutBuffer[max] = item.Index;
                return max + 1;
            };

            return walk(mapWeight[tPos], 0);
        }
    }
}
