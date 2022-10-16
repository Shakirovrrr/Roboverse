﻿namespace Assets.Scripts.Sensors.GroundTruth2D
{
    public sealed class GroundTruth2D : ISensor
    {
        public string Topic => _view.Topic;

        private readonly IGroundTruth2DView _view;

        public GroundTruth2D(IGroundTruth2DView view)
        {
            _view = view;
        }        

        public void Dispose()
        {
            UnityEngine.Object.Destroy(_view.GameObject);
        }

        public void Send(uint seq)
        {
            
        }
    }
}
