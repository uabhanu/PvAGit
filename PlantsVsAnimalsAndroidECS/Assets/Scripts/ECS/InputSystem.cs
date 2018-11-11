using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem
{
    struct Data
    {
        public int _length;
        //public ComponentArray<InputComponent> _inputComponents;
        public Transform _spawnBlock;
    }

    [Inject] Data _data;

    protected override void OnUpdate()
    {
        //for(int i = 0; i < _data._length; i++)
        //{
        //    _data._inputComponents[i].MouseButton(0) = InputSystem.GetMouseButton(0);
        //    _data._inputComponents[i].MouseButton(1) = InputSystem.GetMouseButton(1);
        //}
    }

}
