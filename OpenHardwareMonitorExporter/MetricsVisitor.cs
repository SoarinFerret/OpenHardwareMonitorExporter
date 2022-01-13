using OpenHardwareMonitor.Hardware;
using Prometheus;

namespace OpenHardwareMonitorExporter
{
    internal class MetricsVisitor : IVisitor
    {
        private CollectorRegistry _registry;

        public MetricsVisitor(CollectorRegistry registry)
        {
            _registry = registry;
        }

        public void VisitComputer(IComputer computer)
        {
        }

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            hardware.Traverse(this);
        }

        public void VisitParameter(IParameter parameter)
        {
        }

        private string SensorTypeToHelp(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Voltage:
                    return "Voltage [V]";
                case SensorType.Flow:
                    return "Flow [L/h]";
                case SensorType.Level:
                    return "Level [%}";
                case SensorType.Factor:
                    return "Factor";
                case SensorType.Clock:
                    return "Clock [MHz]";
                case SensorType.Temperature:
                    return "Temperature [C]";
                case SensorType.Load:
                    return "Load [%]";
                case SensorType.Control:
                    return "Control [%]";
                case SensorType.Fan:
                    return "Fan [RPM]";
                case SensorType.Power:
                    return "Power consumption [W]";
                case SensorType.Data:
                    return "Data [GB]";
                case SensorType.SmallData:
                    return "Data [MB]";
                case SensorType.Throughput:
                    return "Throughput [MB/s]";
                default:
                    return "Unknown";
            }
        }


        public void VisitSensor(ISensor sensor)
        {
            if (sensor.Value != null)
            {
                if (sensor.Hardware.HardwareType == HardwareType.CPU)
                {
                    var metricName = $"ohm_cpu_{sensor.SensorType.ToString().ToLowerInvariant()}";
                    var hw = sensor.Hardware.Identifier.ToString();
                    var help = SensorTypeToHelp(sensor.SensorType);

                    var gauge = Metrics.WithCustomRegistry(_registry).CreateGauge(metricName, help, "hw", "name");
                    gauge.Labels(hw, sensor.Name).Set(sensor.Value.Value);
                }
            }
        }
    }
}
