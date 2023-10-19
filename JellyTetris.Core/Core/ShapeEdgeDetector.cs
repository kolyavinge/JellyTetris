using System.Collections.Generic;
using System.Linq;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

internal interface IShapeEdgeDetector
{
    IEnumerable<IMassPoint> GetEdgePoints(ISoftBody body);
}

internal class ShapeEdgeDetector : IShapeEdgeDetector
{
    public IEnumerable<IMassPoint> GetEdgePoints(ISoftBody body)
    {
        var result = new List<IMassPoint>();

        var edges = body.Springs.Where(s => s.IsEdge).ToList();
        var startPoint = edges[0].PointA;
        result.Add(startPoint);
        var currentPoint = edges[0].PointB;
        edges.RemoveAt(0);
        while (currentPoint != startPoint)
        {
            result.Add(currentPoint);
            var edge = edges.First(x => x.PointA == currentPoint || x.PointB == currentPoint);
            if (edge.PointA == currentPoint)
            {
                currentPoint = edge.PointB;
            }
            else
            {
                currentPoint = edge.PointA;
            }
            edges.Remove(edge);
        }

        return result;
    }
}
