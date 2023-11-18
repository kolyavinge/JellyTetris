using System.Collections.Generic;
using JellyTetris.Model;

namespace JellyTetris.Core;

public interface IGame
{
    GameState State { get; }

    IShape CurrentShape { get; }

    IShape NextShape { get; }

    IReadOnlyCollection<IShape> Shapes { get; }

    void Update();

    void MoveCurrentShapeLeft();

    void MoveCurrentShapeRight();

    void DropCurrentShape();

    void RotateCurrentShape();
}
