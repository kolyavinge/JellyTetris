using DependencyInjection;
using JellyTetris.Core;
using SoftBodyPhysics.Core;
using SoftBodyPhysics.Factories;

namespace JellyTetris.Utils;

internal class MainInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        bindingProvider.Bind<IPhysicsWorld>().ToMethod(_ => PhysicsWorldFactory.Make()).ToSingleton();
        bindingProvider.Bind<IShapeBuilder, ShapeBuilder>().ToSingleton();
        bindingProvider.Bind<IShapeFactory, ShapeFactory>().ToSingleton();
        bindingProvider.Bind<IShapeGenerator, ShapeGenerator>().ToSingleton();
        bindingProvider.Bind<IShapeEdgeDetector, ShapeEdgeDetector>().ToSingleton();
        bindingProvider.Bind<IShapePartBuilder, ShapePartBuilder>().ToSingleton();
        bindingProvider.Bind<IShapeMovingLogic, ShapeMovingLogic>().ToSingleton();
        bindingProvider.Bind<IShapeRotationLogic, ShapeRotationLogic>().ToSingleton();
        bindingProvider.Bind<IShapeCollisionChecker, ShapeCollisionChecker>().ToSingleton();
        bindingProvider.Bind<ILineEraseLogic, LineEraseLogic>().ToSingleton();
        bindingProvider.Bind<IGameInitializer, GameInitializer>().ToSingleton();
        bindingProvider.Bind<IGame, Game>().ToSingleton();
    }
}
