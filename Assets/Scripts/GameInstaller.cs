using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller {
    [SerializeField] private GameObject motorbike;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject roadBuilderPrefab;
    [SerializeField] private GameObject linePrefab;

    public override void InstallBindings() {
        SignalBusInstaller.Install(Container);

        Container.Bind<IInput>()
            .To<GameInput>().AsSingle();
        Container.Bind<IMotorbike>()
            .FromInstance(motorbike.GetComponent<IMotorbike>());
        Container.Bind<IUI>()
            .FromInstance(ui.GetComponent<IUI>());
        Container.Bind<RoadBuilder>()
            .FromComponentInNewPrefab(roadBuilderPrefab)
            .AsSingle().NonLazy();

        Container.BindMemoryPool<Line, Line.Pool>()
            .FromComponentInNewPrefab(linePrefab)
            .UnderTransformGroup("Lines");
        
        Container.BindInterfacesAndSelfTo<Game>().AsSingle();

        Container.DeclareSignal<BikeDeadSignal>();
        Container.BindSignal<BikeDeadSignal>()
            .ToMethod<IGame>(x => x.OnBikeDead).FromResolve();

        Container.DeclareSignal<WheelieSignal>();
        Container.BindSignal<WheelieSignal>()
            .ToMethod<IGame>(x => x.OnWheelie).FromResolve();
    }
}
