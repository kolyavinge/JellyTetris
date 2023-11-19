using System;
using System.Collections.Generic;
using System.Linq;
using JellyTetris.Model;
using SoftBodyPhysics.Model;

namespace JellyTetris.Core;

internal interface IShapeEdgeDetector
{
    ShapePoint[] GetEdgePoints(ISoftBody body);
}

internal class ShapeEdgeDetector : IShapeEdgeDetector
{
    public ShapePoint[] GetEdgePoints(ISoftBody body)
    {
        return Array.Empty<ShapePoint>();
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

        return result.Select(x => new ShapePoint(x)).ToArray();
    }
}
