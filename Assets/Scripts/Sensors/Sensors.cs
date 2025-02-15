using Assets.Scripts.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Sensors
{
    public sealed class Sensors : IDisposable
    {
        public List<ISensor> SensorList { get; }

        public List<Camera.Camera> CameraList { get; }

        public List<GroundTruth2D.GroundTruth2D> GroundTruth2DList { get; }

        public List<GroundTruth3D.GroundTruth3D> GroundTruth3DList { get; }

        public List<Lidar.Lidar> LidarList { get; }

        private uint _seq = 0;

        public Sensors(
            ISensorsView view,
            IBridge bridge,
            SensorsSettings settings)
        {
            CameraList = view.CameraViews.Select(v => new Camera.Camera(v, bridge)).ToList();
            GroundTruth2DList = view.GroundTruth2DViews.Select(v => new GroundTruth2D.GroundTruth2D(v)).ToList();
            GroundTruth3DList = view.GroundTruth3DViews.Select(v => new GroundTruth3D.GroundTruth3D(v)).ToList();
            LidarList = view.LidarViews.Select(v => new Lidar.Lidar(v, bridge, settings.LidarSettings)).ToList();

            SensorList = new List<ISensor>();
            SensorList.AddRange(CameraList);
            SensorList.AddRange(GroundTruth2DList);
            SensorList.AddRange(GroundTruth3DList);
            SensorList.AddRange(LidarList);
        }

        public void Dispose()
        {
            foreach (ISensor sensor in SensorList)
            {
                sensor.Dispose();
            }
        }

        public void Measure()
        {
            foreach (ISensor sensor in SensorList)
            {
                sensor.Send(_seq);
            }

            _seq++;
        }

        public void SetDatasetGeneration(bool isActive)
        {
            foreach (ISensor sensor in SensorList)
            {
                sensor.IsGeneratingDataset = isActive;
            }
        }
    }
}