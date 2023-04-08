# Quantidade de possíveis casos de febre por data/hora

from(bucket: "datalake_cloud")
  |> range(start: v.timeRangeStart, stop: v.timeRangeStop)
  |> filter(fn: (r) => r["_measurement"] == "temperature")
  |> filter(fn: (r) => r["_value"] >= 38)
  |> group(columns: ["_time"])
  |> count()
  |> group()