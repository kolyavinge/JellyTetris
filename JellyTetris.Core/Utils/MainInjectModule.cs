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
        bindingProvider.Bind<IGame, Game>().ToSingleton();
    }
}
