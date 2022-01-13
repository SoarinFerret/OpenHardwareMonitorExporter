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
                var hw = sensor.Hardware.Identifier.ToString();
                var help = SensorTypeToHelp(sensor.SensorType);
                var metricName = "";

                switch(sensor.Hardware.HardwareType)
                {
                    case HardwareType.Mainboard:
                    case HardwareType.SuperIO:
                        metricName = $"ohm_mainboard_{sensor.SensorType.ToString().ToLowerInvariant()}";
                        break;
                    default:
                        metricName = $"ohm_{sensor.Hardware.HardwareType.ToString().ToLowerInvariant()}_{sensor.SensorType.ToString().ToLowerInvariant()}";
                        break;
                }
                try
                {
                    var gauge = Metrics.WithCustomRegistry(_registry).CreateGauge(metricName, help, "hw", "name");
                    gauge.Labels(hw, sensor.Name).Set(sensor.Value.Value);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
