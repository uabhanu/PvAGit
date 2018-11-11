using Unity.Entities;

public struct Weapon : IComponentData
{

}

public class WeaponComponent : ComponentDataWrapper<Weapon> 
{

}
