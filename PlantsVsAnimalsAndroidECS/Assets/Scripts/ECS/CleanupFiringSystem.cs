using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class CleanupFiringSystem : JobComponentSystem
{
    struct CleanupFiringJob : IJobParallelFor
    {
        [ReadOnly] public EntityArray m_entities;
        public EntityCommandBuffer.Concurrent m_entityCommandBuffer;

        public void Execute(int i)
        {
            m_entityCommandBuffer.RemoveComponent<Firing>(m_entities[i]);
        }
    }

    struct Data
    {
        public readonly int m_length;
        public EntityArray m_entities;
        public ComponentDataArray<Firing> m_firings;
    }

    [Inject] CleanupFiringBarrier _barrier;
    [Inject] Data _data;
}

public class CleanupFiringBarrier : BarrierSystem
{

}
