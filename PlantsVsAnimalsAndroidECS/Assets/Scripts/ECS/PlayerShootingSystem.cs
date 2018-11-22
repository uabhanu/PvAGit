using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class PlayerShootingSystem : JobComponentSystem
{

    public class PlayerShootingBarrier : BarrierSystem
    {

    }

    struct Data
    {
        public readonly int m_length;
        public EntityArray m_entities;
        public ComponentDataArray<Weapon> m_weapons;
        public SubtractiveComponent<Firing> m_firingToSubtract;
    }

    [Inject] Data _data;
    [Inject] PlayerShootingBarrier _playerShootingBarrier;

    struct PlayerShootingJob : IJobParallelFor
    {
        [ReadOnly] public EntityArray m_entityArray;
        public EntityCommandBuffer.Concurrent m_entityCommandBuffer; // Command Buffers are used to queue up jobs so Unity can decide the best time to use them and Concurrent in particular are used for parallel jobs 
        public bool m_bIsFiring;


        public void Execute(int i)
        {
            if(!m_bIsFiring)
            {
                return;
            }

            m_entityCommandBuffer.AddComponent(m_entityArray[i] , new Firing());
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new PlayerShootingJob
        {
            m_entityArray = _data.m_entities,
            m_entityCommandBuffer = _playerShootingBarrier.CreateCommandBuffer(),
            //m_bIsFiring = This part later
        }.Schedule(_data.m_length , 64 , inputDeps);
    }
}
