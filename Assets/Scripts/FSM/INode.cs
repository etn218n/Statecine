﻿namespace NodeCanvas
{
    public interface INode
    {
        void OnEnter();
        void OnUpdate();
        void OnFixedUpdate();
        void OnExit();
    }
}
