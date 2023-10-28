using System.Collections.Generic;

namespace JellyTetris.Core;

public interface IGame
{
    IShape CurrentShape { get; }

    IShape NextShape { get; }

    IReadOnlyCollection<IShape> Shapes { get; }

    void Update();

    void MoveCurrentShapeLeft();

    void MoveCurrentShapeRight();

    void DropCurrentShape();

    void RotateCurrentShape();
}
