# OpenHardwareMonitorExporter

This is a [Prometheus Exporter](https://prometheus.io/docs/instrumenting/exporters/) Windows service for the [Open Hardware Monitor](http://openhardwaremonitor.org/) sensors.

Install the Windows service with:

```powershell
.\OpenHardwareMonitorExporter.exe install
```

**NB** you can change the default url (`http://localhost:9398/metrics`) with the `-url` parameter, e.g., `-url:http://localhost:9398/ohm/metrics`.

## Special Thanks

Original credit for initial source: https://github.com/rgl/OpenHardwareMonitorExporter