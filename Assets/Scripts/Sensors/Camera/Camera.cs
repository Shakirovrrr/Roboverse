﻿using Assets.Scripts.Bridge;
using Assets.Scripts.Sensors.DatasetGeneration;
using RosMessageTypes.Sensor;

namespace Assets.Scripts.Sensors.Camera
{
    public sealed class Camera : ISensor
    {
        public string Topic => _view.Topic;
        
        public bool IsGeneratingDataset
        {
            get => _view.IsGeneratingDataset;
            set => _view.IsGeneratingDataset = value;
        }

        private readonly ICameraView _view;

        private readonly IPublisher<ImageMsg> _publisher;

        private readonly ImageRenderer _renderer;

        private readonly ImageMessageBuilder _messageBuilder;

        private readonly ImageDataset _dataset;

        public Camera(
            ICameraView view,
            IBridge bridge)
        {
            _view = view;
            _publisher = bridge.CreatePublisher<ImageMsg>(Topic);
            _renderer = new ImageRenderer(_view.Width, _view.Height, _view.Camera);
            _messageBuilder = new ImageMessageBuilder(view.Camera);
            _dataset = new ImageDataset(view.Topic);
        }

        public void Dispose()
        {
            _renderer.Dispose();
            UnityEngine.Object.Destroy(_view.GameObject);            
        }

        public void Send(uint seq)
        {
            byte[] data = _renderer.Render();
            _publisher.Publish(() => _messageBuilder.Build(seq, data));

            if (IsGeneratingDataset)
            {
                _dataset.AddImage(seq, data);
            }
        }
    }
}
